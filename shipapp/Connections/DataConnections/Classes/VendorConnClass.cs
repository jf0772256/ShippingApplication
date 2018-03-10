using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models;

namespace shipapp.Connections.DataConnections.Classes
{
    /// <summary>
    /// Vendor connection interface access only through DataConnectionClass
    /// </summary>
    class VendorConnClass
    {
        public VendorConnClass() { }
        public Vendors GetVendor(long id)
        {
            return new Vendors()
            {
                //
            };
        }
        public void GetVendorList() { }
        public void AddVendor(Vendors value) { }
        public void WriteAllVendors() { }
        public void UpdateVendor(long id) { }
    }
}
