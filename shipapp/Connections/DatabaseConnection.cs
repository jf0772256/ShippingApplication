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
using shipapp.Connections.DataConnections;
using shipapp.Models;

namespace shipapp.Connections
{
    class DatabaseConnection
    {
        #region Class Vars
        private string ConnString { get; set; }
        private SQLHelperClass.DatabaseType DBType { get; set; }
        private string EncodeKey { get; set; }
        private Serialize Serialization { get; set; }
        private SQLHelperClass SQLHelper { get; set; }
        #endregion
        #region Constructors and Tests
        /// <summary>
        /// Uses globalclass connection string or string builder for tasks
        /// </summary>
        protected DatabaseConnection()
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
        }
        /// <summary>
        /// Test connection strings here... must have a connection string in our system as well as a db type.
        /// Gives two replies, "Open" meaning connection success, or an error message with reasons and trace meaning connection failure.
        /// </summary>
        protected void Test_Connection(string testConstring)
        {
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = testConstring;
                try
                {
                    c.Open();
                    System.Windows.Forms.MessageBox.Show(c.State.ToString());
                    c.Close();
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Whoops there was an error...\n" + e.Message + "\n" + ((e.InnerException is null) ? "" : e.InnerException.Message), "Error Connecting to Database");
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
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            List<string> cmdTxt = new List<string>() { };
            if (DBType == SQLHelperClass.DatabaseType.MySQL)
            {
                cmdTxt = new List<string>(){
                    "CREATE TABLE IF NOT EXISTS roles(role_id BigINT NOT NULL PRIMARY KEY AUTO_INCREMENT, role_title VARCHAR(100) NOT NULL UNIQUE)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS email_addresses(email_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, person_id BIGINT NOT NULL, email_address VARCHAR(100) NOT NULL UNIQUE)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE INDEX idx_addr_ids ON email_addresses(person_id);",
                    "CREATE TABLE IF NOT EXISTS phone_numbers(phone_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, person_id BIGINT NOT NULL, phone_number VARCHAR(20) NOT NULL)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE INDEX idx_phone_ids ON phone_numbers(person_id);",
                    "CREATE TABLE IF NOT EXISTS physical_addr(address_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, person_id BIGINT NOT NULL, addr_line1 VARCHAR(100) NOT NULL, addr_line2 VARCHAR(50) DEFAULT NULL, addr_city VARCHAR(100) NOT NULL, addr_state VARCHAR(2) NOT NULL, addr_zip VARCHAR(10) NOT NULL, addr_cntry VARCHAR(2) DEFAULT 'US')engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE INDEX idx_addr_ids ON physical_addr(person_id);",
                    "CREATE TABLE IF NOT EXISTS notes(id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, note_id BIGINT NOT NULL, note_value VARCHAR(5000) NOT NULL)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE INDEX idx_note_ids ON notes(note_id);",
                    "CREATE TABLE IF NOT EXISTS users(user_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, user_fname VARCHAR(100) NOT NULL, user_lname VARCHAR(100) NOT NULL, user_name VARCHAR(100) NOT NULL UNIQUE, user_password VARBINARY(500) NOT NULL, user_role_id BIGINT, FOREIGN KEY (user_role_id) REFERENCES roles(role_id) ON DELETE NO ACTION ON UPDATE NO ACTION)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS employees(empl_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, empl_fname VARCHAR(100) NOT NULL, empl_lname VARCHAR(100), empl_phone_id BIGINT, empl_addr_id BIGINT, empl_email_id BIGINT, empl_notes_id BIGINT)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS vendors(vend_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT,vendor_name VARCHAR(100) NOT NULL UNIQUE, vendor_addr_id BIGINT,vendor_poc_name VARCHAR(100) DEFAULT NULL, vendor_phone_id BIGINT)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS carriers(carrier_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, carrier_name VARCHAR(100) NOT NULL UNIQUE, carrier_phone_id BIGINT)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS purchase_orders(po_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, po_number VARCHAR(25) DEFAULT NULL,po_package_count INT DEFAULT 0, po_created_on DATETIME, po_created_by BIGINT, po_approved_by BIGINT)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS packages(package_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT,package_po_id BIGINT, package_carrier_id BIGINT, package_vendor_id BIGINT, package_deliv_to_id BIGINT, package_deliv_by_id BIGINT, package_signed_for_by_id BIGINT, package_tracking_number VARCHAR(50) DEFAULT NULL, package_receive_date DATE, package_deliver_date DATE, package_notes_id BIGINT)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                };
            }
            else if (DBType == SQLHelperClass.DatabaseType.MSSQL)
            {
                //"IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = roles)CREATE TABLE ",
                cmdTxt = new List<string>(){
                    //attempt to create the first table as a test;;
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'roles')CREATE TABLE roles(role_id BigINT NOT NULL IDENTITY(1,1) PRIMARY KEY, role_title VARCHAR(50) NOT NULL, CONSTRAINT UC_Roles UNIQUE(role_title));",

                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'email_addresses')CREATE TABLE email_addresses(email_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, person_id BIGINT NOT NULL, email_address VARCHAR(100) NOT NULL, CONSTRAINT UC_Email UNIQUE(email_address));CREATE INDEX idx_email_ids ON email_addresses(person_id);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'phone_numbers')CREATE TABLE phone_numbers(phone_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, person_id BIGINT NOT NULL, phone_number VARCHAR(20) NOT NULL);CREATE INDEX idx_phone_ids ON phone_numbers(person_id)",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'physical_addr')CREATE TABLE physical_addr(address_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, person_id BIGINT NOT NULL, addr_line1 VARCHAR(50) NOT NULL, addr_line2 VARCHAR(50) DEFAULT NULL, addr_city VARCHAR(50) NOT NULL, addr_state VARCHAR(2) NOT NULL, addr_zip VARCHAR(10) NOT NULL, addr_cntry VARCHAR(2) DEFAULT 'US');CREATE INDEX idx_addr_ids ON physical_addr(person_id);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'notes')CREATE TABLE notes(id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, note_id BIGINT NOT NULL, note_value VARBINARY(8000) NOT NULL);CREATE INDEX idx_note_ids ON notes(note_id);",

                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'users')CREATE TABLE users(user_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, user_fname VARCHAR(5000) NOT NULL, user_lname VARCHAR(5000) NOT NULL, user_name VARCHAR(5000) NOT NULL, user_password VARBINARY(8000) NOT NULL, user_role_id BIGINT FOREIGN KEY REFERENCES roles(role_id), CONSTRAINT UC_UserName UNIQUE(user_name));",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'employees')CREATE TABLE employees(empl_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, empl_fname VARCHAR(50) NOT NULL, empl_lname VARCHAR(50), empl_phone_id BIGINT, empl_addr_id BIGINT, empl_email_id BIGINT, empl_notes_id BIGINT);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'vendors')CREATE TABLE vendors(vend_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, vendor_name VARCHAR(50) NOT NULL UNIQUE, vendor_addr_id BIGINT, vendor_poc_name VARCHAR(50) DEFAULT NULL, vendor_phone_id BIGINT);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'carriers')CREATE TABLE carriers(carrier_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, carrier_name VARCHAR(50) NOT NULL UNIQUE, carrier_phone_id  BIGINT);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'purchase_orders')CREATE TABLE purchase_orders(po_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, po_number VARCHAR(25) DEFAULT NULL,po_package_count INT DEFAULT 0, po_created_on DATE, po_created_by BIGINT, po_approved_by BIGINT);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'packages')CREATE TABLE packages(package_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,package_po_id BIGINT, package_carrier_id BIGINT, package_vendor_id BIGINT, package_deliv_to_id BIGINT, package_deliv_by_id BIGINT, package_signed_for_by_id BIGINT, package_tracking_number VARCHAR(50) DEFAULT NULL, package_receive_date DATE, package_deliver_date DATE, package_notes_id BIGINT);",
                    "IF (SELECT COUNT(*) FROM sys.symmetric_keys WHERE name = 'secure_data')=0 CREATE SYMMETRIC KEY secure_data WITH ALGORITHM = AES_128 ENCRYPTION BY PASSWORD = '" + EncodeKey +"';"
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
                        //for  confirmation that the tables had been created...MSSQL ONLY
                        if (DBType == SQLHelperClass.DatabaseType.MSSQL)
                        {
                            cmd.CommandText = "SELECT [name] FROM sys.tables;";
                            string message = "Tables exist or were Created::\n";
                            using (OdbcDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    message += reader[0].ToString() + "\n";
                                }
                                System.Windows.Forms.MessageBox.Show(message, "creation results");
                            }
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
        /// <summary>
        /// Drop existing tables ... Use with extreame caution!
        /// </summary>
        /// <param name="all"> drop all tables in list, if true send blank list collection, otherwise send a list of the specific tables you want to drop</param>
        /// <param name="tables_to_drop">if 'all' is false have a list of strings representing table names and we will go from ther, otherwise send a blank list, since 'all' will do all tables.</param>
        protected void Drop_Tables(bool all, List<string> tables_to_drop)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            if (!all && (tables_to_drop is null || tables_to_drop.Count == 0))
            {
                return;
            }
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    if (DBType == SQLHelperClass.DatabaseType.MSSQL)
                    {
                        cmd.CommandText = "SELECT [name] FROM sys.tables;";
                        string tbl_lst = "";
                        if (all)
                        {
                            string temp = "";
                            using (OdbcDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader[0].ToString() == "roles")
                                    {
                                        temp = ", roles";
                                        continue;
                                    }
                                    tbl_lst += ", " + reader[0].ToString();
                                }
                            }
                            if (tbl_lst.Length > 2)
                            {
                                tbl_lst = tbl_lst.Substring(1) + temp;
                            }
                            else
                            {
                                return;
                            }
                            cmd.CommandText = "DROP TABLE IF EXISTS" + tbl_lst + ";";
                            cmd.CommandText += "DROP SYMMETRIC KEY secure_data;";
                            cmd.Transaction = c.BeginTransaction();
                            try
                            {
                                cmd.ExecuteNonQuery();
                                cmd.Transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                cmd.Transaction.Rollback();
                                DatabaseConnectionException exc = new DatabaseConnectionException("", e);
                            }
                        }
                        else
                        {
                            foreach (string tbl in tables_to_drop)
                            {
                                tbl_lst += ", " + tbl;
                            }
                            tbl_lst = tbl_lst.Substring(1);
                            cmd.CommandText = "DROP TABLE IF EXISTS" + tbl_lst + ";";
                            cmd.Transaction = c.BeginTransaction();
                            try
                            {
                                cmd.ExecuteNonQuery();
                                cmd.Transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                cmd.Transaction.Rollback();
                                DatabaseConnectionException exc = new DatabaseConnectionException("", e);
                            }
                        }
                    }
                    else if (DBType == SQLHelperClass.DatabaseType.MySQL)
                    {
                        cmd.CommandText = "SHOW TABLES;";
                        string tbl_lst = "";
                        if (all)
                        {
                            string temp = "";
                            using (OdbcDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader[0].ToString() == "roles")
                                    {
                                        temp = ", " + reader[0].ToString();
                                        continue;
                                    }
                                    tbl_lst += ", " + reader[0].ToString();
                                }
                                if (tbl_lst.Length > 2)
                                {
                                    tbl_lst = tbl_lst.Substring(1)+temp;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            cmd.CommandText = "DROP TABLE IF EXISTS" + tbl_lst + ";";
                            cmd.Transaction = c.BeginTransaction();
                            try
                            {
                                cmd.ExecuteNonQuery();
                                cmd.Transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                cmd.Transaction.Rollback();
                                DatabaseConnectionException exc = new DatabaseConnectionException("", e);
                            }
                        }
                        else
                        {
                            foreach (string tbl in tables_to_drop)
                            {
                                tbl_lst += ", " + tbl;
                            }
                            tbl_lst = tbl_lst.Substring(1);
                            cmd.CommandText = "DROP TABLE IF EXISTS" + tbl_lst + ";";
                            cmd.Transaction = c.BeginTransaction();
                            try
                            {
                                cmd.ExecuteNonQuery();
                                cmd.Transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                cmd.Transaction.Rollback();
                                DatabaseConnectionException exc = new DatabaseConnectionException("", e);
                            }
                        }
                    }
                    else
                    {
                        DatabaseConnectionException exc = new DatabaseConnectionException("Not Authorized.");
                    }
                }
            }
        }
        #endregion
        #region Write Data To Database
        protected void Write_User_To_Database(User newU)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            if (DBType == SQLHelperClass.DatabaseType.MSSQL)
            {
                User u = newU;
                using (OdbcConnection c = new OdbcConnection())
                {
                    c.ConnectionString = ConnString;
                    c.Open();
                    OdbcTransaction tr = c.BeginTransaction();
                    using (OdbcCommand cmd = new OdbcCommand("",c,tr))
                    {
                        //open key
                        cmd.CommandText = "OPEN SYMMETRIC KEY secure_data DECRYPTION BY PASSWORD = '" + EncodeKey + "';";
                        cmd.CommandText += "INSERT INTO users (user_fname,user_lname,user_name,user_password,user_role_id) VALUES (?,?,?,EncryptByKey(Key_GUID('secure_data'),CONVERT(nvarchar,?)),?);";
                        cmd.CommandText += "CLOSE SYMMETRIC KEY secure_data;";
                        cmd.Parameters.AddRange(new OdbcParameter[] { new OdbcParameter("firstname", u.FirstName), new OdbcParameter("lastname", u.LastName), new OdbcParameter("username", u.Username), new OdbcParameter("password", u.PassWord), new OdbcParameter("role", u.Level) });
                        try
                        {
                            cmd.ExecuteNonQuery();
                            cmd.Transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            cmd.Transaction.Rollback();
                            DatabaseConnectionException exc = new DatabaseConnectionException("", e);
                        }
                    }
                }
            }
            else if (DBType == SQLHelperClass.DatabaseType.MySQL)
            {
                Serialize s = new Serialize();
                User u = newU;
                u.FirstName = u.FirstName;
                u.LastName = u.LastName;
                u.Username = u.Username;
                using (OdbcConnection c = new OdbcConnection())
                {
                    c.ConnectionString = ConnString;
                    c.Open();
                    OdbcTransaction tr = c.BeginTransaction();
                    using (OdbcCommand cmd = new OdbcCommand("", c, tr))
                    {
                        cmd.CommandText = "INSERT INTO " + Tables.users.ToString() + " (user_fname,user_lname,user_name,user_password,user_role_id)VALUES(?,?,?,AES_ENCRYPT(?,'"+EncodeKey+"'),?);";
                        OdbcParameter p1 = new OdbcParameter("user_fname", u.FirstName);
                        OdbcParameter p2 = new OdbcParameter("user_lname", u.LastName);
                        OdbcParameter p3 = new OdbcParameter("user_name", u.Username);
                        OdbcParameter p4 = new OdbcParameter("user_password",u.PassWord);
                        OdbcParameter p5 = new OdbcParameter("user_role_id", u.Level);
                        cmd.Parameters.Add(p1);
                        cmd.Parameters.Add(p2);
                        cmd.Parameters.Add(p3);
                        cmd.Parameters.Add(p4);
                        cmd.Parameters.Add(p5);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            cmd.Transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            cmd.Transaction.Rollback();
                            DatabaseConnectionException exc = new DatabaseConnectionException("", e);
                        }
                    }
                }
            }
            else
            {
                DatabaseConnectionException exc = new DatabaseConnectionException("You Must select a valid database type.", new ArgumentException(DBType.ToString() + " is invalid."));
            }
        }
        protected void Write_User_To_Database(BindingList<User> users)
        {
            throw new NotImplementedException();
        }
        protected void Update_User(long id,string[] columns, string[] values)
        {
            if (columns.Length != values.Length)
            {
                throw new ArgumentException("The number of values does not match the number of columns, please correct this and try again.");
            }
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                OdbcTransaction tr = c.BeginTransaction();
                using (OdbcCommand cmd = new OdbcCommand("", c, tr))
                {
                    if (DBType == SQLHelperClass.DatabaseType.MSSQL)
                    {
                        cmd.CommandText = "OPEN SYMMETRIC KEY secure_data DECRYPTION BY PASSWORD='" + EncodeKey + "';";
                        cmd.CommandText += "UPDATE users SET ";
                        foreach (string col in columns)
                        {
                            if (col == "user_password")
                            {
                                cmd.CommandText += col + "=EncryptByKey(Key_GUID('secure_data'),CONVERT(nvarchar,?)),";
                                continue;
                            }
                            cmd.CommandText += col + "=?,";
                        }
                        cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 1);
                        cmd.CommandText += " WHERE user_id=?;";
                        cmd.CommandText += "CLOSE SYMMETRIC KEY secure_data;";
                        for (int i = 0; i < values.Length; i++)
                        {
                            cmd.Parameters.AddWithValue(columns[i], values[i]);
                        }
                        cmd.Parameters.AddWithValue("user_id", id);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            cmd.Transaction.Commit();
                        }
                        catch (Exception)
                        {
                            cmd.Transaction.Rollback();
                            throw;
                        }
                    }
                    else if (DBType == SQLHelperClass.DatabaseType.MySQL)
                    {
                        cmd.CommandText = "UPDATE users SET ";
                        foreach (string col in columns)
                        {
                            if (col == "user_password")
                            {
                                cmd.CommandText += col + "=AES_ENCRYPT(?,'" + EncodeKey + "'),";
                            }
                            cmd.CommandText += col + "=?,";
                        }
                        cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 1);
                        cmd.CommandText += " WHERE user_id=?;";
                        for (int i = 0; i < values.Length; i++)
                        {
                            cmd.Parameters.AddWithValue(columns[i], values[i]);
                        }
                        cmd.Parameters.AddWithValue("user_id", id);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            cmd.Transaction.Commit();
                        }
                        catch (Exception)
                        {
                            cmd.Transaction.Rollback();
                            throw;
                        }
                    }
                    else
                    {
                        DatabaseConnectionException e = new DatabaseConnectionException("Select a valid database type.", new Exception());
                    }
                }
            }
        }
        #endregion
        #region Get Data From Database
        /// <summary>
        /// Test Methos get user id 1
        /// </summary>
        /// <param name="id"> user id</param>
        /// <returns>user class object</returns>
        protected User GetUser(long id)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                OdbcTransaction tr = c.BeginTransaction();
                using (OdbcCommand cmd = new OdbcCommand("",c,tr))
                {
                    if (DBType == SQLHelperClass.DatabaseType.MSSQL)
                    {
                        cmd.CommandText = "OPEN SYMMETRIC KEY secure_data DECRYPTION BY PASSWORD = '" + EncodeKey + "';";
                        cmd.CommandText += "SELECT users.user_id, users.user_fname,users.user_lname,users.user_name,CONVERT(nvarchar, DecryptByKey(users.user_password)) AS 'Password',users.user_role_id FROM users WHERE users.user_id = ?;";
                        cmd.CommandText += "CLOSE SYMMETRIC KEY secure_data;";
                    }
                    else if (DBType == SQLHelperClass.DatabaseType.MySQL)
                    {
                        cmd.CommandText = "SELECT user_id, user_fname, user_lname, user_name, CAST(AES_DECRYPT(user_password,'" + EncodeKey + "') AS CHAR(300)) AS 'Password',user_role_id FROM users WHERE users.user_id = ?;";
                    }
                    cmd.Parameters.AddWithValue("userId", id);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        User u = new User();
                        while (reader.Read())
                        {
                            u.Id = Convert.ToInt64(reader[0].ToString());
                            u.FirstName = reader[1].ToString();
                            u.LastName = reader[2].ToString();
                            u.Username = reader[3].ToString();
                            u.PassWord = reader[4].ToString();
                            u.Level = Convert.ToInt64(reader[5].ToString());
                        }
                        return u;
                    }
                }
            }
        }
        /// <summary>
        /// Gets user by username
        /// </summary>
        /// <param name="username">Username supplied to the application</param>
        /// <returns>User Object</returns>
        protected User GetUser(string username)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                OdbcTransaction tr = c.BeginTransaction();
                using (OdbcCommand cmd = new OdbcCommand("", c, tr))
                {
                    if (DBType == SQLHelperClass.DatabaseType.MSSQL)
                    {
                        cmd.CommandText = "OPEN SYMMETRIC KEY secure_data DECRYPTION BY PASSWORD = '" + EncodeKey + "';";
                        cmd.CommandText += "SELECT users.user_id, users.user_fname,users.user_lname,users.user_name,CONVERT(nvarchar, DecryptByKey(users.user_password)) AS 'Password',users.user_role_id FROM users WHERE users.user_name = ?;";
                        cmd.CommandText += "CLOSE SYMMETRIC KEY secure_data;";
                    }
                    else if (DBType == SQLHelperClass.DatabaseType.MySQL)
                    {
                        cmd.CommandText = "SELECT user_id, user_fname, user_lname, user_name, CAST(AES_DECRYPT(user_password,'" + EncodeKey + "') AS CHAR(300)) AS 'Password',user_role_id FROM users WHERE users.user_name = ?;";
                    }
                    cmd.Parameters.AddWithValue("userName", username);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        User u = new User();
                        while (reader.Read())
                        {
                            u.Id = Convert.ToInt64(reader[0].ToString());
                            u.FirstName = reader[1].ToString();
                            u.LastName = reader[2].ToString();
                            u.Username = reader[3].ToString();
                            u.PassWord = reader[4].ToString();
                            u.Level = Convert.ToInt64(reader[5].ToString());
                        }
                    return u;
                    }
                }
            }
        }
        #endregion
        #region Enums
        /// <summary>
        /// Enum value for the table that you want to access, makes it simpler and more accurate.
        /// </summary>
        public enum Tables
        {
            /// <summary>
            /// default
            /// </summary>
            None =0,
            /// <summary>
            /// Users Table
            /// </summary>
            users=1,
            /// <summary>
            /// Roles Table
            /// </summary>
            roles=2,
            /// <summary>
            /// Employees Table e.g Faculty
            /// </summary>
            employees=3,
            /// <summary>
            /// Vendors Table
            /// </summary>
            vendors=4,
            /// <summary>
            /// Carriers Table
            /// </summary>
            carriers=5,
            /// <summary>
            /// Purchase Orders Table
            /// </summary>
            purchase_orders=6,
            /// <summary>
            /// Packages Table
            /// </summary>
            packages=7,
            /// <summary>
            /// Email Address Table
            /// </summary>
            email_addresses=8,
            /// <summary>
            /// Phone Number Table
            /// </summary>
            phone_numbers=9,
            /// <summary>
            /// Physical Address Table
            /// </summary>
            physical_addr=10,
            /// <summary>
            /// Notes Table
            /// </summary>
            notes=11
        }
        #endregion
    }
    /// <summary>
    /// Database Ecxeption Class
    /// </summary>
    internal class DatabaseConnectionException
    {
        private string Message { get; set; }
        private Exception Inner { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DatabaseConnectionException()
        {
            ThrowException();
        }
        /// <summary>
        /// exception with message
        /// </summary>
        /// <param name="message">Exception message for outer exception</param>
        public DatabaseConnectionException(string message)
        {
            Message = message;
            ThrowException();
        }
        /// <summary>
        /// Exception with outer exception message and inner exception class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="insideException"></param>
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
