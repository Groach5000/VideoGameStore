using VideoGameStore.Data.Base;
using VideoGameStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using VideoGameStore.Data.ViewModels;

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
                .Include(p => p.Developer)
                .Include(am => am.Publishers_VideoGames)
                .ThenInclude(a => a.Publisher)
                .FirstOrDefaultAsync(n => n.Id == id);

            return videoGameDetails;
        }

        public async Task<NewVideoGameDropdownsVM> GetNewVideoGameDropdownsValuesAsync()
        {
            var result = new NewVideoGameDropdownsVM()
            {
                Publishers = await _context.Publishers.OrderBy(a => a.CompanyName).ToListAsync(),
                Developers = await _context.Developers.OrderBy(p => p.CompanyName).ToListAsync(),
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
    }
}
