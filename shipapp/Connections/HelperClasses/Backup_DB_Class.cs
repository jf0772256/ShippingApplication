using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Connections.DataConnections;
using shipapp.Connections.HelperClasses;

namespace shipapp.Connections.HelperClasses
{
    /// <summary>
    /// This will be the class that will manage the connectivity and database backup.
    /// </summary>
    class Backup_DB_Class : DatabaseConnection
    {
        /// <summary>
        /// Set default connector
        /// </summary>
        public Backup_DB_Class() : base()
        {
        }
        /// <summary>
        /// on start tuesday, thursday do back up
        /// else do nothing, unless prompt
        /// </summary>
        public async void CheckToDoBackup()
        {
            DateTime dt = DateTime.Now;
            if (dt.DayOfWeek == DayOfWeek.Tuesday || dt.DayOfWeek == DayOfWeek.Thursday )
            {
                await Task.Run(() => DoBackup(DataConnectionClass.DBType));
            }
        }
        /// <summary>
        /// Manually trigger async back up process -- will not interfere.
        /// </summary>
        public async void ManualDBBackup(SQLHelperClass.DatabaseType type)
        {
            await Task.Run(() => DoBackup(type));
        }
        /// <summary>
        /// process will run sync with main application.
        /// meaning this will block user from main application from continuing until completed.
        /// </summary>
        /// <param name="filepathandname">File path for file to open and read</param>
        public void RestoreDBBackup(string filepathandname)
        {

        }
    }
}
