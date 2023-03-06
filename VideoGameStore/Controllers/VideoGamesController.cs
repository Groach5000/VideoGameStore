using VideoGameStore.Data.Services;
using VideoGameStore.Data.Static;
using VideoGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VideoGameStore.Models.searchModels;
using System.Collections.Generic;
using VideoGameStore.Data.Enums;
using Microsoft.Data.SqlClient;
using System.Security.Policy;
using System.Collections;

namespace VideoGameStore.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class VideoGamesController : Controller

    {
        private readonly IVideoGamesService _service;
        private readonly IPublishersService _publisherService;
        private readonly IDevelopersService _developersService;

        private const int numberOfFeaturedItems = 3;

        public VideoGamesController(IVideoGamesService service, IPublishersService publisherService, 
            IDevelopersService developersService)
        {
            _service = service;
            _publisherService = publisherService;
            _developersService = developersService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
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

            //ViewBag.Publishers = await _publisherService.GetAllAsync();

            VideoGameSearch searchModel = new VideoGameSearch();

            if (searchString == null)
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            searchModel.Title = searchString;
            searchModel.Description = searchString;

            var allGames = await _service.GetAllAsync();

            var searchLogic = new VideoGamesBusinessLogic(_service);

            var result = searchLogic.GetQueriedVideoGames(allGames, searchModel, sortOrder);


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
                GameGenres = videoGameDetails.GameGenre,
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

        [AllowAnonymous]
        public async Task<IActionResult> Filter(PriceRange minPrice, PriceRange? maxPrice, GameAgeRating? gameAgeRating,
            GameGenre? gameGenre, int? publisher, int? developer, string sortOrder)
        {
            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.ShowFeatured = false;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.GameAgeRating = gameAgeRating;
            ViewBag.GameGenre = gameGenre;

            // publisher and developer return the ID field.
            ViewBag.Publisher = publisher;
            ViewBag.Developer = developer;

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
                var searchedForDeveloper = await _developersService.GetByIdAsync((int)developer); 
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
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                GameAgeRating = gameAgeRating,
                GameGenre = gameGenre,
                Publisher = publisherCompanyName,
                Developer = developerCompanyName
            };

            var videoGameDropdownsData = await _service.GetNewVideoGameDropdownsValuesAsync();

            ViewBag.Developers = new SelectList(videoGameDropdownsData.Developers, "Id", "CompanyName");
            ViewBag.Publishers = new SelectList(videoGameDropdownsData.Publishers, "Id", "CompanyName");

            var allGames = await _service.GetAllAsync();

            var searchLogic = new VideoGamesBusinessLogic(_service);

            var result = searchLogic.GetQueriedVideoGames(allGames, searchModel, sortOrder);

            return View("Index", result);
        }
    }
}
