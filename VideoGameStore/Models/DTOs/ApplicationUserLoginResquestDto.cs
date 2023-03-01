using System.ComponentModel.DataAnnotations;

namespace VideoGameStore.Models.DTOs
{
    public class ApplicationUserLoginResquestDto
    {

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
