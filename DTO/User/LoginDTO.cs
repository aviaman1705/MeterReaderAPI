﻿using System.ComponentModel.DataAnnotations;

namespace MeterReaderAPI.DTO.User
{
    public class LoginDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
