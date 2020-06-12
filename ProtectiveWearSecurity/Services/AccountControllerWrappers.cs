using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProtectiveWearSecurity.Interfaces;
using ProtectiveWearSecurity.Models;
using System.Threading.Tasks;

namespace ProtectiveWearSecurity.Services
{
    public class AccountControllerWrappers : IAccountControllerWrappers
    {
        public async Task SignOutAsync(HttpContext context)
        {
            await context.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        public string GetActionLink(IUrlHelper helper, string action, ApplicationUser user, string code)
        {
            return helper.ActionLink(action,
                values: new { userId = user.Id, code = code });
        }

        public bool IsLocalUrl(IUrlHelper helper, string url)
        {
            return helper.IsLocalUrl(url);
        }
    }
}
