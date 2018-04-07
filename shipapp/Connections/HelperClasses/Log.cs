using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp.Connections.HelperClasses
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


        /// <summary>
        /// Convert a pacakge to a log
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static Log ConvertPackageToLog(Models.Package package)
        {
            // Create new log
            Log log = new Log();

            // Fill log
            log.po = package.PONumber;
            log.Building = package.DelivBuildingShortName;
            log.Recipiant = package.PackageDeliveredTo;
            log.TrackingNumber = package.PackageTrackingNumber;
            log.Vendor = package.PackageVendor;
            log.Carrier = package.PackageCarrier;
            log.Signature = "";

            return log;
        }
    }
}
