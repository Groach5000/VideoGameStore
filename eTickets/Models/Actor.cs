using VideoGameStore.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VideoGameStore.Models
{
    public class Actor : Person, IEntityBase
    {
        [Key]
        public int Id { get; set; }

        // Relationships
        //Added a "?" to List<Actor_Movie>? to make it nullable,
            // when creating an new actor, you don't add the movies releaionship
            // without the "?" creating an actor would always fail the .IsValid check.
        public List<Actor_Movie>? Actors_Movies{ get; set;} 

    }
}
