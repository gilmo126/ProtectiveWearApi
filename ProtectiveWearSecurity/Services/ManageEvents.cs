using ProtectiveWearSecurity.Exceptions;
using System.Collections.Generic;
using System.Net;

namespace ProtectiveWearSecurity.Services
{
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
                throw new HttpException(new List<string> { "Producto no encontrado" }, HttpStatusCode.NotFound);
            }
        }
    }
}
