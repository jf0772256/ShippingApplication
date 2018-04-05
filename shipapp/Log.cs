using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp
{
    /// <summary>
    /// This class allows the program to create logs for delievery
    /// </summary>
    class Log
    {
        // Class level variable
        private string po;
        private string vendor;
        private string carrier;
        private string trackingNumber;
        private string building;
        private string recipiant;
        private string signature;

        
        /// <summary>
        /// Default constructor
        /// </summary>
        public Log()
        {

        }


        /// <summary>
        /// Public constructors
        /// </summary>
        public string Po { get => po; set => po = value; }
        public string Vendor { get => vendor; set => vendor = value; }
        public string Carrier { get => carrier; set => carrier = value; }
        public string TrackingNumber { get => trackingNumber; set => trackingNumber = value; }
        public string Building { get => building; set => building = value; }
        public string Recipiant { get => recipiant; set => recipiant = value; }
        public string Signature { get => signature; set => signature = value; }
    }
}
