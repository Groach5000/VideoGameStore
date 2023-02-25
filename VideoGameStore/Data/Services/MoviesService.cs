using VideoGameStore.Data.Base;
using VideoGameStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using VideoGameStore.Data.ViewModels;

namespace VideoGameStore.Data.Services
{
    public class MoviesService : EntityBaseRepository<Movie>, IMoviesService
    {
        private readonly AppDbContext _context;
        
        public MoviesService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewMovieAsync(NewMovieVM newMovieData)
        {
            var newMovie = new Movie()
            {
                Title = newMovieData.Title,
                Description = newMovieData.Description,
                Price = newMovieData.Price,
                StartDate = newMovieData.StartDate,
                EndDate = newMovieData.EndDate,
                MovieCategory = newMovieData.MovieCategory,
                ProducerId = newMovieData.ProducerId,
                CinemaId = newMovieData.CinemaId,
                ImageURL = newMovieData.ImageURL
            };

            // Add the movie to the movie table
            await _context.Movies.AddAsync(newMovie);
            await _context.SaveChangesAsync();

            // Add the movie reference to the Actor's resume via Movie Actor
            foreach (var actorId in newMovieData.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = newMovie.Id,
                    ActorId = actorId
                };
                await _context.Actors_Movies.AddAsync(newActorMovie);
            }
            await _context.SaveChangesAsync();

        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movieDetails = await _context.Movies
                .Include(c => c.Cinema)
                .Include(p => p.Producer)
                .Include(am => am.Actors_Movies)
                .ThenInclude(a => a.Actor)
                .FirstOrDefaultAsync(n => n.Id == id);

            return movieDetails;
        }

        public async Task<NewMovieDropdownsVM> GetNewMovieDropdownsValuesAsync()
        {
            var result = new NewMovieDropdownsVM()
            {
                Actors = await _context.Actors.OrderBy(a => a.FullName).ToListAsync(),
                Producers = await _context.Producers.OrderBy(p => p.FullName).ToListAsync(),
                Cinemas = await _context.Cinemas.OrderBy(c => c.Name).ToListAsync()
            };

            return result;
        }

        public async Task UpdateMovieAsync(NewMovieVM movieData)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(n => n.Id == movieData.Id);
            
            if (movie != null)
            {
                movie.Title = movieData.Title;
                movie.Description = movieData.Description;
                movie.Price = movieData.Price;
                movie.StartDate = movieData.StartDate;
                movie.EndDate = movieData.EndDate;
                movie.MovieCategory = movieData.MovieCategory;
                movie.ProducerId = movieData.ProducerId;
                movie.CinemaId = movieData.CinemaId;
                movie.ImageURL = movieData.ImageURL;

                await _context.SaveChangesAsync();
            };

            // Remove all the old actors before updating with the new list of actors
            var oldActorsList = _context.Actors_Movies.Where(n => n.MovieId == movieData.Id).ToList();
            _context.Actors_Movies.RemoveRange(oldActorsList);

            await _context.SaveChangesAsync();

            // Add the movie reference to the Actor's resume via Movie Actor
            foreach (var actorId in movieData.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = movieData.Id,
                    ActorId = actorId
                };
                await _context.Actors_Movies.AddAsync(newActorMovie);
            }
            await _context.SaveChangesAsync();
        }





    }
}
