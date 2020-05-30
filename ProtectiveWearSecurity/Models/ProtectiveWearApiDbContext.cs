using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProtectiveWearSecurity.Models;

namespace ProtectiveWearSecurity.Models
{
    /// <summary>
    /// Clase instanciada de DBContext, para iniciar la cadena de conexion y el repositorio a consultar.
    /// </summary>
    public class ProtectiveWearApiDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="options">Opciones de conexion.</param>
        public ProtectiveWearApiDbContext(DbContextOptions<ProtectiveWearApiDbContext> options)
        : base(options)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        /// <summary>
        /// Constructor vacio.
        /// </summary>
        public ProtectiveWearApiDbContext() { 
        }
    }
}
