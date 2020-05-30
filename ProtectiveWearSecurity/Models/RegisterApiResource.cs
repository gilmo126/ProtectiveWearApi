using System.ComponentModel.DataAnnotations;

namespace ProtectiveWearSecurity.Models
{
    /// <summary>
    /// Clase encargada de tomar los registros del usuario a registrar.
    /// </summary>
    public class RegisterApiResource
    {
        /// <summary>
        /// Primer nombre del usuario.
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        /// <summary>
        /// Segundo nombre del usuario.
        /// </summary>
        [Required]
        public string LastName { get; set; }
        /// <summary>
        /// Email del Usuario.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// Password del usuario, con un maximo de 6 posiciones.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe estar entre {2} and un maximo {1} caracteres de largo.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// Confirmaación del password.
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La confirmacion del password no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
