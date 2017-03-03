using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetControl.ViewModels
{
    public class UserViewModels
    {

    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }

    public class ChangePasswordViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public void Validate()
        {
            if (String.IsNullOrEmpty(this.Username))
            {
                throw new Exception("Username is required fields.");
            }
            if (String.IsNullOrEmpty(this.Password))
            {
                throw new Exception("Password is required fields.");
            }
            if (String.IsNullOrEmpty(this.NewPassword))
            {
                throw new Exception("New Password is required fields.");
            }
            //TODO: add validate for check newpassward same old
            //TODO: add validate for check mismatch new passoword
        }
    }

    public class LoginResult
    {
        public bool IsAuthority { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }


}