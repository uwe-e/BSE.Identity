﻿using System.ComponentModel.DataAnnotations;

namespace BSE.Identity.Blazor.Client.Models
{
    public class RegisterViewModel : LoginViewModel
    {
        //[Required]
        //public string? UserName
        //{
        //    get;
        //    set;
        //}

        public string? FirstName
        {
            get;
            set;
        }

        public string? LastName
        {
            get;
            set;
        }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword
        {
            get;
            set;
        }
    }
}