﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AvioIndustrija.ViewModels
{
    public class RegisterViewModel
    {
        [StringLength(50)]
        [Required]
        public string Ime { get; set; }
        [StringLength(50)]
        [Required]
        public string Prezime { get; set; }
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is reqired")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is reqired")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
