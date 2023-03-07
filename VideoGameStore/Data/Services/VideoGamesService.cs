using VideoGameStore.Data.Base;
using VideoGameStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using VideoGameStore.Data.ViewModels;
using VideoGameStore.Models.searchModels;

namespace VideoGameStore.Data.Services
{
    public class VideoGamesService : EntityBaseRepository<VideoGame>, IVideoGamesService
    {
        private readonly AppDbContext _context;
        
        public VideoGamesService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewVideoGameAsync(NewVideoGameVM newVideoGameData)
        {
            var newVideoGame = new VideoGame()
            {
                Title = newVideoGameData.Title,
                Description = newVideoGameData.Description,
                Price = newVideoGameData.Price,
                ReleaseDate = newVideoGameData.ReleaseDate,
                GameGenre = newVideoGameData.GameGenres,
                GameAgeRating = newVideoGameData.GameAgeRating,
                DeveloperId = newVideoGameData.DeveloperId,
                ImageURL = newVideoGameData.ImageURL
            };

            // Add the game to the game table
            await _context.VideoGames.AddAsync(newVideoGame);
            await _context.SaveChangesAsync();

            // Add the game reference to the Publisher's resume via Publisher Video Game
            foreach (var publisherId in newVideoGameData.PublisherIds)
            {
                var newPublisherVideoGame = new Publisher_VideoGame()
                {
                    VideoGameId = newVideoGame.Id,
                    PublisherId = publisherId
                };
                await _context.Publishers_VideoGames.AddAsync(newPublisherVideoGame);
            }
            await _context.SaveChangesAsync();

        }

        public async Task<VideoGame> GetVideoGameByIdAsync(int id)
        {
            var videoGameDetails = await _context.VideoGames
                .Include(d => d.Developer)
                .Include(pvg => pvg.Publishers_VideoGames)
                .ThenInclude(p => p.Publisher)
                .FirstOrDefaultAsync(n => n.Id == id);

            return videoGameDetails;
        }

        public async Task<NewVideoGameDropdownsVM> GetNewVideoGameDropdownsValuesAsync()
        {
            var result = new NewVideoGameDropdownsVM()
            {
                Publishers = await _context.Publishers.OrderBy(p => p.CompanyName).ToListAsync(),
                Developers = await _context.Developers.OrderBy(d => d.CompanyName).ToListAsync(),
            };

            return result;
        }

        public async Task UpdateVideoGameAsync(NewVideoGameVM videoGameData)
        {
            var videoGame = await _context.VideoGames.FirstOrDefaultAsync(n => n.Id == videoGameData.Id);
            
            if (videoGame != null)
            {
                videoGame.Title = videoGameData.Title;
                videoGame.Description = videoGameData.Description;
                videoGame.Price = videoGameData.Price;
                videoGame.ReleaseDate = videoGameData.ReleaseDate;
                videoGame.GameGenre = videoGameData.GameGenres;
                videoGame.GameAgeRating = videoGameData.GameAgeRating;
                videoGame.DeveloperId = videoGameData.DeveloperId;
                videoGame.ImageURL = videoGameData.ImageURL;

                await _context.SaveChangesAsync();
            };

            // Remove all the old Publister before updating with the new list of Publisters
            var oldPublishersList = _context.Publishers_VideoGames
                .Where(n => n.VideoGameId == videoGameData.Id).ToList();
            _context.Publishers_VideoGames.RemoveRange(oldPublishersList);

            await _context.SaveChangesAsync();

            // Add the game reference to the Publisher's resume via Publisher Video Game
            foreach (var publisherId in videoGameData.PublisherIds)
            {
                var newPublisherVideoGame = new Publisher_VideoGame()
                {
                    VideoGameId = videoGameData.Id,
                    PublisherId = publisherId
                };
                await _context.Publishers_VideoGames.AddAsync(newPublisherVideoGame);
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        ///     Performs queries on video game list to filter by value in the search model. 
        /// </summary>
        /// <param name="result"> List of queriable videogames </param>
        /// <param name="searchModel"> Filter criteria of parameters that can be null or to be searched for.</param>
        /// <param name="sortOrder"> Order to sort the list by (Asc or Desc)</param>
        /// <returns>Sorted List of filtered videogames sorted by sortOrder</returns>
        public IEnumerable<VideoGame> GetQueriedVideoGames(IEnumerable<VideoGame> result, VideoGameSearch searchModel, string sortOrder)
        {
            if (searchModel != null)
            {
                if (searchModel.Id.HasValue)
                    result = result.Where(x => x.Id == searchModel.Id).ToList();
                if (!string.IsNullOrEmpty(searchModel.Title) || !string.IsNullOrEmpty(searchModel.Description))
                    result = result.Where(n => n.Title.Contains(searchModel.Title, StringComparison.CurrentCultureIgnoreCase) ||
                                n.Description.Contains(searchModel.Description, StringComparison.CurrentCultureIgnoreCase)
                                ).ToList();
                if (searchModel.MaxPrice.HasValue)
                    result = result.Where(x => x.Price <= (int)searchModel.MaxPrice).ToList();
                if (searchModel.MinPrice.HasValue)
                    result = result.Where(x => x.Price >= (int)searchModel.MinPrice).ToList();
                if (searchModel.GameGenre.HasValue)
                    result = result.Where(x => x.GameGenre == searchModel.GameGenre).ToList();
                if (searchModel.GameAgeRating.HasValue)
                    result = result.Where(x => x.GameAgeRating == searchModel.GameAgeRating).ToList();
            }

            switch (sortOrder)
            {
                case "title_desc":
                    result = result.OrderByDescending(n => n.Title);
                    break;

                default:
                    result = result.OrderBy(n => n.Title);
                    break;
            }

            return result;
        }

        /// <summary>
        ///     Performs queries on video game list to filter by publisher and developer.
        /// </summary>
        /// <param name="result"> List of queriable videogames </param>
        /// <param name="publisherId"> Publisher Id to search for, can be null</param>
        /// <param name="developerId"> Developer Id to search for, can be null</param>
        /// <returns>Sorted List of filtered videogames</returns>
        public IEnumerable<VideoGame> GetPublisherAndDeveloperQueriedVideoGames(IEnumerable<VideoGame> result,
            int? publisherId, int? developerId)
        {
            if (publisherId != null && developerId != null)
            {
                // Get all games with matching publisher
                IEnumerable<VideoGame> gamesWithPublisher = from vgp in _context.Publishers_VideoGames
                                                            join g in _context.VideoGames on vgp.VideoGameId equals g.Id
                                                            join p in _context.Publishers on vgp.PublisherId equals p.Id
                                                            where p.Id == (int)publisherId
                                                            select g;

                // Get all games with matching developer
                IEnumerable<VideoGame> gamesWithDeveloper = from gwp in _context.VideoGames
                                                            join d in _context.Developers on gwp.DeveloperId equals d.Id
                                                            where d.Id == (int)developerId
                                                            select gwp;

                // Merge lists and only return matching values. 
                result = gamesWithPublisher.Intersect(gamesWithDeveloper).ToList();
            }
            else if (publisherId != null)
            {
                result = from vgp in _context.Publishers_VideoGames
                         join g in _context.VideoGames on vgp.VideoGameId equals g.Id
                         join p in _context.Publishers on vgp.PublisherId equals p.Id
                         where p.Id == (int)publisherId
                         select g;
            }
            else if (developerId != null)
            {
                result = from gtf in result
                         join d in _context.Developers on gtf.DeveloperId equals d.Id
                         where d.Id == (int)developerId
                         select gtf;
            }

            return result;
        }
    }
}
