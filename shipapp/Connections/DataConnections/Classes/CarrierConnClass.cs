using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models;
using shipapp.Models.ModelData;

namespace shipapp.Connections.DataConnections.Classes
{
    class CarrierConnClass:DatabaseConnection
    {
        public CarrierConnClass() : base()
        {
            //
        }
        public void AddCarrier(Carrier value)
        {
            //
        }
        public void UpdateCarrier(Carrier value)
        {
            //
        }
        public void GetCarrierList()
        {
            //
        }
        public Carrier GetCarrier(long id)
        {
            return new Carrier() { };
        }
    }
}
