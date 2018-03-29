using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models;
using shipapp.Models.ModelData;
using System.Windows.Forms;
using shipapp.Connections.HelperClasses;

namespace shipapp.Connections.DataConnections.Classes
{
    class PackageConnectionClass:DatabaseConnection
    {
        object Sender { get; set; }
        public PackageConnectionClass() : base() { }
        /// <summary>
        /// Gets specified package by id 
        /// </summary>
        /// <param name="id">package id as long</param>
        /// <returns>requested package object</returns>
        public Package GetPackage(long id)
        {
            return Get_Package(id);
        }
        /// <summary>
        /// Gets and sets Datalist.packagelist
        /// </summary>
        public async void GetPackageList(object sender = null)
        {
            if (String.IsNullOrWhiteSpace(DataConnectionClass.ConnectionString))
            {
                return;
            }
            Sender = sender;
            SortableBindingList<Package> pack = await Task.Run(() => Get_Package_List());
            if (!((Manage)Sender is null))
            {
                Manage t = (Manage)Sender;
                DataConnectionClass.DataLists.Packages = pack;
                BindingSource bs = new BindingSource
                {
                    DataSource = DataConnectionClass.DataLists.Packages
                };
                t.dataGridView1.DataSource = bs;
            }
        }
        /// <summary>
        /// Adds a package to database
        /// </summary>
        /// <param name="p">New package object</param>
        public void AddPackage(Package p)
        {
            Write(p);
        }
        /// <summary>
        /// Updates a current package.
        /// </summary>
        /// <param name="p">Modified package object</param>
        public void UpdatePackage(Package p)
        {
            Update(p);
        }
        public void DeletePackage(Package p)
        {
            Delete(p);
        }
    }
}
