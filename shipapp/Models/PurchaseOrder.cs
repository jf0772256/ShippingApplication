﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp.Models
{
    /// <summary>
    /// Purchase order model class
    /// </summary>
    class PurchaseOrder
    {
        /// <summary>
        /// Autogenerated Id Do Not Modify
        /// </summary>
        public long PO_Id { get; set; }
        /// <summary>
        /// PO number as string
        /// </summary>
        public string PONumber { get; set; }
        /// <summary>
        /// Number of packages to be expected on po as integer
        /// </summary>
        public int PackageCount { get; set; }
        /// <summary>
        /// Date po was created as ISO date formatted string ("yyyy-mm-dd")
        /// </summary>
        public string POCreatedOn { get; set; }
        /// <summary>
        /// Faculty that created PO, as Faculty
        /// </summary>
        public Faculty CreatedBy { get; set; }
        /// <summary>
        /// Faculty that approved PO, as Faculty
        /// </summary>
        public Faculty ApprovedBy { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        public PurchaseOrder() { }
    }
}
