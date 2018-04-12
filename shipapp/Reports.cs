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
    /// This form will attomaticly load with all past packages aand allow the
    /// user to add past package to the daily receiving,
    /// </summary>
    public partial class Reports : Form
    {
        // Class level variabels
        private DataGridViewColumnHelper dgvch = new DataGridViewColumnHelper();
        private int role;
        private ListSortDirection[] ColumnDirection { get; set; }

        /// <summary>
        /// Main constructor
        /// </summary>
        public Reports()
        {
            InitializeComponent();
        }
        /// <summary>
        /// When the back button is clicked, close this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Upon form load, do this
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reports_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            GetPackages();
            SetRole();
            //BindingSource bs = new BindingSource
            //{
            //    DataSource = DataConnectionClass.DataLists.PackageHistory
            //};
            //datGridHistory.DataSource = bs;
            // Set form according to the role
            if (role == 1)
            {
                pcBxAddToDaily.Enabled = true;
                pcBxAddToDaily.Show();
            }
            else if (role == 2)
            {
                pcBxAddToDaily.Enabled = true;
                pcBxAddToDaily.Show();
            }
            else if (role == 3)
            {
                pcBxAddToDaily.Enabled = false;
                pcBxAddToDaily.Hide();
            }
            else
            {
                pcBxAddToDaily.Enabled = false;
                pcBxAddToDaily.Hide();
            }
            lblSearch.Text = "";
            txtSearch.Enabled = false;
        }
        /// <summary>
        /// Alert user if an atempt to logout occurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            SignOut();
        }
        /// <summary>
        /// Alert user if an atempt to logout occurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            SignOut();
        }
        /// <summary>
        /// If the user attempts to log out. inform them to go back to the main menu
        /// </summary>
        public void SignOut()
        {
            MessageBox.Show(DataConnectionClass.AuthenticatedUser.LastName + ", " + DataConnectionClass.AuthenticatedUser.FirstName + "\r\n" + DataConnectionClass.AuthenticatedUser.Level.Role_Title + "\r\n\r\nTo Logout exit to the Main Menu.");
        }
        /// <summary>
        /// When the user clicks the button, add all selected packages to the daily receiving list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pcBxAddToDaily_Click(object sender, EventArgs e)
        {
            if (datGridHistory.SelectedRows.Count > 0)
            {
                AddPackageToDaily();
            }
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
        /// Add selected packages to daily receiving
        /// </summary>
        public void AddPackageToDaily()
        {
            //
            string message = "Success!\r\n";
            int count = 0;

            for (int i = 0; i < datGridHistory.SelectedRows.Count; i++)
            {
                DataConnectionClass.PackageConnClass.UpdateLastModified((Package)datGridHistory.SelectedRows[i].DataBoundItem);
                count++;
            }

            //
            DataConnectionClass.PackageConnClass.GetPackageHistoryList(this);

            //
            MessageBox.Show(message + count +" have been added to todays receiving list.");
        }
        /// <summary>
        /// Fill the lsit with packages
        /// </summary>
        public void GetPackages()
        {
            DataConnectionClass.PackageConnClass.GetPackageHistoryList(this);
        }
        /// <summary>
        /// Refreash list
        /// </summary>
        public void Refreash()
        {
            DataConnectionClass.PackageConnClass.GetPackageHistoryList(this);
        }
        /// <summary>
        /// Refreash the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pcBxRefreash_Click(object sender, EventArgs e)
        {
            Refreash();
            MessageBox.Show("The list has refreshed");
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
                    result = DataConnectionClass.DataLists.PackageHistory.Where(a => a.PONumber.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Carrier":
                    result = DataConnectionClass.DataLists.PackageHistory.Where(a => a.PackageCarrier.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Vendor":
                    result = DataConnectionClass.DataLists.PackageHistory.Where(a => a.PackageVendor.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Delivered To":
                    result = DataConnectionClass.DataLists.PackageHistory.Where(a => a.PackageDeliveredTo.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Delivered By":
                    result = DataConnectionClass.DataLists.PackageHistory.Where(a => a.PackageDeleveredBy.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Signed For By":
                    result = DataConnectionClass.DataLists.PackageHistory.Where(a => a.PackageSignedForBy.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Tracking Number":
                    result = DataConnectionClass.DataLists.PackageHistory.Where(a => a.PackageTrackingNumber.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Recieved Date":
                    result = DataConnectionClass.DataLists.PackageHistory.Where(a => a.PackageReceivedDate.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Delivered Date":
                    result = DataConnectionClass.DataLists.PackageHistory.Where(a => a.PackageDeliveredDate.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Delivery Status":
                    result = DataConnectionClass.DataLists.PackageHistory.Where(a => a.Status.ToString().ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                case "Deliver To Short Name":
                    result = DataConnectionClass.DataLists.PackageHistory.Where(a => a.DelivBuildingShortName.ToLower().IndexOf(searchTerm.ToLower()) >= 0).ToList();
                    result.ForEach(i => j.Add(i));
                    bs.DataSource = j;
                    break;
                default:
                    bs.DataSource = DataConnectionClass.DataLists.PackageHistory;
                    break;
            }
            datGridHistory.DataSource = bs;
        }
        private void datGridHistory_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            lblSearch.Text = datGridHistory.Columns[datGridHistory.SelectedCells[0].ColumnIndex].HeaderText;
            if (lblSearch.Text.Length == 0)
            {
                txtSearch.Enabled = false;
            }
            else
            {
                txtSearch.Enabled = true;
            }
        }
        private void datGridHistory_Click(object sender, EventArgs e)
        {
            //lblSearch.Text = datGridHistory.Columns[datGridHistory.SelectedCells[0].ColumnIndex].HeaderCell.Value.ToString();
            //if (lblSearch.Text.Length == 0)
            //{
            //    txtSearch.Enabled = false;
            //}
            //else
            //{
            //    txtSearch.Enabled = true;
            //}
        }
        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            QueryPackages(txtSearch.Text);
        }
        private void pcBxPrint_Click(object sender, EventArgs e)
        {
            Print();
        }
        /// <summary>
        /// Print the selected packages
        /// </summary>
        public void Print()
        {
            PrintPreview printPreview = new PrintPreview(CreateListFromSelectedRows(), 2, null);
            printPreview.ShowDialog();
        }
        public Object CreateListFromSelectedRows()
        {
            BindingList<Package> packages = new BindingList<Package>();
            
            for (int i = 0; i < datGridHistory.SelectedRows.Count; i++)
            {
                packages.Add((Package)datGridHistory.SelectedRows[i].DataBoundItem);
            }

            return packages;
        }
        /// <summary>
        /// for consistancy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datGridHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                Rectangle bnds = datGridHistory.Rows[e.RowIndex].Cells[e.ColumnIndex].ContentBounds;
                int x = bnds.Width / 2;
                int y = bnds.Height / 2;
                DataGridViewCellMouseEventArgs m = new DataGridViewCellMouseEventArgs(e.ColumnIndex, e.RowIndex, x, y, new MouseEventArgs(MouseButtons.Left, 1, x, y, 0));
                datGridHistory_CellMouseClick(this, m);
            }
        }
        /// <summary>
        /// things to do once the binding of the lists has been completed -- rename headers here hide columns here
        /// </summary>
        private void datGridHistory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            datGridHistory.Columns["PackageId"].Visible = false;
            datGridHistory.Columns["Package_PersonId"].Visible = false;
            datGridHistory.Columns["PONumber"].HeaderText = "PO Number";
            datGridHistory.Columns["PackageCarrier"].HeaderText = "Carrier";
            datGridHistory.Columns["PackageVendor"].HeaderText = "Vendor";
            datGridHistory.Columns["PackageDeliveredTo"].HeaderText = "Delivered To";
            datGridHistory.Columns["PackageDeleveredBy"].HeaderText = "Delivered By";
            datGridHistory.Columns["PackageSignedForBy"].HeaderText = "Signed For By";
            datGridHistory.Columns["PackageTrackingNumber"].HeaderText = "Tracking Number";
            datGridHistory.Columns["PackageReceivedDate"].HeaderText = "Received Date";
            datGridHistory.Columns["PackageDeliveredDate"].HeaderText = "Delivered Date";
            datGridHistory.Columns["Status"].HeaderText = "Delivery Status";
            datGridHistory.Columns["DelivBuildingShortName"].HeaderText = "Deliver To Short Name";
        }
    }
}
