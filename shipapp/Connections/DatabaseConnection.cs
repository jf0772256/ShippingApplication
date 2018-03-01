using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Security;
using System.Xml;

namespace shipapp.Connections
{
    class DatabaseConnection
    {
        // Class level variables
        private string ConnString { get; set; }
        protected DatabaseConnection(string connection_url, string database_used, string user_name, string password)
        {
            ConnString = "Data Source=" + connection_url + ";Initial Catalog=" + database_used + ";User ID=" + user_name + ";Password=" + password;
        }
        protected DatabaseConnection(string connection_url, string database_used, string user_name, string password, int port)
        {
            ConnString = "Data Source=" + connection_url + ","+port+";Initial Catalog=" + database_used + ";User ID=" + user_name + ";Password=" + password;
        }
        public void OpenConnection()
        {
            using (SqlConnection c = new SqlConnection(ConnString))
            {
                c.Open();
                System.Windows.Forms.MessageBox.Show(c.State.ToString());
                c.Close();
            }
        }

        public void CloseConnection()
        {

        }

    }
}
