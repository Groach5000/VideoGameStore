using VideoGameStore.Controllers;
using VideoGameStore.Data.Services;
using VideoGameStore.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace VideoGameStore.Tests
{
    public class DeveloperControllerTests
    {

        private IDevelopersService _service;
        private DevelopersController _controller;


        public DeveloperControllerTests()
        {
            // Dependencies
            _service = A.Fake<IDevelopersService>();

            //SUT (System Under Test)
            _controller = new DevelopersController(_service);
        }

        [Fact]
        public void DevelopersController_Index_ReturnSucess()
        {
            //Arrange
            var developer = A.Fake<IEnumerable<Developer>>();
            A.CallTo(() => _service.GetAllAsync()).Returns(developer);
             
            //Act
            var result = _controller.Index();

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void DevelopersControllor_Details_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var developer = A.Fake<Developer>();
            A.CallTo(() => _service.GetByIdAsync(developer.Id)).Returns(developer);

            //Act
            var result = _controller.Details(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void DevelopersControllor_Edit_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var developer = A.Fake<Developer>();
            A.CallTo(() => _service.GetByIdAsync(developer.Id)).Returns(developer);

            //Act
            var result = _controller.Edit(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void DevelopersControllor_Delete_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var developer = A.Fake<Developer>();
            A.CallTo(() => _service.GetByIdAsync(developer.Id)).Returns(developer);

            //Act
            var result = _controller.Delete(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void DevelopersControllor_DeleteConfirmed_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var developer = A.Fake<Developer>();
            A.CallTo(() => _service.GetByIdAsync(developer.Id)).Returns(developer);

            //Act
            var result = _controller.DeleteConfirmed(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}