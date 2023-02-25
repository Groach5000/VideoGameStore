using VideoGameStore.Data.Base;
using VideoGameStore.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoGameStore.Models
{
    public class Movie : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Title is required")]
        [StringLength(50, MinimumLength =1, ErrorMessage ="Title must be between 1 and 50 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Movie poster URL is required")]
        public string ImageURL { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set;}

        [Required(ErrorMessage = "End Date is required")]
        public DateTime EndDate { get; set;}

        [Required(ErrorMessage = "Movie category is required")]
        public MovieCategory MovieCategory { get; set; }

        //Releationships:
        public List<Actor_Movie>? Actors_Movies { get; set; }

        public int CinemaId { get; set; }
        [ForeignKey("CinemaId")]
        public Cinema Cinema { get; set; }

        public int ProducerId { get; set; }
        [ForeignKey("ProducerId")]

        public Producer Producer { get; set;}

    }
}
