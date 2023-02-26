using System.ComponentModel.DataAnnotations.Schema;

namespace VideoGameStore.Models
{
    public class Publisher_VideoGame
    {
        // Link VideoGameId and PublisherId as 1 to many in the AppDbContext.cs file (not as a Foreign key like in the videogame.cs class)
        public int VideoGameId { get; set; }
        public VideoGame VideoGame { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        
    }
}
