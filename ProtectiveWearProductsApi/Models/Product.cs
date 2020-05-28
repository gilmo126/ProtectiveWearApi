
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace ProtectiveWearProductsApi.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Nombre")]
        [JsonProperty("Nombre")]
        [Required]
        public string Nombre { get; set; }

        [BsonElement("Presentacion")]
        [Required]
        public string Presentacion { get; set; }

        [BsonElement("Descripcion")]
        [Required]
        public string Descripcion { get; set; }

        [BsonElement("Precio")]
        [Display(Name = "Precio($)")]
        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public decimal Precio { get; set; }

        [BsonElement("ImageUrl")]
        [Display(Name = "Imagen")]
        [DataType(DataType.ImageUrl)]
        [Required]
        public string ImageUrl { get; set; }
    }

}
