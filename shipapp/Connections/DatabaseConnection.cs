using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// <summary>
        /// Method to create tables should they not exist. Uses DBType enum value set in constructor to make tables for the correct format for the databases
        /// </summary>
        protected void Create_Tables()
        {
            List<string> cmdTxt = new List<string>() { };
            if (DBType == SQLHelperClass.DatabaseType.MySQL)
            {
                cmdTxt = new List<string>(){
                    "CREATE TABLE IF NOT EXISTS users(user_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, user_fname VARCHAR(50) NOT NULL, user_lname VARCHAR(50) NOT NULL, user_name VARCHAR(50) NOT NULL UNIQUE, user_password VARCHAR(50) NOT NULL);",
                    "CREATE TABLE IF NOT EXISTS roles(role_id BigINT NOT NULL PRIMARY KEY AUTO_INCREMENT, role_title VARCHAR(50) NOT NULL UNIQUE);",
                    "CREATE TABLE IF NOT EXISTS employees(empl_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, empl_fname VARCHAR(50) NOT NULL, empl_lname VARCHAR(50), empl_phone_id INT DEFAULT NULL, empl_addr_id INT DEFAULT NULL, empl_email_id INT DEFAULT NULL, empl_notes_id INT DEFAULT NULL);",
                    "CREATE TABLE IF NOT EXISTS vendors(vend_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT,vendor_name VARCHAR(50) NOT NULL UNIQUE, vendor_addr_id INT DEFAULT NULL,vendor_poc_name VARCHAR(50) DEFAULT NULL, vendor_phone_id INT DEFAULT NULL);",
                    "CREATE TABLE IF NOT EXISTS carriers(carrier_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, carrier_name VARCHAR(50) NOT NULL UNIQUE, carrier_phone_id INT DEFAULT NULL);",
                    "CREATE TABLE IF NOT EXISTS purchase_orders(po_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, po_number VARCHAR(25) DEFAULT NULL,po_package_count INT DEFAULT 0, po_created_on TIMESTAMP DEFAULT 0, po_created_by INT NOT NULL, po_approved_by INT NOT NULL);",
                    "CREATE TABLE IF NOT EXISTS packages(package_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT,package_po_id INT DEFAULT NULL, package_carrier_id INT NOT NULL, package_vendor_id INT NOT NULL, package_deliv_to_id INT NOT NULL, package_deliv_by_id INT DEFAULT NULL, package_signed_for_by_id INT DEFAULT NULL, package_tracking_number VARCHAR(50) DEFAULT NULL, package_receive_date DATE DEFAULT NULL, package_deliver_date DATE DEFAULT NULL, package_notes_id INT DEFAULT NULL);",
                    "CREATE TABLE IF NOT EXISTS email_addresses(email_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, person_id BIGINT NOT NULL, email_address VARCHAR(100) NOT NULL UNIQUE);",
                    "CREATE TABLE IF NOT EXISTS phone_numbers(phone_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, person_id BIGINT NOT NULL, phone_number VARCHAR(20) NOT NULL);",
                    "CREATE TABLE IF NOT EXISTS physical_addr(address_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, person_id BIGINT NOT NULL, addr_line1 VARCHAR(50) NOT NULL, addr_line2 VARCHAR(50) DEFAULT NULL, addr_city VARCHAR(50) NOT NULL, addr_state VARCHAR(2) NOT NULL, addr_zip VARCHAR(10) NOT NULL, addr_cntry VARCHAR(2) DEFAULT 'US');",
                    "CREATE TABLE IF NOT EXISTS notes(id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, note_id BIGINT NOT NULL, note_value VARCHAR(5000) NOT NULL);"
                };
            }
            else if (DBType == SQLHelperClass.DatabaseType.MSSQL)
            {
                //"IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = roles)CREATE TABLE ",
                cmdTxt = new List<string>(){
                    //attempt to create the first table as a test;;
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = users)CREATE TABLE users(user_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, user_fname VARCHAR(50) NOT NULL, user_lname VARCHAR(50) NOT NULL, user_name VARCHAR(50) NOT NULL UNIQUE, user_password VARCHAR(50) NOT NULL);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = roles)CREATE TABLE roles(role_id BigINT NOT NULL IDENTITY(1,1) PRIMARY KEY, role_title VARCHAR(50) NOT NULL, CONSTRAINT UC_Roles UNIQUE(role_title));",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = employees)CREATE TABLE employees(empl_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, empl_fname VARCHAR(50) NOT NULL, empl_lname VARCHAR(50), empl_phone_id INT DEFAULT NULL, empl_addr_id INT DEFAULT NULL, empl_email_id INT DEFAULT NULL, empl_notes_id INT DEFAULT NULL);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = vendors)CREATE TABLE vendors(vend_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, vendor_name VARCHAR(50) NOT NULL UNIQUE, vendor_addr_id INT DEFAULT NULL, vendor_poc_name VARCHAR(50) DEFAULT NULL, vendor_phone_id INT DEFAULT NULL);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = carriers)CREATE TABLE carriers(carrier_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, carrier_name VARCHAR(50) NOT NULL UNIQUE, carrier_phone_id INT DEFAULT NULL);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = purchase_orders)CREATE TABLE purchase_orders(po_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, po_number VARCHAR(25) DEFAULT NULL,po_package_count INT DEFAULT 0, po_created_on TIMESTAMP DEFAULT 0, po_created_by INT NOT NULL, po_approved_by INT NOT NULL);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = packages)CREATE TABLE packages(package_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,package_po_id INT DEFAULT NULL, package_carrier_id INT NOT NULL, package_vendor_id INT NOT NULL, package_deliv_to_id INT NOT NULL, package_deliv_by_id INT DEFAULT NULL, package_signed_for_by_id INT DEFAULT NULL, package_tracking_number VARCHAR(50) DEFAULT NULL, package_receive_date DATE DEFAULT NULL, package_deliver_date DATE DEFAULT NULL, package_notes_id INT DEFAULT NULL);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = email_addresses)CREATE TABLE email_addresses(email_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, person_id BIGINT NOT NULL, email_address VARCHAR(100) NOT NULL, CONSTRAINT UC_Email UNIQUE);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = phone_numbers)CREATE TABLE phone_numbers(phone_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, person_id BIGINT NOT NULL, phone_number VARCHAR(20) NOT NULL);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = physical_addr)CREATE TABLE physical_addr(address_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, person_id BIGINT NOT NULL, addr_line1 VARCHAR(50) NOT NULL, addr_line2 VARCHAR(50) DEFAULT NULL, addr_city VARCHAR(50) NOT NULL, addr_state VARCHAR(2) NOT NULL, addr_zip VARCHAR(10) NOT NULL, addr_cntry VARCHAR(2) DEFAULT 'US');",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = notes)CREATE TABLE notes(id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, note_id BIGINT NOT NULL, note_value VARCHAR(5000) NOT NULL);"
                };
            }
            //out side all conditions available
            else
            {
                cmdTxt = new List<string>() { };
                DatabaseConnectionException exc = new DatabaseConnectionException("You have not set a correct database type.");
            }
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                OdbcTransaction tr = c.BeginTransaction();
                using (OdbcCommand cmd = new OdbcCommand("",c,tr))
                {
                    try
                    {
                        foreach (string query in cmdTxt)
                        {
                            cmd.CommandText = query;
                            cmd.ExecuteNonQuery();
                        }
                        cmd.Transaction.Commit();
                        //for  confirmation that the tables had been created...
                        cmd.CommandText = "SELECT [name] FROM sys.tables;";
                        string message = "Tables Created::\n";
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                message += reader["name"].ToString() + "/n";
                            }
                            System.Windows.Forms.MessageBox.Show(message, "creation results");
                        }

                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        DatabaseConnectionException exc = new DatabaseConnectionException("", e);
                    }
                }
            }
        }
        #endregion
    }
    internal class DatabaseConnectionException
    {
        private string Message { get; set; }
        private Exception Inner { get; set; }
        public DatabaseConnectionException()
        {
            ThrowException();
        }
        public DatabaseConnectionException(string message)
        {
            Message = message;
            ThrowException();
        }
        public DatabaseConnectionException(string message,Exception insideException)
        {
            Message = message;
            Inner = insideException;
            ThrowException();
        }
        public void ThrowException()
        {
            if (String.IsNullOrWhiteSpace(Message))
            {
                Message = "An error has occured at some point. Please accept our appologies.";
            }
            if (Inner is null)
            {
                throw new Exception(Message);
            }
            else
            {
                throw new Exception(Message, Inner);
            }
        }
    }
}
