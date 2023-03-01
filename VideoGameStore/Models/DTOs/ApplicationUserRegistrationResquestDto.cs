using System.ComponentModel.DataAnnotations;

namespace VideoGameStore.Models.DTOs
{
    public class ApplicationUserRegistrationResquestDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }    
    }
}
