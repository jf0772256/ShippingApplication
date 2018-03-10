using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models.ModelData;

namespace shipapp.Models
{
    class Vendors
    {
        public long VendorId { get; set; }
        public string VendorName { get; set; }
        public PhysicalAddress VendorAddress { get; set; }
        public string VendorPointOfContactName { get; set; }
        public PhoneNumber VendorPhone { get; set; }
        public List<Note> Notes { get; set; }
        public Vendors() { Notes = new List<Note>() { }; }
    }
}
