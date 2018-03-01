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
        public TestConnClass() : base("stusql.ckwia8qkgyyj.us-east-1.rds.amazonaws.com", "otcshipping", "otcshippingadmin", "cis260SP18Ship")
        {
            OpenConnection();
        }
    }
}
