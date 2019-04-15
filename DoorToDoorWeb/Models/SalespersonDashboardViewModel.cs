using DoorToDoorLibrary.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class SalespersonDashboardViewModel
    {
        public IList<HouseDashboardItem> Houses { get; set; }
        public IList<ProductDashboardItem> Products { get; set; }
        public int MySales { get; set; }
        public double MyRevenue { get; set; }
    }
}
