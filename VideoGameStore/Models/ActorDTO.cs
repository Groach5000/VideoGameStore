using VideoGameStore.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VideoGameStore.Models
{
    public class ActorDTO : Person, IEntityBase
    {
        [Key]
        public int Id { get; set; }

        // Relationships
        //Added a "?" to List<Actor_Movie>? to make it nullable,
            // when creating an new actor, you don't add the movies releaionship
            // without the "?" creating an actor would always fail the .IsValid check.
        public List<Actor_Movie>? Actors_Movies{ get; set;}

        public string ShortBio { get; set; }

        public ActorDTO(int id, string profilePictureURL, string fullName, string bio)
        {
            Id= id;
            ProfilePictureURL= profilePictureURL;
            FullName= fullName;
            Bio= bio;

            ShortBio = bio.Length <= 10 ? bio : bio.Substring(0, 10);

        }

        public ActorDTO(Actor a)
        {
            Id = a.Id;
            ProfilePictureURL = a.ProfilePictureURL;
            FullName = a.FullName;
            Bio = a.Bio;

            ShortBio = a.Bio.Length <= 10 ? a.Bio : a.Bio.Substring(0, 10);

        }

    }
}
