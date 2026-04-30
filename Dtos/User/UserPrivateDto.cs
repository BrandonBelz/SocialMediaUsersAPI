using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class UserPrivateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = String.Empty;

        [Required]
        public string Email { get; set; } = String.Empty;

        [Required]
        public string Biography { get; set; } = String.Empty;

        [Required]
        public DateTime JoinedOn { get; set; } = DateTime.Now;

        [Required]
        public string? ProfilePicUrl { get; set; }
    }
}
