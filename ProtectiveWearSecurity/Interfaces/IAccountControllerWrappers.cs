using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProtectiveWearSecurity.Models;
using System.Threading.Tasks;

namespace ProtectiveWearSecurity.Interfaces
{
    public interface IAccountControllerWrappers
    {
        public Task SignOutAsync(HttpContext context);
        public string GetActionLink(IUrlHelper helper, string action, ApplicationUser user, string code);
        public bool IsLocalUrl(IUrlHelper helper, string url);
    }
}
