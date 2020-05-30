using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProtectiveWearSecurity.Models;

namespace ProtectiveWearSecurity.Controllers
{
    /// <summary>
    /// Clase encargada de generar la autenticación
    /// </summary>    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("v1/api/account")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="configuration"></param>
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._configuration = configuration;
        }

        /// <summary>
        /// Proceso encargado de aplicar la identificacón del usaurio y la generación del token.
        /// </summary>
        /// <param name="model">Objeto de tipo usuario con datos de identificación.</param>
        /// <returns>Retorna un token valido para autorización y los datos del usuario identificado.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginApiResource model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return await GetToken(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return BadRequest(model);
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Método encargado de cerrar la sesion.
        /// </summary>
        /// <returns>retorna una respuesa de salida</returns>
        [HttpPost("Logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out");
        }

        /// <summary>
        /// Método encargado de registrar un nuevo usuario.
        /// </summary>
        /// <param name="model">Objeto de tipo usuario con lo datos para el registro.</param>
        /// <returns>Retorna los datos del usuario registrado.</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterApiResource model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(user);
                }
                AddErrors(result);
            }

            return BadRequest(ModelState);
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
                 

                return Ok(new { 
                               token = new JwtSecurityTokenHandler().WriteToken(token),
                               expiration,
                               Account =   new AccountApiModel{ FirstName = user .FirstName,
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