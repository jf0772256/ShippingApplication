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


        public AddUser()
        {
            InitializeComponent();
        }


        private void AddUser_Load(object sender, EventArgs e)
        {

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
            if (ValidateData())
            {
                // Create usedr entity
                Models.User newUser = new Models.User();

                // Fill entity
                newUser.Id = long.Parse(txtId.Text);
                newUser.FirstName = txtFirstName.Text;
                newUser.LastName = txtLastName.Text;
                newUser.Level = new Models.ModelData.Role() { Role_id = Convert.ToInt64(txtLevel.Text)};
                newUser.Username = txtUsername.Text;
                newUser.PassWord = txtPassword.Text;
                newUser.Person_Id = txtBoxPersonId.Text;

                // Write the data to the DB
                Connections.DataConnections.DataConnectionClass.UserConn.Write1User(newUser);
                Connections.DataConnections.DataConnectionClass.DataLists.UsersList.Add(Connections.DataConnections.DataConnectionClass.UserConn.Get1User(newUser.Username));
                this.Close();
            }
            else if (ValidateData() && message == "EDIT")
            {
                // Create user entity
                Models.User newUser = new Models.User();

                // Fill entity
                newUser.Id = long.Parse(txtId.Text);
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
            else
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
            txtId.BackColor = Color.White;
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
            if (!long.TryParse(txtId.Text, out num0))
            {
                pass = false;
                txtId.BackColor = Color.LightPink;
            }

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
    }
}
