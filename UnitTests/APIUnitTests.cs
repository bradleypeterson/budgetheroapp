using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.TestHelpers;
using Xunit.Abstractions;

namespace UnitTests
{
    public class APIUnitTests
    {
        private readonly ITestOutputHelper output;

        public APIUnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task GetUserAsync()
        {
            // Arrange
            int userId = 2;

            // Act
            bool userWasRetrieved = await APITestHelper.GetAsync<User>($"/api/users/{userId}");

            // Assert
            Assert.True(userWasRetrieved);
        }

        [Fact]
        public async Task GetUsersAsync()
        {
            // Arrange
            string endpoint = $"/api/users";

            // Act
            bool usersWereRetrieved = await APITestHelper.GetAsync<List<User>>(endpoint);

            // Assert
            Assert.True(usersWereRetrieved);
        }

        [Fact]
        public async Task PostUserAsync()
        {
            // Arrange
            string endpoint = $"/api/users";
            User newUser = ModelFactory.CreateUser();
            newUser.UserId = 6;
            
            // Act
            bool userWasAdded = await APITestHelper.PostAsync<User>(endpoint, newUser);

            // Assert
            Assert.True(userWasAdded);
        }

        [Fact]
        public async Task PutUserAsync()
        {
            // Arrange
            int userId = 6;
            string newFirstName = "Jane";
            string endpoint = $"/api/users/{userId}";
            User user = ModelFactory.CreateUser();
            user.UserId = userId;
            user.FirstName = newFirstName;
            

            // Act
            bool userWasUpdated = await APITestHelper.PutAsync<User>(endpoint, user);

            // Assert
            Assert.True(userWasUpdated);
        }

        [Fact]
        public async Task DeleteUserAsync()
        {
            // Arrange
            int userId = 5;
            string endpoint = $"/api/users/{userId}";

            // Act
            bool userWasDeleted = await APITestHelper.DeleteAsync(endpoint);

            // Assert
            Assert.True(userWasDeleted);
        }
    }
}
