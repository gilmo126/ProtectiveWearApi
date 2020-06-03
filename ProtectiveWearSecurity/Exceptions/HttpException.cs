using System;
using System.Collections.Generic;
using System.Net;
/// <summary>
/// NameSpace que alberga los objetos encargado de manejar las execpciones.
/// </summary>
namespace ProtectiveWearSecurity.Exceptions
{
    /// <summary>
    /// Clase encargada de manejar las excepciones de error en Http asociadas a los endpoints
    /// </summary>
    public class HttpException : Exception
    {
        /// <summary>
        /// Propiedad que toma un codigo de error de tipo Http
        /// </summary>
        public HttpStatusCode ErrorCode { get; internal set; }

        /// <summary>
        /// propiedad que almacena mensaje de errores.
        /// </summary>
        public List<string> Messages { get; internal set; }

        /// <summary>
        /// Constructor de la clase que por default toma una lista de mensajes de errores.
        /// </summary>
        public HttpException()
        {
            Messages = new List<string>();
        }

        /// <summary>
        /// Retorna un codido error Http con el que se disponga.
        /// </summary>
        /// <param name="errorCode">Codigo error Http</param>
        public HttpException(HttpStatusCode errorCode = HttpStatusCode.InternalServerError) : this()
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Retorna una lista de errores junto con el codigo de error que disponga el uso.
        /// </summary>
        /// <param name="messages">listado de mensajes</param>
        /// <param name="errorCode">Codigo error Http</param>
        public HttpException(List<string> messages, HttpStatusCode errorCode = HttpStatusCode.InternalServerError)
        {
            Messages = messages ?? new List<string>();
            ErrorCode = errorCode;
        }
    }
}
