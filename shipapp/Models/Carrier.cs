﻿namespace shipapp.Models
{
    /// <summary>
    /// Carrier Model Class
    /// </summary>
    class Carrier
    {
        /// <summary>
        /// Autogenerated ID Do mot modify
        /// </summary>
        public long CarrierId { get; set; }
        /// <summary>
        /// Carrier name as string
        /// </summary>
        public string CarrierName { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public Carrier()
        {
        }
        public override string ToString()
        {
            return CarrierName;
        }
    }
}