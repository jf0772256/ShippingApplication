﻿using shipapp.Models.ModelData;
using System.Collections.Generic;
namespace shipapp.Models
{
    /// <summary>
    /// Package Model Class
    /// </summary>
    class Package
    {
        /// <summary>
        /// Autogenerated Id Do not modify
        /// </summary>
        public long PackageId { get; set; }
        /// <summary>
        /// Purchase Order Number as string
        /// </summary>
        public string PONumber { get; set; }
        /// <summary>
        /// Identifyer in supplement tables, as string, max value 1000 bits and MUST be unique across all primary models
        /// </summary>
        public string Package_PersonId { get; set; }
        /// <summary>
        /// Package Carrier as Carrier
        /// </summary>
        public string PackageCarrier { get; set; }
        /// <summary>
        /// Package Vendor as Vendor
        /// </summary>
        public string PackageVendor { get; set; }
        /// <summary>
        /// Package deliver to as Facultys first name, and last name
        /// </summary>
        public string PackageDeliveredTo { get; set; }
        /// <summary>
        /// Package delivered by as Users name(string first name, string last name)
        /// </summary>
        public string PackageDeleveredBy { get; set; }
        /// <summary>
        /// Package signed for by as Faculty
        /// </summary>
        public string PackageSignedForBy { get; set; }
        /// <summary>
        /// Package Tracking Number as string
        /// </summary>
        public string PackageTrackingNumber { get; set; }
        /// <summary>
        /// Package Receive date as ISO Formatted date('yyyy-mm-dd') as string
        /// </summary>
        public string PackageReceivedDate { get; set; }
        /// <summary>
        /// Package Delivered date as ISO Formatted date('yyyy-mm-dd') as string
        /// </summary>
        public string PackageDeliveredDate { get; set; }
        /// <summary>
        /// Package notes as List of Note
        /// </summary>
        public List<Note> Notes { get; set; }
        /// <summary>
        /// Package Status as either int or string(enum)
        /// </summary>
        public DeliveryStatus Status { get; set; }
        /// <summary>
        /// Building short name, as in abbreviation or known alias
        /// </summary>
        public string DelivBuildingShortName { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        public Package()
        {
            Notes = new List<Note>() { };
            Status = 0;
        }
        /// <summary>
        /// Converts normal string to 'lastname, firstname'
        /// </summary>
        /// <param name="unformatted">name as 'firstname lastname'</param>
        /// <returns>formatted string</returns>
        public string ReFormattedString(string unformatted)
        {
            if (string.IsNullOrWhiteSpace(unformatted))
            {
                return null;
            }
            else if (unformatted.IndexOf(',') > 0)
            {
                return unformatted;
            }
            else
            {
                string[] parts = unformatted.Split(' ');
                return parts[1] + ", " + parts[0];
            }
        }
        /// <summary>
        /// ENUM of Package statuses
        /// </summary>
        public enum DeliveryStatus
        {
            /// <summary>
            /// Default :: Package has not arrived at dock
            /// </summary>
            Not_Received=0,
            /// <summary>
            /// Package has arrived and has been received
            /// </summary>
            Received=1,
            /// <summary>
            /// Package is out of delivery
            /// </summary>
            OutForDelivery=2,
            /// <summary>
            /// Package has been delivered to its destination location
            /// </summary>
            Delivered=3
        }
    }
}