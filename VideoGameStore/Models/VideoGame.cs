using VideoGameStore.Data.Base;
using VideoGameStore.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace VideoGameStore.Models
{
    public class VideoGame : IEntityBase
    {
        public VideoGame ()
        {
            this.Discounts = new HashSet<VideoGame_Discount>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Title is required")]
        [StringLength(50, MinimumLength =1, ErrorMessage ="Title must be between 1 and 50 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Video Game poster URL is required")]
        public string ImageURL { get; set; }

        [Required(ErrorMessage = "Release date is required")]
        public DateTime ReleaseDate { get; set;}

        [Required(ErrorMessage = "Genre is required")]
        public List<GameGenre> GameGenres { get; set; }

        //Releationships:
        public List<Publisher_VideoGame>? Publishers_VideoGames { get; set; }

        [Required(ErrorMessage = "Age rating is required, use 'RP' if rating pending")]
        public GameAgeRating GameAgeRating { get; set; }

        public int DeveloperId { get; set; }
        [ForeignKey("DeveloperId")]

        public Developer? Developer { get; set;}

        public virtual ICollection<VideoGame_Discount> Discounts { get; set; }
    }
}
