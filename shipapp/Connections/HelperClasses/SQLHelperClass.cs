using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace shipapp.Connections.HelperClasses
{
    class SQLHelperClass
    {
        #region Properties
        private string HostAddress { get; set; }
        private string DatabaseName { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private int PortNumber { get; set; }
        private DatabaseType DatabaseConnectionType { get; set; }
        private string BuiltConnectionString { get; set; }
        #endregion
        #region setters
        public SQLHelperClass SetDBHost(string value)
        {
            HostAddress = value;
            return this;
        }
        public SQLHelperClass SetDBName(string value)
        {
            DatabaseName = value;
            return this;
        }
        public SQLHelperClass SetUserName(string value)
        {
            UserName = value;
            return this;
        }
        public SQLHelperClass SetPassword(string value)
        {
            Password = value;
            return this;
        }
        public SQLHelperClass SetPortNumber(int value)
        {
            PortNumber = value;
            return this;
        }
        public SQLHelperClass SetDatabaseType(DatabaseType value)
        {
            DatabaseConnectionType = value;
            return this;
        }
        #endregion
        #region Enums
        public enum DatabaseType
        {
            /// <summary>
            /// DO NOT USE THIS VALUE!!!
            /// </summary>
            Unset = 0,
            MSSQL = 1,
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
                throw new SQLHelperException("You must have a database type selected to connet to any databases. Acceptible data connections are MYSQL (its varients) and MSSQL 2016 or better. Please set this vaue by the chainable method SetDatabseType() and then get the connection string. Thank you.");
            }
            BuiltConnectionString = cs;
            return this;
        }
        public string GetConnectionString()
        {
            return BuiltConnectionString;
        }
        #endregion
    }

    class SQLHelperException:Exception
    {
        private string message;
        public string GetMessage()
        {
            return message;
        }
        public void SetMessage(string value)
        {
            message = value;
        }
        public SQLHelperException(string Message)
        {
            SetMessage(Message);
        }
    }
}
