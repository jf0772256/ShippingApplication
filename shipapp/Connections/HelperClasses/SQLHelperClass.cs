using System;

namespace shipapp.Connections.HelperClasses
{
    /// <summary>
    /// SQL Helper class is a class that is designed to assist with the building on connection string parameters that are passed from settings to build the connection string that will function with either mssql or mysql.
    /// </summary>
    class SQLHelperClass
    {
        #region Properties
        /// <summary>
        /// Location of db server
        /// </summary>
        private string HostAddress { get; set; }
        /// <summary>
        /// Database name to use
        /// </summary>
        private string DatabaseName { get; set; }
        /// <summary>
        /// user name to log in to the db server with
        /// </summary>
        private string UserName { get; set; }
        /// <summary>
        /// user name password for the user that is attempting to conect to the database
        /// </summary>
        private string Password { get; set; }
        /// <summary>
        /// Port number is optional but may be required in some cases
        /// </summary>
        private int PortNumber { get; set; }
        /// <summary>
        /// The only part of this class used outside connection string building: Sets the database type, used heavily with the Database connection class.
        /// </summary>
        private DatabaseType DatabaseConnectionType { get; set; }
        /// <summary>
        /// location to temp store the connection string until retreived and storted in a more permanant location.
        /// </summary>
        private string BuiltConnectionString { get; set; }
        #endregion
        #region setters
        /// <summary>
        /// Method that will set the dbhost
        /// </summary>
        /// <param name="value">DB location</param>
        /// <returns>SQLHelperClass</returns>
        public SQLHelperClass SetDBHost(string value)
        {
            HostAddress = value;
            return this;
        }
        /// <summary>
        /// Method that will set the dbname
        /// </summary>
        /// <param name="value">DB name</param>
        /// <returns>SQLHelperClass</returns>
        public SQLHelperClass SetDBName(string value)
        {
            DatabaseName = value;
            return this;
        }
        /// <summary>
        /// Method that will set the dbusername
        /// </summary>
        /// <param name="value">DB username</param>
        /// <returns>SQLHelperClass</returns>
        public SQLHelperClass SetUserName(string value)
        {
            UserName = value;
            return this;
        }
        /// <summary>
        /// Method that will set the dbuser password
        /// </summary>
        /// <param name="value">DB password</param>
        /// <returns>SQLHelperClass</returns>
        public SQLHelperClass SetPassword(string value)
        {
            Password = value;
            return this;
        }
        /// <summary>
        /// Method that will set the dbport
        /// </summary>
        /// <param name="value">DB Portnumber</param>
        /// <returns>SQLHelperClass</returns>
        public SQLHelperClass SetPortNumber(int value)
        {
            PortNumber = value;
            return this;
        }
        /// <summary>
        /// Method that will set the dbType
        /// </summary>
        /// <param name="value">DB type</param>
        /// <returns>SQLHelperClass</returns>
        public SQLHelperClass SetDatabaseType(DatabaseType value)
        {
            DatabaseConnectionType = value;
            return this;
        }
        #endregion
        #region Enums
        /// <summary>
        /// The database type that the application will be connecting to
        /// </summary>
        public enum DatabaseType
        {
            /// <summary>
            /// DO NOT USE THIS VALUE!!!
            /// </summary>
            Unset = 0,
            /// <summary>
            /// DB is a Microsoft SQL Server
            /// </summary>
            MSSQL = 1,
            /// <summary>
            /// DB is a MySQL Server
            /// </summary>
            MySQL = 2,
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Make sure that you set the rest of the needed parameters for the connection string otherwise you will have many errors.
        /// Required settings are Host, Database name and DatabaseConnectionType
        /// This Class has chainable methods, But this ine is not one, use the instance that you create to set connection string variables and it will return the connection string to you.
        /// </summary>
        public SQLHelperClass()
        {
            HostAddress = "";
            DatabaseName = "";
            UserName = "";
            Password = "";
            PortNumber = 0;
            DatabaseConnectionType = DatabaseType.Unset;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Builds connection string and prepares it to be used by the application
        /// </summary>
        /// <returns>SQLHelperClass</returns>
        public SQLHelperClass BuildConnectionString()
        {
            string cs = null;
            if (DatabaseConnectionType == DatabaseType.MSSQL)
            {
                //cs = "Driver={ODBC Driver 13 for SQL Server};Server=";
                cs = "Driver={SQL Server};Server=";
                cs += HostAddress;
                if (PortNumber > 0)
                {
                    cs += "," + PortNumber;
                }
                cs += ";Database=" + DatabaseName;
                cs += ";Uid=" + UserName;
                cs += ";Pwd=" + Password;
            }
            else if (DatabaseConnectionType == DatabaseType.MySQL)
            {
                cs = "Driver={MySQL ODBC 5.2 ANSI Driver};Server=";
                cs += HostAddress;
                if (PortNumber > 0)
                {
                    cs += ";Port=" + PortNumber;
                }
                cs += ";Database=" + DatabaseName;
                cs += ";Uid=" + UserName + ";Pwd=";
                cs += Password + ";Option=3";
            }
            else
            {
                throw new SQLHelperException("You must have a database type selected to connet to any databases. Acceptible data connections are MYSQL (its varients) and MSSQL 2016 or better. Please set this value by the chainable method SetDatabseType() and then get the connection string. Thank you.");
            }
            BuiltConnectionString = cs;
            return this;
        }
        /// <summary>
        /// Gets compiled connectionstring to test and or save in the main application
        /// </summary>
        /// <returns>String of the db connectionstring</returns>
        public string GetConnectionString()
        {
            return BuiltConnectionString;
        }
        #endregion
    }
    /// <summary>
    /// Exception handeler for exceptions when using this class
    /// </summary>
    class SQLHelperException:Exception
    {
        /// <summary>
        /// exception message
        /// </summary>
        private string message;
        /// <summary>
        /// Gets the message in this instance
        /// </summary>
        /// <returns>xstring</returns>
        public string GetMessage()
        {
            return message;
        }
        /// <summary>
        /// sets message for the exception
        /// </summary>
        /// <param name="value">message to display</param>
        private void SetMessage(string value)
        {
            message = value;
        }
        /// <summary>
        /// constructor to exception
        /// </summary>
        /// <param name="message">Message to use</param>
        public SQLHelperException(string message)
        {
            SetMessage(message);
        }
    }
}
