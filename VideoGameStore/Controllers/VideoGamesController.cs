﻿using VideoGameStore.Data;
using VideoGameStore.Data.Services;
using VideoGameStore.Data.Static;
using VideoGameStore.Data.ViewModels;
using VideoGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace VideoGameStore.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class VideoGamesController : Controller

    {
        private readonly IVideoGamesService _service;

        public VideoGamesController(IVideoGamesService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            var allVideoGames = await _service.GetAllAsync();

            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";

            switch (sortOrder)
            {
                case "title_desc":
                    allVideoGames = allVideoGames.OrderByDescending(n => n.Title);
                    break;

                default:
                    allVideoGames = allVideoGames.OrderBy(n => n.Title);
                    break;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchResults = allVideoGames.Where(n => n.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) ||
                    n.Description.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                    ).ToList();
                TempData["ShowFeatured"] = "false";
                return View("Index", searchResults);
            }
            TempData["ShowFeatured"] = "true";
            return View(allVideoGames);
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
        public async Task<IActionResult> Sort()
        {
            var allVideoGames = await _service.GetAllAsync();

            var searchResults = allVideoGames.OrderBy(n => n.Title).ToList();

            TempData["ShowFeatured"] = "false";

            return View("Index", searchResults);
        }
    }
}
