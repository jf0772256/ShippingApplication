﻿namespace shipapp.Models
{
    /// <summary>
    /// Vendor Model Class
    /// </summary>
    class Vendors
    {
        /// <summary>
        /// Autogenerated Id Do not modify
        /// </summary>
        public long VendorId { get; set; }
        /// <summary>
        /// Vendor Name as string
        /// </summary>
        public string VendorName { get; set; }
        public Vendors()
        {
        }
        /// <summary>
        /// Overriding the tostring method
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return VendorName;
        }
    }
}
