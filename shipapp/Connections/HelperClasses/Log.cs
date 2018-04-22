using shipapp.Models;

namespace shipapp.Connections.HelperClasses
{
    /// <summary>
    /// This class allows the program to create delieverylogs for Package Delivery
    /// </summary>
    class Log
    {
        /// <summary>
        /// Po number assigned to each package
        /// </summary>
        private string po;
        /// <summary>
        /// Vendor name that the package was shipped from
        /// </summary>
        private string vendor;
        /// <summary>
        /// Name of the carrier that delivered the package to receiving department
        /// </summary>
        private string carrier;
        /// <summary>
        /// Carrier tracking number
        /// </summary>
        private string trackingNumber;
        /// <summary>
        /// Building to be delivered to with room or mail box number where nessisary
        /// </summary>
        private string building;
        /// <summary>
        /// Who the package should be delivered to
        /// </summary>
        private string recipiant;
        /// <summary>
        /// Signature of the person with whom the package was delivered to
        /// </summary>
        private string signature;
        /// <summary>
        /// Default constructor
        /// </summary>
        public Log()
        {

        }
        /// <summary>
        /// Gets or sets PO number for packages
        /// </summary>
        public string Po { get => po; set => po = value; }
        /// <summary>
        /// Gets or sets vendor for the packages
        /// </summary>
        public string Vendor { get => vendor; set => vendor = value; }
        /// <summary>
        /// Gets or sets carrier for the packages
        /// </summary>
        public string Carrier { get => carrier; set => carrier = value; }
        /// <summary>
        /// Gets or sets tracking number for the packages
        /// </summary>
        public string TrackingNumber { get => trackingNumber; set => trackingNumber = value; }
        /// <summary>
        /// Gets or sets Delivery building and room/mailbox number for the packages
        /// </summary>
        public string Building { get => building; set => building = value; }
        /// <summary>
        /// Gets or sets recipent(who the package is addressed to) for the packages
        /// </summary>
        public string Recipiant { get => recipiant; set => recipiant = value; }
        /// <summary>
        /// Gets or sets signature for the person that the package was left with
        /// </summary>
        public string Signature { get => signature; set => signature = value; }
        /// <summary>
        /// Convert a pacakge to a log
        /// </summary>
        /// <param name="package">Package object</param>
        /// <returns>Log object for printing</returns>
        public static Log ConvertPackageToLog(Package package)
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
