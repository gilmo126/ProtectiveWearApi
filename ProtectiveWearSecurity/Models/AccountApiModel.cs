
namespace ProtectiveWearSecurity.Models
{
    /// <summary>
    /// Clase encargada de serializar los datos del usuario al momento de identificarse, como respuesta de la autenticación exitosa junto con el token.
    /// </summary>
    public class AccountApiModel
    {
        /// <summary>
        /// Primer nombre del usuario.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Segundo nombre del usuario.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Emal del usuario.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Identificaición unica del usuario
        /// </summary>
        public string Id { get; set; }

    }
}
