using VideoGameStore.Data.Services;
using VideoGameStore.Data.Static;
using VideoGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VideoGameStore.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class CinemasController : Controller
    {
        private readonly ICinemasService _service;

        public CinemasController(ICinemasService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        //Get: Cinemas/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Logo,Description")] Cinema cinema)
        {
            if (ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return View(cinema);
            }
            // Add the cinema to DB if passes valid checks
            await _service.AddAsync(cinema);

            // Return view of cinemas with item in it.
            return RedirectToAction(nameof(Index));
        }

        //Get: Cinemas/Detials/1
        public async Task<IActionResult> Details(int id)
        {
            var cinemaDetails = await _service.GetByIdAsync(id);
            if (cinemaDetails == null)
            {
                return View("NotFound");
            }
            else
            {
                return View(cinemaDetails);
            }
        }

        //Get: Cinemas/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var cinemaDetails = await _service.GetByIdAsync(id);
            if (cinemaDetails == null)
            {
                return View("NotFound");
            }
            return View(cinemaDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Logo,Description")] Cinema cinema)
        {
            if (ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return View(cinema);
            }
            // Add the cinema to DB if passes valid checks
            await _service.UpdateAsync(id, cinema);

            // Return view of cinemas with item in it.
            return RedirectToAction(nameof(Index));
        }

        //Get: Cinemas/Delete/1 , this one will be called
        public async Task<IActionResult> Delete(int id)
        {
            var cinemaDetails = await _service.GetByIdAsync(id);
            if (cinemaDetails == null)
            {
                return View("NotFound");
            }
            return View(cinemaDetails);
        }

        [HttpPost, ActionName("Delete")] // Post request: Post: Cinemas/Delete/1
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinemaDetails = await _service.GetByIdAsync(id);

            if (cinemaDetails == null)
            {
                return View("NotFound");
            }

            await _service.DeleteAsync(cinemaDetails);

            // Return view of cinemas with item in it.
            return RedirectToAction(nameof(Index));
        }
    }
}
