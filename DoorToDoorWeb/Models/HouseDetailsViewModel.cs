using DoorToDoorLibrary.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class HouseDetailsViewModel
    {
        public HouseItem House { get; set; }
        public IList<NoteItem> Notes { get; set; }
        public AddHouseNoteViewModel AddNote { get; set; }
    }
}
