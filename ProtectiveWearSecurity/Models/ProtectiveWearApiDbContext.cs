using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProtectiveWearSecurity.Models;

namespace ProtectiveWearSecurity.Models
{
    public class ProtectiveWearApiDbContext : IdentityDbContext<ApplicationUser>
    {
        public ProtectiveWearApiDbContext(DbContextOptions<ProtectiveWearApiDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public ProtectiveWearApiDbContext() { 
        }
    }
}
