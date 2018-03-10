using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp.Models
{
    class PurchaseOrder
    {
        public long PO_Id { get; set; }
        public string PONumber { get; set; }
        public int PackageCount { get; set; }
        public string POCreatedOn { get; set; }
        public Faculty CreatedBy { get; set; }
        public Faculty ApprovedBy { get; set; }
        public PurchaseOrder() { }
    }
}
