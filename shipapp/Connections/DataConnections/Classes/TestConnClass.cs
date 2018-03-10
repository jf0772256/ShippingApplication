using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Connections;

namespace shipapp.Connections.DataConnections.Classes
{
    class TestConnClass:DatabaseConnection
    {
        /// <summary>
        /// for use by test methods only
        /// </summary>
        public TestConnClass() : base()
        {
            //blank constructor
        }
        /// <summary>
        /// Used for testing purposes only!!!!
        /// </summary>
        public void Testing()
        {
            //Test_Connection();
            Drop_Tables(true, null);
            Create_Tables();
            //the below lines which is now commented out would fail if it ever was run as there is no role_id 10
            //Models.User JesseF = new Models.User() { FirstName = "Jesse", LastName = "Fender", Username = "test_User1", PassWord = "tadaaa!", Level = new Models.ModelData.Role() { Role_id = 10, Role_Title = "godmode" } };
            //Write_User_To_Database(JesseF);
            shipapp.Models.User me = GetUser(1);
            System.Windows.Forms.MessageBox.Show(me.FirstName + " " + me.LastName + ": " + me.Username + ", " + me.PassWord, "did i work?");
        }
        /// <summary>
        /// Use this to test if you have connected successfully to the outside world
        /// </summary>
        public void TestConnectionToDatabase()
        {
            Test_Connection(DataConnections.DataConnectionClass.ConnectionString);
        }

        public void ResetAllDatabaseTables()
        {
            Drop_Tables(true, null);
            Create_Tables();
        }
    }
}
