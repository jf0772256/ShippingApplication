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
using shipapp.Connections.DataConnections;

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
            textBox1.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            textBox2.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
        }
        /// <summary>
        /// This method will add the note to the appropriate entity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnName_Click(object sender, EventArgs e)
        {
            if (AsReadOnly)
            {
                Close();
            }
            else
            {
                if (Fac as Faculty != null)
                {
                    Fac.Notes.Add(new Note() { Note_Value = textBox1.Text + ". Added by: " + DataConnectionClass.AuthenticatedUser.ToString() });
                    DataConnectionClass.EmployeeConn.UpdateFaculty(Fac);
                    Close();
                }
                else if (Usr as User != null)
                {
                    Usr.Notes.Add(new Note() { Note_Value = textBox1.Text + ". Added by: " + DataConnectionClass.AuthenticatedUser.ToString() });
                    DataConnectionClass.UserConn.Update1User(Usr);
                    Close();
                }
                else if (Pck as Package != null)
                {
                    Pck.Notes.Add(new Note() { Note_Value = textBox1.Text + ". Added by: " + DataConnectionClass.AuthenticatedUser.ToString() });
                    DataConnectionClass.PackageConnClass.UpdatePackage(Pck);
                    Close();
                }
            }
        }
        private void AddNote_Load(object sender, EventArgs e)
        {
            if (AsReadOnly)
            {
                //display only
                textBox1.Visible = false;
                btnName.Text = "Close";
                textBox2.Width = textBox2.Width + (textBox1.Width - textBox2.Width) + textBox2.Width;
            }
            else
            {
                //use add note
            }
            // READ NOTES IN READ ONLY BOX
            if (Fac as Faculty != null)
            {
                if (Fac.Notes.Count == 0)
                {
                    textBox2.Text = "No notes have been entered.";
                }
                else
                {
                    foreach (Note note in Fac.Notes)
                    {
                        textBox2.Text += note.Note_Value;
                        textBox2.Text += "\r\n ------------------------- \r\n";
                    }
                }
            }
            else if (Usr as User != null)
            {
                if (Usr.Notes.Count == 0)
                {
                    textBox2.Text = "No notes have been entered.";
                }
                else
                {
                    foreach (Note note in Usr.Notes)
                    {
                        textBox2.Text += note.Note_Value;
                        textBox2.Text += "\r\n ------------------------- \r\n";
                    }
                }
            }
            else if (Pck as Package != null)
            {
                if (Pck.Notes.Count == 0)
                {
                    textBox2.Text = "No notes have been entered.";
                }
                else
                {
                    foreach (Note note in Pck.Notes)
                    {
                        textBox2.Text += note.Note_Value;
                        textBox2.Text += "\r\n ------------------------- \r\n";
                    }
                }
            }
        }
        /// <summary>
        /// Never allow wider than now
        /// </summary>
        private void AddNote_Resize(object sender, EventArgs e)
        {
            this.Width = 559;
        }
    }
}
