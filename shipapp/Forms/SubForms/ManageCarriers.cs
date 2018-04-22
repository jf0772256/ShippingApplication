using shipapp.Connections.DataConnections;
using shipapp.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace shipapp
{
    /// <summary>
    /// This class will allow the application to add and edit carriers
    /// </summary>
    public partial class ManageCarrier : Form
    {
        // Class level variables
        private string message;
        private Carrier newCarrier;


        public ManageCarrier(string message)
        {
            InitializeComponent();
            this.message = message;
        }


        public ManageCarrier(string message, Object carrierToBeEdited)
        {
            InitializeComponent();
            this.message = message;
            newCarrier = (Carrier)carrierToBeEdited;
        }


        private void AddCarrier_Load(object sender, EventArgs e)
        {
            if (message == "EDIT")
            {
                this.Text = "Edit Carrier";
                btnAdd.Text = "EDIT";
                txtName.Text = newCarrier.CarrierName;
            }
        }


        /// <summary>
        /// When the user clicks the ADD button it will check the data, write the data to the DB, and then close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Reset the Errors
            if (CheckData() && message == "ADD")
            {
                AddCarrierToDB();
                this.Close();
            }
            else if (CheckData() && message == "EDIT")
            {
                EditCarrier();
                this.Close();
            }
            else
            {
                MessageBox.Show("All fields must have correct data!", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        /// <summary>
        /// Check the data, then add the object to the DB
        /// </summary>
        public void AddCarrierToDB()
        {
            // Create Enity
            Carrier carrierToBeAdded = new Carrier();

            // Fill entity
            carrierToBeAdded.CarrierName = txtName.Text;

            // Write the data to the DB
            DataConnectionClass.CarrierConn.AddCarrier(carrierToBeAdded);
            DataConnectionClass.AuditLogConnClass.AddRecordToAudit("added a new carrier " + carrierToBeAdded.CarrierName);
        }


        public void EditCarrier()
        {
            string oldname = newCarrier.CarrierName;
            newCarrier.CarrierName = txtName.Text;

            // Write the data to the DB
            DataConnectionClass.CarrierConn.UpdateCarrier(newCarrier);
            DataConnectionClass.AuditLogConnClass.AddRecordToAudit("renamed a carrier from " + oldname + " to " + newCarrier.CarrierName);
        }


        /// <summary>
        /// Reset the errors
        /// </summary>
        public void ResetErrors()
        {
            txtName.BackColor = Color.White;
        }


        /// <summary>
        /// Chect tha t the data is complete
        /// </summary>
        /// <returns></returns>
        public bool CheckData()
        {
            // Method level variables
            bool pass = true;
            string errorMsg = "Please make sure all flieds have correct data";

            // Check that there is a name
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                pass = false;
                txtName.BackColor = Color.LightPink;
                errorMsg += "\t-Must include a name\r\n";
            }

            // If the pass fails, alert the user
            if (!pass)
            {
                MessageBox.Show(errorMsg, "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return pass;
        }
    }
}
