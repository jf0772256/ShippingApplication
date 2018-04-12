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
    /// This class will alow the user to do the following:
    /// 1. Add and track freigth as it comes in.
    /// 2. Print out daily daily logs.
    /// 3. Change the status of current items.
    /// 4. Push the added/updated/deleted items to the DB.
    /// 5. Sign in and Out.
    /// </summary>
    public partial class Receiving : Form
    {
        //Class level variables
        private DataGridViewColumnHelper dgvch = new DataGridViewColumnHelper();
        private string message = "";
        private int role;
        private ListSortDirection[] ColumnDirection { get; set; }
        private BindingList<Log> logList;
        private List<Log> logs = new List<Log>();
        private List<Package> printPackages = new List<Package>();
        /// <summary>
        /// Form constructor
        /// </summary>
        public Receiving()
        {
            InitializeComponent();
        }
        /// <summary>
        /// When the form loads Center it, Set the role, and fill the grid with packages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Receiving_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            SetRole();
            GetPackages();
            lblSearch.Text = "";
            if (lblSearch.Text.Length == 0)
            {
                txtSearch.Enabled = false;
            }
            // Set form to match role
            if (role == 1)
            {
                btnAdd.Enabled = true;
                btnAdd.Show();
                pcBxEdit.Enabled = true;
                pcBxEdit.Show();
                pictureBox3.Enabled = true;
                pictureBox3.Show();
                pcBxPrint.Enabled = true;
                pcBxPrint.Show();
            }
            else if (role == 2)
            {
                btnAdd.Enabled = true;
                btnAdd.Show();
                pcBxEdit.Enabled = true;
                pcBxEdit.Show();
                pictureBox3.Enabled = true;
                pictureBox3.Show();
                pcBxPrint.Enabled = true;
                pcBxPrint.Show();
            }
            else if (role == 3)
            {
                btnAdd.Enabled = false;
                btnAdd.Hide();
                pcBxEdit.Enabled = false;
                pcBxEdit.Hide();
                pictureBox3.Enabled = false;
                pictureBox3.Hide();
                pcBxPrint.Enabled = false;
                pcBxPrint.Hide();
            }
        }
        /// <summary>
        /// When the user presses this button, open the addpackage form and add a package to the DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddPackageToGrid();
        }
        /// <summary>
        /// When the user presses the button, delete the selected row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (dataGridPackages.SelectedRows.Count > 0)
            {
                DeletePackage();
            }
        }
        private void DataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //data sort
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
        /// TODO: Add entity selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //AddNote note = new AddNote();
            //note.Show();
        }
        /// <summary>
        /// Add a pacvkage to the DB and add it to the list
        /// </summary>
        public void AddPackageToGrid()
        {
            message = "ADD";
            AddPackage addPackage = new AddPackage(message,this);
            addPackage.ShowDialog(); 
        }
        /// <summary>
        /// Fill the lsit with packages
        /// </summary>
        public void GetPackages()
        {
            DataConnectionClass.PackageConnClass.GetPackageList(this);
        }
        /// <summary>
        /// Delete a package from the database
        /// </summary>
        public void DeletePackage()
        {
            Package packageToBeRemoved = DataConnectionClass.DataLists.Packages.FirstOrDefault(pid => pid.PackageId == Convert.ToInt64(dataGridPackages.SelectedRows[0].Cells[0].Value));
            DataConnectionClass.PackageConnClass.DeletePackage(packageToBeRemoved);
            DataConnectionClass.PackageConnClass.GetPackageList();
            Refreash();
        }
        /// <summary>
        /// When the use clicks back go back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// When this event fires, send a mssege to edit not add a package
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
        /// Edit a packge
        /// </summary>
        public void EditPackage()
        {
            // Set message to Edit
            message = "EDIT";

            // Create package form and set it to edit
            Package packageToBeEdited = DataConnectionClass.DataLists.Packages.FirstOrDefault(pid => pid.PackageId == Convert.ToInt64(dataGridPackages.SelectedRows[0].Cells[0].Value));
            AddPackage addPackage = new AddPackage(message, packageToBeEdited,this);
            addPackage.ShowDialog();
        }
        /// <summary>
        /// Set role to match the user
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
        /// <summary>
        /// Alert the user on an atempt to signout 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            SignOut();
        }
        /// <summary>
        /// Alert the user on an atempt to signout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            SignOut();
        }
        /// <summary>
        /// Alert the user on an atempt to signout
        /// </summary>
        public void SignOut()
        {
            MessageBox.Show(DataConnectionClass.AuthenticatedUser.LastName + ", " + DataConnectionClass.AuthenticatedUser.FirstName + "\r\n" + DataConnectionClass.AuthenticatedUser.Level.Role_Title + "\r\n\r\nTo Logout exit to the Main Menu." );
        }
        /// <summary>
        /// Print the selected packages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pcBxPrint_Click(object sender, EventArgs e)
        {
            if (dataGridPackages.SelectedRows.Count > 0)
            {
                Print();
            }
        }
        /// <summary>
        /// Print the selected packages
        /// </summary>
        public void Print()
        {
            CreateLogList();
            PrintPreview printPreview = new PrintPreview(logList, 1, printPackages);
            printPreview.ShowDialog();
        }
        /// <summary>
        /// Return a filtered list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //SearchData();
            QueryPackages(txtSearch.Text);
        }
        /// <summary>
        /// filter data
        /// </summary>
        public void SearchData()
        {
            // If column selected and search bar not equal null or whitespace, else do nothing
            // -- Query database and return results
        }
        private void pcBXRefreash_Click(object sender, EventArgs e)
        {
            Refreash();
            MessageBox.Show("The list has refreshed");
        }
        public void Refreash()
        {
            DataConnectionClass.PackageConnClass.GetPackageList(this);
        }
        private void dataGridPackages_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("It worked: " + dataGridPackages.SelectedCells[0].ColumnIndex + "\r\n" + dataGridPackages.Columns[dataGridPackages.SelectedCells[0].ColumnIndex].DataPropertyName);
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
        /// Set the clerk to the packages that 
        /// </summary>
        public void SetClerk()
        {

        }
        /// <summary>
        /// Fill a list with the selected items
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
        /// <summary>
        /// Query the databse when key pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            //QueryPackages(txtSearch.Text);
        }
        /// <summary>
        /// Query packages
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
        private void dataGridPackages_Click(object sender, EventArgs e)
        {
            if (dataGridPackages.SelectedColumns.Count > 0)
            {
                lblSearch.Text = dataGridPackages.SelectedColumns[0].HeaderText;
            }
        }
        private void dataGridPackages_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridPackages.SelectedColumns.Count > 0)
            {
                lblSearch.Text = dataGridPackages.SelectedColumns[0].HeaderText;
            }
        }
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
    }
}
