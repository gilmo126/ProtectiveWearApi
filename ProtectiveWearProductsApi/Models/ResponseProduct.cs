using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProtectiveWearProductsApi.Models
{
    public class ResponseProduct
    {
        /// <summary>
        /// Propiedad que toma el id del producto asigando por la base de datos.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Propiedad que toma la fecha creacion de producto.
        /// </summary>
        [BsonElement("FechaCreacion")]
        [JsonProperty("FechaCreacion")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTimeOffset FechaCreacion { get; set; }

        /// <summary>
        /// En caso de error se devuelve la excepcion
        /// </summary>
        public ErrorMessage ErrorMessage { get; set; }

    }
}
