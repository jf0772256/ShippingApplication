using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Connections.HelperClasses;
using System.Resources;
using System.Xml.Linq;
using shipapp.Models;
using shipapp.Models.ModelData;

namespace shipapp.Connections.DataConnections
{
    class DataConnectionClass
    {
        public static SQLHelperClass SQLHelper { get; set; }
        public static SQLHelperClass.DatabaseType DBType { get; set; }
        public static Serialize Serialization { get; set; }
        public static string ConnectionString { get; set; }
        public static string EncodeString { get; set; }
        public static TestConnClass TestConn { get; set; }
        public static UserConnClass UserConn { get; set; }
        public static Lists DataLists { get; set; }
        public static bool SuccessAuthenticating { get; set; }
        public static User AuthenticatedUser { get; set; }

        static DataConnectionClass()
        {
            Serialization = new Serialize();
            SQLHelper = new SQLHelperClass();
            TestConn = new TestConnClass();
            UserConn = new UserConnClass();
            DataLists = new Lists();
        }
        public DataConnectionClass()
        {

        }
        public static void SaveDatabaseData(string[] value)
        {
            try
            {
                ConnectionString = Serialization.DeSerializeValue(value[1]);
            }
            catch (Exception)
            {
                ConnectionString = value[1];
            }
            XDocument doc = new XDocument();
            doc = XDocument.Load(Environment.CurrentDirectory + "\\Connections\\Assets\\settings.xml");
            var dbelements = from ele in doc.Descendants("default_connections").Elements() select ele;
            foreach (XElement item in dbelements)
            {
                if (item.HasAttributes)
                {
                    if (item.FirstAttribute.Value == "master")
                    {
                        item.SetValue(Serialization.SerializeValue(value[0]));
                    }
                    else if(item.FirstAttribute.Value == value[0])
                    {
                        item.SetValue(Serialization.SerializeValue(value[1]));
                    }
                    else
                    {
                        item.SetValue("");
                    }
                }
            }
            var enc = from ele in doc.Descendants("strings").Elements() select ele;
            foreach (XElement strings in enc)
            {
                strings.SetValue(Serialization.SerializeValue(value[2]));
            }
            //now I need to replace the values in doc to the new values...
            doc.Save(Environment.CurrentDirectory + "\\Connections\\Assets\\settings.xml");
        }
        public static void GetDatabaseData()
        {
            XDocument doc = new XDocument();
            string filepath = Environment.CurrentDirectory + "\\Connections\\Assets\\settings.xml";
            doc = XDocument.Load(filepath);
            var dbelements = from ele in doc.Descendants("default_connections").Elements() select ele;
            foreach (XElement item in dbelements)
            {
                if (item.HasAttributes)
                {
                    if (item.FirstAttribute.Value == "master")
                    {
                        string test = Serialization.DeSerializeValue(item.Value);
                        if (test == SQLHelperClass.DatabaseType.MSSQL.ToString())
                        {
                            DBType = SQLHelperClass.DatabaseType.MSSQL;
                        }
                        else if (test == SQLHelperClass.DatabaseType.MySQL.ToString())
                        {
                            DBType = SQLHelperClass.DatabaseType.MySQL;
                        }
                        else
                        {
                            DBType = SQLHelperClass.DatabaseType.Unset;
                        }
                    }
                    else if (item.FirstAttribute.Value == "MSSQL")
                    {
                        if (!String.IsNullOrWhiteSpace(item.Value))
                        {
                            ConnectionString = Serialization.DeSerializeValue(item.Value);
                        }
                    }
                    else if (item.FirstAttribute.Value == "MySQL")
                    {
                        if (!String.IsNullOrWhiteSpace(item.Value))
                        {
                            ConnectionString = Serialization.DeSerializeValue(item.Value);
                        }
                    }
                    else
                    {
                        item.SetValue("");
                    }
                }
            }
            var enc = from ele in doc.Descendants("strings").Elements() select ele;
            foreach (XElement strings in enc)
            {
                EncodeString = Serialization.DeSerializeValue(strings.Value);
            }
            if (String.IsNullOrWhiteSpace(EncodeString))
            {
                EncodeString = Properties.Resources.backupstring;
            }
        }
        public static void LogUserOut()
        {
            AuthenticatedUser = null;
            SuccessAuthenticating = false;
            UserConn.Authenticate.Password = "";
            UserConn.Authenticate.UserName = "";
        }
    }
    class Lists
    {
        public BindingList<User> UsersList { get; set; }
        public BindingList<Carrier> CarriersList { get; set; }
        public BindingList<PhysicalAddress> BuildingsList { get; set; }
        public BindingList<Faculty> FacultyList { get; set; }
        static Lists()
        {
        }
        public Lists()
        {
            UsersList = new BindingList<User>() { };
            CarriersList = new BindingList<Carrier>() { };
            BuildingsList = new BindingList<PhysicalAddress>() { };
            FacultyList = new BindingList<Faculty>() { };
        }
    }
}
