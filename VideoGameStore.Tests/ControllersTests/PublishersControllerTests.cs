using VideoGameStore.Controllers;
using VideoGameStore.Data.Services;
using VideoGameStore.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace VideoGameStore.Tests
{
    public class PublishersControllerTests
    {

        private readonly IPublishersService _service;
        private readonly PublishersController _controller;


        public PublishersControllerTests()
        {
            // Dependencies
            _service = A.Fake<IPublishersService>();

            //SUT (System Under Test)
            _controller = new PublishersController(_service);
        }

        [Fact]
        public void PublishersController_Index_ReturnSucess()
        {
            //Arrange
            var actors = A.Fake<IEnumerable<Publisher>>();
            A.CallTo(() => _service.GetAllAsync()).Returns(actors);

            //Act
            var result = _controller.Index();

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PublishersControllor_Details_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var actor = A.Fake<Publisher>();
            A.CallTo(() => _service.GetByIdAsync(actor.Id)).Returns(actor);

            //Act
            var result = _controller.Details(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PublishersControllor_Edit_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var actor = A.Fake<Publisher>();
            A.CallTo(() => _service.GetByIdAsync(actor.Id)).Returns(actor);

            //Act
            var result = _controller.Edit(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PublishersControllor_Delete_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var publisher = A.Fake<Publisher>();
            A.CallTo(() => _service.GetByIdAsync(publisher.Id)).Returns(publisher);

            //Act
            var result = _controller.Delete(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PublishersControllor_DeleteConfirmed_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var publisher = A.Fake<Publisher>();
            A.CallTo(() => _service.GetByIdAsync(publisher.Id)).Returns(publisher);

            //Act
            var result = _controller.DeleteConfirmed(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PublishersControllor_Create_ReturnsSucess()
        {
            //Arrange
            var actor = A.Fake<Publisher>();
            A.CallTo(() => _service.AddAsync(actor));

            //Act
            var result = _controller.Create(actor);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}