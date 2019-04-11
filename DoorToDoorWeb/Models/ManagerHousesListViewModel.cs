using DoorToDoorLibrary.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class ManagerHousesListViewModel
    {
        public IList<HouseItem> Houses { get; set; }
        public CreateHouseViewModel CreatedHouse { get; set; }
    }
}
