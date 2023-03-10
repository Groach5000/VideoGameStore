using VideoGameStore.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace VideoGameStore.Models
{
    public class Developer : Company, IEntityBase
    {

        // Relationships
        //Added a "?" to List<VideoGame>? to make it nullable,
        // when creating an new Developer, you don't add the video games releaionship
        // without the "?" creating an developer would always fail the .IsValid check.
        public List<VideoGame>? VideoGames { get; set; }
    }
}
