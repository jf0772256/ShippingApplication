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
        public TestConnClass() : base(HelperClasses.SQLHelperClass.DatabaseType.MySQL)
        {
            OpenConnection();
        }
    }
}
