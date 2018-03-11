using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models;
using shipapp.Models.ModelData;

namespace shipapp.Connections.DataConnections.Classes
{
    class RoleConnClass:DatabaseConnection
    {
        public RoleConnClass() : base() {}
        /// <summary>
        /// Adds role to database
        /// </summary>
        /// <param name="Value">Role to add</param>
        public void Write_Role(Role Value)
        {
            Add_Role(Value);
        }
        /// <summary>
        /// Update Role that exists in database
        /// </summary>
        /// <param name="value">Role to use, role_id must be the id of the role that is being updated.</param>
        public void UpdateRole(Role value)
        {
            Update_Role(value);
        }
    }
}
