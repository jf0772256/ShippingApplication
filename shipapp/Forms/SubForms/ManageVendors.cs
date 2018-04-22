using shipapp.Connections.DataConnections;
using shipapp.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace shipapp
{
    /// <summary>
    /// This class will allow the users to add edit users
    /// </summary>
    public partial class ManageVendors : Form
    {
        // Class level variables
        private string message;
        private Vendors vendorToBeEdited;


        public ManageVendors(string message)
        {
            InitializeComponent();
            this.message = message;
        }


        public ManageVendors(string message, object vendorToBeEdited)
        {
            InitializeComponent();
            this.message = message;
            this.vendorToBeEdited = (Vendors)vendorToBeEdited;
        }


        /// <summary>
        /// IF the user trys to edit a vendor set the form to edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddVendor_Load(object sender, EventArgs e)
        {
            // If EDIT set to edit mode
            if (message == "EDIT")
            {
                // Set textbox
                txtName.Text = vendorToBeEdited.VendorName;

                // Set button
                this.Text = "Edit Vendor";
                btnAdd.Text = "EDIT";
            }
        }


        /// <summary>
        /// Whene the user clicks this button it will check the data, add it to the DB, and close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Reset any errors on the form
            ResetError();

            // Call the correct method depeding on validation and the message
            if (ValidateData() && message == "ADD")
            {
                // Add a vendor
                AddVendorToDB();
                this.Close();
            }
            else if (ValidateData() && message == "EDIT")
            {
                // Edit a vendor
                EditVendor();
                this.Close();
            }
            else if(message != "ADD" && message != "EDIT")
            {
                // If something is wrong with the message
                MessageBox.Show("Their was a problem with the form loading./r/Try Again.", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
            }
            else
            {
                // If the form does not validate
                MessageBox.Show("All fields must have correct data!", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        /// <summary>
        /// Reset the back color after an error
        /// </summary>
        private void ResetError()
        {
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
            if (txtName.Text == "" || txtName.Text == null)
            {
                txtName.BackColor = Color.LightPink;
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
            Vendors vendorToBeAdded = new Vendors();

            // Fill vendor object
            vendorToBeAdded.VendorName = txtName.Text;

            // Write the data to the DB
            DataConnectionClass.VendorConn.AddVendor(vendorToBeAdded);
            DataConnectionClass.AuditLogConnClass.AddRecordToAudit("added a new vendor " + vendorToBeAdded.VendorName);
            
        }

        
        /// <summary>
        /// Edit a vendor in the DB
        /// </summary>
        public void EditVendor()
        {
            string oldname = vendorToBeEdited.VendorName;
            // Set new info
            vendorToBeEdited.VendorName = txtName.Text;

            // Edit the vendor
            DataConnectionClass.VendorConn.UpdateVendor(vendorToBeEdited);
            DataConnectionClass.AuditLogConnClass.AddRecordToAudit("edited vendor name from " + oldname + " to " + vendorToBeEdited.VendorName);
        }
    }
}
