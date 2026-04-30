using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class UserMinimizedDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = String.Empty;

        [Required]
        public string? ProfilePicUrl { get; set; }
    }
}
