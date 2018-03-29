using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shipapp
{
    /// <summary>
    /// This class allows the user to add a validated user to the database
    /// </summary>
    public partial class AddUser : Form
    {
        // Class level variables
        private string message;
        private Models.User userToBeEdited;


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
            this.userToBeEdited = (Models.User)objectToBeEditied;
        }


        /// <summary>
        /// Fill the form with the information of the object and change the button to edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUser_Load(object sender, EventArgs e)
        {
            if (message == "EDIT")
            {
                // Fill Textbox
                txtBoxPersonId.Text = userToBeEdited.Id.ToString();
                txtFirstName.Text = userToBeEdited.FirstName;
                txtLastName.Text = userToBeEdited.LastName;
                txtLevel.Text = userToBeEdited.Level.ToString();
                txtUsername.Text = userToBeEdited.Username;
                txtPassword.Text = userToBeEdited.PassWord;

                // Change Button Text
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
            if (ValidateData() && message == "ADD") // If adding a user
            {
                // Create usedr entity
                Models.User newUser = new Models.User();

                // Fill entity
                newUser.FirstName = txtFirstName.Text;
                newUser.LastName = txtLastName.Text;
                newUser.Level = new Models.ModelData.Role() { Role_id = Convert.ToInt64(txtLevel.Text) };
                newUser.Username = txtUsername.Text;
                newUser.PassWord = txtPassword.Text;
                newUser.Person_Id = txtBoxPersonId.Text;

                // Write the data to the DB
                Connections.DataConnections.DataConnectionClass.UserConn.Write1User(newUser);
                Connections.DataConnections.DataConnectionClass.DataLists.UsersList.Add(Connections.DataConnections.DataConnectionClass.UserConn.Get1User(newUser.Username));
                this.Close();
            }
            else if (ValidateData() && message == "EDIT") // If editing the user
            {
                // Create usedr entity
                Models.User newUser = new Models.User();

                // Fill entity
                newUser.FirstName = txtFirstName.Text;
                newUser.LastName = txtLastName.Text;
                newUser.Level = new Models.ModelData.Role() { Role_id = returnRoleId() };
                newUser.Username = txtUsername.Text;
                newUser.PassWord = txtPassword.Text;
                newUser.Person_Id = txtBoxPersonId.Text;

                // Write the data to the DB
                Connections.DataConnections.DataConnectionClass.UserConn.Update1User(newUser);
                Connections.DataConnections.DataConnectionClass.DataLists.UsersList.Add(Connections.DataConnections.DataConnectionClass.UserConn.Get1User(newUser.Username));
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
            long num0 = 0;

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
        public int returnRoleId()
        {
            // Method levele varables
            int roleId;

            // Find role Id
            if (txtLevel.Text == "Administrator")
            {
                roleId = 1;
            }
            else if (txtLevel.Text == "Supervisor")
            {
                roleId = 2;
            }
            else if(txtLevel.Text == "User")
            {
                roleId = 3;
            }
            else
            {
                roleId = 0;
            }

            return roleId;
        }
    }
}
