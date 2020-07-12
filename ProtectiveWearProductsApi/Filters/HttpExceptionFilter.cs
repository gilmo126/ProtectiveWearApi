using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProtectiveWearProductsApi.Exceptions;
using ProtectiveWearProductsApi.Models;
using System.Collections.Generic;
using System.Net;

/// <summary>
/// Espacion de nombre de los objetos encargados de filtrar las excepciones
/// </summary>
namespace ProtectiveWearProductsApi.Filters
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

            int codeError = 0;
            ErrorMessage errorMessage = null;
            HttpStatusCode statusCode = (HttpStatusCode)500;

            var error = new HttpException();

            context.HttpContext.Response.StatusCode = (int)statusCode;

            if (context.Exception is HttpException)
            {
                error = context.Exception as HttpException;
                codeError = (int)error.ErrorCode;
                #region Region Code errors Http
                switch (codeError)
                {
                    case 500:
                        statusCode = (HttpStatusCode)codeError;
                        error.Messages.Add("A generic error has occurred on the server.");
                        break;
                    case 501:
                        statusCode = (HttpStatusCode)codeError;
                        error.Messages.Add("Server does not support the requested function.");
                        break;
                    case 502:
                        statusCode = (HttpStatusCode)codeError;
                        error.Messages.Add("Proxy server received a bad response from another proxy or the origin server.");
                        break;
                    case 503:
                        statusCode = (HttpStatusCode)codeError;
                        error.Messages.Add("Server is temporarily unavailable, usually due to high load or maintenance.");
                        break;
                    case 504:
                        statusCode = (HttpStatusCode)codeError;
                        error.Messages.Add("Proxy server timed out while waiting for a response from another proxy or the origin server.");
                        break;
                    case 400:
                        statusCode = (HttpStatusCode)codeError;
                        error.Messages.Add("Request could not be understood by the server.");
                        break;
                    case 401:
                        statusCode = (HttpStatusCode)codeError;
                        error.Messages.Add("Requested resource requires authentication.");
                        break;
                    case 403:
                        statusCode = (HttpStatusCode)codeError;
                        error.Messages.Add("Server refuses to fulfill the request.");
                        break;
                    case 404:
                        statusCode = (HttpStatusCode)codeError;
                        error.Messages.Add("Requested resource does not exist on the server.");
                        break;
                    case 405:
                        statusCode = (HttpStatusCode)codeError;
                        error.Messages.Add("that the request method (POST or GET) is not allowed on the requested resource.");
                        break;
                    default:
                        statusCode = (HttpStatusCode)codeError;
                        error.Messages.Add("¡Uuups! something is wrong");
                        error.Messages.Add(context.Exception.Message);
                        break;
                }
                #endregion

                context.HttpContext.Response.StatusCode = (int)statusCode;
            }
            else if (!string.IsNullOrEmpty(context.Exception.ToString()))
            {
                statusCode = (HttpStatusCode)400;
                error.Messages.Add(context.Exception.Message);
                context.HttpContext.Response.StatusCode = (int)statusCode;
            }

            error = new HttpException(error.Messages, statusCode);
            errorMessage = new ErrorMessage();
            errorMessage.code = error.ErrorCode;
            errorMessage.messages = error.Messages;
            
            context.Result = new JsonResult( errorMessage );
            base.OnException(context);          
        
        }
        
    }
}
