using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp
{
    class Vendor
    {
        // Class level variables
        private int vendorId;
        private string vendorName;
        private string streetAddress;
        private string state;
        private string city;
        private int zip;
        private string phone;
        private int noteId;


        /// <summary>
        /// Default constructor
        /// </summary>
        public Vendor()
        {

        }

        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="vendorName"></param>
        /// <param name="streetAddress"></param>
        /// <param name="state"></param>
        /// <param name="city"></param>
        /// <param name="zip"></param>
        /// <param name="phone"></param>
        /// <param name="noteId"></param>
        public Vendor(int vendorId, string vendorName, string streetAddress, string state, string city, int zip, string phone, int noteId)
        {
            this.VendorId = vendorId;
            this.VendorName = vendorName;
            this.StreetAddress = streetAddress;
            this.State = state;
            this.City = city;
            this.Zip = zip;
            this.Phone = phone;
            this.NoteId = noteId;
        }

        #region Vendor properties
        public int VendorId { get => vendorId; set => vendorId = value; }
        public string VendorName { get => vendorName; set => vendorName = value; }
        public string StreetAddress { get => streetAddress; set => streetAddress = value; }
        public string State { get => state; set => state = value; }
        public string City { get => city; set => city = value; }
        public int Zip { get => zip; set => zip = value; }
        public string Phone { get => phone; set => phone = value; }
        public int NoteId { get => noteId; set => noteId = value; }
        #endregion
    }
}
