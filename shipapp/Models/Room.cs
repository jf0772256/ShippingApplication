using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp.Models
{
    class Room
    {
        // Class level variables
        private int buildingId;
        private int roomId;
        private int noteId;


        /// <summary>
        /// Default constructor
        /// </summary>
        public Room()
        {

        }

        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="roomId"></param>
        /// <param name="noteId"></param>
        public Room(int buildingId, int roomId, int noteId)
        {
            this.BuildingId = buildingId;
            this.RoomId = roomId;
            this.NoteId = noteId;
        }

        #region Room Properties
        public int BuildingId { get => buildingId; set => buildingId = value; }
        public int RoomId { get => roomId; set => roomId = value; }
        public int NoteId { get => noteId; set => noteId = value; }
        #endregion
    }
}
