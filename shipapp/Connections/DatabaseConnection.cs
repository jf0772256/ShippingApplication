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
        internal DatabaseConnection(string connection, string encode, SQLHelperClass.DatabaseType dbt)
        {
            DBType = dbt;
            ConnString = connection;
            EncodeKey = encode;
        }
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
                    "CREATE TABLE IF NOT EXISTS buildings(building_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, building_long_name VARCHAR(250) NOT NULL, building_short_name VARCHAR(100) NOT NULL)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE INDEX idx_building ON buildings(building_short_name);",
                    "CREATE TABLE IF NOT EXISTS notes(id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, note_id VARCHAR(1000) NOT NULL, note_value VARCHAR(5000) NOT NULL)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE INDEX idx_note_ids ON notes(note_id);",

                    "CREATE TABLE IF NOT EXISTS users(user_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, user_fname VARCHAR(100) NOT NULL, user_lname VARCHAR(100) NOT NULL, user_name VARCHAR(100) NOT NULL UNIQUE, user_password VARBINARY(500) NOT NULL, user_role_id BIGINT, person_id VARCHAR(1000) NOT NULL UNIQUE, FOREIGN KEY (user_role_id) REFERENCES roles(role_id) ON DELETE NO ACTION ON UPDATE NO ACTION)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS employees(empl_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, empl_fname VARCHAR(100) NOT NULL, empl_lname VARCHAR(100), building_id BIGINT, building_room_number VARCHAR(20), person_id VARCHAR(1000) NOT NULL UNIQUE)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS vendors(vend_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT,vendor_name VARCHAR(100) NOT NULL UNIQUE)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS carriers(carrier_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, carrier_name VARCHAR(100) NOT NULL UNIQUE)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    
                    "CREATE TABLE IF NOT EXISTS packages(package_id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT,package_po varchar(1000), package_carrier VARCHAR(1000), package_vendor VARCHAR(1000), package_deliv_to VARCHAR(1000), package_deliv_by VARCHAR(1000), package_signed_for_by VARCHAR(1000), package_tracking_number VARCHAR(50) DEFAULT NULL, package_receive_date VARCHAR(50),package_deliv_bldg VARCHAR(100), package_deliver_date VARCHAR(50), package_notes_id VARCHAR(1000) NOT NULL UNIQUE,package_status INT DEFAULT 0, last_modified VARCHAR(100) NOT NULL)engine=INNODB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;",
                    "CREATE TABLE IF NOT EXISTS idcounter(id BIGINT NOT NULL PRIMARY KEY, id_value BIGINT NOT NULL, last_id VARCHAR(1000) DEFAULT NULL); "
                };
            }
            else if (DBType == SQLHelperClass.DatabaseType.MSSQL)
            {
                //"IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = roles)CREATE TABLE ",
                cmdTxt = new List<string>(){
                    //attempt to create the first table as a test;;
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'roles')CREATE TABLE roles(role_id BigINT NOT NULL IDENTITY(1,1) PRIMARY KEY, role_title VARCHAR(50) NOT NULL, CONSTRAINT UC_Roles UNIQUE(role_title));",

                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'buildings')CREATE TABLE buildings(building_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, building_long_name VARCHAR(250) NOT NULL, building_short_name VARCHAR(100) NOT NULL);CREATE INDEX idx_bldng on buildings(building_short_name);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'notes')CREATE TABLE notes(id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, note_id VARCHAR(1000) NOT NULL, note_value VARCHAR(5000) NOT NULL);CREATE INDEX idx_note_ids ON notes(note_id);",

                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'users')CREATE TABLE users(user_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, user_fname VARCHAR(2000) NOT NULL, user_lname VARCHAR(2000) NOT NULL, user_name VARCHAR(1000) NOT NULL, user_password VARBINARY(8000) NOT NULL, user_role_id BIGINT FOREIGN KEY REFERENCES roles(role_id), person_id VARCHAR(1000) NOT NULL, CONSTRAINT UC_UserName UNIQUE(user_name), CONSTRAINT UC_PID5 UNIQUE(person_id));",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'employees')CREATE TABLE employees(empl_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, empl_fname VARCHAR(50) NOT NULL, empl_lname VARCHAR(50), building_id BIGINT, building_room_number VARCHAR(20), person_id VARCHAR(1000) NOT NULL, CONSTRAINT UC_PID_0 UNIQUE(person_id));",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'vendors')CREATE TABLE vendors(vend_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, vendor_name VARCHAR(50) NOT NULL UNIQUE);",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'carriers')CREATE TABLE carriers(carrier_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, carrier_name VARCHAR(50) NOT NULL UNIQUE);",

                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'packages')CREATE TABLE packages(package_id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,package_po VARCHAR(1000), package_carrier VARCHAR(1000), package_vendor VARCHAR(1000), package_deliv_to VARCHAR(1000), package_deliv_by VARCHAR(1000), package_signed_for_by VARCHAR(1000), package_tracking_number VARCHAR(50) DEFAULT NULL, package_receive_date VARCHAR(50),package_deliv_bldg VARCHAR(100), package_deliver_date VARCHAR(50), package_note_id VARCHAR(1000) NOT NULL, package_status INT DEFAULT 0, last_modified VARCHAR(100) NOT NULL, CONSTRAINT UC_NID UNIQUE(package_note_id));",
                    "IF NOT EXISTS(SELECT [name] FROM sys.tables WHERE [name] = 'idcounter')CREATE TABLE idcounter(id BIGINT NOT NULL PRIMARY KEY, id_value BIGINT NOT NULL, last_id VARCHAR(1000) DEFAULT NULL);",
                    "IF (SELECT COUNT(*) FROM sys.symmetric_keys WHERE name = 'secure_data')=0 CREATE SYMMETRIC KEY secure_data WITH ALGORITHM = AES_128 ENCRYPTION BY PASSWORD = '" + EncodeKey +"';"
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
                DoDefaultInserts();
            }
        }
        private void DoDefaultInserts()
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            List<string> cmdTxt = new List<string>() { };
            if (DBType == SQLHelperClass.DatabaseType.MSSQL)
            {
                cmdTxt = new List<string>()
                {
                    "INSERT INTO roles(role_title)VALUES('Administrator'),('Dock Supervisor'),('Supervisor'),('User');",
                    "INSERT INTO idcounter(id,id_value)VALUES(1,0);",
                    //"OPEN SYMMETRIC KEY secure_data DECRYPTION BY PASSWORD = '" + EncodeKey + "';",
                    //"INSERT INTO users(users.user_fname,users.user_lname,users.user_name,users.user_password,users.user_role_id,person_id)VALUES('Danny','Lane','danny_lane',EncryptByKey(Key_GUID('secure_data'),CONVERT(nvarchar,'DannyLane')),1,'dannyl001');",
                    //"CLOSE SYMMETRIC KEY secure_data;"
                };
            }
            else if (DBType == SQLHelperClass.DatabaseType.MySQL)
            {
                cmdTxt = new List<string>()
                {
                    "INSERT INTO roles(role_title)VALUES('Administrator'),('Dock Supervisor'),('Supervisor'),('User');",
                    "INSERT INTO idcounter(id,id_value)VALUES(1,0);",
                    //"INSERT INTO users(users.user_fname,users.user_lname,users.user_name,users.user_password,users.user_role_id,users.person_id)VALUES('Danny','Lane','danny_lane',AES_ENCRYPT(DannyLane,"+EncodeKey+"),1,'dannyl001');"
                };
            }
            else
            {
                //fail
                throw new DatabaseConnectionException("Invalid server type passed.");
            }
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                OdbcTransaction tr = c.BeginTransaction();
                using (OdbcCommand cmd = new OdbcCommand("", c, tr))
                {
                    foreach (string command in cmdTxt)
                    {
                        cmd.CommandText += command;
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception exe)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Data failed to proccess see inner exceotion for further details", exe);
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
        #region Writes
        #region Protected Writes
        protected void Write(User newU)
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
                    using (OdbcCommand cmd = new OdbcCommand("", c, tr))
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
                        PWrite(u.Notes, u.Person_Id);
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
                        cmd.CommandText = "INSERT INTO " + Tables.users.ToString() + " (user_fname,user_lname,user_name,user_password,user_role_id,person_id)VALUES(?,?,?,AES_ENCRYPT(?,'" + EncodeKey + "'),?,?);";
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
                        cmd.CommandText += "INSERT INTO notes(note_id,note_value)VALUES";
                        foreach (Note note in u.Notes)
                        {
                            cmd.CommandText += "(?,?),";
                            cmd.Parameters.AddRange(new OdbcParameter[]
                            {
                                new OdbcParameter("pid",u.Person_Id),
                                new OdbcParameter("note_val",note.Note_Value)
                            });
                        }
                        cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 1);
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
        protected void Write(Vendors v)
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
                    cmd.CommandText = "INSERT INTO vendors(vendor_name)VALUES(?);";
                    cmd.Parameters.AddRange(new OdbcParameter[]{
                        new OdbcParameter("vend_name",v.VendorName)
                    });
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
        protected void Write(Role value)
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
        protected void Write(Carrier value)
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
                    cmd.CommandText = "INSERT INTO carriers (carrier_name)VALUES(?);";
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("carrierN",value.CarrierName)
                    });
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
        protected void Write(Faculty f)
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
                    cmd.CommandText = "INSERT INTO employees (empl_fname,empl_lname,person_id,building_id,building_room_number)VALUES(?,?,?,?,?);";
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("fname",f.FirstName),
                        new OdbcParameter("lname",f.LastName),
                        new OdbcParameter("person_id",f.Faculty_PersonId),
                        new OdbcParameter("bid",f.Building_Id),
                        new OdbcParameter("brmno",f.RoomNumber)
                    });
                    PWrite(f.Notes, f.Faculty_PersonId);
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
        protected void Write(BuildingClass v)
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
                    cmd.CommandText = "INSERT INTO buildings(building_long_name,building_short_name)VALUES(?,?);";
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("long",v.BuildingLongName),
                        new OdbcParameter("short",v.BuildingShortName)
                    });
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed Processing Request.", e);
                    }
                }
            }
        }
        protected void Write(Package p, string datestring)
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
                    cmd.CommandText = "INSERT INTO packages(package_po,package_carrier,package_vendor,package_deliv_to,package_deliv_by,package_signed_for_by,package_tracking_number,package_receive_date,package_deliver_date,package_note_id,package_status,package_deliv_bldg,last_modified)VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?);";
                    //most fields can be null so we need to check and make sure that if a field is empty that we set  ids to 0 or null strings
                    //ids all will be 0 for null, strings should roll to null
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("poid", p.PONumber),
                        new OdbcParameter("carrierid", p.PackageCarrier),
                        new OdbcParameter("vendid",p.PackageVendor),
                        new OdbcParameter("delivtoid",p.PackageDeliveredTo),
                        new OdbcParameter("delivbyid",p.PackageDeleveredBy),
                        new OdbcParameter("signedbyid",p.PackageSignedForBy),
                        new OdbcParameter("tracknumb",p.PackageTrackingNumber),
                        new OdbcParameter("recieveddate",p.PackageReceivedDate),
                        new OdbcParameter("delivDate",p.PackageDeliveredDate),
                        new OdbcParameter("noteid",p.Package_PersonId),
                        new OdbcParameter("packstats",Convert.ToInt32(p.Status)),
                        new OdbcParameter("BuildingShortName", p.DelivBuildingShortName),
                        new OdbcParameter("last_modified",datestring)
                    });
                    PWrite(p.Notes, p.Package_PersonId);
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
        #region Private Writes
        private void PWrite(List<Note> v, string personID)
        {
            if (v.Count <= 0)
            {
                return;
            }
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
                    cmd.CommandText = "INSERT INTO notes (note_id,note_value)VALUES";
                    int cnt = 0;
                    foreach (Note note in v)
                    {
                        if (note.Note_Id == 0)
                        {
                            cmd.CommandText += "(?,?),";
                            cmd.Parameters.AddRange(new OdbcParameter[]
                            {
                            new OdbcParameter("pid",personID),
                            new OdbcParameter("value",note.Note_Value)
                            });
                        }
                    }
                    if (cmd.CommandText.Length > 43)
                    {
                        cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 1) + ";";
                    }
                    else
                    {
                        return;
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed to process request", e);
                    }
                }
            }
        }
        #endregion
        #endregion
        #region Updates
        protected void Update(User v)
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
                        cmd.CommandText = "OPEN SYMMETRIC KEY secure_data DECRYPTION BY PASSWORD='" + EncodeKey + "';";
                        cmd.CommandText += "UPDATE users SET user_fname = ? , user_lname = ? , user_password = EncryptByKey(Key_GUID('secure_data'),CONVERT(nvarchar,?)), user_role_id = ? WHERE user_id = ?;";
                        cmd.CommandText += "CLOSE SYMMETRIC KEY secure_data;";
                        cmd.Parameters.AddRange(new OdbcParameter[]
                        {
                            new OdbcParameter("fname", v.FirstName),
                            new OdbcParameter("lname",v.LastName),
                            new OdbcParameter("pwrd",v.PassWord),
                            new OdbcParameter("role",v.Level.Role_id),
                            new OdbcParameter("id",v.Id)
                        });
                    }
                    else if (DBType == SQLHelperClass.DatabaseType.MySQL)
                    {
                        cmd.CommandText = "UPDATE users SET user_fname = ? , user_lname = ? , user_password = AES_ENCRYPT(?,'" + EncodeKey + "') , user_role_id = ? WHERE user_id = ?;";
                        cmd.Parameters.AddRange(new OdbcParameter[]
                        {
                            new OdbcParameter("fname", v.FirstName),
                            new OdbcParameter("lname",v.LastName),
                            new OdbcParameter("pwrd",v.PassWord),
                            new OdbcParameter("role",v.Level.Role_id),
                            new OdbcParameter("id",v.Id)
                        });
                    }
                    else
                    {
                        throw new SQLHelperException("You must have selected a valid database type. set this value and try again.");
                    }
                    PWrite(v.Notes, v.Person_Id);
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
        protected void Update(Vendors v)
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
                    cmd.CommandText = "UPDATE vendors SET ";
                    cmd.CommandText += "vendor_name = ? ";
                    cmd.CommandText += "WHERE vend_id = ?;";
                    cmd.Parameters.AddRange
                        (
                            new OdbcParameter[]
                            {
                                new OdbcParameter("vendorname",v.VendorName),
                                new OdbcParameter("vendorID",v.VendorId)
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
                        throw new DatabaseConnectionException("Failed to execute, see inner exception for further details.", e);
                    }
                }
            }
        }
        protected void Update(Role value)
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
        protected void Update(Carrier value)
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
                    cmd.CommandText = "UPDATE carriers SET carrier_name=? WHERE carrier_id =?;";
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("carrierN",value.CarrierName),
                        new OdbcParameter("carrierid",value.CarrierId)
                    });
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
        protected void Update(Faculty f)
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
                    PWrite(f.Notes, f.Faculty_PersonId);
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
        protected void Update(Package p)
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
                    cmd.CommandText = "UPDATE packages SET package_po=?,package_carrier=?,package_vendor=?,package_deliv_to=?,package_deliv_by=?,package_signed_for_by=?,package_tracking_number=?,package_receive_date=?,package_deliver_date=?,package_status=?,package_deliv_bldg=? WHERE package_id = ?";
                    //most fields can be null so we need to check and make sure that if a field is empty that we set  ids to 0 or null strings
                    //ids all will be 0 for null, strings should roll to null
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("poid", p.PONumber),
                        new OdbcParameter("carrierid", p.PackageCarrier),
                        new OdbcParameter("vendid",p.PackageVendor),
                        new OdbcParameter("delivtoid",p.PackageDeliveredTo),
                        new OdbcParameter("delivbyid",p.PackageDeleveredBy),
                        new OdbcParameter("signedbyid",p.PackageSignedForBy),
                        new OdbcParameter("tracknumb",p.PackageTrackingNumber),
                        new OdbcParameter("recieveddate",p.PackageReceivedDate),
                        new OdbcParameter("delivDate",p.PackageDeliveredDate),
                        new OdbcParameter("packstats",Convert.ToInt32(p.Status)),
                        new OdbcParameter("bldgshortname",p.DelivBuildingShortName),
                        new OdbcParameter("packid",p.PackageId)
                    });
                    PWrite(p.Notes, p.Package_PersonId);
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
        protected void Update(Package p,string datestring)
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
                    cmd.CommandText = "UPDATE packages SET last_modified=? WHERE package_id = ?";
                    //most fields can be null so we need to check and make sure that if a field is empty that we set  ids to 0 or null strings
                    //ids all will be 0 for null, strings should roll to null
                    cmd.Parameters.AddRange(new OdbcParameter[]
                    {
                        new OdbcParameter("lm", datestring),
                        new OdbcParameter("id", p.PackageId)
                    });
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
        protected void Update(BuildingClass b)
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
                    cmd.CommandText = "UPDATE buildings SET building_long_name = ?, building_short_name=? WHERE building_id = ?;";
                    cmd.Parameters.AddWithValue("bln", b.BuildingLongName);
                    cmd.Parameters.AddWithValue("bsn", b.BuildingShortName);
                    cmd.Parameters.AddWithValue("bid", b.BuildingId);
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
        internal void Update(long i, string lid)
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
                    cmd.CommandText = "UPDATE idcounter SET id_value = ?, last_id=? WHERE id = ?;";
                    cmd.Parameters.Add("value",OdbcType.BigInt).Value = i;
                    cmd.Parameters.Add("lastval",OdbcType.NVarChar).Value = lid;
                    cmd.Parameters.Add("id",OdbcType.BigInt).Value = 1;
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
        #endregion
        #region Deletes
        protected void Delete(User v)
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
                    cmd.CommandText = "DELETE FROM notes WHERE note_id = ?;";
                    cmd.Parameters.AddWithValue("pid", v.Person_Id);
                    cmd.CommandText += "DELETE FROM users WHERE user_id = ?;";
                    cmd.Parameters.Add("uid", OdbcType.BigInt).Value=v.Id;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed processing request.", e);
                    }
                }
            }
        }
        protected void Delete(Faculty v)
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
                    cmd.CommandText = "DELETE FROM notes WHERE note_id = ?;";
                    cmd.Parameters.AddWithValue("pid", v.Faculty_PersonId);
                    cmd.CommandText += "DELETE FROM employees WHERE empl_id = ?;";
                    cmd.Parameters.Add("uid", OdbcType.BigInt).Value = v.Id;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed processing request.", e);
                    }
                }
            }
        }
        protected void Delete(Vendors v)
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
                    cmd.CommandText += "DELETE FROM vendors WHERE vend_id = ?;";
                    cmd.Parameters.Add("uid", OdbcType.BigInt).Value = v.VendorId;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed processing request.", e);
                    }
                }
            }
        }
        protected void Delete(Carrier v)
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
                    cmd.CommandText += "DELETE FROM carriers WHERE carrier_id = ?;";
                    cmd.Parameters.Add("uid", OdbcType.BigInt).Value = v.CarrierId;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed processing request.", e);
                    }
                }
            }
        }
        protected void Delete(Package v)
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
                    cmd.CommandText = "DELETE FROM notes WHERE note_id = ?;";
                    cmd.Parameters.AddWithValue("pid", v.Package_PersonId);
                    cmd.CommandText += "DELETE FROM packages WHERE package_id = ?;";
                    cmd.Parameters.Add("uid", OdbcType.BigInt).Value = v.PackageId;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed processing request.", e);
                    }
                }
            }
        }
        protected void Delete(BuildingClass v)
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
                    cmd.CommandText += "DELETE FROM buildings WHERE building_id = ?;";
                    cmd.Parameters.Add("bid", OdbcType.BigInt).Value = v.BuildingId;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("Failed processing request.", e);
                    }
                }
            }
        }
        #endregion
        #endregion
        #region Get Data From Database
        #region protected gets
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
                using (OdbcCommand cmd = new OdbcCommand("", c, tr))
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
                            u.Id = Convert.ToInt64(reader["user_id"].ToString());
                            u.FirstName = reader["user_fname"].ToString();
                            u.LastName = reader["user_lname"].ToString();
                            u.Username = reader["user_name"].ToString();
                            u.PassWord = reader["Password"].ToString();
                            rid = Convert.ToInt64(reader["user_role_id"].ToString());
                            u.Person_Id = reader["person_id"].ToString();
                        }
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT * FROM roles WHERE role_id = ?";
                    cmd.Parameters.AddWithValue("role_id", rid);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            u.Level = new Role()
                            {
                                Role_id = Convert.ToInt64(reader["role_id"].ToString()),
                                Role_Title = reader["role_title"].ToString()
                            };
                        }
                    }
                    u.Notes = GetNotesListById(u.Person_Id);
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
                    u.Notes = GetNotesListById(u.Person_Id);
                    return u;
                }
            }
        }
        /// <summary>
        /// returns all users to dataconnection classes datalists class users binding list
        /// </summary>
        protected SortableBindingList<User> GetUserList()
        {
            //DataConnectionClass.DataLists.UsersList.Clear();
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            SortableBindingList<User> usr = new SortableBindingList<User>();

            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
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
                                    Id = Convert.ToInt64(reader["user_id"].ToString()),
                                    FirstName = reader["user_fname"].ToString(),
                                    LastName = reader["user_lname"].ToString(),
                                    Username = reader["user_name"].ToString(),
                                    PassWord = reader["Password"].ToString(),
                                    Person_Id = reader["person_id"].ToString()
                                };
                                rids.Add(Convert.ToInt64(reader["user_role_id"].ToString()));
                                usr.Add(u);
                            }
                        }
                        int cnt = 0;
                        foreach (User u in usr)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = "SELECT * FROM roles WHERE role_id = ?;";
                            cmd.Parameters.AddWithValue("role_id", rids[cnt]);
                            using (OdbcDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    u.Level = new Role()
                                    {
                                        Role_id = Convert.ToInt64(reader[0].ToString()),
                                        Role_Title = reader[1].ToString()
                                    };
                                }
                            }
                            u.Notes = GetNotesListById(u.Person_Id);
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
                                    Id = Convert.ToInt64(reader["user_id"].ToString()),
                                    FirstName = reader["user_fname"].ToString(),
                                    LastName = reader["user_lname"].ToString(),
                                    Username = reader["user_name"].ToString(),
                                    PassWord = reader["Password"].ToString(),
                                    Person_Id = reader["person_id"].ToString()
                                };
                                rids.Add(Convert.ToInt64(reader["user_role_id"].ToString()));
                                usr.Add(u);
                            }
                        }
                        int cnt = 0;
                        foreach (User u in usr)
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
                                        Role_id = Convert.ToInt64(reader["role_id"].ToString()),
                                        Role_Title = reader["role_title"].ToString()
                                    };
                                }
                            }
                            u.Notes = GetNotesListById(u.Person_Id);
                            cnt++;
                        }
                    }
                    else
                    {
                        DatabaseConnectionException e = new DatabaseConnectionException("You Must select a valid database type", new Exception());
                    }
                }
            }
            return usr;
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
                    cmd.CommandText = "SELECT vend_id, vendor_name FROM vendors WHERE vend_id = ?;";
                    cmd.Parameters.AddWithValue("vend_id", id);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            v.VendorId = Convert.ToInt64(reader[0].ToString());
                            v.VendorName = reader[1].ToString();
                        }
                    }
                }
            }
            return v;
        }
        /// <summary>
        /// Gets all vendors from the database
        /// </summary>
        protected SortableBindingList<Vendors> GetVendorsList()
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            SortableBindingList<Vendors> ven = new SortableBindingList<Vendors>();
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    cmd.CommandText = "SELECT vend_id, vendor_name FROM vendors;";
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Vendors v = new Vendors() { };
                            v.VendorId = Convert.ToInt64(reader[0].ToString());
                            v.VendorName = reader[1].ToString();
                            ven.Add(v);
                        }
                    }
                    return ven;
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
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    Carrier car = null;
                    cmd.CommandText = "SELECT carrier_id, carrier_name FROM carriers WHERE carrier_id = ?;";
                    cmd.Parameters.Add(new OdbcParameter("carid", id));
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            car = new Carrier()
                            {
                                CarrierId = Convert.ToInt64(reader[0].ToString()),
                                CarrierName = reader[1].ToString()
                            };
                        }
                    }
                    return car;
                }
            }
        }
        protected SortableBindingList<Carrier> Get_Carrier_List()
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
                    Carrier car = null;
                    SortableBindingList<Carrier> carList = new SortableBindingList<Carrier>() { };
                    cmd.CommandText = "SELECT carrier_id, carrier_name FROM carriers;";
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            car = new Carrier()
                            {
                                CarrierId = Convert.ToInt64(reader[0].ToString()),
                                CarrierName = reader[1].ToString()
                            };
                            carList.Add(car);
                        }
                    }
                    return carList;
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
                    cmd.CommandText = "SELECT empl_id, empl_fname, empl_lname, person_id, building_id,building_room_number FROM employees WHERE empl_id = ?;";
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
                                Faculty_PersonId = reader[3].ToString(),
                                Building_Id = Convert.ToInt64(reader[4].ToString()),
                                RoomNumber = reader[5].ToString()
                            };
                        }
                    }
                    f.Notes = GetNotesListById(f.Faculty_PersonId);
                    return f;
                }
            }
        }
        protected SortableBindingList<Faculty> Get_Faculty_List()
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            SortableBindingList<Faculty> f = new SortableBindingList<Faculty>() { };
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    cmd.CommandText = "SELECT empl_id, empl_fname, empl_lname, person_id, building_id,building_room_number FROM employees;";
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            f.Add(new Faculty()
                            {
                                Id = Convert.ToInt64(reader[0].ToString()),
                                FirstName = reader[1].ToString(),
                                LastName = reader[2].ToString(),
                                Faculty_PersonId = reader[3].ToString(),
                                Building_Id = Convert.ToInt64(reader[4].ToString()),
                                RoomNumber = reader[5].ToString()
                            });
                        }
                    }
                    foreach (Faculty fac in f)
                    {
                        fac.Building_Name = (GetBuilding(fac.Building_Id).ToString());
                        fac.Notes = GetNotesListById(fac.Faculty_PersonId);
                    }
                    return f;
                }
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
                    Package p = new Package() { };
                    cmd.CommandText = "SELECT package_id,package_po,package_carrier,package_vendor,package_deliv_to,package_deliv_by,package_signed_for_by,package_tracking_number,package_receive_date,package_deliver_date,package_note_id,package_status,package_deliv_bldg FROM packages WHERE package_id=?;";
                    cmd.Parameters.AddWithValue("pid", id);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            p = new Package()
                            {
                                PackageId = Convert.ToInt64(reader["package_id"].ToString()),
                                PONumber = reader["package_po"].ToString(),
                                PackageTrackingNumber = reader["package_tracking_number"].ToString(),
                                PackageCarrier = reader["package_carrier"].ToString(),
                                PackageVendor = reader["package_vendor"].ToString(),
                                PackageDeleveredBy = reader["package_deliv_by"].ToString(),
                                PackageDeliveredTo = reader["package_deliv_to"].ToString(),
                                PackageSignedForBy = reader["package_signed_for_by"].ToString(),
                                PackageReceivedDate = reader["package_receive_date"].ToString(),
                                PackageDeliveredDate = reader["package_deliver_date"].ToString(),
                                Package_PersonId = reader["package_note_id"].ToString(),
                                Status = (Package.DeliveryStatus)Convert.ToInt32(reader["package_status"].ToString()),
                                DelivBuildingShortName = reader["package_deliv_bldg"].ToString()
                            };
                        }
                    }
                    p.Notes = GetNotesListById(p.Package_PersonId);
                    return p;
                }
            }
        }
        protected SortableBindingList<Package> Get_Package_List(string datetoget)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            SortableBindingList<Package> pkg = new SortableBindingList<Package>();
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    Package p = new Package() { };
                    cmd.CommandText = "SELECT package_id,package_po,package_carrier,package_vendor,package_deliv_to,package_deliv_by,package_signed_for_by,package_tracking_number,package_receive_date,package_deliver_date,package_note_id,package_status,package_deliv_bldg FROM packages WHERE last_modified = '" + datetoget + "';";
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            p = new Package()
                            {
                                PackageId = Convert.ToInt64(reader["package_id"].ToString()),
                                PackageTrackingNumber = reader["package_tracking_number"].ToString(),
                                PONumber = reader["package_po"].ToString(),
                                PackageCarrier = reader["package_carrier"].ToString(),
                                PackageVendor = reader["package_vendor"].ToString(),
                                PackageDeleveredBy = reader["package_deliv_by"].ToString(),
                                PackageDeliveredTo = reader["package_deliv_to"].ToString(),
                                PackageSignedForBy = reader["package_signed_for_by"].ToString(),
                                PackageReceivedDate = reader["package_receive_date"].ToString(),
                                PackageDeliveredDate = reader["package_deliver_date"].ToString(),
                                Package_PersonId = reader["package_note_id"].ToString(),
                                Status = (Package.DeliveryStatus)Convert.ToInt32(reader["package_status"].ToString()),
                                DelivBuildingShortName = reader["package_deliv_bldg"].ToString()
                            };
                            pkg.Add(p);
                        }
                    }
                    foreach (Package pac in pkg)
                    {
                        pac.Notes = GetNotesListById(pac.Package_PersonId);
                    }
                    return pkg;
                }
            }
        }
        protected SortableBindingList<Package> Get_Package_List(string startdate,string enddate)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            SortableBindingList<Package> pkg = new SortableBindingList<Package>();
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    Package p = new Package() { };
                    cmd.CommandText = "SELECT package_id,package_po,package_carrier,package_vendor,package_deliv_to,package_deliv_by,package_signed_for_by,package_tracking_number,package_receive_date,package_deliver_date,package_note_id,package_status,package_deliv_bldg FROM packages where last_modified BETWEEN '" + startdate + "' AND '" + enddate + "';";
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            p = new Package()
                            {
                                PackageId = Convert.ToInt64(reader["package_id"].ToString()),
                                PackageTrackingNumber = reader["package_tracking_number"].ToString(),
                                PONumber = reader["package_po"].ToString(),
                                PackageCarrier = reader["package_carrier"].ToString(),
                                PackageVendor = reader["package_vendor"].ToString(),
                                PackageDeleveredBy = reader["package_deliv_by"].ToString(),
                                PackageDeliveredTo = reader["package_deliv_to"].ToString(),
                                PackageSignedForBy = reader["package_signed_for_by"].ToString(),
                                PackageReceivedDate = reader["package_receive_date"].ToString(),
                                PackageDeliveredDate = reader["package_deliver_date"].ToString(),
                                Package_PersonId = reader["package_note_id"].ToString(),
                                Status = (Package.DeliveryStatus)Convert.ToInt32(reader["package_status"].ToString()),
                                DelivBuildingShortName = reader["package_deliv_bldg"].ToString()
                            };
                            pkg.Add(p);
                        }
                    }
                    foreach (Package pac in pkg)
                    {
                        pac.Notes = GetNotesListById(pac.Package_PersonId);
                    }
                    return pkg;
                }
            }
        }
        protected SortableBindingList<BuildingClass> Get_Building_List()
        {
            SortableBindingList<BuildingClass> bl = new SortableBindingList<BuildingClass>() { };
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    cmd.CommandText = "SELECT building_short_name,building_long_name,building_id FROM buildings ORDER BY building_long_name ASC;";
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bl.Add(new BuildingClass()
                            {
                                BuildingId = Convert.ToInt64(reader[2].ToString()),
                                BuildingLongName = reader[1].ToString(),
                                BuildingShortName = reader[0].ToString()
                            });
                        }
                    }
                }
            }
            return bl;
        }
        internal long GetLastNumericalId()
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            long i = 0;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    cmd.CommandText = "SELECT id_value FROM idcounter;";
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            i = Convert.ToInt64(reader[0].ToString());
                        }
                    }
                }
            }
            return i;
        }
        internal string GetLastStringId()
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            string i = "";
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    cmd.CommandText = "SELECT last_id FROM idcounter;";
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            i = reader[0].ToString();
                        }
                    }
                }
            }
            return i;
        }
        /// <summary>
        /// Completes file back up
        /// saves with {date}.sql
        /// runs tues and thurs
        /// and manually
        /// </summary>
        internal void DoBackup(SQLHelperClass.DatabaseType restoreToDBType)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            string filen = DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".sql";
            string path2bu = Environment.CurrentDirectory + "\\Connections\\Backup\\";
            /***********************************************************************************************/
            try
            {
                using (OdbcConnection c = new OdbcConnection())
                {
                    c.ConnectionString = ConnString;
                    c.Open();
                    using (OdbcCommand cmd = new OdbcCommand("", c))
                    {
                        if (DBType == SQLHelperClass.DatabaseType.MSSQL)
                        {
                            cmd.CommandText = "OPEN SYMMETRIC KEY secure_data DECRYPTION BY PASSWORD = '" + EncodeKey + "';";
                            cmd.CommandText += "SELECT users.user_fname,users.user_lname,users.user_name,CONVERT(nvarchar, DecryptByKey(users.user_password)) AS 'Password',users.user_role_id,person_id FROM users;";
                            cmd.CommandText += "CLOSE SYMMETRIC KEY secure_data;\n";
                        }
                        else if (DBType == SQLHelperClass.DatabaseType.MySQL)
                        {
                            cmd.CommandText = "SELECT user_fname, user_lname, user_name, CAST(AES_DECRYPT(user_password,'" + EncodeKey + "') AS CHAR(300)) AS 'Password',user_role_id,person_id FROM users;";
                        }
                        else
                        {
                            throw new DatabaseConnectionException("Database type is missing or unset.");
                        }
                        string sql = "";
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows == true)
                            {
                                if (restoreToDBType == SQLHelperClass.DatabaseType.MSSQL)
                                {
                                    sql += "OPEN SYMMETRIC KEY secure_data DECRYPTION BY PASSWORD = '" + EncodeKey + "';\n";
                                    sql += "INSERT INTO users(user_fname,user_lname,user_name,user_password,user_role_id,person_id)VALUES";
                                    bool done = false;
                                    while (reader.Read())
                                    {
                                        if (!done)
                                        {
                                            sql += "('" + reader["user_fname"].ToString() + "','" + reader["user_lname"].ToString() + "','" + reader["user_name"].ToString() + "',EncryptByKey(Key_GUID('secure_data'),CONVERT(nvarchar,'" + reader["Password"].ToString() + "'))," + Convert.ToInt64(reader["user_role_id"].ToString()) + ",'" + reader["person_id"].ToString() + "')";
                                            done = true;
                                            continue;
                                        }
                                        sql += ",('" + reader["user_fname"].ToString() + "','" + reader["user_lname"].ToString() + "','" + reader["user_name"].ToString() + "',EncryptByKey(Key_GUID('secure_data'),CONVERT(nvarchar,'" + reader["Password"].ToString() + "'))," + Convert.ToInt64(reader["user_role_id"].ToString()) + ",'" + reader["person_id"].ToString() + "')";
                                    }
                                    sql += ";\n";
                                    sql += "CLOSE SYMMETRIC KEY secure_data;\n";
                                }
                                else if (restoreToDBType == SQLHelperClass.DatabaseType.MySQL)
                                {
                                    bool done = false;
                                    sql += "INSERT INTO users(user_fname,user_lname,user_name,user_password,user_role_id,person_id)VALUES";
                                    while (reader.Read())
                                    {
                                        if (!done)
                                        {
                                            sql += "('" + reader["user_fname"].ToString() + "','" + reader["user_lname"].ToString() + "','" + reader["user_name"].ToString() + "',AES_ENCRYPT('" + reader["Password"].ToString() + "','" + EncodeKey + "')," + Convert.ToInt64(reader["user_role_id"].ToString()) + ",'" + reader["person_id"].ToString() + "')";
                                            done = true;
                                            continue;
                                        }
                                        sql += ",('" + reader["user_fname"].ToString() + "','" + reader["user_lname"].ToString() + "','" + reader["user_name"].ToString() + "',AES_ENCRYPT('" + reader["Password"].ToString() + "','" + EncodeKey + "')," + Convert.ToInt64(reader["user_role_id"].ToString()) + ",'" + reader["person_id"].ToString() + "')";
                                    }
                                    sql += ";\n";
                                }
                            }
                        }
                        cmd.CommandText = "SELECT vendor_name FROM vendors;";
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows == true)
                            {
                                sql += "INSERT INTO vendors (vendor_name)VALUES";
                                bool done = false;
                                while (reader.Read())
                                {
                                    if (!done)
                                    {
                                        sql += "('" + reader["vendor_name"].ToString() + "')";
                                        done = true;
                                        continue;
                                    }
                                    sql += ",('" + reader["vendor_name"].ToString() + "')";
                                }
                                sql += ";\n";
                            }
                        }
                        cmd.CommandText = "SELECT carrier_name FROM carriers;";
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows == true)
                            {
                                sql += "INSERT INTO carriers(carrier_name)VALUES";
                                bool done = false;
                                while (reader.Read())
                                {
                                    if (!done)
                                    {
                                        sql += "('" + reader["carrier_name"].ToString() + "')";
                                        done = true;
                                        continue;
                                    }
                                    sql += ",('" + reader["carrier_name"].ToString() + "')";
                                }
                                sql += ";\n";
                            }
                        }
                        cmd.CommandText = "SELECT building_short_name,building_long_name FROM buildings;";
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows == true)
                            {
                                sql += "INSERT INTO buildings(building_long_name,building_short_name)VALUES";
                                bool done = false;
                                while (reader.Read())
                                {
                                    if (!done)
                                    {
                                        sql += "('" + reader["building_long_name"].ToString() + "','" + reader["building_short_name"].ToString() + "')";
                                        done = true;
                                        continue;
                                    }
                                    sql += ",('" + reader["building_long_name"].ToString() + "','" + reader["building_short_name"].ToString() + "')";
                                }
                                sql += ";\n";
                            }
                        }
                        cmd.CommandText = "SELECT empl_fname, empl_lname, person_id, building_id, building_room_number FROM employees;";
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows == true)
                            {
                                sql += "INSERT INTO employees(empl_fname, empl_lname, person_id, building_id, building_room_number)VALUES";
                                bool done = false;
                                while (reader.Read())
                                {
                                    if (!done)
                                    {
                                        sql += "('" + reader["empl_fname"].ToString() + "','" + reader["empl_lname"].ToString() + "','" + reader["person_id"].ToString() + "'," + Convert.ToInt64(reader["building_id"].ToString()) + ",'" + reader["building_room_number"].ToString() + "')";
                                        done = true;
                                        continue;
                                    }
                                    sql += ",('" + reader["empl_fname"].ToString() + "','" + reader["empl_lname"].ToString() + "','" + reader["person_id"].ToString() + "'," + Convert.ToInt64(reader["building_id"].ToString()) + ",'" + reader["building_room_number"].ToString() + "')";
                                }
                                sql += ";\n";
                            }
                        }
                        cmd.CommandText = "SELECT package_po, package_carrier, package_vendor, package_deliv_to, package_deliv_by, package_signed_for_by, package_tracking_number, package_receive_date, package_deliver_date, package_note_id, package_status, package_deliv_bldg FROM packages;";
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows == true)
                            {
                                sql += "INSERT INTO packages(package_po, package_carrier, package_vendor, package_deliv_to, package_deliv_by, package_signed_for_by, package_tracking_number, package_receive_date, package_deliver_date, package_note_id, package_status, package_deliv_bldg)VALUES";
                                bool done = false;
                                while (reader.Read())
                                {
                                    if (!done)
                                    {
                                        sql += "('" + reader["package_po"].ToString() + "','" + reader["package_carrier"].ToString() + "','" + reader["package_vendor"].ToString() + "','" + reader["package_deliv_to"].ToString() + "','" + reader["package_deliv_by"].ToString() + "','" + reader["package_signed_for_by"].ToString() + "','" + reader["package_tracking_number"].ToString() + "','" + reader["package_receive_date"].ToString() + "','" + reader["package_deliver_date"].ToString() + "','" + reader["package_note_id"].ToString() + "'," + Convert.ToInt64(reader["package_status"].ToString()) + ",'" + reader["package_deliv_bldg"].ToString() + "')";
                                        done = true;
                                        continue;
                                    }
                                    sql += ",('" + reader["package_po"].ToString() + "','" + reader["package_carrier"].ToString() + "','" + reader["package_vendor"].ToString() + "','" + reader["package_deliv_to"].ToString() + "','" + reader["package_deliv_by"].ToString() + "','" + reader["package_signed_for_by"].ToString() + "','" + reader["package_tracking_number"].ToString() + "','" + reader["package_receive_date"].ToString() + "','" + reader["package_deliver_date"].ToString() + "','" + reader["package_note_id"].ToString() + "'," + Convert.ToInt64(reader["package_status"].ToString()) + ",'" + reader["package_deliv_bldg"].ToString() + "')";
                                }
                                sql += ";\n";
                            }
                        }
                        cmd.CommandText = "SELECT * FROM notes;";
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows == true)
                            {
                                sql += "INSERT INTO notes (note_id,note_value)VALUES";
                                bool done = false;
                                while (reader.Read())
                                {
                                    if (!done)
                                    {
                                        sql += "('" + reader["note_id"].ToString() + "','" + reader["note_value"].ToString() + "')";
                                        done = false;
                                        continue;
                                    }
                                    sql += ",('" + reader["note_id"].ToString() + "','" + reader["note_value"].ToString() + "')";
                                }
                                sql += ";\n";
                            }
                        }
                        cmd.CommandText = "SELECT id_value,last_id FROM idcounter;";
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            sql += "INSERT INTO idcounter(id_value,last_id)VALUES";
                            bool done = false;
                            while (reader.Read())
                            {
                                if (!done)
                                {
                                    sql += "(" + Convert.ToInt64(reader["id_value"].ToString()) + ",'" + reader["last_id"].ToString() + "')";
                                    done = true;
                                    continue;
                                }
                                sql += ",(" + Convert.ToInt64(reader["id_value"].ToString()) + ",'" + reader["last_id"].ToString() + "')";
                            }
                            sql += ";\n";
                        }
                        //do data dump;
                        WriteFile(path2bu, filen, sql);
                        sql = "";
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// Private internal -- Writes the data pulled from db into file.
        /// </summary>
        /// <param name="path">path to directory</param>
        /// <param name="filen">file name</param>
        /// <param name="data">string being written</param>
        private void WriteFile(string path, string filen, string data)
        {
            string fpn = path + filen;
            using (StreamWriter stream = new StreamWriter(fpn,false))
            {
                stream.Write(data);
            }
        }
        /// <summary>
        /// Private internal -- Reads the data pulled from the database and returns to calling application/form/call
        /// </summary>
        /// <param name="path">path to directory</param>
        /// <param name="filen">file name to read</param>
        /// <returns>String of SQL data</returns>
        private List<string> ReadFile(string filetoread)
        {
            List<string> data = new List<string>();
            using (StreamReader reader = new StreamReader(filetoread))
            {
                while (!String.IsNullOrWhiteSpace(reader.ReadLine()))
                {
                    data.Add(reader.ReadLine());
                }
            }
            return data;
        }
        /// <summary>
        /// Reads back in file from the .sql file and 
        /// reads data into the database. (executes code
        /// saves in text file)
        /// </summary>
        /// <param name="filename">File to restore</param>
        internal void DoRestore(string filename)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            List<string> data = ReadFile(filename);
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                OdbcTransaction tr = c.BeginTransaction();
                using (OdbcCommand cmd = new OdbcCommand("",c))
                {
                    try
                    {
                        foreach (string command in data)
                        {
                            cmd.CommandText = command;
                            cmd.ExecuteNonQuery();
                        }
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        throw new DatabaseConnectionException("There was an error reading data into database. Review inner exception for details.", e);
                    }
                }
            }
        }
        #endregion
        #region private gets
        private List<Note>GetNotesListById(string person_id)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            List<Note> nte = new List<Note>() { };
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    cmd.CommandText = "SELECT * FROM notes WHERE note_id = ?;";
                    cmd.Parameters.AddWithValue("pid", person_id);
                    try
                    {
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nte.Add(new Note
                                {
                                    Note_Id = Convert.ToInt64(reader[0].ToString()),
                                    Note_Value = reader[2].ToString()
                                });
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // do nothing
                    }
                }
            }
            return nte;
        }
        private BuildingClass GetBuilding(long id)
        {
            ConnString = DataConnectionClass.ConnectionString;
            DBType = DataConnectionClass.DBType;
            EncodeKey = DataConnectionClass.EncodeString;
            using (OdbcConnection c = new OdbcConnection())
            {
                c.ConnectionString = ConnString;
                c.Open();
                BuildingClass b = new BuildingClass();
                using (OdbcCommand cmd = new OdbcCommand("", c))
                {
                    cmd.CommandText = "SELECT * FROM buildings WHERE building_id = ?;";
                    cmd.Parameters.AddWithValue("pid", id);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            b = new BuildingClass()
                            {
                                BuildingId = Convert.ToInt64(reader[0].ToString()),
                                BuildingLongName = reader[1].ToString(),
                                BuildingShortName = reader[2].ToString()
                            };
                        }
                    }
                    return b;
                }
            }
        }
        #endregion
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
