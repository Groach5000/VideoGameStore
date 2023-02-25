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
    public class ActorsController : Controller
    {
        private readonly IActorsService _service;

        public ActorsController(IActorsService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        //Get: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,ProfilePictureURL,Bio")]Actor actor)
        {
            if(ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return View(actor);
            }
            // Add the actor to DB if passes valid checks
            await _service.AddAsync(actor);

            // Return view of actors with item in it.
            return RedirectToAction(nameof(Index));
        }

        //Get: Actors/Detials/1
        [AllowAnonymous]
        public async Task<IActionResult> Details (int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
            {
                return View("NotFound");
            }
            else
            {
                return View(actorDetails);
            }
        }
         
        //Get: Actors/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
            {
                return View("NotFound");
            }
            return View(actorDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureURL,Bio")] Actor actor)
        {
            if (ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return View(actor);
            }
            // Add the actor to DB if passes valid checks
            await _service.UpdateAsync(id, actor);

            // Return view of actors with item in it.
            return RedirectToAction(nameof(Index));
        }

        //Get: Actors/Delete/1 , this one will be called
        public async Task<IActionResult> Delete(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
            {
                return View("NotFound");
            }
            return View(actorDetails);
        }

        [HttpPost, ActionName("Delete")] // Post request: Post: Actors/Delete/1
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);

            if (actorDetails == null)
            {
                return View("NotFound");
            }

            await _service.DeleteAsync(actorDetails);

            // Return view of actors with item in it.
            return RedirectToAction(nameof(Index));
        }

    }
}
