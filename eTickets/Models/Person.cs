using System.ComponentModel.DataAnnotations;

namespace VideoGameStore.Models
{
    public abstract class Person
    {
        [Display(Name ="Profile Picture URL")] // for view displaying
        [Required(ErrorMessage = "Picture is required")] //is a required item to add it to the DB
        public string ProfilePictureURL { get; set; }

        [Display(Name ="Full Name")]
        [Required(ErrorMessage = "Full name is required")] //is a required item to add it to the DB
        [StringLength(50, MinimumLength =3, ErrorMessage="Full name must be between 3 and 50 charaters.")]
        public string FullName { get; set; }
        
        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography is required")] //is a required item to add it to the DB
        public string Bio { get; set; }
    }
}
 