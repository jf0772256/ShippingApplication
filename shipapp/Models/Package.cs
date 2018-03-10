using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models.ModelData;

namespace shipapp.Models
{
    class Package
    {
        public long PackageId { get; set; }
        public PurchaseOrder PackagePurchaseOrder { get; set; }
        public Carrier PackageCarrier { get; set; }
        public Vendor PackageVendor { get; set; }
        public Faculty PackageDeliveredTo { get; set; }
        public User PackageDeleveredBy { get; set; }
        public Faculty PackageSignedForBy { get; set; }
        public string PackageTrackingNumber { get; set; }
        public string PackageReceivedDate { get; set; }
        public string PackageDeliveredDate { get; set; }
        public List<Note> Notes { get; set; }
        public DeliveryStatus Status { get; set; }
        public Package() { Notes = new List<Note>() { }; }
        public enum DeliveryStatus
        {
            NotReceived=0,
            Received=1,
            OutForDelivery=2,
            Delivered=3
        }
    }
}
