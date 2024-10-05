using System.ComponentModel.DataAnnotations;

namespace AppLogin.DTOs
{
    public class RegisterDTO : LoginDTO
    {
        [Required]
        public string? Name { get; set; }

        [Required, Compare(nameof(Password)), DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        public string? Role { get; set; }
    }
}
