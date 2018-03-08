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
        private Connections.HelperClasses.SQLHelperClass.DatabaseType DatabaseType { get; set; }
        public Settings()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            Connections.DataConnections.DataConnectionClass.ConnectionString = Connections.DataConnections.DataConnectionClass.SQLHelper.SetDatabaseType(DatabaseType).SetDBHost(dbhost.Text).SetDBName(dbname.Text).SetUserName(dbuser.Text).SetPassword(dbpass.Text).SetPortNumber(Convert.ToInt32(dbport.Text)).BuildConnectionString().GetConnectionString();
            Connections.DataConnections.DataConnectionClass.DBType = DatabaseType;
            Connections.DataConnections.DataConnectionClass.TestConn.TestConnectionToDatabase();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Connections.DataConnections.DataConnectionClass.SaveDatabaseData(new string[] { Connections.DataConnections.DataConnectionClass.DBType.ToString(), Connections.DataConnections.DataConnectionClass.ConnectionString, Connections.DataConnections.DataConnectionClass.EncodeString });
        }
    }
}
