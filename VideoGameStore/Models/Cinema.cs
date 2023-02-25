using VideoGameStore.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace VideoGameStore.Models
{
    public class Cinema : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Cinema Logo")]
        [Required(ErrorMessage = "Logo is required")]
        public string Logo { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 charaters.")]
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Descripition is required")]
        public string Description { get; set; }

        // Relationships:
        public List<Movie>? Movies { get; set; }

    }
}
