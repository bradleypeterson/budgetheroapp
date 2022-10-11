﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ModelsLibrary
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public double? PercentageMod { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? UserImageLink { get; set; }
    }
}