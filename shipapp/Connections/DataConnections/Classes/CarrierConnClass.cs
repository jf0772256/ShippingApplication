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
        /// <summary>
        /// Creates a new carrier
        /// </summary>
        /// <param name="value">New Carrier to be added remember to assign an unique person id</param>
        public void AddCarrier(Carrier value)
        {
            Write(value);
        }
        /// <summary>
        /// Updates some/all values of a carrier, beit the name or just adding a note. Include the carrier object that was modified.
        /// </summary>
        /// <param name="value">Modified carrier object</param>
        public void UpdateCarrier(Carrier value)
        {
            Update(value);
        }
        /// <summary>
        /// collects all carriers from database - returns to dataconnectionclass.datalists.carriers list
        /// </summary>
        public void GetCarrierList()
        {
            GetCarrierList();
        }
        /// <summary>
        /// Collects a single specific carrier from the database - this is important, You must include a valid database id as long.
        /// </summary>
        /// <param name="id">ID of the master carrier, from there we will get the rest of the data.</param>
        /// <returns></returns>
        public Carrier GetCarrier(long id)
        {
            return Get_Carrier(id);
        }
        public void DeleteCarrier(Carrier c)
        {
            Delete(c);
        }
    }
}
