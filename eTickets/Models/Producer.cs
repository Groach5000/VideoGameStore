using VideoGameStore.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace VideoGameStore.Models
{
    public class Producer : Person, IEntityBase
    {
        [Key]
        public int Id { get; set; }


        // Relationships
        //Added a "?" to List<Actor_Movie>? to make it nullable,
        // when creating an new actor, you don't add the movies releaionship
        // without the "?" creating an actor would always fail the .IsValid check.
        public List<Movie>? Movies { get; set; }
    }
}
