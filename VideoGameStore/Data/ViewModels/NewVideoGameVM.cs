using VideoGameStore.Data.Enums;
using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace VideoGameStore.Models
{
    public class NewVideoGameVM
    {

        public int Id { get; set; }

        [DisplayName("Video game title")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [DisplayName("Video game description")]
        [RequiredAttribute(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [DisplayName("Video game price in $ amount")]
        [Required(ErrorMessage ="Price is required")]
        public double Price { get; set; }


        [DisplayName("Video game poster URL")]
        [Required(ErrorMessage = "Poster URL is required")]
        public string ImageURL { get; set; }

        [DisplayName("Video game release date")]
        [Required(ErrorMessage = "Release date is required")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Video game genre")]
        [Required(ErrorMessage = "Genre is required")]
        public List<GameGenre> GameGenres { get; set; }

        [DisplayName("Video game age rating")]
        [Required(ErrorMessage = "Age rating is required, use 'RP' if rating pending")]
        public GameAgeRating GameAgeRating { get; set; }

        [Display(Name = "Select publisher(s)")]
        [Required(ErrorMessage = "Video game publisher(s) is required")]
        public List<int> PublisherIds { get; set; }

        [Display(Name = "Select a developer")]
        [Required(ErrorMessage = "Video game developer is required")]
        public int DeveloperId { get;set; }
    }
}
