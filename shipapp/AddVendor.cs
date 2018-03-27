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
    /// This class will allow the users to add edit users
    /// </summary>
    public partial class AddVendor : Form
    {
        public AddVendor()
        {
            InitializeComponent();
        }

        private void AddVendor_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Whene the user clicks this button it will check the data, add it to the DB, and close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetError();

            if (ValidateData())
            {
                AddVendorToDB();
                this.Close();
            }
            else
            {
                MessageBox.Show("All fields must have correct data!", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        /// <summary>
        /// Reset the back color after an error
        /// </summary>
        private void ResetError()
        {
            txtId.BackColor = Color.White;
            txtName.BackColor = Color.White;
        }


        /// <summary>
        /// Test the data before writing it to the database
        /// </summary>
        /// <returns></returns>
        private bool ValidateData()
        {
            // Method level variables
            bool pass = true;
            string errorMsg = "Check that all fields have correct data.\r\n";

            // Test data
            if (txtId.Text == "" || txtId.Text == null)
            {
                txtId.BackColor = Color.LightPink;
                pass = false;
                errorMsg += "\t-Must provide a ID.\r\n";
            }

            if (txtName.Text == "" || txtName.Text == null)
            {
                txtId.BackColor = Color.LightPink;
                pass = false;
                errorMsg += "\t-Must provide a name.\r\n";
            }

            // If pass fails provide the user with a n error message
            if (!pass)
            {
                MessageBox.Show(errorMsg, "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return pass;
        }


        /// <summary>
        /// Add vendor to the database
        /// </summary>
        public void AddVendorToDB()
        {
            // Create a vendor object
            Models.Vendors vendorToBeAdded = new Models.Vendors();

            // Fill vendor object
            vendorToBeAdded.VendorId = long.Parse(txtId.Text);
            vendorToBeAdded.VendorName = txtName.Text;

            // Write the data to the DB
            Connections.DataConnections.DataConnectionClass.VendorConn.AddVendor(vendorToBeAdded);
            Connections.DataConnections.DataConnectionClass.DataLists.Vendors.Add(Connections.DataConnections.DataConnectionClass.VendorConn.GetVendor(vendorToBeAdded.VendorId));
        }
    }
}
