using System.ComponentModel.DataAnnotations;

namespace ProtectiveWearSecurity.Models
{
    /// <summary>
    /// Clase para tomar los datos a identificar.
    /// </summary>
    public class LoginApiResource
    {
        /// <summary>
        /// Permite tomar el valor de un correo electronico
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// Permite tomar el valor de un password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
