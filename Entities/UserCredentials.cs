﻿using System.ComponentModel.DataAnnotations;

namespace MeterReaderAPI.Entities
{
    public class UserCredentials
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
