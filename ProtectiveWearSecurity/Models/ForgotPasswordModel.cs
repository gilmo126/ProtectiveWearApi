using System.ComponentModel.DataAnnotations;

namespace ProtectiveWearSecurity.Models
{

    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }

}
