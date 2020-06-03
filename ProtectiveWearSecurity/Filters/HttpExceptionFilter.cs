using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProtectiveWearSecurity.Exceptions;
using System.Collections.Generic;
using System.Net;

/// <summary>
/// Espacion de nombre de los objetos encargados de filtrar las excepciones
/// </summary>
namespace ProtectiveWearSecurity.Filters
{
    /// <summary>
    /// Calse encargada de filtrar las peticiones http y devolver aquellas no exitosas, con algun codigo de error.
    /// </summary>
    public class HttpExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Encargado de filtrar las exceptiones a traves de peticiones HTTP.
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            var error = new HttpException(
                new List<string> { "Ayyyy! Internal Server Error", context.Exception.Message },
                HttpStatusCode.InternalServerError);

            context.HttpContext.Response.StatusCode = 500;

            if (context.Exception is HttpException)
            {
                error = context.Exception as HttpException;
                context.HttpContext.Response.StatusCode = (int)error.ErrorCode;

            }

            context.Result = new JsonResult(new { error = error.Messages, code = error.ErrorCode });
            base.OnException(context);

        }

    }
}
