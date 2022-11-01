using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.TestHelpers;

public class ModelFactory
{
    public static User CreateUser()
    {
        return new User()
        {
            FirstName =     "John",
            LastName =      "Doe",
            EmailAddress =  "jdoe@example.com",
            PercentageMod = 20,
            Username =      "doeman",
            Password =      "123456",
            UserImageLink = "https://www.fakepicturelink.com/the_doeman.jpg"
        };
    }

    public static List<User> CreateUsers()
    {
        List<User> users = new()
        {
            new User()
            {
                FirstName =     "Michael",
                LastName =      "Scott",
                EmailAddress =  "mscott@dundermifflinpaperco.com",
                PercentageMod = 50,
                Username =      "agentscarn",
                Password =      "hackme123",
                UserImageLink = "https://www.fakepicturelink.com/agent_scarn.jpg"
            },
            new User()
            {
                FirstName =     "Dwight",
                LastName =      "Schrute",
                EmailAddress =  "dshrute@dundermifflinpaperco.com",
                PercentageMod = 30,
                Username =      "beatfarmer",
                Password =      "ilovepaper",
                UserImageLink = "https://www.fakepicturelink.com/beat_farmer.jpg"
            },
            new User()
            {
                FirstName =     "Jim",
                LastName =      "Halpert",
                EmailAddress =  "jhalpert@dundermifflinpaperco.com",
                PercentageMod = 10,
                Username =      "tuna",
                Password =      "dwightisajoke",
                UserImageLink = "https://www.fakepicturelink.com/bored.jpg"
            },
        };

        return users;
    }
}
