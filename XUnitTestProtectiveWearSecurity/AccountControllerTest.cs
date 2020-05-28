using System;
using Xunit;
using ProtectiveWearSecurity.Controllers;
using ProtectiveWearSecurity.Models;
using ProtectiveWearSecurity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace XUnitTestProtectiveWearSecurity
{
    public class AccountControllerTest
    {

        [Fact]
        public void LoginTest()
        {
            var model = new RegisterApiResource()
            {
                Email = "1234",
                FirstName = "user@test.com",
                LastName = "",
                Password = "1",
                ConfirmPassword = ""
            };
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
            //var mockUserManager = new Mock<UserManager<ApplicationUser>>();
            //mockUserManager.Setup(p => p.CreateAsync(user, model.Password).Result.Succeeded);
            //var controler = new AccountController(mockUserManager.Object);

            Assert.Equal(true, user.EmailConfirmed);
            //var mockManageUsers = new Mock<IManager>();
            //// Note: Pulling fake user data from private methods down below
            //mockManageUsers.Setup(p => p.GetUsers()).Returns(GetUsersListTestData());
            //var controller = new AccountController(mockManageUsers.Object);

            //var result = controller.Login.Index();
            //var viewResult = Assert.IsType<ViewResult>(result);
            //ViewDataDictionary viewData = viewResult.ViewData;
            //Assert.NotNull(viewData["userList"]);


        }

        private List<IdentityUser> GetUsersListTestData()
        {
            var users = new List<IdentityUser>();
            users.Add(GetUserTestData());
            return users;
        }

        private IdentityUser GetUserTestData()
        {
            var model = new RegisterApiResource()
            {
                Email = "1234",
                FirstName = "user@test.com",
                LastName ="",
                Password ="",
                ConfirmPassword=""
            };

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
            return user;
        }

    }

    
}
