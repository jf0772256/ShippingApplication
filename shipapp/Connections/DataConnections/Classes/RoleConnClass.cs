﻿using shipapp.Models.ModelData;

namespace shipapp.Connections.DataConnections.Classes
{
    /// <summary>
    /// User roles class
    /// </summary>
    class RoleConnClass:DatabaseConnection
    {
        public RoleConnClass() : base() {}
        /// <summary>
        /// Adds role to database
        /// </summary>
        /// <param name="Value">Role to add</param>
        public void Write_Role(Role Value)
        {
            Write(Value);
        }
        /// <summary>
        /// Update Role that exists in database
        /// </summary>
        /// <param name="value">Role to use, role_id must be the id of the role that is being updated.</param>
        public void UpdateRole(Role value)
        {
            Update(value);
        }
    }
}
