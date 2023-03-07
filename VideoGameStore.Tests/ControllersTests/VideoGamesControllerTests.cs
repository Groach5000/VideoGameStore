using VideoGameStore.Controllers;
using VideoGameStore.Data.Services;
using VideoGameStore.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using VideoGameStore.Data.Enums;

namespace VideoGameStore.Tests
{
    public class VideoGamesControllerTests
    {
        private IPublishersService _publishersService;
        private IDevelopersService _developerService;
        private IVideoGamesService _videoGameService;
        private VideoGamesController _controller;


        public VideoGamesControllerTests()
        {
            // Dependencies
            _videoGameService = A.Fake<IVideoGamesService>();
            _publishersService = A.Fake<IPublishersService>();
            _developerService = A.Fake<IDevelopersService>();

            //SUT (System Under Test)
            _controller = new VideoGamesController(_videoGameService, _publishersService, _developerService);
        }

        [Fact]
        public void VideoGamesController_Index_ReturnSucess()
        {
            //Arrange
            var videoGame = A.Fake<IEnumerable<VideoGame>>();
            A.CallTo(() => _videoGameService.GetAllAsync()).Returns(videoGame);
            string sortOrder = "";
            string currentFilter = "";
            string searchString = "";

            //Act
            var result = _controller.Index(sortOrder, currentFilter, searchString);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void VideoGamesControllor_Details_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var videoGame = A.Fake<VideoGame>();
            A.CallTo(() => _videoGameService.GetByIdAsync(videoGame.Id)).Returns(videoGame);

            //Act
            var result = _controller.Details(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void VideoGamesControllor_Edit_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var videoGame = A.Fake<VideoGame>();
            A.CallTo(() => _videoGameService.GetByIdAsync(videoGame.Id)).Returns(videoGame);

            //Act
            var result = _controller.Edit(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void VideoGamesControllor_Delete_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var videoGame = A.Fake<VideoGame>();
            A.CallTo(() => _videoGameService.GetByIdAsync(videoGame.Id)).Returns(videoGame);

            //Act
            var result = _controller.Delete(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void VideoGamesController_Filter_ReturnSucess()
        {
            //Arrange
            var videoGame = A.Fake<IEnumerable<VideoGame>>();
            A.CallTo(() => _videoGameService.GetAllAsync()).Returns(videoGame);
            string sortOrder = "";
            string currentFilter = "";
            string searchString = "";
            PriceRange minPrice = PriceRange.Free;
            PriceRange? maxPrice = PriceRange.NoMax;
            GameAgeRating? gameAgeRating = null;
            GameGenre? gameGenre = null;
            int? publisher = null;
            int? developer = null;

            //Act
            var result = _controller.Filter(minPrice, maxPrice, gameAgeRating, gameGenre, publisher, developer, 
                sortOrder, currentFilter);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}