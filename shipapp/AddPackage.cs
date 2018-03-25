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
    /// This form allows for the creation and editing of new packages
    /// </summary>
    public partial class AddPackage : Form
    {
        // Class level variabels
        private Models.Package newPackage;

        public AddPackage()
        {
            InitializeComponent();
        }

        private void AddPackage_Load(object sender, EventArgs e)
        {
            // Instatiate Package
            newPackage = new Models.Package();
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
            // Reset errors messages
            ResetError();

            // Check that data is correct and then write the entity to the database
            if (CheckData())
            {
                AddPackageToDB();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please insure all fields have required and correct data!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Grab the data from the form, check for errors, create a package entity, and add it to the database
        /// </summary>
        public void AddPackageToDB()
        {   
            //// Read in correctly from combo boxes
            //newPackage.PONumber = txtPO.Text;
            //newPackage.PackageCarrier = cmboCarrier.SelectedItem.ToString();
            //newPackage.PackageVendor = cmboVendor.SelectedText.ToString();
            //newPackage.PackageDeliveredTo = cmboRecipiant.SelectedText.ToString();
            //newPackage.PackageTrackingNumber = txtTracking.Text;
            //newPackage.PackageDeleveredBy = cmboDelBy.SelectedText.ToString();
            //newPackage.PackageSignedForBy = cmboSignedBy.SelectedText.ToString();
            //newPackage.PackageDeliveredDate = txtDelDate.SelectedText.ToString();

            // Test expressions 
            newPackage.PONumber = txtPO.Text;
            newPackage.PackageCarrier = cmboCarrier.Text;
            newPackage.PackageVendor = cmboVendor.Text;
            newPackage.PackageDeliveredTo = cmboRecipiant.Text;
            newPackage.PackageTrackingNumber = txtTracking.Text;
            newPackage.PackageDeleveredBy = cmboDelBy.Text;
            newPackage.PackageSignedForBy = cmboSignedBy.Text;
            newPackage.PackageReceivedDate = txtDate.Text;
            newPackage.PackageDeliveredDate = txtDelDate.Text;
            newPackage.Package_PersonId = txtPersonId.Text;
            newPackage.Status = (Models.Package.DeliveryStatus)Convert.ToInt32(cmboStatus.Text);

            Connections.DataConnections.DataConnectionClass.PackageConnClass.AddPackage(newPackage);
        }

        /// <summary>
        /// Reset the error warnings
        /// </summary>
        public void ResetError()
        {

        }

        /// <summary>
        /// Check that correct and neccesary data is enetered to the form
        /// </summary>
        /// <returns></returns>
        public bool CheckData()
        {
            // Method level variables
            bool pass = true;

            return pass;
        }

        /// <summary>
        /// Fill the packge entity with data
        /// </summary>
        /// <returns></returns>
        public void FillPackage()
        {
            if (txtPO.Text != "" && txtPO.Text != null)
            {
                //newPO.PONumber = txtPO.Text;
                
            }

            if (cmboCarrier.Text != null && cmboCarrier.Text != "")
            {
                Models.Carrier newCarier = new Models.Carrier();
                newCarier.CarrierName = cmboCarrier.Text;
                //newPackage.PackageCarrier = newCarier;
            }
        }
    }
}
