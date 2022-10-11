using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using System;
using System.Collections.Generic;
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
        }

        [Fact]
        public void Test1_CreateUser()
        {
            ResetTestingDatabase();

            // Arrange
            using var database = new TestDbContext();
            var newUser = ModelFactory.CreateUser();
            var result = 0;

            // Act
            database.Users.Add(newUser);
            result = database.SaveChanges();

            // Assert
            Assert.True(result == 1);
        }

        [Fact]
        public void Test2_ReadUser()
        {
            // Arrange
            using var database = new TestDbContext();

            // Act
            var existingUser = database.Users.FirstOrDefault();

            // Assert
            Assert.Equal(1, database.Users.Count());
        }

        [Fact]
        public void Test3_UpdateUser()
        {
            // Arrange
            using var database = new TestDbContext();
            var existingUser = database.Users.FirstOrDefault();

            // Act
            if (existingUser != null)
            {
                existingUser.FirstName = "Jane";
                database.SaveChanges();
            }

            // Assert
            Assert.True(database.Users.FirstOrDefault(u => u.FirstName == "John") is null);
            Assert.False(database.Users.FirstOrDefault(u => u.FirstName == "Jane") is null);
        }

        [Fact]
        public void Test4_DeleteUser()
        {
            // Arrange
            using var database = new TestDbContext();
            var existingUser = database.Users.FirstOrDefault();

            // Act
            if (existingUser != null)
            {
                database.Users.Remove(existingUser);
                database.SaveChanges();
            }

            // Assert
            Assert.False(database.Users.Any());
        }

        private static void ResetTestingDatabase()
        {
            using var context = new TestDbContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
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
