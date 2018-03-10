using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp.Models.ModelData
{
    class PhysicalAddress
    {
        public long AddressId { get; set; }
        public string BuildingShortName { get; set; }
        public string BuildingLongName { get; set; }
        public string BuildingRoomNumber { get; set; }
        public List<Note> Notes { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public PhysicalAddress() { Notes = new List<Note>() { }; }
    }
}
