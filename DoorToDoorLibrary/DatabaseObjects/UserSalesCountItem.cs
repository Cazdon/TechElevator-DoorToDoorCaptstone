using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.DatabaseObjects
{
    public class UserSalesCountItem : BaseItem
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SalesCount { get; set; }
    }
}
