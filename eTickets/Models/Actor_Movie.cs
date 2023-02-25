using System.ComponentModel.DataAnnotations.Schema;

namespace VideoGameStore.Models
{
    public class Actor_Movie
    {
        // Link MovieId and ActorId as 1 to many in the AppDbContext.cs file (not as a Foreign key like we did in the movie.cs class)
        public int MovieId { get; set; }
        public Movie Moive { get; set; }
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        
    }
}
