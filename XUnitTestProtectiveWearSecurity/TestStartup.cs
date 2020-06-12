using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ProtectiveWearSecurity.Models;
using ProtectiveWearSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ProtectiveWearSecurity.Services;

namespace XUnitTestProtectiveWearSecurity
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            const string connectionString = "DataSource=:memory:";
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            services.AddDbContext<ProtectiveWearApiDbContext>(options =>
                options.UseNpgsql(connection));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                                                                .AddEntityFrameworkStores<ProtectiveWearApiDbContext>()
                                                                .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, MockEmailSender>();

            services.AddAuthorization();
        }

    }

    
}
