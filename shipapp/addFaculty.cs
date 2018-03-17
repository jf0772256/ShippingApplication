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
    public partial class AddFaculty : Form
    {
        // Class level variables
        private Models.Faculty newFaculty;

        internal Faculty NewFaculty { get => newFaculty; set => newFaculty = value; }

        public AddFaculty()
        {
            InitializeComponent();
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
                Models.Faculty faculty = new Models.Faculty();
                faculty.FirstName = txtFirstName.Text;
                faculty.LastName = txtLastName.Text;
                faculty.Faculty_PersonId = txtId2.Text;
                faculty.Id = long.Parse(txtId1.Text);

                // Add to DB
                Connections.DataConnections.DataConnectionClass.EmployeeConn.AddFaculty(faculty);

                this.Close();
            }
            else
            {
                MessageBox.Show("All fields must have correct data!", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private bool ValidateData()
        {
            // Method level variables
            bool pass = true;
            long num0 = 0;
            int num1 = 0;

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

            if (!int.TryParse(txtId2.Text, out num1))
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
    }
}
