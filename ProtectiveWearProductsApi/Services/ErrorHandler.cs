using ProtectiveWearProductsApi.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProtectiveWearProductsApi.Services
{
    /// <summary>
    /// Clase encargada de manajear errores desplegados en procesos.
    /// </summary>
    public static class ErrorHandler
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
