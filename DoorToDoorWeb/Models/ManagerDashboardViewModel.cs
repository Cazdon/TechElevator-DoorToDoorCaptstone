using DoorToDoorLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class ManagerDashboardViewModel
    {
        public IList<UserItem> Salesmen { get; set; }
        public IList<ProductItem> Products { get; set; }
        public IList<SalesTransactionItem> Transactions { get; set; }
    }
}
