using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class JwtDto
    {
        [Required]
        public string JwtKey { get; set; } = null!;
    }
}
