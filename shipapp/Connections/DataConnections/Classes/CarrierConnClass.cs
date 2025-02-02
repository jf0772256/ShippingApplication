﻿using shipapp.Connections.HelperClasses;
using shipapp.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace shipapp.Connections.DataConnections.Classes
{
    /// <summary>
    /// Carrier Connectio Class
    /// </summary>
    class CarrierConnClass:DatabaseConnection
    {
        /// <summary>
        /// Form Object
        /// </summary>
        object Sender { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierConnClass() : base() { }
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
        public async void GetCarrierList(object sender = null)
        {
            if (String.IsNullOrWhiteSpace(DataConnectionClass.ConnectionString))
            {
                return;
            }
            Sender = sender;
            SortableBindingList<Carrier> carr = await Task.Run(() => Get_Carrier_List());
            if (Sender is Manage)
            {
                Manage t = (Manage)Sender;
                DataConnectionClass.DataLists.CarriersList = carr;
                BindingSource bs = new BindingSource
                {
                    DataSource = DataConnectionClass.DataLists.CarriersList
                };
                t.dataGridView1.DataSource = bs;
                try
                {
                    t.dataGridView1.Columns["CarrierId"].Visible = false;
                }
                catch (Exception)
                {
                    //
                }
            }
            else
            {
                DataConnectionClass.DataLists.CarriersList = carr;
            }
        }
        /// <summary>
        /// Collects a single specific carrier from the database - this is important, You must include a valid database id as long.
        /// </summary>
        /// <param name="id">ID of the master carrier, from there we will get the rest of the data.</param>
        /// <returns>Carrier Object</returns>
        public Carrier GetCarrier(long id)
        {
            return Get_Carrier(id);
        }
        /// <summary>
        /// Method to remove a carrier
        /// </summary>
        /// <param name="c">Carrier to remove</param>
        public void DeleteCarrier(Carrier c)
        {
            Delete(c);
        }
    }
}