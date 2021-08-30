using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class NewRegisterRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
    }
}
