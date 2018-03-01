using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp
{
    class Building
    {
        // Class level variables
        private int buildingId;
        private string buildingShortName;
        private string buildingLongName;
        private string streetAddress;
        private string state;
        private string city;
        private int zipCode;
        private string phone1;
        private string phone2;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Building()
        {

        }


        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="buildingShortName"></param>
        /// <param name="buildingLongName"></param>
        /// <param name="streetAddress"></param>
        /// <param name="state"></param>
        /// <param name="city"></param>
        /// <param name="zipCode"></param>
        /// <param name="phone1"></param>
        /// <param name="phone2"></param>
        public Building(int buildingId, string buildingShortName, string buildingLongName, string streetAddress, string state, string city, int zipCode, string phone1, string phone2)
        {
            this.BuildingId = buildingId;
            this.BuildingShortName = buildingShortName;
            this.BuildingLongName = buildingLongName;
            this.StreetAddress = streetAddress;
            this.State = state;
            this.City = city;
            this.ZipCode = zipCode;
            this.Phone1 = phone1;
            this.Phone2 = phone2;

        }

        #region Building Properties
        public int BuildingId { get => buildingId; set => buildingId = value; }
        public string BuildingShortName { get => buildingShortName; set => buildingShortName = value; }
        public string BuildingLongName { get => buildingLongName; set => buildingLongName = value; }
        public string StreetAddress { get => streetAddress; set => streetAddress = value; }
        public string State { get => state; set => state = value; }
        public string City { get => city; set => city = value; }
        public int ZipCode { get => zipCode; set => zipCode = value; }
        public string Phone1 { get => phone1; set => phone1 = value; }
        public string Phone2 { get => phone2; set => phone2 = value; }
        #endregion
    }
}
