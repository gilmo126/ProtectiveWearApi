using ProtectiveWearSecurity.Exceptions;
using System.Collections.Generic;
using System.Net;

namespace ProtectiveWearSecurity.Services
{
    /// <summary>
    /// Clase encargada de realizar eventos o tomarlos.
    /// </summary>
    public static class ManageEvents
    {
        /// <summary>
        /// Verifica si un objeto buscado o consultado es Nulo
        /// </summary>
        /// <param name="model">retorna una exception de tipo httoCode</param>
        public static void CheckIsNotNull(this object model)
        {
            if (model == null)
            {
                throw new HttpException(new List<string> { "Error al prcesar petición" }, HttpStatusCode.NotFound);
            }
        }
    }
}
