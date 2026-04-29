using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class UserIdDto
    {
        [Required]
        public int Id { get; set; }
    }
}
