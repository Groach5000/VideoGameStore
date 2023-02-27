using VideoGameStore.Data.Cart;
using Microsoft.AspNetCore.Mvc;
using VideoGameStore.Data.Services;

namespace VideoGameStore.Data.ViewComponents
{
    public class FilteredVideoGames : ViewComponent
    {
        private readonly IVideoGamesService _service;

        public FilteredVideoGames(IVideoGamesService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allVideoGames = await _service.GetAllAsync();

            var searchResults = allVideoGames.OrderBy(n => n.Title).ToList();

            return View(searchResults);
        }

    }
}
