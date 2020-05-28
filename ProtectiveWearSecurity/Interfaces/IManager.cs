using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProtectiveWearSecurity.Models;
using Microsoft.Extensions.Configuration;

namespace ProtectiveWearSecurity.Interfaces
{
   public interface IManager
    {
        List<IdentityUser> GetUsers();

    }

    public class ManageUsers : IManager
    {
        private UserManager<IdentityUser> _userManager;
        public ManageUsers(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public List<IdentityUser> GetUsers() => _userManager.Users.ToList();
    }
}
