using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using VideoGameStore.Data;
using VideoGameStore.Data.Services;

namespace VideoGameStore.Models.searchModels
{
    public class VideoGamesBusinessLogic
    {
        private readonly IVideoGamesService _service;
        public VideoGamesBusinessLogic(IVideoGamesService service)
        {
            _service = service;
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
                    result = result.Where(x => x.Price <= searchModel.MaxPrice).ToList();
                if (searchModel.MinPrice.HasValue)
                    result = result.Where(x => x.Price >= searchModel.MinPrice).ToList();
                if (searchModel.GameGenre.HasValue)
                    result = result.Where(x => x.GameGenre == searchModel.GameGenre).ToList();
                if (searchModel.GameGenre.HasValue)
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

    }
}
