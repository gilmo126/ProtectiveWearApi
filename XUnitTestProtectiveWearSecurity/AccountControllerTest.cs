using Xunit;
using ProtectiveWearSecurity.Controllers;
using ProtectiveWearSecurity.Models;
using ProtectiveWearSecurity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Json;
using System.Threading.Tasks;

namespace XUnitTestProtectiveWearSecurity
{
    public class AccountControllerTest
    {
        private Mock<ProtectiveWearApiDbContext> _mockContext;
        private Mock<UserStore<ApplicationUser>> _mockUserStore;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private Mock<IUserClaimsPrincipalFactory<ApplicationUser>> _mockUserClaimsPrincipalFactory;
        private Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private Mock<ILogger<AccountController>> _mockLogger;

        private void Setup()
        {
            _mockContext = new Mock<ProtectiveWearApiDbContext>(new DbContextOptions<ProtectiveWearApiDbContext>());
            _mockUserStore = new Mock<UserStore<ApplicationUser>>(_mockContext.Object, null);
            _mockUserManager =  new Mock<UserManager<ApplicationUser>>(_mockUserStore.Object, null, null, null, null, null, null, null, null);
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockUserClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(_mockUserManager.Object, _mockHttpContextAccessor.Object, _mockUserClaimsPrincipalFactory.Object, null, null, null,null);
            _mockLogger = new Mock<ILogger<AccountController>>();
        }
      
        /// <summary>
        /// Prueba para registrar un nuevo Usuario.
        /// </summary>
        [Fact]
        public void DeberarregistrarNuevoUsuarioPost()
        {
            //Arrange
            Setup();

           
            var context = new DefaultHttpContext();
            var mockWrappers = new Mock<IAccountControllerWrappers>();
            var mockTempData = new TempDataDictionary(context, Mock.Of<ITempDataProvider>()) { ["Information"] = "" };
            var viewModel = new RegisterApiResource
            {
                Email = "gilmo@loquesea.com",
                Password = "test_password1",
                ConfirmPassword = "test_password1"
            };
            var mockConfiguration = new Mock<IConfiguration>();
            var mockEmailSender = new Mock<IEmailSender>();

            mockConfiguration.Setup(x => x["EnableRegistration"]).Returns("true");
            mockWrappers.Setup(x =>
                    x.GetActionLink(It.IsAny<IUrlHelper>(), It.IsAny<string>(), It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns("test");
            mockWrappers.Setup(x => x.IsLocalUrl(It.IsAny<IUrlHelper>(), It.IsAny<string>())).Returns(true);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("123456");
            mockEmailSender.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var accountController = new AccountController(_mockUserManager.Object, null, _mockLogger.Object, mockEmailSender.Object, mockConfiguration.Object)
            {
                ControllerContext = { HttpContext = context }, 
                Wrappers = mockWrappers.Object,
                TempData = mockTempData
            };

            //Act
            var response = accountController.Register(viewModel).Result;

            //Assert
            _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _mockUserManager.Verify(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()), Times.Once);
            mockEmailSender.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
            Assert.IsType<OkObjectResult>(response);
        }
        /// <summary>
        /// Prueba para verificar que als credenciales de un usuario son correctas.
        /// </summary>
        [Fact]
        public void DeberaresponderSiCredencialesusuarioSonValidas()
        {
            //Arrange
            Setup();

            const string email = "test@test.com";
            const string password = "password";
            var loginModel = new LoginApiResource
            {
                Email = email,
                Password = password

            };
            var user = new ApplicationUser
            {
                Email = email
            };
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Token:Key")]).Returns("WERET-12Ret-oipoiRTTY-345345OIO");


            _mockSignInManager.Setup(x => x.PasswordSignInAsync(It.Is<string>(s => s.Equals(email)),
                It.Is<string>(s => s.Equals(password)),
                It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(user, It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            var accountController = new AccountController(_mockUserManager.Object, _mockSignInManager.Object,_mockLogger.Object, null, mockConfiguration.Object);

            //Act
            var response = accountController.Login(loginModel).Result as ObjectResult;

            string jsonString;
            jsonString = JsonSerializer.Serialize(response.Value);

            //Assert            
            _mockSignInManager.Verify(x => x.PasswordSignInAsync(It.Is<string>(s => s.Equals(email)),It.Is<string>(s => s.Equals(password)),It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
            Assert.IsType<string>(jsonString);           
            Assert.Equal("200", (response).StatusCode.Value.ToString());
        }
        /// <summary>
        /// Prueba para verificar que als credenciales de un usuario son incorrectas. 
        /// </summary>
        [Fact]
        public void DeberaresponderSiCredencialesusuarioSonInvalidas()
        {
            //Arrange
            const string email = "test@test.com";
            

            var loginModel = new LoginApiResource { Email = email };
            var mock = new Mock<ILogger<AccountController>>().Object;
            var accountController = new AccountController(null, null, mock, null, null);
            accountController.ModelState.AddModelError("400", "error message");

            //Act
            var response = accountController.Login(loginModel).Result as ObjectResult;

            //Assert
            Assert.IsType<SerializableError>(response.Value);
            Assert.Equal("400", (response).StatusCode.Value.ToString());
        }
        /// <summary>
        /// Prueba para verificar si el email de un usuario es correcto o existe.
        /// </summary>
        [Fact]
        public void DeberaConfirmarUnEmaill()
        {
            //Arrange
            Setup();

            const string testCode = "test_code";
            var user = new ApplicationUser
            {
                Id = "1234",
                Email = "test@test.com"
            };
            var context = new DefaultHttpContext();
            var mockTempData = new TempDataDictionary(context, Mock.Of<ITempDataProvider>()) { ["Information"] = "" };

            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var accountController = new AccountController(_mockUserManager.Object, null, null,
                null, null)
            { TempData = mockTempData };

            //Act
            var response = accountController.ConfirmEmail(user.Id, testCode).Result as ObjectResult;
            var result = Assert.IsType<string>(response.Value.ToString());
            //Assert
            _mockUserManager.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
            _mockUserManager.Verify(x => x.ConfirmEmailAsync(user, testCode), Times.Once);

            Assert.Contains("Registration confirmed!", result.ToString());
            Assert.Equal("200", (response).StatusCode.Value.ToString());
        }
        /// <summary>
        /// Prueba para verificar si el email de un usuario es Incorrecto o no existe.
        /// </summary>
        [Fact]
        public void DeberaProcesarUnEmailIncorrecto()
        {
            //Arrange
            Setup();

            const string testCode = "test_code";
            var user = new ApplicationUser
            {
                Id = "1234",
                Email = "test@test.com"
            };
            var context = new DefaultHttpContext();
            var mockTempData = new TempDataDictionary(context, Mock.Of<ITempDataProvider>()) { ["Information"] = "" };

            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var accountController = new AccountController(_mockUserManager.Object, null, null,
                null, null)
            { TempData = mockTempData };

            //Act
            var response = accountController.ConfirmEmail(user.Id, testCode).Result as ObjectResult;
            var result = Assert.IsType<string>(response.Value.ToString());
            _mockUserManager.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
            _mockUserManager.Verify(x => x.ConfirmEmailAsync(user, testCode), Times.Once);

            Assert.Contains("Something went wrong.", result.ToString());
            Assert.Equal("400", (response).StatusCode.Value.ToString());
        }
        /// <summary>
        /// Prueba para cerrar Sesion de usuario.
        /// </summary>
        [Fact]
        public void DeberarSalirUsuario()
        {
            Setup();

            _mockSignInManager.Setup(x => x.SignOutAsync()).Returns(Task.FromResult(""));

            var accountController = new AccountController(null, _mockSignInManager.Object, _mockLogger.Object,
                null, null);

            //Act
            var response = accountController.Logout().Result as ObjectResult;

            //Assert
            var result = Assert.IsType<string>(response.Value.ToString());
            _mockSignInManager.Verify(x => x.SignOutAsync(), Times.Once);

            Assert.Contains("Logged out", result.ToString());
            Assert.Equal("200",(response).StatusCode.Value.ToString());
        }
        /// <summary>
        /// Prueba para verificar que un usuario no se encuentra por su password. 
        /// </summary>
        [Fact]
        public void PasswordOlvidadoDeberaValidarQueNoFueEncontrado()
        {
            //Arrange
            Setup();

            var forgotPasswordViewModel = new ForgotPasswordModel
            {
                Email = "test@test.com"
            };
            var context = new DefaultHttpContext();
            var mockTempData = new TempDataDictionary(context, Mock.Of<ITempDataProvider>()) { ["Error"] = "" };

            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult<ApplicationUser>(null));

            var accountController = new AccountController(_mockUserManager.Object, null, null,
                null, null)
            { TempData = mockTempData };

            //Act
            var response = accountController.ForgotPassword(forgotPasswordViewModel).Result as ObjectResult;            

            //Assert
            var result = Assert.IsType<string>(response.Value.ToString());
            _mockUserManager.Verify(x => x.FindByEmailAsync(forgotPasswordViewModel.Email), Times.Once);
            Assert.Contains("No user with that email address found.", result.ToString());
            Assert.Equal("404", (response).StatusCode.Value.ToString());
        }
        /// <summary>
        /// Prueba para determinar que un password fue encontrado.
        /// </summary>
        [Fact]
        public void DeberarEncontrarUnPasswordOlvidado()
        {
            //Arrange
            Setup();

            const string email = "test@test.com";
            const string code = "1234";
            var forgotPasswordViewModel = new ForgotPasswordModel
            {
                Email = email
            };

            var user = new ApplicationUser
            {
                Email = email
            };
            var context = new DefaultHttpContext();
            var mockTempData = new TempDataDictionary(context, Mock.Of<ITempDataProvider>()) { ["Information"] = "" };
            var mockWrappers = new Mock<IAccountControllerWrappers>();
            var mockEmailSender = new Mock<IEmailSender>();

            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(code);
            mockWrappers.Setup(x => x.GetActionLink(It.IsAny<IUrlHelper>(), It.IsAny<string>(), It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns("callbackUrl");
            mockEmailSender.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(""));

            var accountController = new AccountController(_mockUserManager.Object, null, null,
                mockEmailSender.Object, null)
            { Wrappers = mockWrappers.Object, TempData = mockTempData };

            //Act
            var response = accountController.ForgotPassword(forgotPasswordViewModel).Result as ObjectResult;

            //Assert
            var result = Assert.IsType<string>(response.Value.ToString());
            _mockUserManager.Verify(x => x.FindByEmailAsync(forgotPasswordViewModel.Email), Times.Once);
            _mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(user), Times.Once);
            mockWrappers.Verify(x => x.GetActionLink(It.IsAny<IUrlHelper>(), It.IsAny<string>(), user, code), Times.Once);
            mockEmailSender.Verify(x => x.SendEmailAsync(email, "Reset your password", It.IsAny<string>()), Times.Once());

            Assert.Equal("200", (response).StatusCode.Value.ToString());
            Assert.Contains("Password reset email sent.", result.ToString());
        }
        /// <summary>
        /// Prueba para validar el restablecimiento de password exitoso.
        /// </summary>
        [Fact]
        public void ReestablecimientoPasswordExitoso()
        {
            //Arrange
            Setup();

            const string code = "1234";
            const string email = "test@test.com";
            const string password = "password";

            var resetPasswordViewModel = new ResetPasswordModel
            {
                Code = code,
                Email = email,
                Password = password
            };

            var user = new ApplicationUser
            {
                Email = email
            };

            var context = new DefaultHttpContext();
            var mockTempData = new TempDataDictionary(context, Mock.Of<ITempDataProvider>()) { ["Information"] = "" };
            var mockEmailSender = new Mock<IEmailSender>();

            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()));
            mockEmailSender.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var accountController = new AccountController(_mockUserManager.Object, null, null, mockEmailSender.Object, null)
            {
                TempData = mockTempData
            };

            //Act
            var response = accountController.ResetPasswordConfirm(resetPasswordViewModel).Result as ObjectResult;

            //Assert
            var result = Assert.IsType<string>(response.Value.ToString());
            _mockUserManager.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _mockUserManager.Verify(x => x.ResetPasswordAsync(user, code, password), Times.Once);
            mockEmailSender.Verify(x => x.SendEmailAsync(email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.Contains("Password reset successfully.", result.ToString());
            Assert.Equal("200", (response).StatusCode.Value.ToString());
        }
        /// <summary>
        /// Prueba para determinar un resstablecimiento que sera Exitoso.
        /// </summary>
        [Fact]
        public void ReestablecimientoPasswordSeraExitoso()
        {
            //Arrange
            Setup();

            const string userId = "1234";
            const string code = "4321";
            const string email = "test@test.com";

            var user = new ApplicationUser
            {
                Id = userId,
                Email = email
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

            var accountController = new AccountController(_mockUserManager.Object, null, null, null, null);

            //Act
            var response = accountController.ResetPassword(userId, code).Result as ObjectResult;

            //Assert
            _mockUserManager.Verify(x => x.FindByIdAsync(userId), Times.Once);
            Assert.Equal(email, (response.Value as ResetPasswordModel).Email);
        }
        /// <summary>
        /// Prueba para determinar que un restablecimiento de password es fallido.
        /// </summary>
        [Fact]
        public void RestablecimientoPasswordFallido()
        {
            //Arrange
            Setup();

            const string code = "1234";
            const string email = "test@test.com";
            const string password = "password";

            var resetPasswordViewModel = new ResetPasswordModel
            {
                Code = code,
                Email = email,
                Password = password
            };

            var context = new DefaultHttpContext();
            var mockTempData = new TempDataDictionary(context, Mock.Of<ITempDataProvider>()) { ["Error"] = "" };
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult<ApplicationUser>(null));

            var accountController = new AccountController(_mockUserManager.Object, null, null, null, null)
            {
                TempData = mockTempData
            };

            //Act
            var response = accountController.ResetPasswordConfirm(resetPasswordViewModel).Result as ObjectResult;

            //Assert
            var result = Assert.IsType<string>(response.Value.ToString());
            _mockUserManager.Verify(x => x.FindByEmailAsync(email), Times.Once);
            Assert.Contains("No user with that email address found.", result.ToString());
            Assert.Equal("404", response.StatusCode.Value.ToString());
        }
        /// <summary>
        /// Prueba para determinar que un reestablecimiento de password sera fallido
        /// </summary>
        [Fact]
        public void RestablecimientoPasswordSeraFallido()
        {
            //Arrange
            Setup();

            const string userId = "1234";
            const string code = "4321";
                  

            var context = new DefaultHttpContext();
            var mockTempData = new TempDataDictionary(context, Mock.Of<ITempDataProvider>()) { ["Error"] = "" };
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult<ApplicationUser>(null));

            var accountController = new AccountController(_mockUserManager.Object, null, null, null, null)
            {
                TempData = mockTempData
            };

            //Act
            var response = accountController.ResetPassword(userId, code).Result as ObjectResult;

            //Assert
            var result = Assert.IsType<string>(response.Value.ToString());
            _mockUserManager.Verify(x => x.FindByIdAsync(userId), Times.Once);
            Assert.Contains("Invalid confirmation code.", result.ToString());
            Assert.Equal("404", response.StatusCode.Value.ToString());
        }


    }

    
}
