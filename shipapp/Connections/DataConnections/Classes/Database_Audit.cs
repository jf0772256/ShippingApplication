using shipapp.Connections.HelperClasses;
using shipapp.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace shipapp.Connections.DataConnections.Classes
{
    /// <summary>
    /// Db audit class
    /// </summary>
    class Database_Audit:DatabaseConnection
    {
        /// <summary>
        /// Constructor
        /// </summary>
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
            string[] goodsgoods = GetTimeStamp();
            Write(whatdidtheydo, who, goodsgoods[0], goodsgoods[1]);
        }
        /// <summary>
        /// Gets the audit log
        /// </summary>
        /// <param name="sender">Form object</param>
        public async void GetAuditLog(object sender = null)
        {
            SortableBindingList<AuditItem> al = await Task.Run(() => Get_Audit_Log());
            foreach (AuditItem ai in al)
            {
                string[] pd = UpdateTSFMT(ai.Date, ai.Time);
                ai.Date = pd[0];
                ai.Time = pd[1];
            }
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
        private string[] GetTimeStamp()
        {
            string[] rval = new string[2];
            DateTime dt = DateTime.Now;
            string h = "", i = "", s = "", n = "";
            string dte = dt.ToShortDateString();
            string[] d = dte.Split('/');
            h = dt.Hour.ToString();
            i = dt.Minute.ToString();
            s = dt.Second.ToString();
            n = dt.Millisecond.ToString();
            //add filler to dates if less than 10 am or more than 12 am or dates  reason for this is the sorting 
            if (Convert.ToInt32(h) <= 9)
            {
                h = "0" + h;
            }
            if (Convert.ToInt32(i) <= 9)
            {
                i = "0" + i;
            }
            if (Convert.ToInt32(s) <= 9)
            {
                s = "0" + s;
            }
            if (Convert.ToInt32(n) <= 9)
            {
                n = "0" + n;
            }
            if (Convert.ToInt32(d[0]) <= 9)
            {
                d[0] = "0" + d[0];
            }
            if (Convert.ToInt32(d[1]) <= 9)
            {
                d[1] = "0" + d[1];
            }
            if (Convert.ToInt32(d[2]) <= 9)
            {
                d[2] = "0" + d[2];
            }
            dte = d[0] + "/" + d[1] + "/" + d[2];
            string tme =  h + ":" + i + ":" + s + "." + n;
            rval[0] = dte;
            rval[1] = tme;
            return rval;
        }
        /// <summary>
        /// This information on the history section to ensure that the values are confirmed and standard
        /// </summary>
        /// <param name="date">string to mm/dd/yy or mm/dd/yyyy and not m/d/yy m/d/yyyy</param>
        /// <param name="time">string to HH:mm:ss.nn</param>
        /// <returns>string array 0=date, 1=time</returns>
        private string[] UpdateTSFMT(string date, string time)
        {
            string[] rval = new string[2];
            string[] d = date.Split('/');
            string[] t = time.Split(':', '.');
            if (Convert.ToInt32(d[0]) <= 9)
            {
                d[0] = "0" + d[0];
            }
            if (Convert.ToInt32(d[1]) <= 9)
            {
                d[1] = "0" + d[1];
            }
            if (Convert.ToInt32(d[2]) <= 9)
            {
                d[2] = "0" + d[2];
            }
            if (Convert.ToInt32(t[0]) <= 9)
            {
                t[0] = "0" + t[0];
            }
            if (Convert.ToInt32(t[1]) <= 9)
            {
                t[1] = "0" + t[1];
            }
            if (Convert.ToInt32(t[2]) <= 9)
            {
                t[2] = "0" + t[2];
            }
            date = d[0] + "/" + d[1] + "/" + d[2];
            time = t[0] + ":" + t[1] + ":" + t[2] + "." + t[3];
            rval[0] = date;
            rval[1] = time;
            return rval;
        }
    }
}