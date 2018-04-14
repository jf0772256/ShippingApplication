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
        /// pretty prints data to value
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return Item.ToString();
        }
    }
}
