using VideoGameStore.Data.Services;
using VideoGameStore.Data.Static;
using VideoGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VideoGameStore.Controllers
{

    [Authorize(Roles = UserRoles.Admin)]
    public class DevelopersController : Controller
    {
        private readonly IDevelopersService _service;

        public DevelopersController(IDevelopersService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        //Get: developers/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("CompanyName,LogoURL,About")] Developer developer)
        {
            if (ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return View(developer);
            }
            // Add the developer to DB if passes valid checks
            await _service.AddAsync(developer);

            // Return view of developers with item in it.
            return RedirectToAction(nameof(Index));
        }

        //Get: developers/Detials/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var developerDetails = await _service.GetByIdAsync(id);
            if (developerDetails == null)
            {
                return View("NotFound");
            }
            else
            {
                return View(developerDetails);
            }
        }

        //Get: developers/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var developerDetails = await _service.GetByIdAsync(id);
            if (developerDetails == null)
            {
                return View("NotFound");
            }
            return View(developerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyName,LogoURL,About")] Developer developer)
        {
            if (ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return View(developer);
            }
            // Add the developer to DB if passes valid checks
            await _service.UpdateAsync(id, developer);

            // Return view of developers with item in it.
            return RedirectToAction(nameof(Index));
        }

        //Get: developers/Delete/1 , this one will be called
        public async Task<IActionResult> Delete(int id)
        {
            var developerDetails = await _service.GetByIdAsync(id);
            if (developerDetails == null)
            {
                return View("NotFound");
            }
            return View(developerDetails);
        }

        [HttpPost, ActionName("Delete")] // Post request: Post: developers/Delete/1
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var developerDetails = await _service.GetByIdAsync(id);

            if (developerDetails == null)
            {
                return View("NotFound");
            }

            await _service.DeleteAsync(developerDetails);

            // Return view of developers with item in it.
            return RedirectToAction(nameof(Index));
        }
    }
}
