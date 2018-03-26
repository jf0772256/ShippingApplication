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
    class VendorConnClass:DatabaseConnection
    {
        public VendorConnClass():base() { }
        public Vendors GetVendor(long id)
        {
            return GetVendor_From_Database(id);
        }
        public async void GetVendorList()
        {
            DataConnectionClass.DataLists.Vendors = await Task.Run(()=>GetVendorsList());
        }
        public void AddVendor(Vendors value)
        {
            Write(value);
        }
        /// <summary>
        /// do not use yet.
        /// </summary>
        public void WriteAllVendors() { }
        public void UpdateVendor(Vendors value)
        {
            Update(value);
        }
        public void DeleteVendor(Vendors v)
        {
            Delete(v);
        }
    }
}
