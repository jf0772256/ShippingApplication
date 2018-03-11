using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models;
using shipapp.Models.ModelData;

namespace shipapp.Connections.DataConnections.Classes
{
    class EmployeeConnClass:DatabaseConnection
    {
        public EmployeeConnClass() : base() { }

        public Faculty GetFaculty(long id)
        {
            return new Faculty() { };
        }
        public void AddFaculty(Faculty f)
        {
            //
        }
        public void UpdateFaculty(Faculty f)
        {
            //
        }
        public void GetAllAfaculty()
        {
            //
        }
    }
}
