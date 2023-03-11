using VideoGameStore.Data.Services;
using VideoGameStore.Data.Static;
using VideoGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VideoGameStore.Models.searchModels;
using VideoGameStore.Data.Enums;
using VideoGameStore.Data;

namespace VideoGameStore.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class VideoGamesController : Controller

    {
        private readonly IVideoGamesService _service;
        private readonly IPublishersService _publisherService;
        private readonly IDevelopersService _developerService;

        private const int numberOfFeaturedItems = 3;

        public VideoGamesController(IVideoGamesService service, IPublishersService publisherService, 
            IDevelopersService developersService)
        {
            _service = service;
            _publisherService = publisherService;
            _developerService = developersService;
        }

        /// <summary>
        ///     Displays home page and games. A filter can be applied by the search bar
        /// </summary>
        /// <param name="sortOrder"> Sort order either Asc or Desc</param>
        /// <param name="currentFilter"> Current text filter applied by the previous navbar search to be updated/used. </param>
        /// <param name="searchString"> Current text filter applied by the navbar search box. </param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "" : "title_desc";
            ViewBag.ShowFeatured = false;

            ViewBag.MinPrice = PriceRange.Free;
            ViewBag.MaxPrice = PriceRange.NoMax;
            ViewBag.GameAgeRating = null;
            ViewBag.GameGenre = null;
            ViewBag.Publisher = null;
            ViewBag.Developer = null;
            ViewBag.previousPublisherDescription = "";
            ViewBag.previousDeveloperDescription = "";


            var videoGameDropdownsData = await _service.GetNewVideoGameDropdownsValuesAsync();

            ViewBag.Developers = new SelectList(videoGameDropdownsData.Developers, "Id", "CompanyName");
            ViewBag.Publishers = new SelectList(videoGameDropdownsData.Publishers, "Id", "CompanyName");

            VideoGameSearch searchModel = new VideoGameSearch();

            if (searchString == null)
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            searchModel.Title = searchString;
            searchModel.Description = searchString;

            var allGames = await _service.GetAllVideoGamesAsync();

            var filterResult = _service.GetQueriedVideoGames(allGames, searchModel, sortOrder);

            var result = new List<VideoGameVM>();

            foreach (var game in filterResult)
            {
                string discount = "";
                double discountedPrice = game.Price;
                if (game.Discounts.Count() > 0 )
                {
                    int i = 0;
                    var gameDiscounts = game.Discounts.ToList();
                    foreach (var dis in gameDiscounts)
                    {
                        if(dis.IsActive && DateTime.UtcNow < dis.DateExpiry)
                        {
                            if (dis.DiscountUnit == "percent")
                            {
                                discount = dis.DiscountValue.ToString() + "%";
                                discountedPrice = game.Price - (game.Price * dis.DiscountValue) / 100;
                            }
                            else
                            {
                                discount = "$" + dis.DiscountValue.ToString();
                                discountedPrice = game.Price - dis.DiscountValue;
                            }
                        }
                    }
                }

                var videoGameDetails = new VideoGameVM()
                {
                    Id = game.Id,
                    Title = game.Title,
                    Description = game.Description,
                    Price = game.Price,
                    ImageURL = game.ImageURL,
                    ReleaseDate = game.ReleaseDate,
                    GameGenres = game.GameGenres,
                    GameAgeRating = game.GameAgeRating,
                    PublisherIds = new List<int>(),
                    DeveloperId = game.DeveloperId,
                    Discount = discount,
                    DiscountedPrice = discountedPrice
                };

                if ( game.Publishers_VideoGames != null)
                {
                    foreach (var pub in game.Publishers_VideoGames)
                    {
                        videoGameDetails.PublisherIds.Add(pub.PublisherId);
                    }
                }

                result.Add(videoGameDetails);
            }

            if (result.Count() >= numberOfFeaturedItems && searchString == null)
            {
                Random rand = new Random();
                ViewBag.featuredGames = result.OrderBy(x => rand.Next()).Take(3).ToList();
                ViewBag.ShowFeatured = true;
            }


            return View(result);
        }

        //Get: VideoGames/Detials/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var videoGameDetails = await _service.GetVideoGameByIdAsync(id);
            if (videoGameDetails == null)
            {
                return View("NotFound");
            }
            else
            {
                return View(videoGameDetails);
            }
        }

        public async Task<IActionResult> Create( NewVideoGameVM videoGame)
        {
            if (ModelState.IsValid == false)
            {
                var videoGameDropdownData = await _service.GetNewVideoGameDropdownsValuesAsync();

                ViewBag.Publishers = new SelectList(videoGameDropdownData.Publishers, "Id", "CompanyName");
                ViewBag.Developers = new SelectList(videoGameDropdownData.Developers, "Id", "CompanyName");

                return View(videoGame);
            }

            await _service.AddNewVideoGameAsync(videoGame);

            return RedirectToAction(nameof(Index));
        }

        //Get: VideoGames/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var videoGameDetails = await _service.GetByIdAsync(id);
            if (videoGameDetails == null)
            {
                return View("NotFound");
            }

            await _service.DeleteAsync(videoGameDetails);

            // Return view of publishers with item in it.
            return RedirectToAction(nameof(Index));
        }


        //GET: VideoGames/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var videoGameDetails = await _service.GetVideoGameByIdAsync(id);

            if (videoGameDetails == null)
            {
                return View("NotFound");
            }

            var result = new NewVideoGameVM()
            {
                Id = videoGameDetails.Id,
                Title = videoGameDetails.Title,
                Description = videoGameDetails.Description,
                Price = videoGameDetails.Price,
                ReleaseDate = videoGameDetails.ReleaseDate,
                ImageURL = videoGameDetails.ImageURL,
                GameGenres = videoGameDetails.GameGenres.Select(n => n).ToList(),
                GameAgeRating = videoGameDetails.GameAgeRating,
                DeveloperId = videoGameDetails.DeveloperId,
                PublisherIds = videoGameDetails.Publishers_VideoGames.Select(n => n.PublisherId).ToList(),
            };

            var videoGameDropdownsData = await _service.GetNewVideoGameDropdownsValuesAsync();
            ViewBag.Developers = new SelectList(videoGameDropdownsData.Developers, "Id", "CompanyName");
            ViewBag.Publishers = new SelectList(videoGameDropdownsData.Publishers, "Id", "CompanyName");

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewVideoGameVM videoGame)
        {
            if (id != videoGame.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var videoGameDropdownsData = await _service.GetNewVideoGameDropdownsValuesAsync();

                ViewBag.Developers = new SelectList(videoGameDropdownsData.Developers, "Id", "CompanyName");
                ViewBag.Publishers = new SelectList(videoGameDropdownsData.Publishers, "Id", "CompanyName");

                return View(videoGame);
            }

            await _service.UpdateVideoGameAsync(videoGame);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        ///     Performs filter on all video games in the DB. 
        /// </summary>
        /// <param name="minPrice"> Min required price of a game, can be null </param>
        /// <param name="maxPrice"> Max required price of a game, can be null </param>
        /// <param name="gameAgeRating"> Game age rating, can be null</param>
        /// <param name="gameGenre"> Game Genre, can be null</param>
        /// <param name="publisher"> Publisher Id, can be null</param>
        /// <param name="developer"> Developer Id, can be null</param>
        /// <param name="sortOrder"> Sort order either Asc or Desc</param>
        /// <param name="currentFilter"> Current text filter applied by the navbar search box. </param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Filter(PriceRange minPrice, PriceRange? maxPrice, GameAgeRating? gameAgeRating,
            GameGenre? gameGenre, int? publisher, int? developer, string sortOrder, string currentFilter)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "" : "title_desc";
            ViewBag.ShowFeatured = false;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.GameAgeRating = gameAgeRating;
            ViewBag.GameGenre = gameGenre;

            // publisher and developer return the ID field.
            ViewBag.Publisher = publisher;
            ViewBag.Developer = developer;

            ViewBag.CurrentFilter = currentFilter;

            if (publisher != null)
            {
                var searchedForPublisher = await _publisherService.GetByIdAsync((int)publisher);
                ViewBag.previousPublisherDescription = searchedForPublisher.CompanyName;
            }
            else
            {
                ViewBag.previousPublisherDescription = "";
            }

            if ( developer != null)
            {
                var searchedForDeveloper = await _developerService.GetByIdAsync((int)developer); 
                ViewBag.previousDeveloperDescription = searchedForDeveloper.CompanyName;
            }
            else
            {
                ViewBag.previousDeveloperDescription = "";
            }

            string publisherCompanyName = ViewBag.previousPublisherDescription;
            string developerCompanyName = ViewBag.previousDeveloperDescription;


            VideoGameSearch searchModel = new VideoGameSearch()
            {
                Title = currentFilter,
                Description = currentFilter,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                GameAgeRating = gameAgeRating,
                GameGenre = gameGenre,
                Publisher = publisherCompanyName,
                Developer = developerCompanyName
            };

            IEnumerable<Models.Developer> developerList = await _developerService.GetAllAsync();

            var videoGameDropdownsData = await _service.GetNewVideoGameDropdownsValuesAsync();

            ViewBag.Developers = new SelectList(videoGameDropdownsData.Developers, "Id", "CompanyName");
            ViewBag.Publishers = new SelectList(videoGameDropdownsData.Publishers, "Id", "CompanyName");

            IEnumerable<VideoGame> gamesToFilter = await _service.GetAllVideoGamesAsync();

            var gamesPubDevFiltered = _service.GetPublisherAndDeveloperQueriedVideoGames(gamesToFilter, publisher, developer);

            var result = _service.GetQueriedVideoGames(gamesPubDevFiltered, searchModel, sortOrder);

            return View("Index", result);
        }
    }
}
