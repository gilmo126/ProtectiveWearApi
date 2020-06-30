using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ProtectiveWearSecurity.Exceptions;
using System.Collections.Generic;
using System.Net;

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
            try
            {

                Database.Migrate();

            }
            catch (NpgsqlException ex)
            {
                var Error = new List<string> {
                    ex.Message                    

                };
                throw new HttpException(Error, HttpStatusCode.BadRequest);
            }
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
        public ProtectiveWearApiDbContext()
        {
        }
    }
}
