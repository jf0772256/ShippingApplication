using shipapp.Connections.DataConnections;
using shipapp.Models;
using shipapp.Models.ModelData;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace shipapp
{
    /// <summary>
    /// This class allows the user to add a validated user to the database
    /// and to edit an existing on
    /// </summary>
    public partial class ManageUsers : Form
    {
        // Class level variables
        private string message;
        private User userToBeEdited;
        private char c = '\u2022';
        private User newUser = new User();
        private Regex passwordReg = new Regex(@"(?=^.{8,15}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$");


        /// <summary>
        /// Form constructor
        /// </summary>
        /// <param name="message"></param>
        public ManageUsers(string message)
        {
            InitializeComponent();
            this.message = message;
        }
        /// <summary>
        /// Form constructer for editing
        /// </summary>
        /// <param name="message"></param>
        /// <param name="objectToBeEditied"></param>
        public ManageUsers(string message, object objectToBeEditied)
        {
            InitializeComponent();
            this.message = message;
            userToBeEdited = (User)objectToBeEditied;
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
                userToBeEdited.Notes = DataConnectionClass.UserConn.GetNotesList(userToBeEdited.Person_Id);
                // Change Button Text
                Text = "Edit Users";
                btnAdd.Text = "EDIT";
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
            if (message == "ADD" && ValidateData()) // If adding a user
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
            else if (message == "EDIT" && ValidateData()) // If editing the user
            {
                // Create user entity
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
        /// Validate the data and check for errors
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

            if (!passwordReg.IsMatch(txtPassword.Text))
            {
                pass = false;
                txtPassword.BackColor = Color.LightPink;

                // alert user of bad password
                MessageBox.Show("It looks like there was an issue with your Password.\r\n" +
                    "Passwords must be:\r\n"
                    + c.ToString() + "between 8 and 15 characters long\r\n" 
                    + c.ToString() + "contain at least one number\r\n" 
                    + c.ToString() + "contain at least one uppercase letter\r\n" 
                    + c.ToString() + "must contain at least one lowercase letter", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        /// <summary>
        /// Create part of the use id from the fist name field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFirstName_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtFirstName.Text) && message != "EDIT")
            {
                if (txtFirstName.Text.Length < 4)
                {
                    DataConnectionClass.CreatePersonId(txtFirstName.Text.ToLower().Substring(0, txtFirstName.Text.Length));
                    txtBoxPersonId.Text = DataConnectionClass.PersonIdGenerated;
                }
                else
                {
                    DataConnectionClass.CreatePersonId(txtFirstName.Text.ToLower().Substring(0, 4));
                    txtBoxPersonId.Text = DataConnectionClass.PersonIdGenerated;
                }
            }
        }
        /// <summary>
        /// Create part of the user id from the last name field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                DataConnectionClass.CreatePersonId(pidstring);
                txtBoxPersonId.Text = DataConnectionClass.PersonIdGenerated;
            }
            else if (!String.IsNullOrWhiteSpace(txtLastName.Text) && message != "EDIT")
            {
                if (txtLastName.Text.Length < 4)
                {
                    DataConnectionClass.CreatePersonId(txtLastName.Text.ToLower().Substring(0, txtLastName.Text.Length));
                    txtBoxPersonId.Text = DataConnectionClass.PersonIdGenerated;
                }
                else
                {
                    DataConnectionClass.CreatePersonId(txtLastName.Text.ToLower().Substring(0, 4));
                    txtBoxPersonId.Text = DataConnectionClass.PersonIdGenerated;
                }
            }
        }
        /// <summary>
        /// Allow the user to view user notes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnViewNote_Click(object sender, EventArgs e)
        {
            if (message == "ADD")
            {
                using (ManageNotes note = new ManageNotes(newUser, true))
                {
                    note.ShowDialog();
                }
            }
            else
            {
                using (ManageNotes note = new ManageNotes(userToBeEdited, true))
                {
                    note.ShowDialog();
                }
            }
        }
        /// <summary>
        /// Allow a user to add user notes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddNote_Click(object sender, EventArgs e)
        {
            if (message == "ADD")
            {
                using (ManageNotes note = new ManageNotes(newUser, false))
                {
                    note.ShowDialog();
                    newUser = (User)note.GetObjectData;
                }
            }
            else
            {
                using (ManageNotes note = new ManageNotes(userToBeEdited, false))
                {
                    note.ShowDialog();
                    userToBeEdited = (User)note.GetObjectData;
                }
            }
        }
        /// <summary>
        /// Set the selected user role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmboRole_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cmboRole.Text = cmboRole.SelectedItem.ToString();
        }
    }
}
