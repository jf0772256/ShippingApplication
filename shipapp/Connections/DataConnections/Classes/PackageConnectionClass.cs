using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models;
using shipapp.Models.ModelData;

namespace shipapp.Connections.DataConnections.Classes
{
    class PackageConnectionClass:DatabaseConnection
    {
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
        public async void GetPackageList()
        {
            await Task.Run(()=>Get_Package_List());
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
