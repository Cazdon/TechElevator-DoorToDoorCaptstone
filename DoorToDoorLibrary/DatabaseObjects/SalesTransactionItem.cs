using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorToDoorLibrary.Models
{
    public class SalesTransactionItem : BaseItem
    {
        public DateTime Date { get; set; }
        public int HouseID { get; set; }
        public int ProductID { get; set; }
        public int SalesmenID { get; set; }
        public double Amount { get; set; }
    }
}
