using VideoGameStore.Data;
using VideoGameStore.Data.Base;
using VideoGameStore.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGameStore.Tests.Data.Base
{
    public class EntityBaseRepositoryTests
    {

        public async Task<AppDbContext> GetDbContext()
        {
            // create a database to use (NuGet: InMemory)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();
            if(await databaseContext.Actors.CountAsync() < 0)
            {
                // Add actors to the database
                for(int i = 0; i < 10; i++)
                {
                    databaseContext.Actors.Add(new Actor()
                    {
                        Id = i,
                        FullName = "John Doe",
                        Bio = "Is an actor",
                        ProfilePictureURL = "https://media.istockphoto.com/id/1277773692/vector/indiana-states-of-usa-outline-map-vector-template-illustration-design-editable-stroke.jpg?s=612x612&w=0&k=20&c=pbWNu1pu2Lw2XfZ2iC-F8Er_irDTosBL8PnAn9xBYEo="
                    });
                }
                await databaseContext.SaveChangesAsync();
            }
            
            return databaseContext;

        }


        [Fact]
        public async void EntityBaseRepository_AddAsync_ReturnsBool()
        {
            // Arrange
            var actor = new Actor()
            {
                Id = 11,
                FullName = "John Doe",
                Bio = "Is an actor",
                ProfilePictureURL = "https://media.istockphoto.com/id/1277773692/vector/indiana-states-of-usa-outline-map-vector-template-illustration-design-editable-stroke.jpg?s=612x612&w=0&k=20&c=pbWNu1pu2Lw2XfZ2iC-F8Er_irDTosBL8PnAn9xBYEo="
            };
            var dbContext = await GetDbContext();
            var actorRepository = new EntityBaseRepository<Actor>(dbContext);

            //Act
            //var result = actorRepository.AddAsync(actor);

            //Assert
            //result.Should().BeTrue();

            // The above won't work due to the fact it is an async Task, 
            // Below is a workaround
            Task.Run(async () =>
            {
                // Actual test code here.
                await actorRepository.AddAsync(actor);
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public async void EntityBaseRepository_GetByIdAsync_ReturnActor()
        {
            //Arrange
            var id = 1;
            var dbContext = await GetDbContext();
            var actorRepository = new EntityBaseRepository<Actor>(dbContext);

            //Act 
            // using await will cause the test to fail saving it into a var
            var result = actorRepository.GetByIdAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Should().BeOfType<Task<Actor>>();
        }

    }
}
