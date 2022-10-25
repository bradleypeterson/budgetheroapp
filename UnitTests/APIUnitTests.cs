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
            User newUser = ModelFactory.CreateUser();
            newUser.UserId = 2;

            // Act
            User existingUser = await APIServiceManager.GetUserAsync(newUser);

            // Assert
            Assert.True(existingUser.UserId == newUser.UserId);
        }
    }
}
