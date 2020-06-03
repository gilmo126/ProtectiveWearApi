using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ProtectiveWearSecurity.Models
{
    /// <summary>
    /// Clase para serializar los productos consultados.
    /// </summary>
    public class ProductApiModel
    {
        /// <summary>
        /// Propiedad que toma el nombre del producto.
        /// </summary>
        [JsonProperty("Nombre")]
        public string Nombre { get; set; }

        /// <summary>
        /// Propiedad que toma la presentación del producto.
        /// </summary>
        public string Presentacion { get; set; }

        /// <summary>
        /// Propiedad que toma la descripción del producto.
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Propiedad que toma el precion del producto.
        /// </summary>
        [Display(Name = "Precio($)")]
        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public decimal Precio { get; set; }
    }
}
