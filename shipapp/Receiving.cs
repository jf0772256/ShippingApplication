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

            //
            if (dataGridPackages.SelectedColumns.Count == 0)
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
            DeletePackage();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            EditPackage();
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
            PrintLog();
        }


        /// <summary>
        /// Print the selected packages
        /// </summary>
        public void PrintLog()
        {
            MessageBox.Show("Hey look, Im printing!");
        }


        /// <summary>
        /// Return a filtered list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchData();
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
        }


        public void Refreash()
        {
            DataConnectionClass.PackageConnClass.GetPackageList(this);
        }


        private void dataGridPackages_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("It worked: " + dataGridPackages.SelectedCells[0].ColumnIndex + "\r\n" + dataGridPackages.Columns[dataGridPackages.SelectedCells[0].ColumnIndex].DataPropertyName);
            lblSearch.Text = dataGridPackages.Columns[dataGridPackages.SelectedCells[0].ColumnIndex].DataPropertyName;
        }
    }
}
