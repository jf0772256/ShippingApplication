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
using shipapp.Connections.DataConnections.Classes;

namespace shipapp.Connections.DataConnections
{
    class DataConnectionClass
    {
        public static SQLHelperClass SQLHelper { get; set; }
        public static SQLHelperClass.DatabaseType DBType { get; set; }
        public static Serialize Serialization { get; set; }
        public static string ConnectionString { get; set; }
        public static string EncodeString { get; set; }
        /// <summary>
        /// Tester connection class and its methods amd properties
        /// </summary>
        public static TestConnClass TestConn { get; set; }
        /// <summary>
        /// Users connection class and its methods amd properties
        /// </summary>
        public static UserConnClass UserConn { get; set; }
        /// <summary>
        /// Vendors connection class and its methods and properties
        /// </summary>
        public static VendorConnClass VendorConn { get; set; }
        /// <summary>
        /// Role Connection Class and its Methods Handle the add and update of roles
        /// </summary>
        public static RoleConnClass RoleConn { get; set; }
        /// <summary>
        /// Carrier Connection Class and its Methods Handle the add and update of roles
        /// </summary>
        public static CarrierConnClass CarrierConn { get; set; }
        /// <summary>
        /// Employee(faculty) connection class and its methods and properties
        /// </summary>
        public static EmployeeConnClass EmployeeConn { get; set; }
        /// <summary>
        /// Purchase Order connection class and its methods and properties
        /// </summary>
        public static PurchaseOrderConnectionClass POConnClass { get; set; }
        /// <summary>
        /// A collection of bindable lists of used classes especially for use with datagridviews and the database
        /// </summary>
        public static Lists DataLists { get; set; }
        /// <summary>
        /// User successfully authenticated
        /// </summary>
        public static bool SuccessAuthenticating { get; set; }
        /// <summary>
        /// Successfully athenticated user object for use with in the application
        /// </summary>
        public static User AuthenticatedUser { get; set; }

        static DataConnectionClass()
        {
            Serialization = new Serialize();
            SQLHelper = new SQLHelperClass();
            TestConn = new TestConnClass();
            UserConn = new UserConnClass();
            VendorConn = new VendorConnClass();
            RoleConn = new RoleConnClass();
            CarrierConn = new CarrierConnClass();
            EmployeeConn = new EmployeeConnClass();
            POConnClass = new PurchaseOrderConnectionClass();
            DataLists = new Lists();
        }
        public DataConnectionClass()
        {

        }
        /// <summary>
        /// Used to update database connection with a new connection string, this new value is saved.
        /// </summary>
        /// <param name="value">Saveable connectionstring in an asrray</param>
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
        /// <summary>
        /// Recovers connectionstring during application load to be used while the application is in operation.
        /// </summary>
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
        /// <summary>
        /// Processes user logout
        /// </summary>
        public static void LogUserOut()
        {
            AuthenticatedUser = null;
            SuccessAuthenticating = false;
            UserConn.Authenticate.Password = "";
            UserConn.Authenticate.UserName = "";
        }
    }
    /// <summary>
    /// Data lists for classes used by application.
    /// </summary>
    class Lists
    {
        /// <summary>
        /// List of Users (or in other words receiving employees)
        /// </summary>
        public BindingList<User> UsersList { get; set; }
        /// <summary>
        /// List of Carriers
        /// </summary>
        public BindingList<Carrier> CarriersList { get; set; }
        /// <summary>
        /// List of Faculty
        /// </summary>
        public BindingList<Faculty> FacultyList { get; set; }
        /// <summary>
        /// List of Vendors
        /// </summary>
        public BindingList<Vendors> Vendors { get; set; }
        /// <summary>
        /// List of Packages expected /or/ all
        /// </summary>
        public BindingList<Package> Packages { get; set; }
        /// <summary>
        /// Lists of all used classes (not including sub models or model helpers)
        /// </summary>
        public Lists()
        {
            UsersList = new BindingList<User>() { };
            CarriersList = new BindingList<Carrier>() { };
            FacultyList = new BindingList<Faculty>() { };
            Packages = new BindingList<Package>() { };
            Vendors = new BindingList<Vendors>() { };
        }
    }
}
