using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp.Models.ModelData
{
    /// <summary>
    /// Building Class
    /// </summary>
    class BuildingClass
    {
        /// <summary>
        /// Building ID - IS AUTO GENERATED Do Not Modify
        /// </summary>
        public long BuildingId { get; set; }
        /// <summary>
        /// Building Long name
        /// </summary>
        public string BuildingLongName { get; set; }
        /// <summary>
        /// Building Sort Name
        /// </summary>
        public string BuildingShortName { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public BuildingClass() { }
        /// <summary>
        /// ToString Method returns short name
        /// </summary>
        /// <returns>String representing building short name</returns>
        public override string ToString()
        {
            return BuildingShortName;
        }
    }
}
