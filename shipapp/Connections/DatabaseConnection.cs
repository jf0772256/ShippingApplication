using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Xml.Linq;
using System.IO;
using shipapp.Connections.HelperClasses;

namespace shipapp.Connections
{
    class DatabaseConnection
    {
        #region Class Vars
        private string ConnString { get; set; }
        private SQLHelperClass.DatabaseType DBType { get; set; }
        #endregion
        #region Constructors and Tests
        /// <summary>
        /// Let us build the nessisary connection string for you.
        /// </summary>
        /// <param name="databaseType">Please tell us what database server type we will be connecting to.</param>
        protected DatabaseConnection(SQLHelperClass.DatabaseType t)
        {
            //uses a builder to make the strings
            SQLHelperClass helperClass = new SQLHelperClass();
            Serialize serialed = new Serialize();
            XDocument doc = new XDocument();
            DBType = t;
            int dbs = 0; string h = "", d = "", u = "", p = "", prt = "0";
            doc = XDocument.Load(Environment.CurrentDirectory + "\\Connections\\Assets\\settings.xml");
            XElement rt = doc.Root;
            XElement defdbcon = (XElement)rt.FirstNode;
            dbs = Convert.ToInt32(((XElement)(defdbcon.FirstNode)).Value);
            if (dbs == 1)
            {
                //deserialize values and continue;
                var dbnodes = defdbcon.FirstNode.NodesAfterSelf();
                foreach (XNode node in dbnodes)
                {
                    XElement temp = (XElement)node;
                    switch (temp.Name.ToString())
                    {
                        case "host":
                            h = serialed.DeSerializeValue(temp.Value.ToString());
                            break;
                        case "database_name":
                            d = serialed.DeSerializeValue(temp.Value.ToString());
                            break;
                        case "user_name":
                            u = serialed.DeSerializeValue(temp.Value.ToString());
                            break;
                        case "password":
                            p = serialed.DeSerializeValue(temp.Value.ToString());
                            break;
                        case "port":
                            prt = serialed.DeSerializeValue(temp.Value.ToString());
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                //not been serialized
                var dbelements = from ele in doc.Descendants("default_connection").Elements() select ele;
                foreach (XElement item in dbelements)
                {
                    switch (item.Name.ToString())
                    {
                        case "host":
                            h = item.Value.ToString();
                            item.SetValue(serialed.SerializeValue(h));
                            break;
                        case "database_name":
                            d = item.Value.ToString();
                            item.SetValue(serialed.SerializeValue(d));
                            break;
                        case "user_name":
                            u = item.Value.ToString();
                            item.SetValue(serialed.SerializeValue(u));
                            break;
                        case "password":
                            p = item.Value.ToString();
                            item.SetValue(serialed.SerializeValue(p));
                            break;
                        case "port":
                            prt = item.Value.ToString();
                            item.SetValue(serialed.SerializeValue(prt));
                            break;
                        case "file_is_serialized":
                            item.SetValue("1");
                            break;
                        default:
                            break;
                    }
                }
                //now I need to replace the values in doc to the new values...
                doc.Save(Environment.CurrentDirectory + "\\Connections\\Assets\\settings.xml");
            }
            ConnString = helperClass.SetDatabaseType(t).SetDBHost(h).SetDBName(d).SetUserName(u).SetPassword(p).SetPortNumber(Convert.ToInt32(prt)).BuildConnectionString().GetConnectionString();
        }
        /// <summary>
        /// Use this if you already have a db connection string written and worked out. Note though that we are using ODBC to connect so 
        /// you should probably check that 1: you have the odbc driver for teh sql server. 2: your connection string will work with your driver. and 3: you include your driver in the connection string properly.
        /// You can test your connection string using Test_Connection();
        /// </summary>
        /// <param name="connection_string">Pre built Connection string</param>
        /// <param name="t">Enum value for defining the database type that is being used.</param>
        protected DatabaseConnection(string connection_string, SQLHelperClass.DatabaseType t)
        {
            ConnString = connection_string;
            DBType = t;
        }
        /// <summary>
        /// Test connection strings here... must have a connection string in our system as well as a db type.
        /// Gives two replies, "Open" meaning connection success, or an error message with reasons and trace meaning connection failure.
        /// </summary>
        public void Test_Connection()
        {
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                try
                {
                    c.Open();
                    System.Windows.Forms.MessageBox.Show(c.State.ToString());
                    c.Close();
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Whoops there was an error...\n" + e.Message + "\n" + e.StackTrace + "\n" + ((e.InnerException is null) ? "" : e.InnerException.Message), "Error Connecting to Database");
                }
            }
        }
        #endregion
        #region General Database Methods
        //
        #endregion
    }
}
