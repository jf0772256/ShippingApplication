using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp
{
    class User
    {
        // Class level variables
        private int id;
        private string firstName;
        private string lastName;
        private char level;
        private string passWord;

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
        public User(int id, string firstName, string lastName, char level, string password)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.level = level;
            this.passWord = password;
        }

        #region User Properties
        public int Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public char Level { get => level; set => level = value; }
        public string PassWord { get => passWord; set => passWord = value; }
        #endregion
    }
}
