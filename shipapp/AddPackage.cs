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
using shipapp.Models.ModelData;
using shipapp.Connections.DataConnections;

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
        private string WorkingPID { get; set; }


        #region form basic
        public AddPackage(string message, Receiving parent)
        {
            ParentForm = parent;
            InitializeComponent();
            this.message = message;
            cmboStatus.Items.Add("Not_Recieved");
            cmboStatus.Items.Add("Received");
            cmboStatus.Items.Add("OutForDelivery");
            cmboStatus.Items.Add("Delivered");
            RefreshLists();
        }

        public AddPackage(string message, object packageToBeEdited, Receiving parent)
        {
            InitializeComponent();
            newPackage = (Package)packageToBeEdited;
            ParentForm = parent;
            this.message = message;
            cmboStatus.Items.Add("Not_Recieved");
            cmboStatus.Items.Add("Received");
            cmboStatus.Items.Add("OutForDelivery");
            cmboStatus.Items.Add("Delivered");
            RefreshLists();
        }
        
        private void RefreshLists()
        {
            DataConnectionClass.UserConn.GetManyUsers();
            DataConnectionClass.VendorConn.GetVendorList();
            DataConnectionClass.CarrierConn.GetCarrierList();
            DataConnectionClass.buildingConn.GetBuildingList();
            DataConnectionClass.EmployeeConn.GetAllAfaculty();
        }

        private void AddPackage_Load(object sender, EventArgs e)
        {
            // If edit, fill form with the pakcage info
            if (message == "EDIT")
            {
                // TODO: Filter ComboBoxe with correct info  
                txtPO.Text = newPackage.PONumber;
                txtTracking.Text = newPackage.PackageTrackingNumber;
                cmboCarrier.Text = newPackage.PackageCarrier;
                cmboVendor.Text = newPackage.PackageVendor;
                cmboRecipiant.Text = newPackage.PackageDeliveredTo;
                cmboSignedBy.Text = newPackage.PackageSignedForBy;
                cmboDelBy.Text = newPackage.PackageDeleveredBy;
                cmboStatus.Text = newPackage.Status.ToString();
                txtRoleId.Text = newPackage.Package_PersonId;
                cmboBuilding.Text = newPackage.DelivBuildingShortName;
                foreach (Carrier car in DataConnectionClass.DataLists.CarriersList)
                {
                    cmboCarrier.Items.Add(car.ToString());
                }
                foreach (Vendors vnd in DataConnectionClass.DataLists.Vendors)
                {
                    cmboVendor.Items.Add(vnd.ToString());
                }
                foreach (Faculty fac in DataConnectionClass.DataLists.FacultyList)
                {
                    cmboRecipiant.Items.Add(fac.ToString());
                    cmboSignedBy.Items.Add(fac.ToString());
                }
                foreach (BuildingClass bldg in DataConnectionClass.DataLists.BuildingNames)
                {
                    cmboBuilding.Items.Add(bldg.ToString());
                }
                foreach (User usr in DataConnectionClass.DataLists.UsersList)
                {
                    cmboDelBy.Items.Add(usr.ToFormattedString());
                }
            }
            else if (message == "ADD")
            {
                // Instatiate Package
                newPackage = new Package();
                foreach (Carrier car in DataConnectionClass.DataLists.CarriersList)
                {
                    cmboCarrier.Items.Add(car.ToString());
                }
                foreach (Vendors vnd in DataConnectionClass.DataLists.Vendors)
                {
                    cmboVendor.Items.Add(vnd.ToString());
                }
                foreach (Faculty fac in DataConnectionClass.DataLists.FacultyList)
                {
                    cmboRecipiant.Items.Add(fac.ToString());
                    cmboSignedBy.Items.Add(fac.ToString());
                }
                foreach (BuildingClass bldg in DataConnectionClass.DataLists.BuildingNames)
                {
                    cmboBuilding.Items.Add(bldg.ToString());
                }
                foreach (User usr in DataConnectionClass.DataLists.UsersList)
                {
                    cmboDelBy.Items.Add(usr.ToFormattedString());
                }
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
            if (CheckData() && message == "ADD")
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
            DataConnectionClass.PackageConnClass.AddPackage(newPackage);
            //do this ONLY on add
            DataConnectionClass.SavePersonId();
            //Connections.DataConnections.DataConnectionClass.DataLists.Packages.Add(newPackage);
            DataConnectionClass.PackageConnClass.GetPackageList(this.ParentForm);
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
            dTDel.CalendarMonthBackground = Color.White;
            dTRec.CalendarMonthBackground = Color.White;
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
            if (String.IsNullOrWhiteSpace(cmboCarrier.Text))
            {
                pass = false;
                cmboCarrier.BackColor = Color.LightPink;
                errorMsg += "\t-Must select a carrier.\r\n";
            }

            // Check that a vendor is selected
            if (String.IsNullOrWhiteSpace(cmboVendor.Text))
            {
                pass = false;
                cmboVendor.BackColor = Color.LightPink;
                errorMsg += "\t-Must slect a vendor.\r\n";
            }

            // Check that a recipiant is selected
            if (String.IsNullOrWhiteSpace(cmboRecipiant.Text))
            {
                pass = false;
                cmboRecipiant.BackColor = Color.LightPink;
                errorMsg += "\t-Must select a recipiant.\r\n";
            }

            // Check that the package has a tracking number
            if (String.IsNullOrWhiteSpace(txtTracking.Text))
            {
                pass = false;
                txtTracking.BackColor = Color.LightPink;
                errorMsg += "\t-Must include a tracking number.\r\n";
            }

            //// Check that a deliveiry person is selected
            //if (String.IsNullOrWhiteSpace(cmboDelBy.Text))
            //{
            //    pass = false;
            //    cmboDelBy.BackColor = Color.LightPink;
            //    errorMsg += "\t-Must select a delivery person.\r\n";
            //}
            
            //// Check that a person ID has been assigned
            //if (txtPersonId.Text == "" || txtPersonId == null)
            //{
            //    pass = false;
            //    cmboSignedBy.BackColor = Color.LightPink;
            //    errorMsg += "\t-Must include a person ID.\r\n";
            //}

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
            newPackage.PackageReceivedDate = dTRec.Value.ToShortDateString();
            newPackage.PackageDeliveredDate = dTDel.Value.ToShortDateString();
            newPackage.Package_PersonId = txtRoleId.Text;
            newPackage.DelivBuildingShortName = cmboBuilding.Text;
            newPackage.Status = (Models.Package.DeliveryStatus)FormatStatus(cmboStatus.Text);
        }
        #endregion
        
        /// <summary>
        /// Set status to proper int
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public int FormatStatus(string status)
        {
            if (status == "Received")
            {
                return 1;
            }
            else if (status == "OutForDelivery")
            {
                return 2;
            }
            else if (status == "Delivered")
            {
                return 3;
            }

            return 0;
        }
        private void cmboStatus_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string selText = cmboStatus.SelectedItem.ToString();
            if (selText == "Not Recieved")
            {
                newPackage.Status = Models.Package.DeliveryStatus.Not_Received;
            }
            else if (selText == "Recieved")
            {
                newPackage.Status = Models.Package.DeliveryStatus.Received;
            }
            else if (selText == "Out For Delivery")
            {
                newPackage.Status = Models.Package.DeliveryStatus.OutForDelivery;
            }
            else if (selText == "Delivered")
            {
                newPackage.Status = Models.Package.DeliveryStatus.Delivered;
            }
            else
            {
                newPackage.Status = Models.Package.DeliveryStatus.Not_Received;
            }
        }
        private void cmboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selText = cmboStatus.SelectedItem.ToString();
            if (selText == "Not Recieved")
            {
                newPackage.Status = Models.Package.DeliveryStatus.Not_Received;
            }
            else if (selText == "Recieved")
            {
                newPackage.Status = Models.Package.DeliveryStatus.Received;
            }
            else if (selText == "Out For Delivery")
            {
                newPackage.Status = Models.Package.DeliveryStatus.OutForDelivery;
            }
            else if (selText == "Delivered")
            {
                newPackage.Status = Models.Package.DeliveryStatus.Delivered;
            }
            else
            {
                newPackage.Status = Models.Package.DeliveryStatus.Not_Received;
            }
        }

        #region For creation of the person id on the fly
        private void txtPO_Leave(object sender, EventArgs e)
        {
            if (message != "EDIT")
            {
                if (!String.IsNullOrWhiteSpace(txtPO.Text))
                {
                    if (txtPO.Text.Length < 4)
                    {
                        WorkingPID = txtPO.Text.ToLower().Substring(0, txtPO.Text.Length);
                    }
                    else
                    {
                        WorkingPID = txtPO.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboCarrier.Text))
                {
                    if (!String.IsNullOrWhiteSpace(txtPO.Text))
                    {
                        if (cmboCarrier.Text.Length < 4)
                        {
                            WorkingPID += cmboCarrier.Text.ToLower().Substring(0, cmboCarrier.Text.Length);
                        }
                        else
                        {
                            WorkingPID += cmboCarrier.Text.ToLower().Substring(0, 4);
                        }
                    }
                    else
                    {
                        if (cmboCarrier.Text.Length < 4)
                        {
                            WorkingPID = cmboCarrier.Text.ToLower().Substring(0, cmboCarrier.Text.Length);
                        }
                        else
                        {
                            WorkingPID = cmboCarrier.Text.ToLower().Substring(0, 4);
                        }
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboVendor.Text))
                {
                    if (cmboVendor.Text.Length < 4)
                    {
                        WorkingPID += cmboVendor.Text.ToLower().Substring(0, cmboVendor.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboVendor.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboRecipiant.Text))
                {
                    if (cmboRecipiant.Text.Length < 4)
                    {
                        WorkingPID += cmboRecipiant.Text.ToLower().Substring(0, cmboRecipiant.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboRecipiant.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboBuilding.Text))
                {
                    if (cmboBuilding.Text.Length < 4)
                    {
                        WorkingPID += cmboBuilding.Text.ToLower().Substring(0, cmboBuilding.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboBuilding.Text.ToLower().Substring(0, 4);
                    }
                }
                DataConnectionClass.CreatePersonId(WorkingPID);
                txtRoleId.Text = DataConnectionClass.PersonIdGenerated;
            }
        }
        private void cmboCarrier_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (message != "EDIT")
            {
                cmboCarrier.Text = cmboCarrier.SelectedItem.ToString();
                if (!String.IsNullOrWhiteSpace(txtPO.Text))
                {
                    if (txtPO.Text.Length < 4)
                    {
                        WorkingPID = txtPO.Text.ToLower().Substring(0, txtPO.Text.Length);
                    }
                    else
                    {
                        WorkingPID = txtPO.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboCarrier.Text))
                {
                    if (!String.IsNullOrWhiteSpace(txtPO.Text))
                    {
                        if (cmboCarrier.Text.Length < 4)
                        {
                            WorkingPID += cmboCarrier.Text.ToLower().Substring(0, cmboCarrier.Text.Length);
                        }
                        else
                        {
                            WorkingPID += cmboCarrier.Text.ToLower().Substring(0, 4);
                        }
                    }
                    else
                    {
                        if (cmboCarrier.Text.Length < 4)
                        {
                            WorkingPID = cmboCarrier.Text.ToLower().Substring(0, cmboCarrier.Text.Length);
                        }
                        else
                        {
                            WorkingPID = cmboCarrier.Text.ToLower().Substring(0, 4);
                        }
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboVendor.Text))
                {
                    if (cmboVendor.Text.Length < 4)
                    {
                        WorkingPID += cmboVendor.Text.ToLower().Substring(0, cmboVendor.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboVendor.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboRecipiant.Text))
                {
                    if (cmboRecipiant.Text.Length < 4)
                    {
                        WorkingPID += cmboRecipiant.Text.ToLower().Substring(0, cmboRecipiant.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboRecipiant.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboBuilding.Text))
                {
                    if (cmboBuilding.Text.Length < 4)
                    {
                        WorkingPID += cmboBuilding.Text.ToLower().Substring(0, cmboBuilding.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboBuilding.Text.ToLower().Substring(0, 4);
                    }
                }
                DataConnectionClass.CreatePersonId(WorkingPID);
                txtRoleId.Text = DataConnectionClass.PersonIdGenerated;
            }
        }
        private void cmboVendor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (message != "EDIT")
            {
                cmboVendor.Text = cmboVendor.SelectedItem.ToString();
                if (!String.IsNullOrWhiteSpace(txtPO.Text))
                {
                    if (txtPO.Text.Length < 4)
                    {
                        WorkingPID = txtPO.Text.ToLower().Substring(0, txtPO.Text.Length);
                    }
                    else
                    {
                        WorkingPID = txtPO.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboCarrier.Text))
                {
                    if (!String.IsNullOrWhiteSpace(txtPO.Text))
                    {
                        if (cmboCarrier.Text.Length < 4)
                        {
                            WorkingPID += cmboCarrier.Text.ToLower().Substring(0, cmboCarrier.Text.Length);
                        }
                        else
                        {
                            WorkingPID += cmboCarrier.Text.ToLower().Substring(0, 4);
                        }
                    }
                    else
                    {
                        if (cmboCarrier.Text.Length < 4)
                        {
                            WorkingPID = cmboCarrier.Text.ToLower().Substring(0, cmboCarrier.Text.Length);
                        }
                        else
                        {
                            WorkingPID = cmboCarrier.Text.ToLower().Substring(0, 4);
                        }
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboVendor.Text))
                {
                    if (cmboVendor.Text.Length < 4)
                    {
                        WorkingPID += cmboVendor.Text.ToLower().Substring(0, cmboVendor.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboVendor.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboRecipiant.Text))
                {
                    if (cmboRecipiant.Text.Length < 4)
                    {
                        WorkingPID += cmboRecipiant.Text.ToLower().Substring(0, cmboRecipiant.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboRecipiant.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboBuilding.Text))
                {
                    if (cmboBuilding.Text.Length < 4)
                    {
                        WorkingPID += cmboBuilding.Text.ToLower().Substring(0, cmboBuilding.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboBuilding.Text.ToLower().Substring(0, 4);
                    }
                }
                DataConnectionClass.CreatePersonId(WorkingPID);
                txtRoleId.Text = DataConnectionClass.PersonIdGenerated;
            }
        }
        private void cmboRecipiant_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (message != "EDIT")
            {
                cmboRecipiant.Text = cmboRecipiant.SelectedItem.ToString();
                if (!String.IsNullOrWhiteSpace(txtPO.Text))
                {
                    if (txtPO.Text.Length < 4)
                    {
                        WorkingPID = txtPO.Text.ToLower().Substring(0, txtPO.Text.Length);
                    }
                    else
                    {
                        WorkingPID = txtPO.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboCarrier.Text))
                {
                    if (!String.IsNullOrWhiteSpace(txtPO.Text))
                    {
                        if (cmboCarrier.Text.Length < 4)
                        {
                            WorkingPID += cmboCarrier.Text.ToLower().Substring(0, cmboCarrier.Text.Length);
                        }
                        else
                        {
                            WorkingPID += cmboCarrier.Text.ToLower().Substring(0, 4);
                        }
                    }
                    else
                    {
                        if (cmboCarrier.Text.Length < 4)
                        {
                            WorkingPID = cmboCarrier.Text.ToLower().Substring(0, cmboCarrier.Text.Length);
                        }
                        else
                        {
                            WorkingPID = cmboCarrier.Text.ToLower().Substring(0, 4);
                        }
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboVendor.Text))
                {
                    if (cmboVendor.Text.Length < 4)
                    {
                        WorkingPID += cmboVendor.Text.ToLower().Substring(0, cmboVendor.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboVendor.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboRecipiant.Text))
                {
                    if (cmboRecipiant.Text.Length < 4)
                    {
                        WorkingPID += cmboRecipiant.Text.ToLower().Substring(0, cmboRecipiant.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboRecipiant.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboBuilding.Text))
                {
                    if (cmboBuilding.Text.Length < 4)
                    {
                        WorkingPID += cmboBuilding.Text.ToLower().Substring(0, cmboBuilding.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboBuilding.Text.ToLower().Substring(0, 4);
                    }
                }
                DataConnectionClass.CreatePersonId(WorkingPID);
                txtRoleId.Text = DataConnectionClass.PersonIdGenerated;
            }
        }
        private void cmboBuilding_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (message != "EDIT")
            {
                cmboBuilding.Text = cmboBuilding.SelectedItem.ToString();
                if (!String.IsNullOrWhiteSpace(txtPO.Text))
                {
                    if (txtPO.Text.Length < 4)
                    {
                        WorkingPID = txtPO.Text.ToLower().Substring(0, txtPO.Text.Length);
                    }
                    else
                    {
                        WorkingPID = txtPO.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboCarrier.Text))
                {
                    if (!String.IsNullOrWhiteSpace(txtPO.Text))
                    {
                        if (cmboCarrier.Text.Length < 4)
                        {
                            WorkingPID += cmboCarrier.Text.ToLower().Substring(0, cmboCarrier.Text.Length);
                        }
                        else
                        {
                            WorkingPID += cmboCarrier.Text.ToLower().Substring(0, 4);
                        }
                    }
                    else
                    {
                        if (cmboCarrier.Text.Length < 4)
                        {
                            WorkingPID = cmboCarrier.Text.ToLower().Substring(0, cmboCarrier.Text.Length);
                        }
                        else
                        {
                            WorkingPID = cmboCarrier.Text.ToLower().Substring(0, 4);
                        }
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboVendor.Text))
                {
                    if (cmboVendor.Text.Length < 4)
                    {
                        WorkingPID += cmboVendor.Text.ToLower().Substring(0, cmboVendor.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboVendor.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboRecipiant.Text))
                {
                    if (cmboRecipiant.Text.Length < 4)
                    {
                        WorkingPID += cmboRecipiant.Text.ToLower().Substring(0, cmboRecipiant.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboRecipiant.Text.ToLower().Substring(0, 4);
                    }
                }
                if (!String.IsNullOrWhiteSpace(cmboBuilding.Text))
                {
                    if (cmboBuilding.Text.Length < 4)
                    {
                        WorkingPID += cmboBuilding.Text.ToLower().Substring(0, cmboBuilding.Text.Length);
                    }
                    else
                    {
                        WorkingPID += cmboBuilding.Text.ToLower().Substring(0, 4);
                    }
                }
                DataConnectionClass.CreatePersonId(WorkingPID);
                txtRoleId.Text = DataConnectionClass.PersonIdGenerated;
            }
        }
        #endregion
    }
}
