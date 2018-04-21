using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using shipapp.Models;
using shipapp.Models.ModelData;
using System.Windows.Forms;
using shipapp.Connections.HelperClasses;
using Extentions;

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
            string dte = FormatDateString(DateTime.Now.ToString());
            SortableBindingList<Package> pack = await Task.Run(() => Get_Package_List(dte));
            if (Sender is Manage)
            {
                pack.ForEach(i => { i.PackageDeliveredDate = ReturnUSStandardDateFormat(i.PackageDeliveredDate); i.PackageReceivedDate = ReturnUSStandardDateFormat(i.PackageReceivedDate); });
                Manage t = (Manage)Sender;
                DataConnectionClass.DataLists.Packages = pack;
                BindingSource bs = new BindingSource
                {
                    DataSource = DataConnectionClass.DataLists.Packages
                };
                t.dataGridView1.DataSource = bs;
            }
            else if (sender is Receiving)
            {
                pack.ForEach(i => { i.PackageDeliveredDate = ReturnUSStandardDateFormat(i.PackageDeliveredDate); i.PackageReceivedDate = ReturnUSStandardDateFormat(i.PackageReceivedDate); });
                Receiving t = (Receiving)Sender;
                DataConnectionClass.DataLists.Packages = pack;
                BindingSource bs = new BindingSource
                {
                    DataSource = DataConnectionClass.DataLists.Packages
                };
                t.dataGridPackages.DataSource = bs;
            }
            else
            {
                DataConnectionClass.DataLists.Packages = pack;
            }
        }
        /// <summary>
        /// Gets history list for past six months and uses yesterday and yesterday - 6 months
        /// </summary>
        /// <param name="sender">Form object to use when binding</param>
        public async void GetPackageHistoryList(object sender = null)
        {
            DateTime dt1 = DateTime.Today.AddDays(-1);
            DateTime dt2 = DateTime.Today.AddMonths(-6);
            Sender = sender;
            string test1 = FormatDateString(dt1.ToString()), test2 = FormatDateString(dt2.ToString());
            SortableBindingList<Package> hist = await Task.Run(() => Get_Package_List(test2, test1));
            if (Sender is Reports t)
            {
                DataConnectionClass.DataLists.PackageHistory = hist;
                BindingSource bs = new BindingSource
                {
                    DataSource = DataConnectionClass.DataLists.PackageHistory
                };
                t.datGridHistory.DataSource = bs;
            }
        }
        /// <summary>
        /// Gets history list from Yesterday to the specified end date
        /// </summary>
        /// <param name="enddate">Search end date as a string (Must be a valid date string)</param>
        /// <param name="sender">Form object to use when binding</param>
        public async void GetPackageHistoryList(string enddate, object sender = null)
        {
            DateTime dt1 = DateTime.Today.AddDays(-1);
            bool success = DateTime.TryParse(enddate, out DateTime dt2);
            Sender = sender;
            string test1 = FormatDateString(dt1.ToString()), test2 = FormatDateString(dt2.ToString());
            SortableBindingList<Package> hist = await Task.Run(() => Get_Package_List(test2, test1));
            if (Sender is Reports t)
            {
                DataConnectionClass.DataLists.PackageHistory = hist;
                BindingSource bs = new BindingSource
                {
                    DataSource = DataConnectionClass.DataLists.PackageHistory
                };
                t.datGridHistory.DataSource = bs;
            }
        }
        /// <summary>
        /// Gets history list from specified start date to the specified end date
        /// </summary>
        /// <param name="startdate">Search start date as a string (Must be a valid date string)</param>
        /// <param name="enddate">Search end date as a string (Must be a valid date string)</param>
        /// <param name="sender">Form object to use when binding</param>
        public async void GetPackageHistoryList(string startdate, string enddate, object sender = null)
        {
            bool s1 = DateTime.TryParse(startdate, out DateTime dt1);
            bool s2 = DateTime.TryParse(enddate, out DateTime dt2);
            Sender = sender;
            string test1 = FormatDateString(dt1.ToString()), test2 = FormatDateString(dt2.ToString());
            SortableBindingList<Package> hist = await Task.Run(() => Get_Package_List(test2, test1));
            if (Sender is Reports t)
            {
                DataConnectionClass.DataLists.PackageHistory = hist;
                BindingSource bs = new BindingSource
                {
                    DataSource = DataConnectionClass.DataLists.PackageHistory
                };
                t.datGridHistory.DataSource = bs;
            }
        }
        /// <summary>
        /// Adds a package to database
        /// </summary>
        /// <param name="p">New package object</param>
        public void AddPackage(Package p)
        {
            p.PackageReceivedDate = FormatDateString(p.PackageReceivedDate);
            p.PackageDeliveredDate = FormatDateString(p.PackageDeliveredDate);
            string dte = FormatDateString(DateTime.Today.ToShortDateString());
            Write(p,dte);
        }
        /// <summary>
        /// Updates a current package.
        /// </summary>
        /// <param name="p">Modified package object</param>
        public void UpdatePackage(Package p)
        {
            Update(p);
        }
        /// <summary>
        /// removes a database row from database
        /// </summary>
        /// <param name="p">package to remove</param>
        public void DeletePackage(Package p)
        {
            Delete(p);
        }
        /// <summary>
        /// Updates the selected package last modified date to todays date
        /// </summary>
        /// <param name="p">package to update</param>
        public void UpdateLastModified(Package p)
        {
            string dte = FormatDateString(DateTime.Today.ToShortDateString());
            base.Update(p, dte);
        }
        /// <summary>
        /// Updates The packages stored in a standard list last modified date to today.
        /// </summary>
        /// <param name="p">List of Package Objects</param>
        public void UpdateLastModified(List<Package> p)
        {
            string dte = FormatDateString(DateTime.Today.ToShortDateString());
            p.ForEach(i => Update(i, dte));
        }
        /// <summary>
        /// Formats the string from month/day/year to yyyy-mm-dd
        /// </summary>
        /// <param name="indate">US standard date notation</param>
        /// <returns>ISO date Formated yyyy-mm-dd</returns>
        private string FormatDateString(string indate)
        {
            DateTime dt = new DateTime();
            DateTime.TryParse(indate, out dt);
            string yr, mo, day;
            yr = dt.Year.ToString();
            mo = ((dt.Month > 9)?dt.Month.ToString():"0"+ dt.Month.ToString());
            day = ((dt.Day > 9) ? dt.Day.ToString() : "0" + dt.Day.ToString());
            return yr + "-" + mo + "-" + day;
        }
        /// <summary>
        /// Formats the date string from ISO date(yyyy-mm-dd) to US Stadard format(m/d/yyyy)
        /// </summary>
        /// <param name="indate">Date from datasource</param>
        /// <returns>formatted string for display</returns>
        private string ReturnUSStandardDateFormat(string indate)
        {
            DateTime dt = new DateTime();
            DateTime.TryParse(indate, out dt);
            string yr, mo, day;
            yr = dt.Year.ToString();
            mo = dt.Month.ToString();
            day = dt.Day.ToString();
            return mo + "/" + day + "/" + yr;
        }

        public List<Note> GetNotesList(string pid)
        {
            return GetNotesListById(pid);
        }
    }
}
