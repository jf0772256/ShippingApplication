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
using shipapp.Models.ModelData;

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
                    "CREATE TABLE IF NOT EXISTS email_addresses(email_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, person_id VARCHAR(1000) NOT NULL, email_address VARCHAR(100) NOT NULL UNIQUE)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE INDEX idx_addr_ids ON email_addresses(person_id);",
                    "CREATE TABLE IF NOT EXISTS phone_numbers(phone_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, person_id VARCHAR(1000) NOT NULL, phone_number VARCHAR(20) NOT NULL)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE INDEX idx_phone_ids ON phone_numbers(person_id);",
                    "CREATE TABLE IF NOT EXISTS physical_addr(address_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, person_id VARCHAR(1000) NOT NULL,building_long_name VARCHAR(100),building_short_name VARCHAR(10),room_number VARCHAR(10), addr_line1 VARCHAR(100) NOT NULL, addr_line2 VARCHAR(50) DEFAULT NULL, addr_city VARCHAR(100) NOT NULL, addr_state VARCHAR(2) NOT NULL, addr_zip VARCHAR(10) NOT NULL, addr_cntry VARCHAR(2) DEFAULT 'US')engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE INDEX idx_physaddr_ids ON physical_addr(person_id);",
                    "CREATE TABLE IF NOT EXISTS notes(id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, note_id VARCHAR(1000) NOT NULL, note_value VARCHAR(5000) NOT NULL)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE INDEX idx_note_ids ON notes(note_id);",

                    "CREATE TABLE IF NOT EXISTS users(user_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, user_fname VARCHAR(100) NOT NULL, user_lname VARCHAR(100) NOT NULL, user_name VARCHAR(100) NOT NULL UNIQUE, user_password VARBINARY(500) NOT NULL, user_role_id BIGINT, person_id VARCHAR(1000) NOT NULL UNIQUE, FOREIGN KEY (user_role_id) REFERENCES roles(role_id) ON DELETE NO ACTION ON UPDATE NO ACTION)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS employees(empl_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, empl_fname VARCHAR(100) NOT NULL, empl_lname VARCHAR(100), person_id VARCHAR(1000) NOT NULL UNIQUE)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS vendors(vend_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT,vendor_name VARCHAR(100) NOT NULL UNIQUE, vendor_poc_name VARCHAR(100) DEFAULT NULL, person_id VARCHAR(1000) NOT NULL UNIQUE)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS carriers(carrier_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, carrier_name VARCHAR(100) NOT NULL UNIQUE, person_id VARCHAR(1000) NOT NULL UNIQUE)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",

                    "CREATE TABLE IF NOT EXISTS purchase_orders(po_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, po_number VARCHAR(25) DEFAULT NULL,po_package_count INT DEFAULT 0, po_created_on DATETIME, po_created_by BIGINT, po_approved_by BIGINT, FOREIGN KEY (po_created_by) REFERENCES employees(empl_id) ON DELETE NO ACTION ON UPDATE NO ACTION, FOREIGN KEY (po_approved_by) REFERENCES employees(empl_id) ON DELETE NO ACTION ON UPDATE NO ACTION)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS packages(package_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT,package_po_id BIGINT, package_carrier_id BIGINT, package_vendor_id BIGINT, package_deliv_to_id BIGINT, package_deliv_by_id BIGINT, package_signed_for_by_id BIGINT, package_tracking_number VARCHAR(50) DEFAULT NULL, package_receive_date DATE, package_deliver_date DATE, package_notes_id VARCHAR(1000) NOT NULL UNIQUE,package_status INT DEFAULT 0, FOREIGN KEY (package_carrier_id) REFERENCES carriers(carrier_id) ON DELETE NO ACTION ON UPDATE NO ACTION, FOREIGN KEY (package_vendor_id) REFERENCES vendors(vend_id) ON DELETE NO ACTION ON UPDATE NO ACTION, FOREIGN KEY (package_deliv_to_id) REFERENCES employees(empl_id) ON DELETE NO ACTION ON UPDATE NO ACTION, FOREIGN KEY (package_deliv_by_id) REFERENCES users(user_id) ON DELETE NO ACTION ON UPDATE NO ACTION, FOREIGN KEY (package_signed_for_by_id) REFERENCES employees(empl_id) ON DELETE NO ACTION ON UPDATE NO ACTION)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    //create default roles;
                    "INSERT INTO roles(role_title)VALUES('Administrator'),('Supervisor'),('User');"
                };
            }
            else if (DBType == SQLHelperClass.DatabaseType.MSSQL)
            {
                //"IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = roles)CREATE TABLE ",
                cmdTxt = new List<string>(){
                    //attempt to create the first table as a test;;
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'roles')CREATE TABLE roles(role_id BigINT NOT NULL IDENTITY(1,1) PRIMARY KEY, role_title VARCHAR(50) NOT NULL, CONSTRAINT UC_Roles UNIQUE(role_title));",

                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'email_addresses')CREATE TABLE email_addresses(email_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, person_id VARCHAR(1000) NOT NULL, email_address VARCHAR(100) NOT NULL, CONSTRAINT UC_Email UNIQUE(email_address));CREATE INDEX idx_email_ids ON email_addresses(person_id);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'phone_numbers')CREATE TABLE phone_numbers(phone_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, person_id VARCHAR(1000) NOT NULL, phone_number VARCHAR(20) NOT NULL);CREATE INDEX idx_phone_ids ON phone_numbers(person_id)",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'physical_addr')CREATE TABLE physical_addr(address_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, person_id VARCHAR(1000) NOT NULL,building_long_name VARCHAR(100),building_short_name VARCHAR(10),room_number VARCHAR(10), addr_line1 VARCHAR(50) NOT NULL, addr_line2 VARCHAR(50) DEFAULT NULL, addr_city VARCHAR(50) NOT NULL, addr_state VARCHAR(2) NOT NULL, addr_zip VARCHAR(10) NOT NULL, addr_cntry VARCHAR(2) DEFAULT 'US', address_note_id BIGINT);CREATE INDEX idx_addr_ids ON physical_addr(person_id);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'notes')CREATE TABLE notes(id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, note_id BIGINT NOT NULL, note_value VARBINARY(8000) NOT NULL);CREATE INDEX idx_note_ids ON notes(note_id);",

                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'users')CREATE TABLE users(user_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, user_fname VARCHAR(2000) NOT NULL, user_lname VARCHAR(2000) NOT NULL, user_name VARCHAR(1000) NOT NULL, user_password VARBINARY(8000) NOT NULL, user_role_id BIGINT FOREIGN KEY REFERENCES roles(role_id), person_id VARCHAR(1000) NOT NULL, CONSTRAINT UC_UserName UNIQUE(user_name), CONSTRAINT UC_PID5 UNIQUE(person_id));",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'employees')CREATE TABLE employees(empl_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, empl_fname VARCHAR(50) NOT NULL, empl_lname VARCHAR(50), person_id VARCHAR(1000) NOT NULL, CONSTRAINT UC_PID_0 UNIQUE(person_id));",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'vendors')CREATE TABLE vendors(vend_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, vendor_name VARCHAR(50) NOT NULL UNIQUE, vendor_poc_name VARCHAR(50) DEFAULT NULL, person_id VARCHAR(1000) NOT NULL, CONSTRAINT UC_PID_1 UNIQUE(person_id));",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'carriers')CREATE TABLE carriers(carrier_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, carrier_name VARCHAR(50) NOT NULL UNIQUE, person_id VARCHAR(1000) NOT NULL, CONSTRAINT UC_PID_2 UNIQUE(person_id));",

                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'purchase_orders')CREATE TABLE purchase_orders(po_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, po_number VARCHAR(25) DEFAULT NULL,po_package_count INT DEFAULT 0, po_created_on DATE, po_created_by BIGINT FOREIGN KEY REFERENCES employees(empl_id), po_approved_by BIGINT FOREIGN KEY REFERENCES employees(empl_id));",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'packages')CREATE TABLE packages(package_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,package_po_id BIGINT, package_carrier_id BIGINT FOREIGN KEY REFERENCES carriers(carrier_id), package_vendor_id BIGINT FOREIGN KEY REFERENCES vendors(vend_id), package_deliv_to_id BIGINT FOREIGN KEY REFERENCES employees(empl_id), package_deliv_by_id BIGINT FOREIGN KEY REFERENCES users(user_id), package_signed_for_by_id BIGINT FOREIGN KEY REFERENCES employees(empl_id), package_tracking_number VARCHAR(50) DEFAULT NULL, package_receive_date DATE, package_deliver_date DATE, package_note_id VARCHAR(1000) NOT NULL, package_status INT DEFAULT 0, CONSTRAINT UC_NID UNIQUE(package_note_id));",
                    "IF (SELECT COUNT(*) FROM sys.symmetric_keys WHERE name = 'secure_data')=0 CREATE SYMMETRIC KEY secure_data WITH ALGORITHM = AES_128 ENCRYPTION BY PASSWORD = '" + EncodeKey +"';",
                    //create default roles;
                    "INSERT INTO roles(role_title)VALUES('Administrator'),('Supervisor'),('User');"
                };
            }
            //out side all conditions available
            else
            {
                cmdTxt = new List<string>() { };
                throw new DatabaseConnectionException("You have not set a correct database type.");
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
                        throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
                    }
                }
            }
        }
        /// <summary>
        /// Drop existing tables ... Use with extreame caution!
        /// </summary>
        /// <param name="all"> drop all tables in list, if true send blank list collection, otherwise send a list of the specific tables you want to drop</param>
        /// <param name="tables_to_drop">if 'all' is false have a list of strings representing table names and we will go from ther, otherwise send a blank list, since 'all' will do all tables.</param>
        protected void Drop_Tables(List<string> tables_to_drop)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            if (tables_to_drop is null || tables_to_drop.Count == 0)
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
                        foreach (string tbl in tables_to_drop)
                        {
                            tbl_lst += ", " + tbl;
                        }
                        tbl_lst = tbl_lst.Substring(1);
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
                            throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
                        }
                    }
                    else if (DBType == SQLHelperClass.DatabaseType.MySQL)
                    {
                        cmd.CommandText = "SHOW TABLES;";
                        string tbl_lst = "";
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
                            throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
                        }
                    }
                    else
                    {
                        throw new DatabaseConnectionException("Not Authorized.");
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
                        cmd.CommandText += "INSERT INTO users (user_fname,user_lname,user_name,user_password,user_role_id,person_id) VALUES (?,?,?,EncryptByKey(Key_GUID('secure_data'),CONVERT(nvarchar,?)),?,?);";
                        cmd.CommandText += "CLOSE SYMMETRIC KEY secure_data;";
                        cmd.Parameters.AddRange(
                            new OdbcParameter[] 
                            {
                                new OdbcParameter("firstname", u.FirstName),
                                new OdbcParameter("lastname", u.LastName),
                                new OdbcParameter("username", u.Username),
                                new OdbcParameter("password", u.PassWord),
                                new OdbcParameter("role", u.Level.Role_id),
                                new OdbcParameter("personid", u.Person_Id)
                            }
                        );
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
                using (OdbcConnection c = new OdbcConnection())
                {
                    c.ConnectionString = ConnString;
                    c.Open();
                    OdbcTransaction tr = c.BeginTransaction();
                    using (OdbcCommand cmd = new OdbcCommand("", c, tr))
                    {
                        cmd.CommandText = "INSERT INTO " + Tables.users.ToString() + " (user_fname,user_lname,user_name,user_password,user_role_id,person_id)VALUES(?,?,?,AES_ENCRYPT(?,'"+EncodeKey+"'),?,?);";
                        cmd.Parameters.AddRange(
                            new OdbcParameter[]
                            {
                                new OdbcParameter("user_fname", u.FirstName),
                                new OdbcParameter("user_lname", u.LastName),
                                new OdbcParameter("user_name", u.Username),
                                new OdbcParameter("user_password", u.PassWord),
                                new OdbcParameter("user_role_id", u.Level.Role_id),
                                new OdbcParameter("personid", u.Person_Id),
                            }
                        );
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
                throw new DatabaseConnectionException("You Must select a valid database type.", new ArgumentException(DBType.ToString() + " is invalid."));
            }
        }
        protected void Write_User_To_Database(BindingList<User> users)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            if (DBType == SQLHelperClass.DatabaseType.MSSQL)
            {
                using (OdbcConnection c = new OdbcConnection())
                {
                    c.ConnectionString = ConnString;
                    c.Open();
                    OdbcTransaction tr = c.BeginTransaction();
                    using (OdbcCommand cmd = new OdbcCommand("", c, tr))
                    {
                        foreach (User u in users)
                        {
                            //open key
                            cmd.CommandText = "OPEN SYMMETRIC KEY secure_data DECRYPTION BY PASSWORD = '" + EncodeKey + "';";
                            cmd.CommandText += "INSERT INTO users (user_fname,user_lname,user_name,user_password,user_role_id,person_id) VALUES (?,?,?,EncryptByKey(Key_GUID('secure_data'),CONVERT(nvarchar,?)),?,?);";
                            cmd.CommandText += "CLOSE SYMMETRIC KEY secure_data;";
                            cmd.Parameters.AddRange(
                                new OdbcParameter[]
                                {
                                new OdbcParameter("firstname", u.FirstName),
                                new OdbcParameter("lastname", u.LastName),
                                new OdbcParameter("username", u.Username),
                                new OdbcParameter("password", u.PassWord),
                                new OdbcParameter("role", u.Level.Role_id),
                                new OdbcParameter("personid", u.Person_Id),
                                }
                            );
                            try
                            {
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "";
                                cmd.Parameters.Clear();
                            }
                            catch (Exception e)
                            {
                                cmd.Transaction.Rollback();
                                throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
                            }
                        }
                        cmd.Transaction.Commit();
                    }
                }
            }
            else if (DBType == SQLHelperClass.DatabaseType.MySQL)
            {
                using (OdbcConnection c = new OdbcConnection())
                {
                    c.ConnectionString = ConnString;
                    c.Open();
                    OdbcTransaction tr = c.BeginTransaction();
                    using (OdbcCommand cmd = new OdbcCommand("", c, tr))
                    {
                        foreach (User u in users)
                        {
                            cmd.CommandText = "INSERT INTO " + Tables.users.ToString() + " (user_fname,user_lname,user_name,user_password,user_role_id)VALUES(?,?,?,AES_ENCRYPT(?,'" + EncodeKey + "'),?,?);";
                            cmd.Parameters.AddRange(
                                new OdbcParameter[]
                                {
                                new OdbcParameter("user_fname", u.FirstName),
                                new OdbcParameter("user_lname", u.LastName),
                                new OdbcParameter("user_name", u.Username),
                                new OdbcParameter("user_password", u.PassWord),
                                new OdbcParameter("user_role_id", u.Level.Role_id),
                                new OdbcParameter("personid", u.Person_Id),
                                }
                            );
                            try
                            {
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                cmd.CommandText = "";
                            }
                            catch (Exception e)
                            {
                                cmd.Transaction.Rollback();
                                throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
                            }
                        }
                        cmd.Transaction.Commit();
                    }
                }
            }
            else
            {
                throw new DatabaseConnectionException("You Must select a valid database type.", new ArgumentException(DBType.ToString() + " is invalid."));
            }
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
                        /**
                         *   TODO:: Add add notes with person_id as note_id
                         */
                        try
                        {
                            cmd.ExecuteNonQuery();
                            cmd.Transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            cmd.Transaction.Rollback();
                            throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
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
                        /**
                         * TODO: Add updates to notes (add niote to table if not exists)
                         */
                        try
                        {
                            cmd.ExecuteNonQuery();
                            cmd.Transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            cmd.Transaction.Rollback();
                            throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
                        }
                    }
                    else
                    {
                        throw new DatabaseConnectionException("Select a valid database type.", new Exception());
                    }
                }
            }
        }
        protected void Write_Vendor_To_Database(Vendors v)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                OdbcTransaction tr = c.BeginTransaction();
                using (OdbcCommand cmd = new OdbcCommand())
                {
                    cmd.CommandText = "INSERT INTO vendors(vendor_name,vendor_poc_name,person_id)VALUES(?,?,?);";
                    cmd.Parameters.AddRange(new OdbcParameter[]{
                        new OdbcParameter("vend_name",v.VendorName),
                        new OdbcParameter("vend_poc_name",v.VendorPointOfContactName),
                        new OdbcParameter("person_id",v.Vendor_PersonId)
                    });
                    if (!(v.VendorPhone is null))
                    {
                        cmd.CommandText += "INSERT INTO phone_numbers (phone_number, person_id)VALUES(?,?);";
                        cmd.Parameters.AddRange(new OdbcParameter[]{
                            new OdbcParameter("phone",v.VendorPhone.Phone_Number),
                            new OdbcParameter("personid",v.Vendor_PersonId)
                        });
                    }
                    if (!(v.VendorAddress is null))
                    {
                        cmd.CommandText += "INSERT INTO physical_addr(person_id,building_long_name,building_short_name,room_number,addr_line1,addr_line2,addr_city,addr_state,addr_zip,addr_cntry,address_note_id)VALUES(?,?,?,?,?,?,?,?,?,?,?);";
                        cmd.Parameters.AddRange(new OdbcParameter[]{
                            new OdbcParameter("person_id",v.Vendor_PersonId),
                            new OdbcParameter("buildinglname",v.VendorAddress.BuildingLongName),
                            new OdbcParameter("buildingsname",v.VendorAddress.BuildingShortName),
                            new OdbcParameter("buildingroom",v.VendorAddress.BuildingRoomNumber),
                            new OdbcParameter("line1",v.VendorAddress.Line1),
                            new OdbcParameter("line2",v.VendorAddress.Line2),
                            new OdbcParameter("city",v.VendorAddress.City),
                            new OdbcParameter("state",v.VendorAddress.State),
                            new OdbcParameter("zip",v.VendorAddress.ZipCode),
                            new OdbcParameter("country",v.VendorAddress.Country),
                            new OdbcParameter("addressNoteId",v.Vendor_PersonId)
                        });
                    }
                    if (v.Notes.Count > 0)
                    {
                        foreach (Models.ModelData.Note notes in v.Notes)
                        {
                            cmd.CommandText += "INSERT INTO notes(note_id,note_value)VALUES(?,?);";
                            cmd.Parameters.AddRange(new OdbcParameter[]
                            {
                                new OdbcParameter("id",v.Vendor_PersonId),
                                new OdbcParameter("value",notes.Note_Value)
                            });
                        }
                    }
                    if (v.VendorAddress.Notes.Count > 0)
                    {
                        foreach (Models.ModelData.Note note in v.VendorAddress.Notes)
                        {
                            cmd.CommandText += "INSERT INTO notes(note_id,note_value)VALUE(?,?);";
                            cmd.Parameters.AddRange(new OdbcParameter[]
                            {
                                new OdbcParameter("id",v.Vendor_PersonId),
                                new OdbcParameter("value",note.Note_Value)
                            });
                        }
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
                    }
                }
            }
        }
        /// <summary>
        /// Not used Do Not Use
        /// </summary>
        protected void Write_Vendor_List_To_Database()
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
        }
        protected void Update_Vendor(Vendors v)
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
                    cmd.CommandText = "UPDATE vendors SET ";
                    cmd.CommandText += "vendor_name = ?, vendor_poc_name = ? ";
                    cmd.CommandText += "WHERE vend_id = ?;";
                    cmd.Parameters.AddRange
                        (
                            new OdbcParameter[]
                            {
                                new OdbcParameter("vendorname",v.VendorName),
                                new OdbcParameter("vendorPOCname",v.VendorPointOfContactName),
                                new OdbcParameter("vendorID",v.VendorId)
                            }
                        );
                    cmd.CommandText += "UPDATE phone_numbers SET phone_number WHERE person_id = ? AND phone_id = ?;";
                    cmd.Parameters.AddRange
                        (
                            new OdbcParameter[]
                            {
                                new OdbcParameter("phoneNumber",v.VendorPhone.Phone_Number),
                                new OdbcParameter("personID",v.Vendor_PersonId),
                                new OdbcParameter("phoneID",v.VendorPhone.PhoneId)
                            }
                        );
                    cmd.CommandText += "UPDATE physical_addr SET ";
                    cmd.CommandText += "building_long_name = ?, building_short_name = ?, room_number = ?, ";
                    cmd.CommandText += "addr_line1 = ?, addr_line2 = ?, addr_city = ?, addr_state = ?, ";
                    cmd.CommandText += "addr_zip = ?, addr_cntry = ? WHERE person_id = ? AND address_id = ?;";
                    cmd.Parameters.AddRange
                        (
                            new OdbcParameter[]
                            {
                                new OdbcParameter("blongname",v.VendorAddress.BuildingLongName),
                                new OdbcParameter("bshortname",v.VendorAddress.BuildingShortName),
                                new OdbcParameter("broomnumber",v.VendorAddress.BuildingRoomNumber),
                                new OdbcParameter("line1",v.VendorAddress.Line1),
                                new OdbcParameter("line2",v.VendorAddress.Line2),
                                new OdbcParameter("city",v.VendorAddress.City),
                                new OdbcParameter("state",v.VendorAddress.State),
                                new OdbcParameter("zip",v.VendorAddress.ZipCode),
                                new OdbcParameter("country",v.VendorAddress.Country),
                                new OdbcParameter("personid",v.Vendor_PersonId),
                                new OdbcParameter("addrId",v.VendorAddress.AddressId)
                            }
                        );
                    //look for new notes and insert them into the database
                    foreach (Models.ModelData.Note note in v.VendorAddress.Notes)
                    {
                        if (note.Note_Id == 0)
                        {
                            //new note was added
                            cmd.CommandText += "INSERT INTO notes(note_id,note_value)VALUES(?,?);";
                            cmd.Parameters.AddWithValue("personId", v.Vendor_PersonId);
                            cmd.Parameters.AddWithValue("note_text", note.Note_Value);
                        }
                    }
                    foreach (Models.ModelData.Note note in v.Notes)
                    {
                        if (note.Note_Id == 0)
                        {
                            //new note was added
                            cmd.CommandText += "INSERT INTO notes(note_id,note_value)VALUES(?,?);";
                            cmd.Parameters.AddWithValue("personId", v.Vendor_PersonId);
                            cmd.Parameters.AddWithValue("note_text", note.Note_Value);
                        }
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
                    }
                }
            }
        }
        protected void Add_Role(Role value)
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
                    cmd.CommandText = "INSERT INTO roles(role_title)VALUES(?);";
                    cmd.Parameters.AddWithValue("title", value.Role_Title);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
                    }
                }
            }
        }
        protected void Update_Role(Role value)
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
                    cmd.CommandText = "UPDATE roles SET role_title = ? WHERE role_id = ?;";
                    cmd.Parameters.AddWithValue("title", value.Role_Title);
                    cmd.Parameters.AddWithValue("id", value.Role_id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
                    }
                }
            }
        }
        protected void Write_Carrier_To_Database(Carrier value)
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
                    cmd.CommandText = "INSERT INTO carriers (carrier_name,person_id)VALUES(?,?);";
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("carrierN",value.CarrierName),
                        new OdbcParameter("personid",value.Carrier_PersonId)
                    });
                    cmd.CommandText += "INSERT INTO phone_numbers (phone_number,person_id)VALUES(?,?);";
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("phone",value.PhoneNumber.Phone_Number),
                        new OdbcParameter("personid",value.Carrier_PersonId)
                    });
                    foreach (Note note in value.Notes)
                    {
                        cmd.CommandText += "INSERT INTO notes (note_id,not_value)VALUES(?,?);";
                        cmd.Parameters.AddRange(new OdbcParameter[]
                        {
                        new OdbcParameter("id",value.Carrier_PersonId),
                        new OdbcParameter("text",note.Note_Value)
                        });
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
                    }
                }
            }
        }
        protected void Update_Carrier(Carrier value)
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
                    cmd.CommandText = "UPDATE carriers SET carrier_name=? WHERE person_id = ? AND carrier_id =?;";
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("carrierN",value.CarrierName),
                        new OdbcParameter("personid",value.Carrier_PersonId),
                        new OdbcParameter("carrierid",value.CarrierId)
                    });
                    cmd.CommandText += "UPDATE phone_numbers SET phone_number=? WHERE person_id = ? AND phone_id = ?;";
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("phone",value.PhoneNumber.Phone_Number),
                        new OdbcParameter("personid",value.Carrier_PersonId),
                        new OdbcParameter("phoneid",value.PhoneNumber.PhoneId)
                    });
                    foreach (Note note in value.Notes)
                    {
                        if (note.Note_Id <= 0)
                        {
                            cmd.CommandText += "INSERT INTO notes (note_id,not_value)VALUES(?,?);";
                            cmd.Parameters.AddRange(new OdbcParameter[]
                            {
                                new OdbcParameter("id",value.Carrier_PersonId),
                                new OdbcParameter("text",note.Note_Value)
                            });
                        }
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
                    }
                }
            }
        }
        protected void Write_Faculty_To_Database(Faculty f)
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
                    cmd.CommandText = "INSERT INTO employees (empl_fname,empl_lname,person_id)VALUES(?,?,?);";
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("fname",f.FirstName),
                        new OdbcParameter("lname",f.LastName),
                        new OdbcParameter("person_id",f.Faculty_PersonId)
                    });
                    foreach (PhoneNumber phone in f.Phone)
                    {
                        cmd.CommandText += "INSERT INTO phone_numbers(phone_number,person_id)VALUES(?,?);";
                        cmd.Parameters.AddRange(new OdbcParameter[]
                        {
                            new OdbcParameter("phone",phone.Phone_Number),
                            new OdbcParameter("person_id",f.Faculty_PersonId)
                        });
                    }
                    foreach (EmailAddress email in f.Email)
                    {
                        cmd.CommandText += "INSERT INTO email_addresses(email_address,person_id)VALUES(?,?);";
                        cmd.Parameters.AddRange(new OdbcParameter[]
                        {
                            new OdbcParameter("email",email.Email_Address),
                            new OdbcParameter("person_id",f.Faculty_PersonId)
                        });
                    }
                    foreach (Note note in f.Notes)
                    {
                        cmd.CommandText += "INSERT INTO notes(note_value,note_id)VALUES(?,?);";
                        cmd.Parameters.AddRange(new OdbcParameter[]
                        {
                            new OdbcParameter("note",note.Note_Value),
                            new OdbcParameter("person_id",f.Faculty_PersonId)
                        });
                    }
                    foreach (PhysicalAddress paddr in f.Address)
                    {
                        cmd.CommandText += "INSERT INTO physical_addr(person_id,building_long_name,building_short_name,room_number,addr_line1,addr_line2,addr_city,addr_state,addr_zip,addr_cntry,addr_note_id)VALUES(?,?,?,?,?,?,?,?,?,?,?);";
                        cmd.Parameters.AddRange(new OdbcParameter[]
                        {
                            new OdbcParameter("person_id",f.Faculty_PersonId),
                            new OdbcParameter("bln",paddr.BuildingLongName),
                            new OdbcParameter("bsn",paddr.BuildingShortName),
                            new OdbcParameter("brn",paddr.BuildingRoomNumber),
                            new OdbcParameter("ln1",paddr.Line1),
                            new OdbcParameter("ln2",paddr.Line2),
                            new OdbcParameter("cty",paddr.City),
                            new OdbcParameter("state",paddr.State),
                            new OdbcParameter("zip",paddr.ZipCode),
                            new OdbcParameter("ctry",paddr.Country),
                            new OdbcParameter("noteid",f.Faculty_PersonId)
                        });
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failure to process data, please review inner exception for further detail.", e);
                    }
                }
            }
        }
        protected void Update_Faculty(Faculty f)
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
                    cmd.CommandText = "UPDATE employees SET empl_fname=?,empl_lname=? WHERE person_id = ? AND empl_id = ?;";
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("fname",f.FirstName),
                        new OdbcParameter("lname",f.LastName),
                        new OdbcParameter("person_id",f.Faculty_PersonId),
                        new OdbcParameter("empl_id", f.Id)
                    });
                    foreach (PhoneNumber phone in f.Phone)
                    {
                        cmd.CommandText += "UPDATE phone_numbers SET phone_number = ? WHERE person_id = ? AND phone_id = ?;";
                        cmd.Parameters.AddRange(new OdbcParameter[]
                        {
                            new OdbcParameter("phone",phone.Phone_Number),
                            new OdbcParameter("person_id",f.Faculty_PersonId),
                            new OdbcParameter("pid",phone.PhoneId)
                        });
                    }
                    foreach (EmailAddress email in f.Email)
                    {
                        cmd.CommandText += "UPDATE email_addresses SET email_address = ? WHERE person_id = ? AND email_id = ?;";
                        cmd.Parameters.AddRange(new OdbcParameter[]
                        {
                            new OdbcParameter("email",email.Email_Address),
                            new OdbcParameter("person_id",f.Faculty_PersonId),
                            new OdbcParameter("eid",email.Email_Id)
                        });
                    }
                    foreach (Note note in f.Notes)
                    {
                        if (note.Note_Id <= 0)
                        {
                            cmd.CommandText += "INSERT INTO notes(note_value,note_id)VALUES(?,?);";
                            cmd.Parameters.AddRange(new OdbcParameter[]
                            {
                                new OdbcParameter("note",note.Note_Value),
                                new OdbcParameter("person_id",f.Faculty_PersonId)
                            });
                        }
                    }
                    foreach (PhysicalAddress paddr in f.Address)
                    {
                        cmd.CommandText += "UPDATE physical_addr SET building_long_name = ?,building_short_name = ?,room_number = ?,addr_line1 = ?,addr_line2 = ?,addr_city = ?,addr_state = ?,addr_zip = ?,addr_cntry = ? WHERE person_id = ? AND address_id = ?;";
                        cmd.Parameters.AddRange(new OdbcParameter[]
                        {
                            new OdbcParameter("bln",paddr.BuildingLongName),
                            new OdbcParameter("bsn",paddr.BuildingShortName),
                            new OdbcParameter("brn",paddr.BuildingRoomNumber),
                            new OdbcParameter("ln1",paddr.Line1),
                            new OdbcParameter("ln2",paddr.Line2),
                            new OdbcParameter("cty",paddr.City),
                            new OdbcParameter("state",paddr.State),
                            new OdbcParameter("zip",paddr.ZipCode),
                            new OdbcParameter("ctry",paddr.Country),
                            new OdbcParameter("person_id",f.Faculty_PersonId),
                            new OdbcParameter("address_id", paddr.AddressId)
                        });
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failure to process data, please review inner exception for further detail.", e);
                    }
                }
            }
        }
        protected void Write_PurchaseOrder_ToDatabase(PurchaseOrder p)
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
                    cmd.CommandText = "INSERT INTO purchase_order(po_number,po_package_count,po_created_on,po_created_by,po_approved_by)VALUES(?,?,?,?,?);";
                    cmd.Parameters.Add(new OdbcParameter[]
                    {
                        new OdbcParameter("ponumber",p.PONumber),
                        new OdbcParameter("popackagecount",p.PackageCount),
                        new OdbcParameter("pocreatedon",p.POCreatedOn),
                        new OdbcParameter("pocreatedby",p.CreatedBy.Id),
                        new OdbcParameter("poapprovedby",p.ApprovedBy.Id)
                    });
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed to process data, please review the inner exception for further details", e);
                    }
                }
            }
        }
        protected void Update_PurchaseOrder(PurchaseOrder p)
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
                    cmd.CommandText = "UPDATE purchase_order SET po_number = ?,po_package_count = ?,po_created_on = ?,po_created_by = ?,po_approved_by = ? WHERE po_id = ?;";
                    cmd.Parameters.Add(new OdbcParameter[]
                    {
                        new OdbcParameter("ponumber",p.PONumber),
                        new OdbcParameter("popackagecount",p.PackageCount),
                        new OdbcParameter("pocreatedon",p.POCreatedOn),
                        new OdbcParameter("pocreatedby",p.CreatedBy.Id),
                        new OdbcParameter("poapprovedby",p.ApprovedBy.Id),
                        new OdbcParameter("POID",p.PO_Id)
                    });
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed to process data, please review the inner exception for further details", e);
                    }
                }
            }
        }
        protected void Write_Package_To_Database(Package p)
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
                    cmd.CommandText = "INSERT INTO packages(package_po_id,package_carrier_id,package_vendor_id,package_deliv_to_id,package_devliv_by_id,package_signed_for_by_id,package_tracking_number,package_received_date,package_deliver_date,package_note_id,package_status)VALUES(?,?,?,?,?,?,?,?,?,?,?);";
                    //most fields can be null so we need to check and make sure that if a field is empty that we set  ids to 0 or null strings
                    //ids all will be 0 for null, strings should roll to null
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("poid", p.PackagePurchaseOrder.PO_Id),
                        new OdbcParameter("carrierid", p.PackageCarrier.CarrierId),
                        new OdbcParameter("vendid",p.PackageVendor.VendorId),
                        new OdbcParameter("delivtoid",p.PackageDeliveredTo.Id),
                        new OdbcParameter("delivbyid",p.PackageDeleveredBy.Id),
                        new OdbcParameter("signedbyid",p.PackageSignedForBy.Id),
                        new OdbcParameter("tracknumb",p.PackageTrackingNumber),
                        new OdbcParameter("recieveddate",p.PackageReceivedDate),
                        new OdbcParameter("delivDate",p.PackageDeliveredDate),
                        new OdbcParameter("noteid",p.Package_PersonId),
                        new OdbcParameter("packstats",p.Status.ToString())
                    });
                    cmd.CommandText += "INSERT INTO notes(note_id,note_value)VALUES(?,?)";
                    cmd.Parameters.AddRange(new OdbcParameter[] { new OdbcParameter("v" + 0, p.Notes[0].Note_Id), new OdbcParameter("n" + 0, p.Notes[0].Note_Value) });
                    for (int i = 1; i < p.Notes.Count; i++)
                    {
                        if (i==p.Notes.Count-1)
                        {
                            cmd.CommandText = ",(?,?);";
                            cmd.Parameters.AddRange(new OdbcParameter[] { new OdbcParameter("v" + i, p.Notes[i].Note_Id), new OdbcParameter("n" + i, p.Notes[i].Note_Value) });
                        }
                        else
                        {
                            cmd.CommandText = ",(?,?)";
                            cmd.Parameters.AddRange(new OdbcParameter[] { new OdbcParameter("v" + i, p.Notes[i].Note_Id), new OdbcParameter("n" + i, p.Notes[i].Note_Value) });
                        }
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Data Processing Failed to write, view inner exceltion for details", e);
                    }
                }
            }
        }
        protected void Update_Package(Package p)
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
                    cmd.CommandText = "UPDATE packages SET package_po_id=?,package_carrier_id=?,package_vendor_id=?,package_deliv_to_id=?,package_devliv_by_id=?,package_signed_for_by_id=?,package_tracking_number=?,package_received_date=?,package_deliver_date=?,package_status=? WHERE package_id = ?";
                    //most fields can be null so we need to check and make sure that if a field is empty that we set  ids to 0 or null strings
                    //ids all will be 0 for null, strings should roll to null
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("poid", p.PackagePurchaseOrder.PO_Id),
                        new OdbcParameter("carrierid", p.PackageCarrier.CarrierId),
                        new OdbcParameter("vendid",p.PackageVendor.VendorId),
                        new OdbcParameter("delivtoid",p.PackageDeliveredTo.Id),
                        new OdbcParameter("delivbyid",p.PackageDeleveredBy.Id),
                        new OdbcParameter("signedbyid",p.PackageSignedForBy.Id),
                        new OdbcParameter("tracknumb",p.PackageTrackingNumber),
                        new OdbcParameter("recieveddate",p.PackageReceivedDate),
                        new OdbcParameter("delivDate",p.PackageDeliveredDate),
                        new OdbcParameter("packstats",p.Status.ToString()),
                        new OdbcParameter("packid",p.PackageId)
                    });
                    foreach (Note note in p.Notes)
                    {
                        if (note.Note_Id == 0)
                        {
                            //insert new note
                            cmd.CommandText += "INSERT INTO notes(note_id,note_value)VALUES(?,?);";
                            cmd.Parameters.AddRange(new OdbcParameter[]
                            {
                                new OdbcParameter("pid",p.Package_PersonId),
                                new OdbcParameter("noteval",note.Note_Value)
                            });
                        }
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Data Processing Failed to write, view inner exceltion for details", e);
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
                        cmd.CommandText += "SELECT users.user_id, users.user_fname,users.user_lname,users.user_name,CONVERT(nvarchar, DecryptByKey(users.user_password)) AS 'Password',users.user_role_id,person_id FROM users WHERE users.user_id = ?;";
                        cmd.CommandText += "CLOSE SYMMETRIC KEY secure_data;";
                    }
                    else if (DBType == SQLHelperClass.DatabaseType.MySQL)
                    {
                        cmd.CommandText = "SELECT user_id, user_fname, user_lname, user_name, CAST(AES_DECRYPT(user_password,'" + EncodeKey + "') AS CHAR(300)) AS 'Password',user_role_id,person_id FROM users WHERE users.user_id = ?;";
                    }
                    cmd.Parameters.AddWithValue("userId", id);
                    User u = new User();
                    long rid = 0;
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            u.Id = Convert.ToInt64(reader[0].ToString());
                            u.FirstName = reader[1].ToString();
                            u.LastName = reader[2].ToString();
                            u.Username = reader[3].ToString();
                            u.PassWord = reader[4].ToString();
                            rid = Convert.ToInt64(reader[5].ToString());
                            u.Person_Id = reader[6].ToString();
                        }
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT * FROM roles WHERE role_id = ?";
                    cmd.Parameters.AddWithValue("role_id", rid);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            u.Level = new Models.ModelData.Role()
                            {
                                Role_id = Convert.ToInt64(reader[0].ToString()),
                                Role_Title = reader[1].ToString()
                            };
                        }
                    }
                    return u;
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
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    if (DBType == SQLHelperClass.DatabaseType.MSSQL)
                    {
                        cmd.CommandText = "OPEN SYMMETRIC KEY secure_data DECRYPTION BY PASSWORD = '" + EncodeKey + "';";
                        cmd.CommandText += "SELECT users.user_id, users.user_fname,users.user_lname,users.user_name,CONVERT(nvarchar, DecryptByKey(users.user_password)) AS 'Password',users.user_role_id,person_id FROM users WHERE users.user_name = ?;";
                        cmd.CommandText += "CLOSE SYMMETRIC KEY secure_data;";
                    }
                    else if (DBType == SQLHelperClass.DatabaseType.MySQL)
                    {
                        cmd.CommandText = "SELECT user_id, user_fname, user_lname, user_name, CAST(AES_DECRYPT(user_password,'" + EncodeKey + "') AS CHAR(300)) AS 'Password',user_role_id,person_id FROM users WHERE users.user_name = ?;";
                    }
                    cmd.Parameters.AddWithValue("userName", username);
                    User u = new User();
                    long rid = 0;
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            u.Id = Convert.ToInt64(reader[0].ToString());
                            u.FirstName = reader[1].ToString();
                            u.LastName = reader[2].ToString();
                            u.Username = reader[3].ToString();
                            u.PassWord = reader[4].ToString();
                            rid = Convert.ToInt64(reader[5].ToString());
                            u.Person_Id = reader[6].ToString();
                        }
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT * FROM roles WHERE role_id = ?";
                    cmd.Parameters.AddWithValue("role_id", rid);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            u.Level = new Models.ModelData.Role()
                            {
                                Role_id = Convert.ToInt64(reader[0].ToString()),
                                Role_Title = reader[1].ToString()
                            };
                        }
                    }
                    return u;
                }
            }
        }
        /// <summary>
        /// returns all users to dataconnection classes datalists class users binding list
        /// </summary>
        protected void GetUserList()
        {
            DataConnectionClass.DataLists.UsersList.Clear();
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;

            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("",c))
                {
                    if (DBType == SQLHelperClass.DatabaseType.MSSQL)
                    {
                        cmd.CommandText = "OPEN SYMMETRIC KEY secure_data DECRYPTION BY PASSWORD = '" + EncodeKey + "';";
                        cmd.CommandText += "SELECT users.user_id, users.user_fname,users.user_lname,users.user_name,CONVERT(nvarchar, DecryptByKey(users.user_password)) AS 'Password',users.user_role_id,person_id FROM users;";
                        cmd.CommandText += "CLOSE SYMMETRIC KEY secure_data;";
                        List<long> rids = new List<long>() { };
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User u = new User()
                                {
                                    Id = Convert.ToInt64(reader[0].ToString()),
                                    FirstName = reader[1].ToString(),
                                    LastName = reader[2].ToString(),
                                    Username = reader[3].ToString(),
                                    PassWord = reader[4].ToString(),
                                    Person_Id = reader[6].ToString()
                                };
                                rids.Add(Convert.ToInt64(reader[5].ToString()));
                                DataConnectionClass.DataLists.UsersList.Add(u);
                            }
                        }
                        int cnt = 0;
                        foreach (User u in DataConnectionClass.DataLists.UsersList)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = "SELECT * FROM roles WHERE role_id = ?;";
                            cmd.Parameters.AddWithValue("role_id", rids[cnt]);
                            using (OdbcDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    u.Level = new Models.ModelData.Role()
                                    {
                                        Role_id = Convert.ToInt64(reader[0].ToString()),
                                        Role_Title = reader[1].ToString()
                                    };
                                }
                            }
                            cnt++;
                        }
                    }
                    else if (DBType == SQLHelperClass.DatabaseType.MySQL)
                    {
                        List<long> rids = new List<long>() { };
                        cmd.CommandText = "SELECT user_id, user_fname, user_lname, user_name, CAST(AES_DECRYPT(user_password,'" + EncodeKey + "') AS CHAR(300)) AS 'Password',user_role_id,person_id FROM users;";
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User u = new User()
                                {
                                    Id = Convert.ToInt64(reader[0].ToString()),
                                    FirstName = reader[1].ToString(),
                                    LastName = reader[2].ToString(),
                                    Username = reader[3].ToString(),
                                    PassWord = reader[4].ToString(),
                                    Person_Id = reader[6].ToString()
                                };
                                rids.Add(Convert.ToInt64(reader[5].ToString()));
                                DataConnectionClass.DataLists.UsersList.Add(u);
                            }
                        }
                        int cnt = 0;
                        foreach (User u in DataConnectionClass.DataLists.UsersList)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = "SELECT * FROM roles WHERE role_id = ?;";
                            cmd.Parameters.AddWithValue("role_id", rids[cnt]);
                            using (OdbcDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    u.Level = new Models.ModelData.Role()
                                    {
                                        Role_id = Convert.ToInt64(reader[0].ToString()),
                                        Role_Title = reader[1].ToString()
                                    };
                                }
                            }
                            cnt++;
                        }
                    }
                    else
                    {
                        DatabaseConnectionException e = new DatabaseConnectionException("You Must select a valid database type", new Exception());
                    }
                }
            }
        }
        /// <summary>
        /// Gets selected vendor from list by id
        /// </summary>
        /// <returns></returns>
        protected Vendors GetVendor_From_Database(long id)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            Vendors v = new Vendors() { };
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    cmd.CommandText = "SELECT vend_id, person_id, vendor_name, vendor_poc_name FROM vendors WHERE vend_id = ?;";
                    cmd.Parameters.AddWithValue("vend_id", id);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            v.VendorId = Convert.ToInt64(reader[0].ToString());
                            v.Vendor_PersonId = reader[1].ToString();
                            v.VendorName = reader[2].ToString();
                            v.VendorPointOfContactName = reader[3].ToString();
                        }
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT phone_id, phone_number FROM phone_numbers WHERE person_id = ?;";
                    cmd.Parameters.AddWithValue("person_id", v.Vendor_PersonId);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            v.VendorPhone = new Models.ModelData.PhoneNumber()
                            {
                                PhoneId = Convert.ToInt64(reader[0].ToString()),
                                Phone_Number = reader[1].ToString()
                            };
                        }
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT address_id, building_long_name, building_short_name,room_number,addr_line1,addr_line2,addr_city,addr_state,addr_zip,addr_cntry,address_note_id FROM physical_addr WHERE person_id = ?;";
                    cmd.Parameters.AddWithValue("person_id", v.Vendor_PersonId);
                    string noteid = "";
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            v.VendorAddress = new Models.ModelData.PhysicalAddress()
                            {
                                AddressId = Convert.ToInt64(reader[0].ToString()),
                                BuildingLongName = reader[1].ToString(),
                                BuildingShortName = reader[2].ToString(),
                                BuildingRoomNumber = reader[3].ToString(),
                                Line1 = reader[4].ToString(),
                                Line2 = reader[5].ToString(),
                                City = reader[6].ToString(),
                                State = reader[7].ToString(),
                                ZipCode = reader[8].ToString(),
                                Country = reader[9].ToString()
                            };
                            noteid = reader[10].ToString();
                        }
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT id, note_value FROM notes WHERE note_id = ?;";
                    cmd.Parameters.AddWithValue("note_id", v.Vendor_PersonId);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            v.Notes.Add
                            (
                                new Models.ModelData.Note()
                                {
                                    Note_Id = Convert.ToInt64(reader[0].ToString()),
                                    Note_Value = reader[1].ToString()
                                }
                            );
                        }
                    }
                }
            }
            return v;
        }
        /// <summary>
        /// Gets all vendors from the database
        /// </summary>
        protected void GetVendorsList()
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    cmd.CommandText = "SELECT vend_id, person_id, vendor_name, vendor_poc_name FROM vendors;";
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Vendors v = new Vendors() { };
                            v.VendorId = Convert.ToInt64(reader[0].ToString());
                            v.Vendor_PersonId = reader[1].ToString();
                            v.VendorName = reader[2].ToString();
                            v.VendorPointOfContactName = reader[3].ToString();
                            DataConnectionClass.DataLists.Vendors.Add(v);
                        }
                    }
                    foreach (Vendors vend in DataConnectionClass.DataLists.Vendors)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "SELECT phone_id, phone_number FROM phone_numbers WHERE person_id = ?;";
                        cmd.Parameters.AddWithValue("person_id", vend.Vendor_PersonId);
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vend.VendorPhone = new Models.ModelData.PhoneNumber()
                                {
                                    PhoneId = Convert.ToInt64(reader[0].ToString()),
                                    Phone_Number = reader[1].ToString()
                                };
                            }
                        }
                        cmd.Parameters.Clear();
                        cmd.CommandText = "SELECT address_id, building_long_name, building_short_name,room_number,addr_line1,addr_line2,addr_city,addr_state,addr_zip,addr_cntry,address_note_id FROM physical_addr WHERE person_id = ?;";
                        cmd.Parameters.AddWithValue("person_id", vend.Vendor_PersonId);
                        string noteid = "";
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vend.VendorAddress = new Models.ModelData.PhysicalAddress()
                                {
                                    AddressId = Convert.ToInt64(reader[0].ToString()),
                                    BuildingLongName = reader[1].ToString(),
                                    BuildingShortName = reader[2].ToString(),
                                    BuildingRoomNumber = reader[3].ToString(),
                                    Line1 = reader[4].ToString(),
                                    Line2 = reader[5].ToString(),
                                    City = reader[6].ToString(),
                                    State = reader[7].ToString(),
                                    ZipCode = reader[8].ToString(),
                                    Country = reader[9].ToString()
                                };
                                noteid = reader[10].ToString();
                            }
                        }
                        cmd.Parameters.Clear();
                        cmd.CommandText = "SELECT id, note_value FROM notes WHERE note_id = ?;";
                        cmd.Parameters.AddWithValue("note_id", vend.Vendor_PersonId);
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vend.Notes.Add
                                (
                                    new Models.ModelData.Note()
                                    {
                                        Note_Id = Convert.ToInt64(reader[0].ToString()),
                                        Note_Value = reader[1].ToString()
                                    }
                                );
                            }
                        }
                    }
                    
                }
            }
        }
        protected Carrier Get_Carrier(long id)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                OdbcTransaction tr = c.BeginTransaction();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    Carrier car = null;
                    cmd.CommandText = "SELECT carrier_id, carrier_name, person_id FROM carriers WHERE carrier_id = ?;";
                    cmd.Parameters.Add(new OdbcParameter("carid", id));
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            car = new Carrier()
                            {
                                CarrierId = Convert.ToInt64(reader[0].ToString()),
                                CarrierName = reader[1].ToString(),
                                Carrier_PersonId = reader[2].ToString()
                            };
                        }
                    }
                    //clear params::
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT phone_id, phone_number FROM phone_numbers WHERE person_id = ?;";
                    cmd.Parameters.Add(new OdbcParameter("personid", car.Carrier_PersonId));
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            car.PhoneNumber = new PhoneNumber()
                            {
                                PhoneId = Convert.ToInt64(reader[0].ToString()),
                                Phone_Number = reader[1].ToString()
                            };
                        }
                    }
                    //clear params::
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT phone_id, phone_number FROM phone_numbers WHERE person_id = ?;";
                    cmd.Parameters.Add(new OdbcParameter("personid", car.Carrier_PersonId));
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            car.Notes.Add(new Note()
                            {
                                Note_Id = Convert.ToInt64(reader[0].ToString()),
                                Note_Value = reader[1].ToString()
                            });
                        }
                    }
                    return car;
                }
            }
        }
        protected void Get_Carrier_List()
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                OdbcTransaction tr = c.BeginTransaction();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    Carrier car = null;
                    BindingList<Carrier> carList = new BindingList<Carrier>() { };
                    cmd.CommandText = "SELECT carrier_id, carrier_name, person_id FROM carriers;";
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            car = new Carrier()
                            {
                                CarrierId = Convert.ToInt64(reader[0].ToString()),
                                CarrierName = reader[1].ToString(),
                                Carrier_PersonId = reader[2].ToString()
                            };
                            carList.Add(car);
                        }
                    }
                    foreach (Carrier carrier in carList)
                    {
                        //clear params::
                        cmd.Parameters.Clear();
                        cmd.CommandText = "SELECT phone_id, phone_number FROM phone_numbers WHERE person_id = ?;";
                        cmd.Parameters.Add(new OdbcParameter("personid", carrier.Carrier_PersonId));
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                carrier.PhoneNumber = new PhoneNumber()
                                {
                                    PhoneId = Convert.ToInt64(reader[0].ToString()),
                                    Phone_Number = reader[1].ToString()
                                };
                            }
                        }
                        //clear params::
                        cmd.Parameters.Clear();
                        cmd.CommandText = "SELECT id, note_value FROM notes WHERE note_id = ?;";
                        cmd.Parameters.Add(new OdbcParameter("personid", carrier.Carrier_PersonId));
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                carrier.Notes.Add(new Note()
                                {
                                    Note_Id = Convert.ToInt64(reader[0].ToString()),
                                    Note_Value = reader[1].ToString()
                                });
                            }
                        }
                    }
                    DataConnectionClass.DataLists.CarriersList = carList;
                }
            }
        }
        protected Faculty Get_Faculty(long id)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            Faculty f = null;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                OdbcTransaction tr = c.BeginTransaction();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    cmd.CommandText = "SELECT empl_id, empl_fname, empl_lname, person_id FROM employees WHERE empl_id = ?;";
                    cmd.Parameters.Add(new OdbcParameter("id", id));
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            f = new Faculty()
                            {
                                Id = Convert.ToInt64(reader[0].ToString()),
                                FirstName = reader[1].ToString(),
                                LastName = reader[2].ToString(),
                                Faculty_PersonId = reader[3].ToString()
                            };
                        }
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT email_id, email_address FROM email_addresses WHERE person_id = ?;";
                    cmd.Parameters.Add(new OdbcParameter("per_id", f.Faculty_PersonId));
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            f.Email.Add(new EmailAddress()
                            {
                                Email_Id = Convert.ToInt64(reader[0].ToString()),
                                Email_Address = reader[1].ToString()
                            });
                        }
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT phone_id, phone_number FROM phone_numbers WHERE person_id = ?;";
                    cmd.Parameters.Add(new OdbcParameter("per_id", f.Faculty_PersonId));
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            f.Phone.Add(new PhoneNumber()
                            {
                                PhoneId = Convert.ToInt64(reader[0].ToString()),
                                Phone_Number = reader[1].ToString()
                            });
                        }
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT id, note_value FROM notes WHERE note_id = ?;";
                    cmd.Parameters.Add(new OdbcParameter("per_id", f.Faculty_PersonId));
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            f.Notes.Add(new Note()
                            {
                                Note_Id = Convert.ToInt64(reader[0].ToString()),
                                Note_Value = reader[1].ToString()
                            });
                        }
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT address_id, building_long_name, building_short_name, room_number, addr_line1, addr_line2, addr_city, addr_state, addr_zip, addr_cntry FROM physical_addr WHERE person_id = ?;";
                    cmd.Parameters.Add(new OdbcParameter("per_id", f.Faculty_PersonId));
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            f.Address.Add(new PhysicalAddress()
                            {
                                AddressId = Convert.ToInt64(reader[0].ToString()),
                                BuildingLongName = reader[1].ToString(),
                                BuildingShortName = reader[2].ToString(),
                                BuildingRoomNumber = reader[3].ToString(),
                                Line1 = reader[4].ToString(),
                                Line2 = reader[5].ToString(),
                                City = reader[6].ToString(),
                                State = reader[7].ToString(),
                                ZipCode = reader[8].ToString(),
                                Country = reader[9].ToString()
                            });
                        }
                    }
                    return f;
                }
            }
        }
        protected void Get_Faculty_List()
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            BindingList<Faculty> f = new BindingList<Faculty>() { };
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    cmd.CommandText = "SELECT empl_id, empl_fname, empl_lname, person_id FROM employees;";
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            f.Add(new Faculty()
                            {
                                Id = Convert.ToInt64(reader[0].ToString()),
                                FirstName = reader[1].ToString(),
                                LastName = reader[2].ToString(),
                                Faculty_PersonId = reader[3].ToString()
                            });
                        }
                    }
                    foreach (Faculty fac in f)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "SELECT email_id, email_address FROM email_addresses WHERE person_id = ?;";
                        cmd.Parameters.Add(new OdbcParameter("per_id", fac.Faculty_PersonId));
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                fac.Email.Add(new EmailAddress()
                                {
                                    Email_Id = Convert.ToInt64(reader[0].ToString()),
                                    Email_Address = reader[1].ToString()
                                });
                            }
                        }
                        cmd.Parameters.Clear();
                        cmd.CommandText = "SELECT phone_id, phone_number FROM phone_numbers WHERE person_id = ?;";
                        cmd.Parameters.Add(new OdbcParameter("per_id", fac.Faculty_PersonId));
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                fac.Phone.Add(new PhoneNumber()
                                {
                                    PhoneId = Convert.ToInt64(reader[0].ToString()),
                                    Phone_Number = reader[1].ToString()
                                });
                            }
                        }
                        cmd.Parameters.Clear();
                        cmd.CommandText = "SELECT id, note_value FROM notes WHERE note_id = ?;";
                        cmd.Parameters.Add(new OdbcParameter("per_id", fac.Faculty_PersonId));
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                fac.Notes.Add(new Note()
                                {
                                    Note_Id = Convert.ToInt64(reader[0].ToString()),
                                    Note_Value = reader[1].ToString()
                                });
                            }
                        }
                        cmd.Parameters.Clear();
                        cmd.CommandText = "SELECT address_id, building_long_name, building_short_name, room_number, addr_line1, addr_line2, addr_city, addr_state, addr_zip, addr_cntry FROM physical_addr WHERE person_id = ?;";
                        cmd.Parameters.Add(new OdbcParameter("per_id", fac.Faculty_PersonId));
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                fac.Address.Add(new PhysicalAddress()
                                {
                                    AddressId = Convert.ToInt64(reader[0].ToString()),
                                    BuildingLongName = reader[1].ToString(),
                                    BuildingShortName = reader[2].ToString(),
                                    BuildingRoomNumber = reader[3].ToString(),
                                    Line1 = reader[4].ToString(),
                                    Line2 = reader[5].ToString(),
                                    City = reader[6].ToString(),
                                    State = reader[7].ToString(),
                                    ZipCode = reader[8].ToString(),
                                    Country = reader[9].ToString()
                                });
                            }
                        }
                    }
                    DataConnectionClass.DataLists.FacultyList = f;
                }
            }
        }
        protected PurchaseOrder Get_PurchaseOrder(long id)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            PurchaseOrder p = null;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                p = new PurchaseOrder() { };
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    cmd.CommandText = "SELECT po_id,po_number,po_package_count,po_created_on,po_created_by,po_approved_by FROM purchase_order WHERE po_id = ?;";
                    cmd.Parameters.Add(new OdbcParameter("poid",id));
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            p.PO_Id = Convert.ToInt64(reader[0].ToString());
                            p.PONumber = reader[1].ToString();
                            p.PackageCount = Convert.ToInt32(reader[2].ToString());
                            p.POCreatedOn = reader[3].ToString();
                            p.CreatedBy = new Faculty() { Id = Convert.ToInt64(reader[4].ToString()) };
                            p.ApprovedBy = new Faculty() { Id = Convert.ToInt64(reader[5].ToString()) };
                        }
                    }
                }
                return p;
            }
        }
        protected Package Get_Package(long id)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    long a = 0, b = 0, g = 0, d = 0, i = 0, f = 0; Package p = new Package() { };
                    cmd.CommandText = "SELECT package_po_id,package_carrier_id,package_vendor_id,package_deliv_to_id,package_devliv_by_id,package_signed_for_by_id,package_tracking_number,package_received_date,package_deliver_date,package_note_id,package_status FROM packages WHERE packageid=?;";
                    cmd.Parameters.AddWithValue("pid", id);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            p = new Package()
                            {
                                PackageTrackingNumber = reader["package_tracking_number"].ToString(),
                                PackageReceivedDate = reader["package_received_date"].ToString(),
                                PackageDeliveredDate = reader["package_deliver_date"].ToString(),
                                Package_PersonId = reader["package_note_id"].ToString(),
                                Status = (Package.DeliveryStatus)Convert.ToInt32(reader["package_status"].ToString())
                            };
                            a = Convert.ToInt64(reader[0].ToString()); //po
                            b = Convert.ToInt64(reader[1].ToString()); //carrier
                            d = Convert.ToInt64(reader[2].ToString()); //vendor
                            f = Convert.ToInt64(reader[3].ToString()); //fac
                            g = Convert.ToInt64(reader[4].ToString()); //usr
                            i = Convert.ToInt64(reader[5].ToString()); //fac
                            p.PackagePurchaseOrder = Get_PurchaseOrder(a);
                            p.PackageCarrier = Get_Carrier(b);
                            p.PackageVendor = GetVendor_From_Database(d);
                            p.PackageDeliveredTo = Get_Faculty(f);
                            p.PackageDeleveredBy = GetUser(g);
                            p.PackageSignedForBy = Get_Faculty(i);
                        }
                    }
                    cmd.CommandText = "SELECT id,note_val FROM notes WHERE note_id = ?;";
                    cmd.Parameters.AddWithValue("nid", p.Package_PersonId);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Note n = new Note()
                            {
                                Note_Id = Convert.ToInt64(reader[0].ToString()),
                                Note_Value = reader[1].ToString()
                            };
                            p.Notes.Add(n);
                        }
                    }
                    return p;
                }
            }
        }
        protected void Get_Package_List()
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    long a = 0, b = 0, g = 0, d = 0, i = 0, f = 0; Package p = new Package() { };
                    cmd.CommandText = "SELECT package_po_id,package_carrier_id,package_vendor_id,package_deliv_to_id,package_devliv_by_id,package_signed_for_by_id,package_tracking_number,package_received_date,package_deliver_date,package_note_id,package_status FROM packages;";
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            p = new Package()
                            {
                                PackageTrackingNumber = reader["package_tracking_number"].ToString(),
                                PackageReceivedDate = reader["package_received_date"].ToString(),
                                PackageDeliveredDate = reader["package_deliver_date"].ToString(),
                                Package_PersonId = reader["package_note_id"].ToString(),
                                Status = (Package.DeliveryStatus)Convert.ToInt32(reader["package_status"].ToString())
                            };
                            a = Convert.ToInt64(reader[0].ToString()); //po
                            b = Convert.ToInt64(reader[1].ToString()); //carrier
                            d = Convert.ToInt64(reader[2].ToString()); //vendor
                            f = Convert.ToInt64(reader[3].ToString()); //fac
                            g = Convert.ToInt64(reader[4].ToString()); //usr
                            i = Convert.ToInt64(reader[5].ToString()); //fac
                            p.PackagePurchaseOrder = Get_PurchaseOrder(a);
                            p.PackageCarrier = Get_Carrier(b);
                            p.PackageVendor = GetVendor_From_Database(d);
                            p.PackageDeliveredTo = Get_Faculty(f);
                            p.PackageDeleveredBy = GetUser(g);
                            p.PackageSignedForBy = Get_Faculty(i);
                            DataConnectionClass.DataLists.Packages.Add(p);
                        }
                    }
                    foreach (Package pac in DataConnectionClass.DataLists.Packages)
                    {
                        cmd.CommandText = "SELECT id,note_val FROM notes WHERE note_id = ?;";
                        cmd.Parameters.AddWithValue("nid", pac.Package_PersonId);
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Note n = new Note()
                                {
                                    Note_Id = Convert.ToInt64(reader[0].ToString()),
                                    Note_Value = reader[1].ToString()
                                };
                                pac.Notes.Add(n);
                            }
                        }
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
    internal class DatabaseConnectionException:Exception
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DatabaseConnectionException():base()
        {
        }
        /// <summary>
        /// exception with message
        /// </summary>
        /// <param name="message">Exception message for outer exception</param>
        public DatabaseConnectionException(string message):base(message)
        {
        }
        /// <summary>
        /// Exception with outer exception message and inner exception class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="insideException"></param>
        public DatabaseConnectionException(string message, Exception insideException) : base(message, insideException)
        {
        }
    }
}
