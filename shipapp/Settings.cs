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
            Connections.DataConnections.TestConnClass tc = new Connections.DataConnections.TestConnClass(dbhost.Text,dbname.Text,dbuser.Text,dbpass.Text,dbport.Text,DatabaseType);
            tc.TestConnectionToDatabase();
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
    }
}
