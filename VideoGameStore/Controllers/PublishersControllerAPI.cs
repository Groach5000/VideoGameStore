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
    [Authorize(Roles = UserRoles.Admin)]
    public class PublishersControllerAPI : ControllerBase 
    {
        private readonly IPublishersService _service;

        public PublishersControllerAPI(IPublishersService service)
        {
            _service = service;
        }

        // API can use to see JSON in brower or details in "Postman" app

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Publisher>> Index()
        {
            var data = await _service.GetAllAsync();
            return data;
        }

        //Get: Publishers/Details/1
        [HttpGet ("Publishers/Details/{id}")]
        [AllowAnonymous]
        public async Task<Publisher> Details(int id)
        {
            var PublisherDetails = await _service.GetByIdAsync(id);
            if (PublisherDetails == null)
            {
                return null;
            }
            else
            {
                return PublisherDetails;
            }
        }

        //Get: Publishers/Edit/1
        [HttpGet("Publishers/Edit/{id}")]
        public async Task<Publisher> Edit(int id)
        {
            var PublisherDetails = await _service.GetByIdAsync(id);
            if (PublisherDetails == null)
            {
                return null;
            }
            return PublisherDetails;
        }

        
        [HttpPost("Publishers/Create")]
        public async Task<string> Create(Publisher Publisher)
        {
            if(ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return "Invalid Publisher input";
            }
            // Add the Publisher to DB if passes valid checks
            await _service.AddAsync(Publisher);

            // Return view of Publishers with item in it.
            return "Publisher with id: " + Publisher.Id.ToString() + " deleted";
        }

        [HttpPut("Publishers/Edit/{id}")]
        public async Task<int> Edit(int id, Publisher Publisher)
        {
            if (ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return -1;
            }
            // Add the Publisher to DB if passes valid checks
            await _service.UpdateAsync(id, Publisher);

            // Return view of Publishers with item in it.
            return Publisher.Id;
        }

        [HttpDelete("Publishers/Delete/{id}")] // Delete request: Publishers/Delete/1
        public async Task<string> DeleteConfirmed(int id)
        {
            var PublisherDetails = await _service.GetByIdAsync(id);

            if (PublisherDetails == null)
            {
                return "Publisher Not Found";
            }

            await _service.DeleteAsync(PublisherDetails);

            // Return view of Publishers with item in it.
            return "Publisher with id: " + id + " deleted";
        }
    }
}
