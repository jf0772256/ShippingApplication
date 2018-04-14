using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models.ModelData;
using shipapp.Models;
using shipapp.Connections.HelperClasses;
using shipapp.Connections.DataConnections;
using System.Windows.Forms;

namespace shipapp.Connections.DataConnections.Classes
{
    class Database_Audit:DatabaseConnection
    {
        public Database_Audit() : base()
        {

        }
        /// <summary>
        /// Writes to audit log. NOTE time is captured in method no need to provide it.
        /// </summary>
        /// <param name="whodidit">Typically the logged in user</param>
        /// <param name="whatdidtheydo">The action that they did</param>
        public void AddRecordToAudit(string whatdidtheydo)
        {
            string who = DataConnectionClass.AuthenticatedUser.ToString();
            Write(whatdidtheydo, who, GetTimeStamp());
        }
        public async void GetAuditLog(object sender = null)
        {
            SortableBindingList<string> al = await Task.Run(() => Get_Audit_Log());
            if (sender is Manage)
            {
                Manage t = (Manage)sender;
                DataConnectionClass.DataLists.AuditLog = al;
                BindingSource bs = new BindingSource()
                {
                    DataSource = DataConnectionClass.DataLists.AuditLog
                };
                t.dataGridView1.DataSource = bs;
            }
            else
            {
                DataConnectionClass.DataLists.AuditLog = al;
            }
        }
        /// <summary>
        /// Gets the current time stamp, formats it and returns it as a string
        /// </summary>
        /// <returns>String of time stamp, date is short date format, with h:m:s.micro pretty printed</returns>
        private string GetTimeStamp()
        {
            string rval = "";
            DateTime dt = DateTime.Now;
            string h = "", i = "", s = "", n = "";
            h = dt.Hour.ToString();
            i = dt.Minute.ToString();
            s = dt.Second.ToString();
            n = dt.Millisecond.ToString();
            string tme = " at " + h + ":" + i + ":" + s + "." + n;
            string dte = dt.ToShortDateString();
            rval = dte + tme;
            return rval;
        }
    }
}
