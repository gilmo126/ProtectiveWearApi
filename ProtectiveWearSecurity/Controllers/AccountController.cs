using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProtectiveWearSecurity.Models;
using ProtectiveWearSecurity.Interfaces;
using ProtectiveWearSecurity.Services;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ProtectiveWearSecurity.Controllers
{
    /// <summary>
    /// Clase encargada de generar la autenticación
    /// </summary>    
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("v1/api/account")]
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;

        public IAccountControllerWrappers Wrappers { get; set; }

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="configuration"></param>
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IEmailSender emailSender,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this._configuration = configuration;
            Wrappers = new AccountControllerWrappers();
        }


        /// <summary>
        /// Proceso encargado de aplicar la identificacón del usaurio y la generación del token.
        /// </summary>
        /// <param name="model">Objeto de tipo usuario con datos de identificación.</param>
        /// <returns>Retorna un token valido para autorización y los datos del usuario identificado.</returns>
        /// <response code="200">OK. Solicitud exitosa.</response>        
        /// <response code="404">NotFound. Requested resource does not exist on the server.</response> 
        /// <response code="400">BadRequest. Request could not be understood by the server.</response> 
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginApiResource model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return await GetToken(model);
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError("404", "Invalid login attempt.");
                    return NotFound(new
                    {
                        Error = new { Message = "Invalid login attempt." },
                        result
                    });
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Método encargado de cerrar la sesion.
        /// </summary>
        /// <returns>retorna una respuesa de salida</returns>
        /// <response code="200">OK. Solicitud exitosa.</response>        

        [HttpPost]
        [AllowAnonymous]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok(new { Succeess = new { Message = "Logged out" } });
        }
        /// <summary>
        /// Método encargado de bloquear un usuario.
        /// </summary>
        /// <returns>Retorna un error Http 400 "User account locked out."</returns>
        /// <response code="400">BadRequest. Request could not be understood by the server.</response> 

        [HttpGet]
        [AllowAnonymous]
        [Route("Lockout")]
        public IActionResult Lockout()
        {
            return BadRequest(new { Error = new { Message = "User account locked out." } });
        }
        /// <summary>
        /// Método encargado de recuperar un password de usuario olvidado a traves de un link enviado al cliente solicitante. 
        /// </summary>
        /// <param name="model">Parametro con la información del correo eléctronico del usuario.</param>
        /// <returns>retorna una respuesta Http, 200 ("Password reset email sent.") o 404 ("No user with that email address found.")</returns>
        /// <response code="200">OK. Solicitud exitosa.</response>        
        /// <response code="404">NotFound. Requested resource does not exist on the server.</response> 
        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Wrappers.GetActionLink(Url, nameof(ResetPassword), user, code);

                await _emailSender.SendEmailAsync(model.Email, "Reset your password", $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return Ok(new { Succeess = new { Message = "Password reset email sent." } });
            }


            return NotFound(new { Error = new { Message = "No user with that email address found." } });
        }
        /// <summary>
        /// Método de tomar un Id Usuario y un token para iniciar el proceso de cambio de password tras ser enviado por Forgotpassword.
        /// </summary>
        /// <param name="userId">Identificación del usuario</param>
        /// <param name="code">Código o token generado por Forgotpassword, por medio de un link enviado al correo del usuario.</param>
        /// <returns>Retorna un código 200 (Modelo de identificación del usuario) o 404 ("Invalid confirmation code.")</returns>
        /// <response code="200">OK. Solicitud exitosa.</response>        
        /// <response code="404">NotFound. Requested resource does not exist on the server.</response> 

        [HttpGet]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var viewModel = new ResetPasswordModel { Email = user.Email, Code = code };

                return Ok(viewModel);
            }

            return NotFound(new { Error = new { Message = "Invalid confirmation code." } });
        }
        /// <summary>
        /// Método encargado de realizar el cambio del password luego de ser verificado por resetPassword, al generar un token y IdUsuario.
        /// </summary>
        /// <param name="model">Modelo que contiene la información retornada por ResetPassword, junto con los datos a cambiar.</param>
        /// <returns>retorna un codifo 200 ("Password reset successfully.") o 400 ("No user with that email address found.")</returns>
        /// <response code="200">OK. Solicitud exitosa.</response>        
        /// <response code="404">NotFound. Requested resource does not exist on the server.</response> 
        [HttpGet]
        [AllowAnonymous]
        [Route("ResetPasswordConfirm")]
        public async Task<IActionResult> ResetPasswordConfirm(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                await _emailSender.SendEmailAsync(model.Email, "Password reset notification", $"Your password has been reset successfully.");

                return Ok(new
                {
                    Successror = new { Message = "Password reset successfully." }
                }
                );
            }

            return NotFound(new
            {
                Error = new { Message = "No user with that email address found." }
            });
        }
        /// <summary>
        /// Método encargado de registrar un nuevo usuario.
        /// </summary>
        /// <param name="model">Objeto de tipo usuario con lo datos para el registro.</param>
        /// <returns>Retorna los datos del usuario registrado.</returns>
        /// <response code="200">OK. Solicitud exitosa.</response>  
        /// <response code="201">Created. Se ha creado el objeto.</response> 
        /// <response code="400">BadRequest. Request could not be understood by the server.</response> 
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterApiResource model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogWarning("User created a new account with password.");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Wrappers.GetActionLink(Url, nameof(ConfirmEmail), user, code);
                    await _emailSender.SendEmailAsync(model.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    var userApi = new AccountApiModel
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email
                    };

                    return Ok(new
                    {
                        userApi,
                        Success = new { Message = "User created." }
                    });
                }

                AddErrors(result);

                return BadRequest(new
                {
                    Error = new { Message = "Something went wrong." },
                    result
                });


            }

            return BadRequest(ModelState);
        }
        /// <summary>
        /// Método encargado de confirmar el registro del usuario, luego de que se haya dado aceptacion a traves del link de confirmación por la operación register.
        /// </summary>
        /// <param name="userId">Identificación del usuario</param>
        /// <param name="code">Código o token generado por Register, por medio de un link enviado al correo del usuario.</param>
        /// <returns>Retorna un codigo http 200("Registration confirmed!") o 400("Something went wrong.")</returns>
        /// <response code="200">OK. Solicitud exitosa.</response>        
        /// <response code="400">BadRequest. Request could not be understood by the server.</response> 
        [HttpGet]
        [AllowAnonymous]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return Ok(new { Succeess = new { Message = "Registration confirmed!" } });
            }
            else
            {
                return BadRequest(new
                {
                    Error = new { Message = "Something went wrong." },
                    result
                });
            }

        }

        /// <summary>
        /// Método encargado de interpretar los errores que puedan presentarsen en uso del api.
        /// </summary>
        /// <param name="result">retorna un mensaje de error.</param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        /// <summary>
        /// Método encargado de generar el token despues de validar la identificación del usuario.
        /// </summary>
        /// <param name="model">Objeto de tipo usuario con datos de identificación.</param>
        /// <returns>Retorna un token valido para autorización y los datos del usuario identificado.</returns>
        private async Task<IActionResult> GetToken(LoginApiResource model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {

                var result = await _signInManager.CheckPasswordSignInAsync
                                (user, model.Password, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    return Unauthorized();
                }

                var claims = new[]
                {
                        new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                var expiration = DateTime.UtcNow.AddHours(1);

                var token = new JwtSecurityToken
                (
                    issuer: _configuration["Token:Issuer"],
                    audience: _configuration["Token:Audience"],
                    claims: claims,
                    expires: expiration,
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                                (Encoding.UTF8.GetBytes(_configuration["Token:Key"])),
                            SecurityAlgorithms.HmacSha256)
                );


                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration,
                    Account = new AccountApiModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                    }
                });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}