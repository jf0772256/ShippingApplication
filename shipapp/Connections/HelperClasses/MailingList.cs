using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipapp.Connections.HelperClasses
{
    /// <summary>
    /// This class will allow the user to print out a formated faculty
    /// list for the wall of names.
    /// </summary>
    class MailingList
    {
        // Class level variables
        private string name;
        private string location;
        private string mailbox;
        private string printPadding;


        /// <summary>
        /// Default constructor
        /// </summary>
        public MailingList()
        {

        }


        /// <summary>
        /// Public properties
        /// </summary>
        public string Name { get => name; set => name = value; }
        public string Location { get => location; set => location = value; }
        public string Mailbox { get => mailbox; set => mailbox = value; }
        public string PrintPadding { get => printPadding; set => printPadding = value; }



        public static MailingList ConveretToMailingListItem(Models.Faculty faculty)
        {
            MailingList mailing = new MailingList();

            mailing.Name = faculty.LastName + ", " + faculty.FirstName;
            mailing.location = faculty.Building_Name;

            return mailing;
        }
    }
}
