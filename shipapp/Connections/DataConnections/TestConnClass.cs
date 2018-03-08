using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Connections;

namespace shipapp.Connections.DataConnections
{
    class TestConnClass:DatabaseConnection
    {
        /// <summary>
        /// for use by test methods only
        /// </summary>
        public TestConnClass() : base(HelperClasses.SQLHelperClass.DatabaseType.MSSQL)
        {
            //blank constructor
        }
        /// <summary>
        /// This should be used for testing connections from Settings Form.
        /// </summary>
        /// <param name="databaseHostURI">Where the database is located in the webverse</param>
        /// <param name="databaseName">What database will we be accessing</param>
        /// <param name="databaseUser">Authorized database user</param>
        /// <param name="databasePassword">Authorized database users password</param>
        /// <param name="databasePort">Port number (if used, if not leave 0)</param>
        /// <param name="databaseType">The type of the database engine.</param>
        public TestConnClass(string databaseHostURI, string databaseName, string databaseUser, string databasePassword, string databasePort, HelperClasses.SQLHelperClass.DatabaseType databaseType) : base(databaseHostURI, databaseName, databaseUser, databasePassword, databasePort, databaseType)
        {
            //second blank constructor
        }
        /// <summary>
        /// Used for testing purposes only!!!!
        /// </summary>
        public void Testing()
        {
            Test_Connection();
            Drop_Tables(true, null);
            Create_Tables();
            Models.User JesseF = new Models.User() { FirstName = "Jesse", LastName = "Fender", Username = "test_User1", PassWord = "tadaaa!", Level = 10 };
            Write_User_To_Database(JesseF);
            shipapp.Models.User me = GetUser(1);
            System.Windows.Forms.MessageBox.Show(me.FirstName + " " + me.LastName + ": " + me.Username + ", " + me.PassWord, "did i work?");
        }
        /// <summary>
        /// Use this to test if you have connected successfully to the outside world
        /// </summary>
        public void TestConnectionToDatabase()
        {
            Test_Connection();
        }
        /// <summary>
        /// Same for the most part to the test constructor except that this constructor will actually save the new connection string, Used ONLY with submit on the settings form
        /// </summary>
        /// <param name="dbtype">The type of the database engine.</param>
        /// <param name="dbhost">Where the database is located in the webverse</param>
        /// <param name="dbname">What database will we be accessing</param>
        /// <param name="dbuser">Authorized database user</param>
        /// <param name="dbpass">Authorized database users password</param>
        /// <param name="dbport">Port number (if used, if not leave 0)</param>
        public TestConnClass(HelperClasses.SQLHelperClass.DatabaseType dbtype, string dbhost, string dbname, string dbuser, string dbpass, string dbport) : base(dbhost, dbname, dbuser, dbpass, dbport, dbtype, true)
        {

        }
    }
}
