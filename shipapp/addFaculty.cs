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

namespace shipapp
{
    /// <summary>
    /// This calss allows the user to add a faculty to the database
    /// </summary>
    public partial class AddFaculty : Form
    {
        // Class level variables
        private Faculty newFaculty;
        internal Faculty NewFaculty
        {
            get => newFaculty;
            set => newFaculty = value;
        }
        public AddFaculty()
        {
            InitializeComponent();
            NewFaculty = new Faculty();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// When the user clicks this button it will check the data, add it to the DB, and close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Reset the background color
            ResetError();

            // Check that the appropriate data exist before writing to the DB.
            if (ValidateData())
            {
                NewFaculty.FirstName = txtFirstName.Text;
                NewFaculty.LastName = txtLastName.Text;
                NewFaculty.Faculty_PersonId = txtId2.Text;
                NewFaculty.Id = long.Parse(txtId1.Text);

                // Add to DB
                Connections.DataConnections.DataConnectionClass.EmployeeConn.AddFaculty(NewFaculty);

                this.Close();
            }
            else
            {
                MessageBox.Show("All fields must have correct data!", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        /// <summary>
        /// Valiadate the data
        /// </summary>
        /// <returns></returns>
        private bool ValidateData()
        {
            // Method level variables
            bool pass = true;
            long num0 = 0;

            // Test Data
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

            if (!long.TryParse(txtId1.Text, out num0))
            {
                pass = false;
                txtId1.BackColor = Color.LightPink;
            }

            if (String.IsNullOrWhiteSpace(txtId2.Text))
            {
                pass = false;
                txtId2.BackColor = Color.LightPink;
            }

            return pass;
        }
        /// <summary>
        /// Reset the backcolor after errors
        /// </summary>
        private void ResetError()
        {
            txtFirstName.BackColor = Color.White;
            txtLastName.BackColor = Color.White;
            txtId1.BackColor = Color.White;
            txtId2.BackColor = Color.White;
        }
        /// <summary>
        /// Add email address to faculty list
        /// </summary>
        private void BtnAddEmail_Click(object sender, EventArgs e)
        {
            //
        }
        /// <summary>
        /// Add phone number to faculty
        /// </summary>
        private void BtnAddPhone_Click(object sender, EventArgs e)
        {
            //
        }
        /// <summary>
        /// Adds address to faculty list
        /// </summary>
        private void BtnAddAddress_Click(object sender, EventArgs e)
        {
            //
        }
    }
}
