using VideoGameStore.Data;
using VideoGameStore.Data.Services;
using VideoGameStore.Data.Static;
using VideoGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VideoGameStore.Controllers
{

    [Authorize(Roles = UserRoles.Admin)]
    public class ProducersController : Controller
    {
        private readonly IProducersService _service;

        public ProducersController(IProducersService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        //Get: Producers/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName,ProfilePictureURL,Bio")] Producer producer)
        {
            if (ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return View(producer);
            }
            // Add the producer to DB if passes valid checks
            await _service.AddAsync(producer);

            // Return view of producers with item in it.
            return RedirectToAction(nameof(Index));
        }

        //Get: Producers/Detials/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var producerDetails = await _service.GetByIdAsync(id);
            if (producerDetails == null)
            {
                return View("NotFound");
            }
            else
            {
                return View(producerDetails);
            }
        }

        //Get: Producers/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var producerDetails = await _service.GetByIdAsync(id);
            if (producerDetails == null)
            {
                return View("NotFound");
            }
            return View(producerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureURL,Bio")] Producer producer)
        {
            if (ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return View(producer);
            }
            // Add the producer to DB if passes valid checks
            await _service.UpdateAsync(id, producer);

            // Return view of producers with item in it.
            return RedirectToAction(nameof(Index));
        }

        //Get: Producers/Delete/1 , this one will be called
        public async Task<IActionResult> Delete(int id)
        {
            var producerDetails = await _service.GetByIdAsync(id);
            if (producerDetails == null)
            {
                return View("NotFound");
            }
            return View(producerDetails);
        }

        [HttpPost, ActionName("Delete")] // Post request: Post: Producers/Delete/1
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producerDetails = await _service.GetByIdAsync(id);

            if (producerDetails == null)
            {
                return View("NotFound");
            }

            await _service.DeleteAsync(producerDetails);

            // Return view of producers with item in it.
            return RedirectToAction(nameof(Index));
        }
    }
}
