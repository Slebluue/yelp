using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace yelp.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Review> Reviews {get; set;}
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public User () {
            Reviews = new List<Review>();
        }

    }

    public class LoginViewModel : BaseEntity
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string LoginEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string LoginPassword { get; set; }
    }
    public class RegisterViewModel : BaseEntity
    {
        [Required(ErrorMessage = "You must enter a first name")]
        [MinLength(2, ErrorMessage = "First name must be at least two characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must enter a last name")]
        [MinLength(2, ErrorMessage = "Last name must be at least two characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You must enter an email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must enter a password")]
        [MinLength(8, ErrorMessage = "Password cannot be less than eight characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string RegPassword { get; set; }

        [Required(ErrorMessage = "Plase confirm password")]
        [Compare("RegPassword", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string RegConfirmPassword { get; set; }
    }

    public class LoginRegViewModel : BaseEntity
    {
        public LoginViewModel LoginVM { get; set; }
        public RegisterViewModel RegisterVM { get; set; }
    }
}