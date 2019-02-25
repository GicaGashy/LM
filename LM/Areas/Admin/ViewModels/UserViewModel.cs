using LM.Models.LM;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Areas.Admin.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name ="User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Please insert a correct password, min 6 Char, must include Upper & Lower Case, Number and a Special Character", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        public AppUser User { get; set; }
        public string UserId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select your team")]
        public int TeamId { get; set; }
        public Team Team { get; set; }

    }
}
