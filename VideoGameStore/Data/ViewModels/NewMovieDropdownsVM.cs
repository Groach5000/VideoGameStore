using VideoGameStore.Models;

namespace VideoGameStore.Data.ViewModels
{
    public class NewMovieDropdownsVM
    {
        public List<Producer> Producers { get; set; }
        public List<Cinema> Cinemas { get; set; }
        public List<Actor> Actors { get; set; }

        public NewMovieDropdownsVM()
        {
            Producers = new List<Producer>();
            Cinemas = new List<Cinema>();
            Actors = new List<Actor>();
        }

        
    }
}
