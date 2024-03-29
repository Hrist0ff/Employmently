﻿using employmently_be.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace employmently_be.Data.Models
{
    public class RegisterCompanyDto
    {
        [Required]

        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string CompanyId { get; set; }
    }
}
