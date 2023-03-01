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
            var publisherDetails = await _service.GetByIdAsync(id);
            if (publisherDetails == null)
            {
                return null;
            }
            else
            {
                return publisherDetails;
            }
        }

        //Get: Publishers/Edit/1
        [HttpGet("Publishers/Edit/{id}")]
        public async Task<Publisher> Edit(int id)
        {
            var publisherDetails = await _service.GetByIdAsync(id);
            if (publisherDetails == null)
            {
                return null;
            }
            return publisherDetails;
        }

        
        [HttpPost("Publishers/Create")]
        public async Task<string> Create(Publisher publisher)
        {
            if(ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return "Invalid Publisher input";
            }
            // Add the Publisher to DB if passes valid checks
            await _service.AddAsync(publisher);

            // Return view of Publishers with item in it.
            return "Publisher with id: " + publisher.Id.ToString() + " created.";
        }

        [HttpPut("Publishers/Edit/{id}")]
        public async Task<string> Edit(int id, Publisher publisher)
        {
            if (ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return "Invalid Publisher input";
            }
            // Add the Publisher to DB if passes valid checks
            await _service.UpdateAsync(id, publisher);

            // Return view of Publishers with item in it.
            return "Publisher with id: " + publisher.Id.ToString() + " edited.";
        }

        [HttpDelete("Publishers/Delete/{id}")] // Delete request: Publishers/Delete/1
        public async Task<string> DeleteConfirmed(int id)
        {
            var publisherDetails = await _service.GetByIdAsync(id);

            if (publisherDetails == null)
            {
                return "Publisher Not Found";
            }

            await _service.DeleteAsync(publisherDetails);

            // Return view of Publishers with item in it.
            return "Publisher with id: " + id + " deleted";
        }
    }
}
