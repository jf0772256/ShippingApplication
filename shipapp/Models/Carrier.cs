using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models.ModelData;

namespace shipapp.Models
{
    class Carrier
    {
        public long CarrierId { get; set; }
        public string CarrierName { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
        public List<Note> Notes { get; set; }
        public Carrier() { Notes = new List<Note>() { }; }
    }
}
