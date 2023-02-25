using VideoGameStore.Data.Enums;
using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace VideoGameStore.Models
{
    public class NewMovieVM
    {

        public int Id { get; set; }

        [DisplayName("Movie title")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [DisplayName("Movie description")]
        [RequiredAttribute(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [DisplayName("Movie price in $ amount")]
        [Required(ErrorMessage ="Price is required")]
        public double Price { get; set; }


        [DisplayName("Movie poster URL")]
        [Required(ErrorMessage = "Poster URL is required")]
        public string ImageURL { get; set; }

        [DisplayName("Movie start date")]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [DisplayName("Movie end date")]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [DisplayName("Movie category")]
        [Required(ErrorMessage = "Category is required")]
        public MovieCategory MovieCategory { get; set; }

        [Display(Name = "Select actor(s)")]
        [Required(ErrorMessage = "Movie actor(s) is required")]
        public List<int> ActorIds { get; set; }

        [Display(Name = "Select a cinema")]
        [Required(ErrorMessage = "Movie cinema is required")]
        public int CinemaId { get; set; }

        [Display(Name = "Select a producer")]
        [Required(ErrorMessage = "Movie producer is required")]
        public int ProducerId { get;set; }
    }
}
