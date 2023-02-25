using VideoGameStore.Data;
using VideoGameStore.Data.Services;
using VideoGameStore.Data.Static;
using VideoGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace VideoGameStore.Controllers
{
    //This is an REST API, to see/allow JSON to Create/Read/Update/Delete items (CRUD)
    [ApiController]
    [Route("api/")]
    [Authorize(Roles = UserRoles.Admin)]
    public class ActorsControllerAPI : ControllerBase 
    {
        private readonly IActorsService _service;

        public ActorsControllerAPI(IActorsService service)
        {
            _service = service;
        }

        // API can use to see JSON in brower or details in "Postman" app

        [HttpGet]
        public async Task<IEnumerable<Actor>> Index()
        {
            var data = await _service.GetAllAsync();
            return data;
        }

        //Get: Actors/Details/1
        [HttpGet ("Actors/Details/{id}")]
        public async Task<Actor> Details(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
            {
                return null;
            }
            else
            {
                return actorDetails;
            }
        }

        //Get: Actors/Edit/1
        [HttpGet("Actors/Edit/{id}")]
        public async Task<Actor> Edit(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
            {
                return null;
            }
            return actorDetails;
        }

        
        [HttpPost("Actors/Create")]
        public async Task<string> Create(Actor actor)
        {
            if(ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return "Invalid Actor input";
            }
            // Add the actor to DB if passes valid checks
            await _service.AddAsync(actor);

            // Return view of actors with item in it.
            return "Actor with id: " + actor.Id.ToString() + " deleted";
        }

        [HttpPut("Actors/Edit/{id}")]
        public async Task<int> Edit(int id, Actor actor)
        {
            if (ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return -1;
            }
            // Add the actor to DB if passes valid checks
            await _service.UpdateAsync(id, actor);

            // Return view of actors with item in it.
            return actor.Id;
        }

        [HttpDelete("Actors/Delete/{id}")] // Delete request: Actors/Delete/1
        public async Task<string> DeleteConfirmed(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);

            if (actorDetails == null)
            {
                return "Actor Not Found";
            }

            await _service.DeleteAsync(actorDetails);

            // Return view of actors with item in it.
            return "Actor with id: " + id + " deleted";
        }
    }
}
