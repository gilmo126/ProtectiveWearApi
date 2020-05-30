using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace ProtectiveWearSecurity.Services
{
    /// <summary>
    /// Clase encargada de filtrar las peticiones cuyas autorizaciones esten individualizadas.
    /// </summary>
    public class AuthOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Metodo instanciado de IOperationFilter con override, para permitir los casos necesario de autorizar.
        /// </summary>
        /// <param name="operation">Nombre de la operacion a filtrar.</param>
        /// <param name="context">Contexto o ambiemte de la operacion filtrada.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            var isAuthorized = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                              context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (!isAuthorized) return;

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            var jwtbearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [ jwtbearerScheme ] = new string [] { }
                    }
                };
        }
    }
}
