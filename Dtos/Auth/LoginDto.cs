using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = String.Empty;

        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
