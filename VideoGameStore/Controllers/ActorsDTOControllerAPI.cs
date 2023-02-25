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
    public class ActorsDTOControllerAPI : ControllerBase 
    {
        private readonly IActorsService _service;

        public ActorsDTOControllerAPI(IActorsService service)
        {
            _service = service;
        }

        // API can use to see JSON in brower or details in "Postman" app

        [HttpGet("ActorsDTO/Index")]
        
        public async Task<IEnumerable<ActorDTO>> Index()
        {
            var data = await _service.GetAllAsync();

            // Can do in two ways, method one in LINQ, both do the same thing.
            IEnumerable<ActorDTO> actorDTO = from p in data select new ActorDTO(p);

            // Second method is in a foreach loop (commnented out)
            /*
            List<ActorDTO> actorDTO = new List<ActorDTO>();
            foreach (Actor item in data)
            {
                actorDTO.Add(new ActorDTO(item));
            } */
            
            return actorDTO;
        }

        //Get: ActorsDTO/Details/1
        [HttpGet ("ActorsDTO/Details/{id}")]
        public async Task<ActorDTO> Details(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
            {
                return null;
            }
            else
            {
                ActorDTO actorDTO = new ActorDTO(actorDetails);
                return actorDTO;
            }
        }

        /* The following are yet to be implemented using DTO.

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

        */
    }
}
