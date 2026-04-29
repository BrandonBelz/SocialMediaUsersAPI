using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class UpdateUserRequestDto
    {
        [RegularExpression(
            @"^[a-zA-Z0-9_-]+$",
            ErrorMessage = "Username can only contain letters, numbers, underscores, and hyphens"
        )]
        [StringLength(50, MinimumLength = 1)]
        public string? Username { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(300, MinimumLength = 0)]
        public string? Biography { get; set; }
    }
}
