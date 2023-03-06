using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Policy;
using VideoGameStore.Data;
using VideoGameStore.Data.Services;

namespace VideoGameStore.Models.searchModels
{
    public class VideoGamesBusinessLogic
    {
        AppDbContext _context;
        public VideoGamesBusinessLogic(AppDbContext context)
        {
            _context= context;
        }

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
