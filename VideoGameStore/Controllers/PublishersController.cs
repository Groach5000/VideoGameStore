using VideoGameStore.Data;
using VideoGameStore.Data.Services;
using VideoGameStore.Data.Static;
using VideoGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace VideoGameStore.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class PublishersController : Controller
    {
        private readonly IPublishersService _service;

        public PublishersController(IPublishersService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        //Get: publishers/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyName,LogoURL,About")]Publisher publisher)
        {
            if(ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return View(publisher);
            }
            // Add the publisher to DB if passes valid checks
            await _service.AddAsync(publisher);

            // Return view of publishers with item in it.
            return RedirectToAction(nameof(Index));
        }

        //Get: publishers/Detials/1
        [AllowAnonymous]
        public async Task<IActionResult> Details (int id)
        {
            var publisherDetails = await _service.GetByIdAsync(id);
            if (publisherDetails == null)
            {
                return View("NotFound");
            }
            else
            {
                return View(publisherDetails);
            }
        }
         
        //Get: publishers/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var publisherDetails = await _service.GetByIdAsync(id);
            if (publisherDetails == null)
            {
                return View("NotFound");
            }
            return View(publisherDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyName,LogoURL,About")] Publisher publisher)
        {
            if (ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return View(publisher);
            }
            // Add the publisher to DB if passes valid checks
            await _service.UpdateAsync(id, publisher);

            // Return view of publishers with item in it.
            return RedirectToAction(nameof(Index));
        }

        //Get: publishers/Delete/1 , this one will be called
        public async Task<IActionResult> Delete(int id)
        {
            var publisherDetails = await _service.GetByIdAsync(id);
            if (publisherDetails == null)
            {
                return View("NotFound");
            }
            return View(publisherDetails);
        }

        [HttpPost, ActionName("Delete")] // Post request: Post: publishers/Delete/1
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publisherDetails = await _service.GetByIdAsync(id);

            if (publisherDetails == null)
            {
                return View("NotFound");
            }

            await _service.DeleteAsync(publisherDetails);

            // Return view of publishers with item in it.
            return RedirectToAction(nameof(Index));
        }

    }
}
