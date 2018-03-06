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
        public TestConnClass() : base(HelperClasses.SQLHelperClass.DatabaseType.MSSQL)
        {
            Test_Connection();
            //Drop_Tables(true, null);
            Create_Tables();
            Models.User JesseF = new Models.User() { FirstName = "Jesse", LastName = "Fender", Username = "test_User1", PassWord = "tadaaa!", Level = 10 };
            Write_User_To_Database(JesseF);
        }
    }
}
