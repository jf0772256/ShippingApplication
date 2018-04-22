using shipapp.Connections.HelperClasses;
using shipapp.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shipapp.Connections.DataConnections.Classes
{
    /// <summary>
    /// Vendor connection interface access only through DataConnectionClass
    /// </summary>
    class VendorConnClass:DatabaseConnection
    {
        /// <summary>
        /// Form object sent.
        /// </summary>
        object Sender { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        public VendorConnClass():base() { }
        /// <summary>
        /// Gets single vendor object and returns
        /// </summary>
        /// <param name="id">Id to get</param>
        /// <returns>Vendor object</returns>
        public Vendors GetVendor(long id)
        {
            return GetVendor_From_Database(id);
        }
        /// <summary>
        /// Gets vendors as a list - binds them to the datagrids in respective forms
        /// </summary>
        /// <param name="sender">Form object</param>
        public async void GetVendorList(object sender = null)
        {
            if (String.IsNullOrWhiteSpace(DataConnectionClass.ConnectionString))
            {
                return;
            }
            Sender = sender;
            SortableBindingList<Vendors> vend = await Task.Run(() => GetVendorsList());
            if (Sender is Manage)
            {
                Manage t = (Manage)Sender;
                DataConnectionClass.DataLists.Vendors = vend;
                BindingSource bs = new BindingSource
                {
                    DataSource = DataConnectionClass.DataLists.Vendors
                };
                t.dataGridView1.DataSource = bs;
                try
                {
                    t.dataGridView1.Columns["VendorId"].Visible = false;
                }
                catch (Exception)
                {
                    //
                }
            }
            else
            {
                DataConnectionClass.DataLists.Vendors = vend;
            }
        }
        /// <summary>
        /// passes new vendor to method to write it to the database
        /// </summary>
        /// <param name="value">Vendor to add</param>
        public void AddVendor(Vendors value)
        {
            Write(value);
        }
        /// <summary>
        /// do not use yet.
        /// </summary>
        public void WriteAllVendors() { }
        /// <summary>
        /// Updates vendor in the database by calling the enclosed protected method to do so in the database
        /// </summary>
        /// <param name="value">Vendor object to update</param>
        public void UpdateVendor(Vendors value)
        {
            Update(value);
        }
        /// <summary>
        /// Deletes vendor from database
        /// </summary>
        /// <param name="v">Vendor to remove</param>
        public void DeleteVendor(Vendors v)
        {
            Delete(v);
        }
    }
}
