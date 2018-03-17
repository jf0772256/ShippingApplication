using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp.Models.ModelData
{
    /// <summary>
    /// Physical address helper model class
    /// </summary>
    class PhysicalAddress
    {
        /// <summary>
        /// Auto generated ID from database do not modify
        /// </summary>
        public long AddressId { get; set; }
        /// <summary>
        /// Building short name as string
        /// </summary>
        public string BuildingShortName { get; set; }
        /// <summary>
        /// Building long name as string
        /// </summary>
        public string BuildingLongName { get; set; }
        /// <summary>
        /// Building room number as string
        /// </summary>
        public string BuildingRoomNumber { get; set; }
        /// <summary>
        /// Notes List of note models as List of Notes
        /// </summary>
        public List<Note> Notes { get; set; }
        /// <summary>
        /// Street address line 1 as string
        /// </summary>
        public string Line1 { get; set; }
        /// <summary>
        /// Street address line 2 as string
        /// </summary>
        public string Line2 { get; set; }
        /// <summary>
        /// Address state as string
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Address city as string
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Address zipcode as string
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// Address country (DEFAULT 'US') as string
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// person Id for address specific notes
        /// </summary>
        public string AddrNoteId { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        public PhysicalAddress()
        {
            Notes = new List<Note>() { };
        }
    }
}
