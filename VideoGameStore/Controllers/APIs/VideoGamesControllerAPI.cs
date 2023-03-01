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
    public class VideoGamesControllerAPI : ControllerBase 
    {
        private readonly IVideoGamesService _gameService;

        public VideoGamesControllerAPI(IVideoGamesService service)
        {
            _gameService = service;
        }

        // API can use to see JSON in brower or details in "Postman" app

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<VideoGame>> Index()
        {
            var data = await _gameService.GetAllAsync();
            return data;
        }

        //Get: VideoGames/Details/1
        [HttpGet ("VideoGames/Details/{id}")]
        [AllowAnonymous]
        public async Task<VideoGame> Details(int id)
        {
            var videoGameDetails = await _gameService.GetByIdAsync(id);
            if (videoGameDetails == null)
            {
                return null;
            }
            else
            {
                return videoGameDetails;
            }
        }

        //Get: VideoGames/Edit/1
        [HttpGet("VideoGames/Edit/{id}")]
        public async Task<VideoGame> Edit(int id)
        {
            var videoGameDetails = await _gameService.GetByIdAsync(id);
            if (videoGameDetails == null)
            {
                return null;
            }
            return videoGameDetails;
        }

        
        [HttpPost("VideoGames/Create")]
        public async Task<string> Create(VideoGame videoGame)
        {
            if(ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return "Invalid VideoGame input";
            }
            // Add the VideoGame to DB if passes valid checks
            await _gameService.AddAsync(videoGame);

            // Return view of VideoGames with item in it.
            return "VideoGame with id: " + videoGame.Id.ToString() + " created.";
        }

        [HttpPut("VideoGames/Edit/{id}")]
        public async Task<string> Edit(int id, VideoGame videoGame)
        {
            if (ModelState.IsValid == false)//Checks if items that are required in model are present
            {
                return "Invalid VideoGame input";
            }
            // Add the VideoGame to DB if passes valid checks
            await _gameService.UpdateAsync(id, videoGame);

            // Return view of VideoGames with item in it.
            return "VideoGame with id: " + videoGame.Id.ToString() + " edited.";
        }

        [HttpDelete("VideoGames/Delete/{id}")] // Delete request: VideoGames/Delete/1
        public async Task<string> DeleteConfirmed(int id)
        {
            var videoGameDetails = await _gameService.GetByIdAsync(id);

            if (videoGameDetails == null)
            {
                return "VideoGame Not Found";
            }

            await _gameService.DeleteAsync(videoGameDetails);

            // Return view of VideoGames with item in it.
            return "VideoGame with id: " + id + " deleted";
        }
    }
}
