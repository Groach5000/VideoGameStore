using VideoGameStore.Controllers;
using VideoGameStore.Data.Services;
using VideoGameStore.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace VideoGameStore.Tests
{
    public class ProducerControllerTests
    {

        private IProducersService _service;
        private ProducersController _controller;


        public ProducerControllerTests()
        {
            // Dependencies
            _service = A.Fake<IProducersService>();

            //SUT (System Under Test)
            _controller = new ProducersController(_service);
        }

        [Fact]
        public void ProducersController_Index_ReturnSucess()
        {
            //Arrange
            var producers = A.Fake<IEnumerable<Producer>>();
            A.CallTo(() => _service.GetAllAsync()).Returns(producers);
             
            //Act
            var result = _controller.Index();

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ProducersControllor_Details_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var producer = A.Fake<Producer>();
            A.CallTo(() => _service.GetByIdAsync(producer.Id)).Returns(producer);

            //Act
            var result = _controller.Details(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ProducersControllor_Edit_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var producer = A.Fake<Producer>();
            A.CallTo(() => _service.GetByIdAsync(producer.Id)).Returns(producer);

            //Act
            var result = _controller.Edit(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ProducersControllor_Delete_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var producer = A.Fake<Producer>();
            A.CallTo(() => _service.GetByIdAsync(producer.Id)).Returns(producer);

            //Act
            var result = _controller.Delete(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ProducersControllor_DeleteConfirmed_ReturnsSucess()
        {
            //Arrange
            var id = 1;
            var producer = A.Fake<Producer>();
            A.CallTo(() => _service.GetByIdAsync(producer.Id)).Returns(producer);

            //Act
            var result = _controller.DeleteConfirmed(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}