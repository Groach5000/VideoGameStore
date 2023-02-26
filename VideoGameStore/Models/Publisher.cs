using VideoGameStore.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VideoGameStore.Models
{
    public class Publisher : Company, IEntityBase
    {
        [Key]
        public int Id { get; set; }

        // Relationships
        //Added a "?" to List<Publisher_VideoGame>? to make it nullable,
        // when creating an new publisher, you don't add the games releaionship
        // without the "?" creating an publisher would always fail the .IsValid check.
        public List<Publisher_VideoGame>? Publishers_VideoGames{ get; set;} 

    }
}
