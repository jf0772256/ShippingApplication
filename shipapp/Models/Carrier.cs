using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp.Models
{
    public class Carrier
    {
        // Class level variables
        private int carrierId;
        private string carrierName;
        private string streetAddress;
        private string city;
        private string state;
        private string phone1;
        private string phone2;
        private string noteId;


        /// <summary>
        /// Default constructor
        /// </summary>
        public Carrier()
        {

        }


        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="carrierId"></param>
        /// <param name="carrierName"></param>
        /// <param name="streetAddress"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="phone1"></param>
        /// <param name="phone2"></param>
        /// <param name="noteId"></param>
        public Carrier( int carrierId, string carrierName, string streetAddress, string city, string state, string phone1, string phone2, string noteId)
        {
            this.CarrierId = carrierId;
            this.StreetAddress = streetAddress;
            this.City = city;
            this.State = state;
            this.Phone1 = phone1;
            this.Phone2 = phone2;
            this.NoteId = noteId;
        }

        #region Carrier Properties
        public int CarrierId { get => carrierId; set => carrierId = value; }
        public string CarrierName { get => carrierName; set => carrierName = value; }
        public string StreetAddress { get => streetAddress; set => streetAddress = value; }
        public string City { get => city; set => city = value; }
        public string State { get => state; set => state = value; }
        public string Phone1 { get => phone1; set => phone1 = value; }
        public string Phone2 { get => phone2; set => phone2 = value; }
        public string NoteId { get => noteId; set => noteId = value; }
        #endregion
    }
}
