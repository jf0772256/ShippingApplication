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
        private string message = "NONE";
        private new Receiving ParentForm { get; set; }


        #region form basic
        public AddPackage(string message, Receiving parent)
        {
            ParentForm = parent;
            InitializeComponent();
        }

        public AddPackage(string message, object packageToBeEdited, Receiving parent)
        {
            InitializeComponent();
            newPackage = (Models.Package)packageToBeEdited;
            ParentForm = parent;
        }


        private void AddPackage_Load(object sender, EventArgs e)
        {
            // If edit, fill form with the pakcage info
            if (message == "EDIT")
            {
                // TODO: Filter ComboBoxe with correct info  
                txtDate.Text = newPackage.PackageReceivedDate;
                txtDelDate.Text = newPackage.PackageDeliveredDate;
                txtPersonId.Text = newPackage.Package_PersonId;
                txtPO.Text = newPackage.PONumber;
                txtTracking.Text = newPackage.PackageTrackingNumber;
                cmboCarrier.Text = newPackage.PackageCarrier;
                cmboVendor.Text = newPackage.PackageVendor;
                cmboRecipiant.Text = newPackage.PackageDeliveredTo;
                cmboSignedBy.Text = newPackage.PackageSignedForBy;
                cmboDelBy.Text = newPackage.PackageDeleveredBy;
                cmboStatus.Text = newPackage.Status.ToString();
            }
            else if (message == "ADD")
            {
                // Instatiate Package
                newPackage = new Models.Package();
            }
        }
        #endregion


        #region Form Add
        /// <summary>
        /// When the button is clicked attempt to add a package to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceive_Click(object sender, EventArgs e)
        {
            // Reset errors messages
            ResetError();

            // Check the data, then write the package to the database
            if (CheckData())
            {
                AddPackageToDB();
                this.Close();
            }
            else if (CheckData() && message == "EDIT")
            {
                EditPackageInDb();
                this.Close();
            }
        }

  
        /// <summary>
        /// Grab the data from the form, check for errors, create a package entity, and add it to the database
        /// </summary>
        public void AddPackageToDB()
        {
            // Create Package
            FillPackage();

            // Write Package
            Connections.DataConnections.DataConnectionClass.PackageConnClass.AddPackage(newPackage);
            //Connections.DataConnections.DataConnectionClass.DataLists.Packages.Add(newPackage);
            Connections.DataConnections.DataConnectionClass.PackageConnClass.GetPackageList(this.ParentForm);
        }


        /// <summary>
        /// Fill the object
        /// </summary>
        public void EditPackageInDb()
        {
            // Create Package
            FillPackage();

            // Edit Package
            Connections.DataConnections.DataConnectionClass.PackageConnClass.UpdatePackage(newPackage);
            //Connections.DataConnections.DataConnectionClass.DataLists.Packages.Add(newPackage);
            Connections.DataConnections.DataConnectionClass.PackageConnClass.GetPackageList(this.ParentForm);
        }
        #endregion


        #region Data Integrity
        /// <summary>
        /// Reset the error warnings
        /// </summary>
        public void ResetError()
        {
            cmboCarrier.BackColor = Color.White;
            cmboVendor.BackColor = Color.White;
            cmboRecipiant.BackColor = Color.White;
            txtTracking.BackColor = Color.White;
            cmboDelBy.BackColor = Color.White;
            cmboSignedBy.BackColor = Color.White;
            txtPersonId.BackColor = Color.White;
        }


        /// <summary>
        /// Check that correct and neccesary data is enetered to the form
        /// </summary>
        /// <returns></returns>
        public bool CheckData()
        {
            // Method level variables
            bool pass = true;
            string errorMsg = "Unable to add package.\r\nPlease insure the following items are filled with correct data.\r\n";

            // Check that a carrier is selected
            if (cmboCarrier.Text == "" || cmboCarrier.Text == null)
            {
                pass = false;
                cmboCarrier.BackColor = Color.LightPink;
                errorMsg += "\t-Must select a carrier.\r\n";
            }

            // Check that a vendor is selected
            if (cmboVendor.Text == "" || cmboVendor.Text == null)
            {
                pass = false;
                cmboVendor.BackColor = Color.LightPink;
                errorMsg += "\t-Must slect a vendor.\r\n";
            }

            // Check that a recipiant is selected
            if (cmboRecipiant.Text == "" || cmboRecipiant.Text == null)
            {
                pass = false;
                cmboRecipiant.BackColor = Color.LightPink;
                errorMsg += "\t-Must select a recipiant.\r\n";
            }

            // Check that the package has a tracking number
            if (txtTracking.Text == "" || txtTracking.Text == null)
            {
                pass = false;
                txtTracking.BackColor = Color.LightPink;
                errorMsg += "\t-Must include a tracking number.\r\n";
            }

            // Check that a deliveiry person is selected
            if (cmboDelBy.Text == "" || cmboDelBy.Text == null)
            {
                pass = false;
                cmboDelBy.BackColor = Color.LightPink;
                errorMsg += "\t-Must select a delivery person.\r\n";
            }
            
            // Check that a person ID has been assigned
            if (txtPersonId.Text == "" || txtPersonId == null)
            {
                pass = false;
                cmboSignedBy.BackColor = Color.LightPink;
                errorMsg += "\t-Must include a person ID.\r\n";
            }

            // If the data is not correct alert the user with a message
            if (!pass)
            {
                MessageBox.Show(errorMsg, "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return pass;
        }


        /// <summary>
        /// Fill the packge entity with data
        /// </summary>
        /// <returns></returns>
        public void FillPackage()
        {
            // Create packagae
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
        }
        #endregion
    }
}
