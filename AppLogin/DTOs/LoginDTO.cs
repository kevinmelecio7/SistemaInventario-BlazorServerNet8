using System.ComponentModel.DataAnnotations;

namespace AppLogin.DTOs
{
    public class LoginDTO
    {
        //[Required, DataType(DataType.EmailAddress), EmailAddress]
        public string? Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
