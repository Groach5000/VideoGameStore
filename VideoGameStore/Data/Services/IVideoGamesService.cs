using VideoGameStore.Data.Base;
using VideoGameStore.Data.ViewModels;
using VideoGameStore.Models;
using VideoGameStore.Models.searchModels;

namespace VideoGameStore.Data.Services
{
    public interface IVideoGamesService : IEntityBaseRepository<VideoGame>
    {
        // Most now inherited by IEntityBaseRepository

        Task<VideoGame> GetVideoGameByIdAsync(int id);

        Task<NewVideoGameDropdownsVM> GetNewVideoGameDropdownsValuesAsync();

        Task AddNewVideoGameAsync(NewVideoGameVM newVideoGameData);

        Task UpdateVideoGameAsync(NewVideoGameVM videoGameData);

        IEnumerable<VideoGame> GetQueriedVideoGames(IEnumerable<VideoGame> result, 
            VideoGameSearch searchModel, string sortOrder);

        IEnumerable<VideoGame> GetPublisherAndDeveloperQueriedVideoGames(IEnumerable<VideoGame> result,
            int? publisherId, int? developerId);

        Task<IEnumerable<VideoGame>> GetAllVideoGamesAsync();

        IEnumerable<VideoGameVM> GetMultipleVideoGamesVM(IEnumerable<VideoGame> games);

        VideoGameVM GetVideoGameVM(VideoGame game);
    }
}
