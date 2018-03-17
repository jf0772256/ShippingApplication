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
    public partial class AddUser : Form
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void AddUser_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Reset
            ResetError();

            // Test data before writing to the DB
            if (ValidateData())
            {
                Models.User newUser = new Models.User();
                newUser.Id = long.Parse(txtId.Text);
                newUser.FirstName = txtFirstName.Text;
                newUser.LastName = txtLastName.Text;
                newUser.Level = new Models.ModelData.Role() { Role_id = 1 };
                newUser.Username = txtUsername.Text;
                newUser.PassWord = txtPassword.Text;
                newUser.Person_Id = txtBoxPersonId.Text;

                Connections.DataConnections.DataConnectionClass.UserConn.Write1User(newUser);

                this.Close();
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
            txtLevel.BackColor = Color.White;
            txtUsername.BackColor = Color.White;
            txtPassword.BackColor = Color.White;
            txtBoxPersonId.BackColor = Color.White;
        }

        private bool ValidateData()
        {
            // Method level variables
            bool pass = true;
            long num0 = 0;

            // Validate Data
            if (long.TryParse(txtId.Text, out num0))
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

            if (txtLevel.Text == "")
            {
                pass = false;
                txtLevel.BackColor = Color.LightPink;
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
