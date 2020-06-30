using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProtectiveWearSecurity.Models
{
    /// <summary>
    /// Clase para tomar los datos del producto a recuperar.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Propiedad que toma el id del producto asigando por la base de datos.
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Propiedad que toma el nombre del producto.
        /// </summary>
        [JsonProperty("Nombre")]
        [Required]
        public string Nombre { get; set; }

        /// <summary>
        /// Propiedad que toma la presentación del producto.
        /// </summary>        
        [Required]
        public string Presentacion { get; set; }

        /// <summary>
        /// Propiedad que toma la descripción del producto.
        /// </summary>
        [Required]
        public string Descripcion { get; set; }

        /// <summary>
        /// Propiedad que toma el precion del producto.
        /// </summary>
        [Display(Name = "Precio($)")]
        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public decimal Precio { get; set; }
        /// <summary>
        /// Propiedad que toma la fecha creacion de producto.
        /// </summary>
        [JsonProperty("FechaCreacion")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTimeOffset FechaCreacion { get; set; }


        /// <summary>
        /// Propiedad que toma la ruta de la imagen asociada al producto.
        /// </summary>
        [Display(Name = "Imagen")]
        [DataType(DataType.ImageUrl)]
        [Required]
        public string ImageUrl { get; set; }
    }
}
