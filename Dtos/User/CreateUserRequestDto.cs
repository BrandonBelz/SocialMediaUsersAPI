using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class CreateUserRequestDto
    {
        [Required]
        [RegularExpression(
            @"^[a-zA-Z0-9_-]+$",
            ErrorMessage = "Username can only contain letters, numbers, underscores, and hyphens"
        )]
        [StringLength(50, MinimumLength = 1)]
        public string Username { get; set; } = String.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Password { get; set; } = String.Empty;
    }
}
