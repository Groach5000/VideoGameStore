using VideoGameStore.Data.Base;
using VideoGameStore.Data.ViewModels;
using VideoGameStore.Models;

namespace VideoGameStore.Data.Services
{
    public interface IVideoGamesService : IEntityBaseRepository<VideoGame>
    {
        // Most now inherited by IEntityBaseRepository

        Task<VideoGame> GetVideoGameByIdAsync(int id);

        Task<NewVideoGameDropdownsVM> GetNewVideoGameDropdownsValuesAsync();

        Task AddNewVideoGameAsync(NewVideoGameVM newVideoGameData);

        Task UpdateVideoGameAsync(NewVideoGameVM videoGameData);
    }
}
