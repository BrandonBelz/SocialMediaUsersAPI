using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class Error409Dto
    {
        [Required]
        public string Message { get; set; }

        public Error409Dto(string message)
        {
            Message = message;
        }
    }
}
