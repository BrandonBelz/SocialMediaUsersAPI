using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class UserPublicDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = String.Empty;

        [Required]
        public string Biography { get; set; } = String.Empty;

        [Required]
        public DateTime JoinedOn { get; set; } = DateTime.Now;
    }
}
