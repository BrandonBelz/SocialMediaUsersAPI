using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class CreateUserRequestDto
    {
        [Required]
        public string Username { get; set; } = String.Empty;

        [Required]
        public string Email { get; set; } = String.Empty;

        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
