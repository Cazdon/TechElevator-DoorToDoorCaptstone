using DoorToDoorLibrary.DatabaseObjects;
using DoorToDoorLibrary.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class AddTransactionViewModel
    {
        [Display(Name = "Amount")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [MaxLength(ErrorConsts.MaxCharFifty, ErrorMessage = ErrorConsts.MaxFiftyError)]
        public double Amount { get; set; }

        [Display(Name = "House")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        public int HouseID { get; set; }

        [Display(Name = "Product")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        public int ProductID { get; set; }
    }
}
