using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.DBContext;
using UnitTests.TestHelpers;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace UnitTests
{
    public class ModelUnitTests
    {
        private readonly ITestOutputHelper output;

        public ModelUnitTests(ITestOutputHelper output)
        {
            this.output = output;
            ResetTestingDatabase();
            SeedUserTable();
        }

        [Fact]
        public void CreateUser()
        {
            // Arrange
            using var database = new TestDbContext();
            var newUser = ModelFactory.CreateUser();
            var insertResult = 0;
            int expectedTotalRecords = 4;
            int actualTotalRecords = 0;

            // Act
            database.Users.Add(newUser);
            insertResult = database.SaveChanges();
            actualTotalRecords = database.Users.Count();

            // Assert
            Assert.True(insertResult == 1);
            Assert.True(actualTotalRecords == expectedTotalRecords);
        }

        [Fact]
        public void ReadUser()
        {
            // Arrange
            string expectedUserFirstName = "Michael";
            string actualUserFirstName = "Failed";
            using var database = new TestDbContext();

            // Act
            var existingUser = database.Users.FirstOrDefault(u => u.FirstName == expectedUserFirstName);
            if (existingUser != null)
                actualUserFirstName = existingUser.FirstName;

            // Assert
            Assert.True(actualUserFirstName == expectedUserFirstName);
        }

        [Fact]
        public void UpdateUser()
        {
            // Arrange
            string oldUserFirstName = "John";
            string newUserFirstName = "Jane";
            using var database = new TestDbContext();
            var existingUser = database.Users.FirstOrDefault();

            // Act
            if (existingUser != null)
            {
                existingUser.FirstName = newUserFirstName;
                database.SaveChanges();
            }

            // Assert
            Assert.True(database.Users.FirstOrDefault(u => u.FirstName == oldUserFirstName) is null);
            Assert.False(database.Users.FirstOrDefault(u => u.FirstName == newUserFirstName) is null);
        }

        [Fact]
        public void DeleteUser()
        {
            // Arrange
            string existingUserFirstName = "Dwight";
            using var database = new TestDbContext();
            var existingUser = database.Users.FirstOrDefault(u => u.FirstName == existingUserFirstName);

            // Act
            if (existingUser != null)
            {
                database.Users.Remove(existingUser);
                database.SaveChanges();
            }

            // Assert
            Assert.True(database.Users.FirstOrDefault(u => u.FirstName == existingUserFirstName) is null);
        }

        private static void ResetTestingDatabase()
        {
            using var context = new TestDbContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        private static void SeedUserTable()
        {
            using var database = new TestDbContext();

            var hasUsers = database.Users.Any();

            if (!hasUsers)
            {
                List<User> testUsers = ModelFactory.CreateUsers();

                testUsers.ForEach(u => database.Users.Add(u));
            }

            database.SaveChanges();
        }

        private void OutputModelData(List<User> users)
        {
            int counter = 0;

            foreach (User user in users)
            {
                output.WriteLine("----------| User: {0} |----------", ++counter);
                output.WriteLine("UserID: \t\t{0}", user.UserId);
                output.WriteLine("FirstName: \t\t{0}", user.FirstName);
                output.WriteLine("LastName: \t\t{0}", user.LastName);
                output.WriteLine("EmailAddress: \t{0}", user.EmailAddress);
                output.WriteLine("PercentageMod: \t{0}", user.PercentageMod);
                output.WriteLine("Username: \t\t{0}", user.Username);
                output.WriteLine("Password: \t\t{0}", user.Password);
                output.WriteLine("UserImageLink: \t{0}", user.UserImageLink);
                output.WriteLine("---------------------------------");
            }
        }
    }
}
