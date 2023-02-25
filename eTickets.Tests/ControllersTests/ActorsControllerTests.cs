using VideoGameStore.Controllers;
using VideoGameStore.Data.Services;
using VideoGameStore.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace VideoGameStore.Tests
{
    public class ActorsControllerTests
    {

        private readonly IActorsService _service;
        private readonly ActorsController _controller;


        public ActorsControllerTests()
        {
            // Dependencies
            _service = A.Fake<IActorsService>();

            //SUT (System Under Test)
            _controller = new ActorsController(_service);
        }

        [Fact]
        public void ActorsController_Index_ReturnSucess()
        {
            //Arrange
            var actors = A.Fake<IEnumerable<Actor>>();
            A.CallTo(() => _service.GetAllAsync()).Returns(actors);

            //Act
            var result = _controller.Index();

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ActorsControllor_Details_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var actor = A.Fake<Actor>();
            A.CallTo(() => _service.GetByIdAsync(actor.Id)).Returns(actor);

            //Act
            var result = _controller.Details(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ActorsControllor_Edit_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var actor = A.Fake<Actor>();
            A.CallTo(() => _service.GetByIdAsync(actor.Id)).Returns(actor);

            //Act
            var result = _controller.Edit(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ActorsControllor_Delete_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var actor = A.Fake<Actor>();
            A.CallTo(() => _service.GetByIdAsync(actor.Id)).Returns(actor);

            //Act
            var result = _controller.Delete(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ActorsControllor_DeleteConfirmed_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var actor = A.Fake<Actor>();
            A.CallTo(() => _service.GetByIdAsync(actor.Id)).Returns(actor);

            //Act
            var result = _controller.DeleteConfirmed(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ActorsControllor_Create_ReturnsSucess()
        {
            //Arrange
            var actor = A.Fake<Actor>();
            A.CallTo(() => _service.AddAsync(actor));

            //Act
            var result = _controller.Create(actor);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}