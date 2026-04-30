using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class UploadProfilePicDto
    {
        [Required]
        public IFormFile ProfilePic { get; set; } = null!;
    }
}
