using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VideoGameStore.Data.Enums;

namespace VideoGameStore.Models.searchModels
{
    public class VideoGameSearch
    {
        public int? Id { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }
        public PriceRange? MinPrice { get; set; }

        public PriceRange? MaxPrice { get; set; }

        public GameGenre? GameGenre { get; set; } // TODO:  Make List to allow for multiple genres

        //Releationships:
        public string? Publisher { get; set; }

        public GameAgeRating? GameAgeRating { get; set; }

        public int? DeveloperId { get; set; }
        public string? Developer { get; set; }
    }
}
