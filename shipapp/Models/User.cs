using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp.Models
{
    class User
    {
        // Class level variables
        private long id;
        private string firstName;
        private string lastName;
        private long level;
        private string passWord;
        private string username;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public User()
        {

        }

        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="level"></param>
        /// <param name="password"></param>
        public User(long id, string firstName, string lastName, long level, string password)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.level = level;
            this.passWord = password;
        }

        #region User Properties
        public long Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public long Level { get => level; set => level = value; }
        public string PassWord { get => passWord; set => passWord = value; }
        public string Username { get => username; set => username = value; }
        #endregion
    }
}
