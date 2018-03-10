using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp.Models.ModelData
{
    /// <summary>
    /// Email Helper Model, for sub model classifiying
    /// </summary>
    class EmailAddress
    {
        /// <summary>
        /// Email Id -- Auto Generated in Database Do Not Modify
        /// </summary>
        public long Email_Id { get; set; }
        /// <summary>
        /// Email Address - Must be unique on database as string
        /// </summary>
        public string Email_Address { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public EmailAddress() { }
    }
}
