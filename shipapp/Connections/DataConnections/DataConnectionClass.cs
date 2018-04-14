using System;
using System.Linq;
using shipapp.Connections.HelperClasses;
using System.Xml.Linq;
using shipapp.Models;
using shipapp.Models.ModelData;
using shipapp.Connections.DataConnections.Classes;

namespace shipapp.Connections.DataConnections
{
    class DataConnectionClass
    {
        internal static string Dbname { get; set; }
        public static SQLHelperClass SQLHelper { get; set; }
        public static SQLHelperClass.DatabaseType DBType { get; set; }
        public static Serialize Serialization { get; set; }
        public static string ConnectionString { get; set; }
        private static long PersonIdNumberCounter { get; set; }
        public static string PersonIdGenerated { get; set; }
        public static string EncodeString { get; set; }
        /// <summary>
        /// methods to handle the back up and restore of the databse data
        /// </summary>
        public static Backup_DB_Class Backup_DB { get; set; }
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
        /// Package connection class and its methods and properties
        /// </summary>
        public static PackageConnectionClass PackageConnClass { get; set; }
        /// <summary>
        /// A collection of available methods for buildings such as insert, delete and get lists
        /// </summary>
        public static BuildingConnClass buildingConn { get; set; }
        /// <summary>
        /// Handles all in and out from the database auditlog
        /// </summary>
        public static Database_Audit AuditLogConnClass { get; set; }
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
            PackageConnClass = new PackageConnectionClass();
            buildingConn = new BuildingConnClass();
            Backup_DB = new Backup_DB_Class();
            DataLists = new Lists();
        }
        public DataConnectionClass()
        {
            GetDatabaseData();
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
                    if (ConnectionString != null)
                    {
                        string[] cracked = ConnectionString.Split(';');
                        if (DBType == SQLHelperClass.DatabaseType.MSSQL)
                        {
                            string[] db = cracked[2].Split('=');
                            Dbname = db[1];
                        }
                        else if (DBType == SQLHelperClass.DatabaseType.MySQL)
                        {
                            if (cracked[2].Substring(0, 4) == "Port")
                            {
                                string[] db = cracked[3].Split('=');
                                Dbname = db[1];
                            }
                            else
                            {
                                string[] db = cracked[2].Split('=');
                                Dbname = db[1];
                            }
                        }
                        TestConn.Checktables();
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
        /// <summary>
        /// Fucntion to genetare the person id automatically, by passing a string to use as a prefix the function will append a numer to it and return it.
        /// </summary>
        /// <param name="str">some string value</param>
        public static void CreatePersonId(string str)
        {
            DatabaseConnection dbc = new DatabaseConnection(ConnectionString,EncodeString,DBType);
            long lastidnum = dbc.GetLastNumericalId();
            PersonIdNumberCounter = lastidnum += 1;
            PersonIdGenerated = str + PersonIdNumberCounter;
        }
        /// <summary>
        /// Saves the person id to the database idcounter. this will help make data safer
        /// </summary>
        public static void SavePersonId()
        {
            DatabaseConnection dbc = new DatabaseConnection(ConnectionString, EncodeString, DBType);
            dbc.Update(PersonIdNumberCounter, PersonIdGenerated);
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
        public SortableBindingList<User> UsersList { get; set; }
        /// <summary>
        /// List of Carriers
        /// </summary>
        public SortableBindingList<Carrier> CarriersList { get; set; }
        /// <summary>
        /// List of Faculty
        /// </summary>
        public SortableBindingList<Faculty> FacultyList { get; set; }
        /// <summary>
        /// List of Vendors
        /// </summary>
        public SortableBindingList<Vendors> Vendors { get; set; }
        /// <summary>
        /// List of Packages expected /or/ all
        /// </summary>
        public SortableBindingList<Package> Packages { get; set; }
        /// <summary>
        /// regular List of strings representing Faculty
        /// </summary>
        public SortableBindingList<BuildingClass> BuildingNames { get; set; }
        /// <summary>
        /// Packages received and or modifiesd from teh day before today to beginning of time
        /// </summary>
        public SortableBindingList<Package> PackageHistory { get; set; }
        /// <summary>
        /// Database Audit log list
        /// </summary>
        public SortableBindingList<string> AuditLog { get; set; }
        /// <summary>
        /// Lists of all used classes (not including sub models or model helpers)
        /// </summary>
        public Lists()
        {
            UsersList = new SortableBindingList<User>() { };
            CarriersList = new SortableBindingList<Carrier>() { };
            FacultyList = new SortableBindingList<Faculty>() { };
            Packages = new SortableBindingList<Package>() { };
            Vendors = new SortableBindingList<Vendors>() { };
            BuildingNames = new SortableBindingList<BuildingClass>() { };
            PackageHistory = new SortableBindingList<Package>() { };
            AuditLog = new SortableBindingList<string>() { };
        }
    }
}
