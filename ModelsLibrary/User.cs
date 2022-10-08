using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ModelsLibrary
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public double? PercentageMod { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? UserImageLink { get; set; }
    }
}
