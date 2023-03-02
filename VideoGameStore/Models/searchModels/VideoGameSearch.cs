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

        public double? MaxPrice { get; set; }

        public double? MinPrice { get; set; }

        public GameGenre? GameGenre { get; set; } // TODO:  Make List to allow for multiple genres

        //Releationships:
        public List<Publisher_VideoGame>? Publishers_VideoGames { get; set; }

        public GameAgeRating? GameAgeRating { get; set; }

        public int? DeveloperId { get; set; }
        public Developer? Developer { get; set; }
    }
}
