using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shipapp
{
    public partial class Settings : Form
    {
        /// <summary>
        /// Class level variables
        /// Objects
        /// </summary>
        private Connections.HelperClasses.SQLHelperClass.DatabaseType DatabaseType { get; set; }


        #region Setup
        /// <summary>
        /// Public constructor
        /// </summary>
        public Settings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On load center
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }
        #endregion

        #region Buttons
        /// <summary>
        /// Test the connection to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTest_Click(object sender, EventArgs e)
        {
            Connections.DataConnections.DataConnectionClass.ConnectionString = Connections.DataConnections.DataConnectionClass.SQLHelper.SetDatabaseType(DatabaseType).SetDBHost(dbhost.Text).SetDBName(dbname.Text).SetUserName(dbuser.Text).SetPassword(dbpass.Text).SetPortNumber(Convert.ToInt32(dbport.Text)).BuildConnectionString().GetConnectionString();
            Connections.DataConnections.DataConnectionClass.DBType = DatabaseType;
            Connections.DataConnections.DataConnectionClass.TestConn.TestConnectionToDatabase();
        }
        /// <summary>
        /// Create connection based on selected databse type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            comboBox2.SelectedItem = null;
            DatabaseType = Connections.HelperClasses.SQLHelperClass.DatabaseType.Unset;
            if (comboBox1.SelectedItem.ToString() == "MS SQL Server")
            {
                DatabaseType = Connections.HelperClasses.SQLHelperClass.DatabaseType.MSSQL;
            }
            else if (comboBox1.SelectedItem.ToString() == "MySQL")
            {
                DatabaseType = Connections.HelperClasses.SQLHelperClass.DatabaseType.MySQL;
            }
            else
            {
                DatabaseType = Connections.HelperClasses.SQLHelperClass.DatabaseType.Unset;
            }
        }
        /// <summary>
        /// Save the database connection, and close the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Connections.DataConnections.DataConnectionClass.AuditLogConnClass.AddRecordToAudit("added a new or changed exising database connection");
            Connections.DataConnections.DataConnectionClass.SaveDatabaseData(new string[] { Connections.DataConnections.DataConnectionClass.DBType.ToString(), Connections.DataConnections.DataConnectionClass.ConnectionString, Connections.DataConnections.DataConnectionClass.EncodeString });
            MessageBox.Show("The new Databae Connection has been saved!\r\n The application must restart for the new connection to take effect.\r\n The application will now Restart", "Database Connection Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            this.Close();
            Application.Restart();
        }
        /// <summary>
        /// Create manual backup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Connections.DataConnections.DataConnectionClass.AuditLogConnClass.AddRecordToAudit("created a manual database backup");
            Connections.HelperClasses.SQLHelperClass.DatabaseType type = (DatabaseType == Connections.HelperClasses.SQLHelperClass.DatabaseType.Unset) ? Connections.DataConnections.DataConnectionClass.DBType : DatabaseType;
            Connections.DataConnections.DataConnectionClass.Backup_DB.DoBackup(type);
        }
        /// <summary>
        /// Slect databse type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = null;
            DatabaseType = Connections.HelperClasses.SQLHelperClass.DatabaseType.Unset;
            if (comboBox2.SelectedItem.ToString() == "MS SQL Server")
            {
                DatabaseType = Connections.HelperClasses.SQLHelperClass.DatabaseType.MSSQL;
            }
            else if (comboBox2.SelectedItem.ToString() == "MySQL")
            {
                DatabaseType = Connections.HelperClasses.SQLHelperClass.DatabaseType.MySQL;
            }
            else
            {
                DatabaseType = Connections.HelperClasses.SQLHelperClass.DatabaseType.Unset;
            }
        }
        /// <summary>
        /// Restore database from backup file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            OpenSQLFile.InitialDirectory = Environment.CurrentDirectory + "\\Connections\\Backup";
            DialogResult dr = OpenSQLFile.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Connections.DataConnections.DataConnectionClass.AuditLogConnClass.AddRecordToAudit("recovered or restored the database");
                string fin = OpenSQLFile.FileName;
                Connections.DataConnections.DataConnectionClass.Backup_DB.RestoreDBBackup(fin);
            }
        }
        #endregion

        #region Functionality
       /// <summary>
       /// Close the form
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion


    }
}
