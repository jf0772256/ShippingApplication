using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using shipapp.Models;
using shipapp.Models.ModelData;

namespace shipapp
{
    public partial class AddNote : Form
    {
        private Faculty Fac { get; set; }
        private User Usr { get; set; }
        private Package Pck { get; set; }
        private bool AsReadOnly { get; set; }
        /// <summary>
        /// Add Note form, also may act as view
        /// Send either:
        /// --Package
        /// --User
        /// --Faculty
        /// </summary>
        /// <param name="sender">Object to get and add notes from/to</param>
        /// <param name="readOnly">Bool value if only for view</param>
        public AddNote(object sender, bool readOnly)
        {
            AsReadOnly = readOnly;
            if (sender is Faculty)
            {
                Fac = (Faculty)sender;
            }
            else if (sender is User)
            {
                Usr = (User)sender;
            }
            else if (sender is Package)
            {
                Pck = (Package)sender;
            }
            else
            {
                throw new ArgumentException("Invalid object as sender");
            }
            InitializeComponent();
        }

        /// <summary>
        /// This method will add the note to the appropriate entity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnName_Click(object sender, EventArgs e)
        {
            /***
             * TODO add note to enity then add note to list with in entity datalist for the perticular enitity
             **/
        }

        private void AddNote_Load(object sender, EventArgs e)
        {

        }
    }
}
