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
    /// <summary>
    /// This class allows the user to add a validated user to the database
    /// </summary>
    public partial class AddUser : Form
    {
        // Class level variables
        private string message;
        private User userToBeEdited;
        private char c = '\u2022';
        private User newUser = new User();
        
        /// <summary>
        /// Form constructor for adding
        /// </summary>
        /// <param name="message"></param>
        public AddUser(string message)
        {
            InitializeComponent();
            this.message = message;
        }


        /// <summary>
        /// Form constructer for editing
        /// </summary>
        /// <param name="message"></param>
        /// <param name="objectToBeEditied"></param>
        public AddUser(string message, Object objectToBeEditied)
        {
            InitializeComponent();
            this.message = message;
            this.userToBeEdited = (User)objectToBeEditied;
        }


        /// <summary>
        /// Fill the form with the information of the object and change the button to edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUser_Load(object sender, EventArgs e)
        {
            // Set Password Char
            txtPassword.PasswordChar = c;

            // If EDIT set form to edit mode
            if (message == "EDIT")
            {
                // Fill Textbox
                txtBoxPersonId.Text = userToBeEdited.Person_Id;
                txtFirstName.Text = userToBeEdited.FirstName;
                txtLastName.Text = userToBeEdited.LastName;
                cmboRole.Text = userToBeEdited.Level.ToString();
                txtUsername.Text = userToBeEdited.Username;
                txtPassword.Text = userToBeEdited.PassWord;

                // Change Button Text
                btnAdd.Text = "EDIT";
            }
            else
            {
                //BtnAddNote.Enabled = false;
                //BtnViewNote.Enabled = false;
            }
        }


        /// <summary>
        /// When the user clicks this button validate the data and write it to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Reset
            ResetError();

            // Test data before writing to the DB
            if (ValidateData() && message == "ADD") // If adding a user
            {
                // Fill entity
                newUser.FirstName = txtFirstName.Text;
                newUser.LastName = txtLastName.Text;
                newUser.Level = new Role() { Role_id = returnRoleId() };
                newUser.Username = txtUsername.Text;
                newUser.PassWord = txtPassword.Text;
                newUser.Person_Id = txtBoxPersonId.Text;

                // Write the data to the DB
                DataConnectionClass.UserConn.Write1User(newUser);
                DataConnectionClass.SavePersonId();
                DataConnectionClass.AuditLogConnClass.AddRecordToAudit("added a new user: " + newUser.ToString());
                this.Close();
            }
            else if (ValidateData() && message == "EDIT") // If editing the user
            {
                // Create usedr entity
                User newUser = new User();

                // Fill entity
                newUser.Id = userToBeEdited.Id;
                newUser.Notes = userToBeEdited.Notes;
                newUser.FirstName = txtFirstName.Text;
                newUser.LastName = txtLastName.Text;
                newUser.Level = new Role() { Role_id = returnRoleId() };
                newUser.Username = txtUsername.Text;
                newUser.PassWord = txtPassword.Text;
                newUser.Person_Id = txtBoxPersonId.Text;

                // Write the data to the DB
                DataConnectionClass.UserConn.Update1User(newUser);
                DataConnectionClass.AuditLogConnClass.AddRecordToAudit("edited user: " + newUser.ToString());
                this.Close();
            }
            else if (message != "ADD" && message != "EDIT") // If the message is empty
            {
                MessageBox.Show("It seems there was an error with the form.\r\n\r\nTry Again!", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
            }
            else // If there is bad data
            {
                MessageBox.Show("All fields must have correct data!", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        /// <summary>
        /// Reset the back color after errors
        /// </summary>
        private void ResetError()
        {
            txtFirstName.BackColor = Color.White;
            txtLastName.BackColor = Color.White;
            lblLevel.BackColor = Color.White;
            txtUsername.BackColor = Color.White;
            txtPassword.BackColor = Color.White;
            txtBoxPersonId.BackColor = Color.White;
        }


        /// <summary>
        /// Validate the data
        /// </summary>
        /// <returns></returns>
        private bool ValidateData()
        {
            // Method level variables
            bool pass = true;

            // Validate data
            if (txtFirstName.Text == "")
            {
                pass = false;
                txtFirstName.BackColor = Color.LightPink;
            }

            if (txtLastName.Text == "")
            {
                pass = false;
                txtLastName.BackColor = Color.LightPink;
            }

            if (lblLevel.Text == "")
            {
                pass = false;
                lblLevel.BackColor = Color.LightPink;
            }

            if (txtUsername.Text == "")
            {
                pass = false;
                txtUsername.BackColor = Color.LightPink;
            }

            if (txtPassword.Text == "")
            {
                pass = false;
                txtPassword.BackColor = Color.LightPink;
            }

            if (txtBoxPersonId.Text == "")
            {
                pass = false;
                txtBoxPersonId.BackColor = Color.LightPink;
            }

            return pass;
        }


        /// <summary>
        /// When editing the form return the correct int for the role
        /// </summary>
        /// <returns></returns>
        public long returnRoleId()
        {
            // Method levele varables
            long roleId=0;

            // Find role Id
            if (cmboRole.Text == "Administrator")
            {
                roleId = 1;
            }
            else if (cmboRole.Text == "Dock Supervisor")
            {
                roleId = 2;
            }
            else if (cmboRole.Text == "Supervisor")
            {
                roleId = 3;
            }
            else if (cmboRole.Text == "User")
            {
                roleId = 4;
            }
            else
            {
                roleId = 0;
            }

            return roleId;
        }

        private void txtFirstName_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtFirstName.Text) && message != "EDIT")
            {
                if (txtFirstName.Text.Length < 4)
                {
                    Connections.DataConnections.DataConnectionClass.CreatePersonId(txtFirstName.Text.ToLower().Substring(0, txtFirstName.Text.Length));
                    txtBoxPersonId.Text = Connections.DataConnections.DataConnectionClass.PersonIdGenerated;
                }
                else
                {
                    Connections.DataConnections.DataConnectionClass.CreatePersonId(txtFirstName.Text.ToLower().Substring(0, 4));
                    txtBoxPersonId.Text = Connections.DataConnections.DataConnectionClass.PersonIdGenerated;
                }
            }
        }

        private void txtLastName_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtFirstName.Text)  && !String.IsNullOrWhiteSpace(txtLastName.Text) && message != "EDIT")
            {
                string pidstring = "";
                if (txtFirstName.Text.Length < 4)
                {
                    pidstring = txtFirstName.Text.ToLower().Substring(0, txtFirstName.Text.Length);
                }
                else
                {
                    pidstring = txtFirstName.Text.ToLower().Substring(0, 4);
                }
                if (txtLastName.Text.Length < 4)
                {
                    pidstring += txtLastName.Text.ToLower().Substring(0, txtLastName.Text.Length);
                }
                else
                {
                    pidstring += txtLastName.Text.ToLower().Substring(0, 4);
                }
                Connections.DataConnections.DataConnectionClass.CreatePersonId(pidstring);
                txtBoxPersonId.Text = Connections.DataConnections.DataConnectionClass.PersonIdGenerated;
            }
            else if (!String.IsNullOrWhiteSpace(txtLastName.Text) && message != "EDIT")
            {
                if (txtLastName.Text.Length < 4)
                {
                    Connections.DataConnections.DataConnectionClass.CreatePersonId(txtLastName.Text.ToLower().Substring(0, txtLastName.Text.Length));
                    txtBoxPersonId.Text = Connections.DataConnections.DataConnectionClass.PersonIdGenerated;
                }
                else
                {
                    Connections.DataConnections.DataConnectionClass.CreatePersonId(txtLastName.Text.ToLower().Substring(0, 4));
                    txtBoxPersonId.Text = Connections.DataConnections.DataConnectionClass.PersonIdGenerated;
                }
            }
        }

        private void BtnViewNote_Click(object sender, EventArgs e)
        {
            if (message == "ADD")
            {
                using (AddNote note = new AddNote(newUser, true))
                {
                    note.ShowDialog();
                }
            }
            else
            {
                using (AddNote note = new AddNote(userToBeEdited, true))
                {
                    note.ShowDialog();
                }
            }
        }

        private void BtnAddNote_Click(object sender, EventArgs e)
        {
            if (message == "ADD")
            {
                using (AddNote note = new AddNote(newUser, false))
                {
                    note.ShowDialog();
                }
            }
            else
            {
                using (AddNote note = new AddNote(userToBeEdited, false))
                {
                    note.ShowDialog();
                }
            }
        }

        private void cmboRole_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cmboRole.Text = cmboRole.SelectedItem.ToString();
        }
    }
}
