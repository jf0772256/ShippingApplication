using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp
{
    class Faculty
    {
        // Class level variable
        private int id;
        private string firstName;
        private char middleInital;
        private string lastName;
        private string suffix;
        private string prefix;
        private int buildingId;
        private int roomId;
        private int noteId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Faculty()
        {

        }

        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firstName"></param>
        /// <param name="middleInital"></param>
        /// <param name="lastName"></param>
        /// <param name="suffix"></param>
        /// <param name="prefix"></param>
        /// <param name="buildingId"></param>
        /// <param name="roomId"></param>
        /// <param name="noteId"></param>
        public Faculty(int id, string firstName, char middleInital, string lastName, string suffix, string prefix, int buildingId, int roomId, int noteId)
        {
            this.id = id;
            this.firstName = firstName;
            this.middleInital = middleInital;
            this.lastName = lastName;
            this.suffix = suffix;
            this.prefix = prefix;
            this.buildingId = buildingId;
            this.roomId = roomId;
            this.noteId = noteId;
        }

        #region Faculty Properties
        public int Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public char MiddleInital { get => middleInital; set => middleInital = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Suffix { get => suffix; set => suffix = value; }
        public string Prefix { get => prefix; set => prefix = value; }
        public int BuildingId { get => buildingId; set => buildingId = value; }
        public int RoomId { get => roomId; set => roomId = value; }
        public int NoteId { get => noteId; set => noteId = value; }
        #endregion
    }
}
