using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Identity
{
    public class User
    {
        public string Id { get; set; }
        [Required]

        public string UserName { get; set; }
      
        [EmailAddress]
        public string Email { get; set; }
      
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm Password")]
        //[Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        //public string ConfirmPassword { get; set; }

    }
}
