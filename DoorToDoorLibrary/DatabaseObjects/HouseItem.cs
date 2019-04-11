using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.DatabaseObjects
{
    public class HouseItem : BaseItem
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public int ManagerID { get; set; }
        public int AssignedSalespersonID { get; set; }
        public int StatusID { get; set; }
    }
}
