using VideoGameStore.Data.Enums;
using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using System.Data.SqlTypes;

namespace VideoGameStore.Models
{
    public class VideoGameVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageURL { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<GameGenre> GameGenres { get; set; }
        public GameAgeRating GameAgeRating { get; set; }
        public List<int> PublisherIds { get; set; }
        public int DeveloperId { get;set; }
        public string? Discount { get; set; }
        public double? DiscountedPrice { get; set; }

    }
}
