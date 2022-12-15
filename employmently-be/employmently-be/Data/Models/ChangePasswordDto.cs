﻿using System.ComponentModel.DataAnnotations;

namespace employmently_be.Data.Models
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Password is required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        
        [Required(ErrorMessage = "Password is required")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string NewPasswordRepeat { get; set; }
    }
}