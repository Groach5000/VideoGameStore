using System.ComponentModel.DataAnnotations;

namespace VideoGameStore.Models
{
    public abstract class Company
    {
        [Display(Name ="Logo")] // for view displaying
        [Required(ErrorMessage = "Logo is required")] //is a required item to add it to the DB
        public string LogoURL { get; set; }

        [Display(Name ="Company Name")]
        [Required(ErrorMessage = "Company name is required")] //is a required item to add it to the DB
        [StringLength(50, MinimumLength =3, ErrorMessage="Full name must be between 3 and 50 charaters.")]
        public string CompanyName { get; set; }
        
        [Display(Name = "About")]
        [Required(ErrorMessage = "About is required")] //is a required item to add it to the DB
        public string About { get; set; }
    }
}
 