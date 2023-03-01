using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using VideoGameStore.Data.Services;
using VideoGameStore.Data.Static;
using VideoGameStore.Models;

namespace eTickets.Controllers
{
    //This is an REST API, to see/allow JSON to Create/Read/Update/Delete items (CRUD)
    [ApiController]
    [Route("api/")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.Admin)]
    public class DevelopersControllerAPI : ControllerBase 
    {
        private readonly IDevelopersService _service;

        public DevelopersControllerAPI(IDevelopersService service)
        {
            _service = service;
        }

        // API can use to see JSON in brower or details in "Postman" app

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Developer>> Index()
        {
            var data = await _service.GetAllAsync();
            return data;
        }

        //Get: Developers/Details/1
        [HttpGet ("Developers/Details/{id}")]
        [AllowAnonymous]
        public async Task<Developer> Details(int id)
        {
            var developerDetails = await _service.GetByIdAsync(id);
            if (developerDetails == null)
            {
                return null;
            }
            else
            {
                return developerDetails;
            }
        }

        //Get: Developers/Edit/1
        [HttpGet("Developers/Edit/{id}")]
        public async Task<Developer> Edit(int id)
        {
            var developerDetails = await _service.GetByIdAsync(id);
            if (developerDetails == null)
            {
                return null;
            }
            return developerDetails;
        }

        
        [HttpPost("Developers/Create")]
        public async Task<string> Create(Developer developer)
        {
            if(ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return "Invalid Developer input";
            }
            // Add the Developer to DB if passes valid checks
            await _service.AddAsync(developer);

            // Return view of Developers with item in it.
            return "Developer with id: " + developer.Id.ToString() + " created.";
        }

        [HttpPut("Developers/Edit/{id}")]
        public async Task<string> Edit(int id, Developer developer)
        {
            if (ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return "Invalid Developer input";
            }
            // Add the Developer to DB if passes valid checks
            await _service.UpdateAsync(id, developer);

            // Return view of Developers with item in it.
            return "Developer with id: " + developer.Id.ToString() + " edited.";
        }

        [HttpDelete("Developers/Delete/{id}")] // Delete request: Developers/Delete/1
        public async Task<string> DeleteConfirmed(int id)
        {
            var developerDetails = await _service.GetByIdAsync(id);

            if (developerDetails == null)
            {
                return "Developer Not Found";
            }

            await _service.DeleteAsync(developerDetails);

            // Return view of Developers with item in it.
            return "Developer with id: " + id + " deleted";
        }
    }
}
