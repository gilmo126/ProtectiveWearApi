
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
namespace ProtectiveWearProductsApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Propiedad que toma el id del producto asigando por la base de datos.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Propiedad que toma el nombre del producto.
        /// </summary>
        [BsonElement("Nombre")]
        [JsonProperty("Nombre")]
        [Required]
        public string Nombre { get; set; }

        /// <summary>
        /// Propiedad que toma la presentación del producto.
        /// </summary>
        [BsonElement("Presentacion")]
        [Required]
        public string Presentacion { get; set; }

        /// <summary>
        /// Propiedad que toma la descripción del producto.
        /// </summary>
        [BsonElement("Descripcion")]
        [Required]
        public string Descripcion { get; set; }

        /// <summary>
        /// Propiedad que toma el precion del producto.
        /// </summary>
        [BsonElement("Precio")]
        [Display(Name = "Precio($)")]
        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public decimal Precio { get; set; }
        /// <summary>
        /// Propiedad que toma la fecha creacion de producto.
        /// </summary>
        [BsonElement("FechaCreacion")]
        [JsonProperty("FechaCreacion")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTimeOffset FechaCreacion { get; set; }

        /// <summary>
        /// Propiedad que toma la ruta de la imagen asociada al producto.
        /// </summary>
        [BsonElement("ImageUrl")]
        [Display(Name = "Imagen")]
        [DataType(DataType.ImageUrl)]
        [Required]
        public string ImageUrl { get; set; }
    }

}
