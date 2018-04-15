using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp.Models
{
    /// <summary>
    /// Audit Class to display the audit information in data grid view
    /// </summary>
    class AuditItem
    {
        /// <summary>
        /// Audit line item and it's data.
        /// </summary>
        public string Item { get; set; }
        /// <summary>
        /// date of occurence
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Time of occurance
        /// </summary>
        public string Time { get; set; }
    }
}
