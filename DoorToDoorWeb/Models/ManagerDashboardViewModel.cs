using DoorToDoorLibrary.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class ManagerDashboardViewModel
    {
        public IList<UserSalesCountItem> Transactions { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalIncome { get; set; }
    }
}
