using System.ComponentModel.DataAnnotations;

namespace AppLogin.DTOs
{
    public class LogoutDTO
    {
        [Required]
        public string? Email { get; set; }
    }
}
