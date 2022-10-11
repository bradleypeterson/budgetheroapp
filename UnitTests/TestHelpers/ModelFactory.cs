using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.TestHelpers
{
    public class ModelFactory
    {
        public static User CreateUser()
        {
            return new User()
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "jdoe@example.com",
                PercentageMod = 20,
                Username = "doeman",
                Password = "123456",
                UserImageLink = "https://www.fakepicturelink.com/the_doeman.jpg"
            };
        }
    }
}
