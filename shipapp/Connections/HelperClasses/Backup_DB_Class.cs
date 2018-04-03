using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        }
        /// <summary>
        /// Manually trigger async back up process -- will not interfere.
        /// </summary>
        public async void ManualDBBackup()
        {

        }
        /// <summary>
        /// process will run sync with main application.
        /// meaning this will block user from main application from continuing until completed.
        /// </summary>
        public void RestoreDBBackup()
        {

        }
    }
}
