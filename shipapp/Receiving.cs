using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using shipapp.Connections.HelperClasses;
using shipapp.Models;
using shipapp.Models.ModelData;
using shipapp.Connections.DataConnections;


namespace shipapp
{
    /// <summary>
    ///  This class allows the user to receive and manage freight as it comes in,
    /// as well as, create delivery logs.
    /// </summary>
    public partial class Receiving : Form
    {
        /// Class level variables
        //  Data list
        private DataGridViewColumnHelper dgvch = new DataGridViewColumnHelper();
        private ListSortDirection[] ColumnDirection { get; set; }
        private BindingList<Log> logList;
        private List<Log> logs = new List<Log>();
        private List<Package> printPackages = new List<Package>();
        //  Other variabels
        private string message = "";
        private int role;

        #region Form Setup
        /// <summary>
        /// Form constructor for Recieving
        /// </summary>
        public Receiving()
        {
            InitializeComponent();
        }
        /// <summary>
        /// When the form loads Center it, set the role, and fill the grid with packages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Receiving_Load(object sender, EventArgs e)
        {
            // Set form
            this.CenterToParent();
            SetRole();
            GetPackages();
            lblSearch.Text = "";
            // Disable search until a column is selected
            if (lblSearch.Text.Length == 0)
            {
                txtSearch.Enabled = false;
            }
            // Set form to match role
            if (role == 1)
            {
                btnAdd.Enabled = true;
                btnAdd.BackColor = Color.Transparent;
                pcBxEdit.Enabled = true;
                pcBxEdit.BackColor = Color.Transparent;
                pictureBox3.Enabled = true;
                pictureBox3.BackColor = Color.Transparent;
                pcBxPrint.Enabled = true;
                pcBxPrint.BackColor = Color.Transparent;
            }
            else if (role == 2)
            {
                btnAdd.Enabled = true;
                btnAdd.BackColor = Color.Transparent;
                pcBxEdit.Enabled = true;
                pcBxEdit.BackColor = Color.Transparent;
                pictureBox3.Enabled = true;
                pictureBox3.BackColor = Color.Transparent;
                pcBxPrint.Enabled = true;
                pcBxPrint.BackColor = Color.Transparent;
            }
            else if (role == 3)
            {
                btnAdd.Enabled = false;
                btnAdd.BackColor = Color.LightPink;
                pcBxEdit.Enabled = false;
                pcBxEdit.BackColor = Color.LightPink;
                pictureBox3.Enabled = false;
                pictureBox3.BackColor = Color.LightPink;
                pcBxPrint.Enabled = false;
                pcBxPrint.BackColor = Color.LightPink;
            }
            else if (role == 4)
            {

            }
        }
        /// <summary>
        /// Fill the list with packages from the database.
        /// </summary>
        public void GetPackages()
        {
            DataConnectionClass.PackageConnClass.GetPackageList(this);
        }

        /// <summary>
        /// Set role to match the users privileges.
        /// </summary>
        public void SetRole()
        {
            if (DataConnectionClass.AuthenticatedUser.Level.Role_Title == "Administrator")
            {
                this.role = 1;
            }
            else if (DataConnectionClass.AuthenticatedUser.Level.Role_Title == "Supervisor")
            {
                role = 2;
            }
            else if (DataConnectionClass.AuthenticatedUser.Level.Role_Title == "User")
            {
                role = 3;
            }
            else
            {
                role = 0;
            }
        }
        #endregion

        #region Events AllEvents
        /// <summary>
        /// Open the AddPackage form and add a package to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddPackageToGrid();
        }
        /// <summary>
        /// Remove the selected row from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // Test that a row is selected before deletion
            if (dataGridPackages.SelectedRows.Count > 0)
            {
                DeletePackage();
            }
        }
        /// <summary>
        /// Sort the data is the grid view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // Data sort
                if (ColumnDirection.Length > 0 && ColumnDirection[e.ColumnIndex] == ListSortDirection.Descending)
                {
                    dataGridPackages.Sort(dataGridPackages.Columns[e.ColumnIndex], ListSortDirection.Ascending);
                    ColumnDirection[e.ColumnIndex] = ListSortDirection.Ascending;
                }
                else if (ColumnDirection.Length > 0 && ColumnDirection[e.ColumnIndex] == ListSortDirection.Ascending)
                {
                    dataGridPackages.Sort(dataGridPackages.Columns[e.ColumnIndex], ListSortDirection.Descending);
                    ColumnDirection[e.ColumnIndex] = ListSortDirection.Descending;
                }
            }
            catch (Exception)
            {
                //do nothing but quietly handle error
            }
        }
        /// <summary>
        /// Go back to the main menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Edit a packages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pcBxEdit_Click(object sender, EventArgs e)
        {
            if (dataGridPackages.SelectedRows.Count > 0)
            {
                EditPackage();
            }
        }
        /// <summary>
        /// Alert the user on an atempt to signout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            SignOut();
        }
        /// <summary>
        /// Alert the user on an atempt to signout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            SignOut();
        }
        /// <summary>
        /// Return a filtered list as search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            QueryPackages(txtSearch.Text);
        }
        /// <summary>
        /// Refreash the data grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pcBXRefreash_Click(object sender, EventArgs e)
        {
            Refreash();
            MessageBox.Show("The list has refreshed");
        }
        /// <summary>
        /// Print the selected packages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pcBxPrint_Click(object sender, EventArgs e)
        {
            // Test that at least one row is selected
            if (dataGridPackages.SelectedRows.Count > 0)
            {
                Print();
            }
        }
        /// <summary>
        /// Set search to the selected column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridPackages_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lblSearch.Text = dataGridPackages.Columns[dataGridPackages.SelectedCells[0].ColumnIndex].HeaderText;
            if (lblSearch.Text.Length == 0)
            {
                txtSearch.Enabled = false;
            }
            else
            {
                txtSearch.Enabled = true;
            }
        }
        /// <summary>
        /// Set search to correct column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridPackages_Click(object sender, EventArgs e)
        {
            if (dataGridPackages.SelectedColumns.Count > 0)
            {
                lblSearch.Text = dataGridPackages.SelectedColumns[0].HeaderText;
            }
        }
        /// <summary>
        /// Set search to correct column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridPackages_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridPackages.SelectedColumns.Count > 0)
            {
                lblSearch.Text = dataGridPackages.SelectedColumns[0].HeaderText;
            }
        }
        /// <summary>
        /// Set column headers to correct text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridPackages_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridPackages.Columns["PackageId"].Visible = false;
            dataGridPackages.Columns["Package_PersonId"].Visible = false;
            dataGridPackages.Columns["PONumber"].HeaderText = "PO Number";
            dataGridPackages.Columns["PackageCarrier"].HeaderText = "Carrier";
            dataGridPackages.Columns["PackageVendor"].HeaderText = "Vendor";
            dataGridPackages.Columns["PackageDeliveredTo"].HeaderText = "Delivered To";
            dataGridPackages.Columns["PackageDeleveredBy"].HeaderText = "Delivered By";
            dataGridPackages.Columns["PackageSignedForBy"].HeaderText = "Signed For By";
            dataGridPackages.Columns["PackageTrackingNumber"].HeaderText = "Tracking Number";
            dataGridPackages.Columns["PackageReceivedDate"].HeaderText = "Received Date";
            dataGridPackages.Columns["PackageDeliveredDate"].HeaderText = "Delivered Date";
            dataGridPackages.Columns["Status"].HeaderText = "Delivery Status";
            dataGridPackages.Columns["DelivBuildingShortName"].HeaderText = "Deliver To Short Name";
        }
        #endregion

        #region Add Package
        /// <summary>
        /// Add a package to the database.
        /// </summary>
        public void AddPackageToGrid()
        {
            message = "ADD";
            AddPackage addPackage = new AddPackage(message, this);
            addPackage.ShowDialog();
        }
        #endregion

        #region Edit Package
        /// <summary>
        /// Edit a package.
        /// </summary>
        public void EditPackage()
        {
            // Set message to Edit
            message = "EDIT";

            // Create package form and set it to edit
            Package packageToBeEdited = DataConnectionClass.DataLists.Packages.FirstOrDefault(pid => pid.PackageId == Convert.ToInt64(dataGridPackages.SelectedRows[0].Cells[0].Value));
            AddPackage addPackage = new AddPackage(message, packageToBeEdited, this);
            addPackage.ShowDialog();
        }
        #endregion

        #region Delete Package
        /// <summary>
        /// Delete a package from the database.
        /// </summary>
        public void DeletePackage()
        {
            Package packageToBeRemoved = DataConnectionClass.DataLists.Packages.FirstOrDefault(pid => pid.PackageId == Convert.ToInt64(dataGridPackages.SelectedRows[0].Cells[0].Value));

            if (packageToBeRemoved.PackageReceivedDate == DateTime.Today.ToShortDateString())
            {
                DataConnectionClass.PackageConnClass.DeletePackage(packageToBeRemoved);
                DataConnectionClass.PackageConnClass.GetPackageList();
            }
            else
            {
                MessageBox.Show("You cannot delete this package.\r\nTHis package was not created in the past and cannot be deleted.", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Refreash();
        }
        #endregion

        #region Print Log
        /// <summary>
        /// Print the selected packages.
        /// </summary>
        public void Print()
        {
            CreateLogList();
            PrintPreview printPreview = new PrintPreview(logList, 1, printPackages);
            printPreview.ShowDialog();
        }
        /// <summary>
        /// Fill a log with the selected items.
        /// </summary>
        public void CreateLogList()
        {
            // Test for old list
            if (logList != null)
            {
                logList.Clear();
            }

            // Create new list object
            logList = new BindingList<Log>();

            // Fill list with logs
            for (int i = 0; i < dataGridPackages.SelectedRows.Count; i++)
            {
                // Convert packages to logs
                logList.Add(Log.ConvertPackageToLog((Package)dataGridPackages.SelectedRows[i].DataBoundItem));
                printPackages.Add((Package)dataGridPackages.SelectedRows[i].DataBoundItem);
            }
        }
        #endregion

        #region Refresh Package
        /// <summary>
        /// Refreash the data grid.
        /// </summary>
        public void Refreash()
        {
            DataConnectionClass.PackageConnClass.GetPackageList(this);
        }
        #endregion

        #region Search Package
        /// <summary>
        /// Query packages based on selected column
        /// </summary>
        public void QueryPackages(string searchTerm)
        {
            BindingSource bs = new BindingSource();
            List<Package> result = new List<Package>();
            SortableBindingList<Package> j = new SortableBindingList<Package>();
            switch (lblSearch.Text)
            {
                case "PO Number":
                    result = DataConnectionClass.DataLists.Packages.Where(a => a.PONumber.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Carrier":
                    result = DataConnectionClass.DataLists.Packages.Where(a => a.PackageCarrier.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Vendor":
                    result = DataConnectionClass.DataLists.Packages.Where(a => a.PackageVendor.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Delivered To":
                    result = DataConnectionClass.DataLists.Packages.Where(a => a.PackageDeliveredTo.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Delivered By":
                    result = DataConnectionClass.DataLists.Packages.Where(a => a.PackageDeleveredBy.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Signed For By":
                    result = DataConnectionClass.DataLists.Packages.Where(a => a.PackageSignedForBy.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "PackageTrackingNumber":
                    result = DataConnectionClass.DataLists.Packages.Where(a => a.PackageTrackingNumber.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Recieved Date":
                    result = DataConnectionClass.DataLists.Packages.Where(a => a.PackageReceivedDate.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Delivered Date":
                    result = DataConnectionClass.DataLists.Packages.Where(a => a.PackageDeliveredDate.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Delivery Status":
                    result = DataConnectionClass.DataLists.Packages.Where(a => a.Status.ToString().ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Deliver To Short Name":
                    result = DataConnectionClass.DataLists.Packages.Where(a => a.DelivBuildingShortName.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                default:
                    bs.DataSource = DataConnectionClass.DataLists.Packages;
                    break;
            }
            dataGridPackages.DataSource = bs;
        }
        #endregion

        #region Signout
        /// <summary>
        /// Alert the user on an atempt to signout.
        /// </summary>
        public void SignOut()
        {
            MessageBox.Show(DataConnectionClass.AuthenticatedUser.LastName + ", " + DataConnectionClass.AuthenticatedUser.FirstName + "\r\n" + DataConnectionClass.AuthenticatedUser.Level.Role_Title + "\r\n\r\nTo Logout exit to the Main Menu.");
        }
        #endregion
    }
}
