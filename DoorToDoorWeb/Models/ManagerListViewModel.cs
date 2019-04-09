using DoorToDoorLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class ManagerListViewModel
    {
        public IList<UserItem> Managers { get; set; }
    }
}
