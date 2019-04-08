using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class Salesperson
    {
        public int SalespersonID { get; set; }

        [Required(ErrorMessage = "*")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "*")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*")]
        public string ConfirmPassword { get; set; }
    }
}

//- Add Salesperson fields:
//- - First Name
//- - - Required
//- - - Max Length: 50 characters
//- - Last Name
//- - - Required
//- - - Max Length: 50 characters
//- - Email Address
//- - - Required
//- - - Unique
//- - - Max Length: 100 characters
//- - Password
//- - - Required
//- - Confirm Password
//- - - Required
//- - - Must match Password
