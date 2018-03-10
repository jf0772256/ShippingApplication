using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models.ModelData;

namespace shipapp.Models
{
    class User
    {
        // Class level variables
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Level { get; set; }
        public string PassWord { get; set; }
        public string Username { get; set; }
        public List<Note> Notes { get; set; }
        public User() { }
    }
}
