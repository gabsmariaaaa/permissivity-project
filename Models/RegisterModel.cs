﻿using System.ComponentModel.DataAnnotations;

namespace PermissivityProject.Models
{
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; } = "Agente"; 
    }
}
