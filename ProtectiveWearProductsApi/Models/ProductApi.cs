using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProtectiveWearProductsApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductApi 
    {

        /// <summary>
        /// Identificador de Producto
        /// </summary>
        [BsonElement("Id")]
        [JsonProperty("Id")]
        public string Id { get; set; }
        /// <summary>
        /// Propiedad que toma el nombre del producto.
        /// </summary>
        [BsonElement("Nombre")]
        [JsonProperty("Nombre")]      
        public string Nombre { get; set; }

        /// <summary>
        /// Propiedad que toma la presentación del producto.
        /// </summary>
        [BsonElement("Presentacion")]
        public string Presentacion { get; set; }

        /// <summary>
        /// Propiedad que toma la descripción del producto.
        /// </summary>
        [BsonElement("Descripcion")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Propiedad que toma el precion del producto.
        /// </summary>
        [BsonElement("Precio")]
        [Display(Name = "Precio($)")]
        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public decimal Precio { get; set; }

    }
}
