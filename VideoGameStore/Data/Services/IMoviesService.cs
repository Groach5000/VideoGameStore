using VideoGameStore.Data.Base;
using VideoGameStore.Data.ViewModels;
using VideoGameStore.Models;

namespace VideoGameStore.Data.Services
{
    public interface IMoviesService : IEntityBaseRepository<Movie>
    {
        // All now inherited by IEntityBaseRepository

        Task<Movie> GetMovieByIdAsync(int id);

        Task<NewMovieDropdownsVM> GetNewMovieDropdownsValuesAsync();

        Task AddNewMovieAsync(NewMovieVM newMovieData);

        Task UpdateMovieAsync(NewMovieVM movieData);
    }
}
