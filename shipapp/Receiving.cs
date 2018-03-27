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
        private ListSortDirection[] ColumnDirection { get; set; }


        public Receiving()
        {
            InitializeComponent();
        }


        private void Receiving_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            GetPackages();
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
            AddNote note = new AddNote();
            note.Show();
        }


        public void AddPackageToGrid()
        {
            ////TODO Fill list with query from Database
            //dataGridPackages.DataSource = null;
            //dataGridPackages.Columns.Clear(); ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            //DataConnectionClass.EmployeeConn.GetAllAfaculty();
            //dataGridPackages.DataSource = DataConnectionClass.DataLists.FacultyList;
            ////adds combo columns
            //dgvch.AddCustomColumn(dataGridPackages, "Phone Numbers", "phone_number", 9);
            //dgvch.AddCustomColumn(dataGridPackages, "E-Mail Address", "email_address", 10);
            //dgvch.AddCustomColumn(dataGridPackages, "Address", "address", 11);
            ////add values to drop downs
            //for (int i = 0; i < DataConnectionClass.DataLists.FacultyList.Count; i++)
            //{
            //    DataGridViewComboBoxCell tcel = (DataGridViewComboBoxCell)dataGridPackages.Rows[i].Cells["phone_number"];
            //    DataConnectionClass.DataLists.FacultyList[i].Phone.ForEach(p => tcel.Items.Add(p.Phone_Number.ToString()));

            //    DataGridViewComboBoxCell ucel = (DataGridViewComboBoxCell)dataGridPackages.Rows[i].Cells["email_address"];
            //    DataConnectionClass.DataLists.FacultyList[i].Email.ForEach(h => ucel.Items.Add(h.Email_Address.ToString()));

            //    DataGridViewComboBoxCell vcel = (DataGridViewComboBoxCell)dataGridPackages.Rows[i].Cells["address"];
            //    DataConnectionClass.DataLists.FacultyList[i].Address.ForEach(a => vcel.Items.Add(a.GetBuildingDetails(true)));
            //}
            message = "ADD";
            AddPackage addPackage = new AddPackage();
            addPackage.ShowDialog();
                
        }


        public void GetPackages()
        {
            dataGridPackages.DataSource = DataConnectionClass.DataLists.Packages;
        }


        /// <summary>
        /// Delete a package from the database
        /// </summary>
        public void DeletePackage()
        {
            Package packageToBeRemoved = DataConnectionClass.DataLists.Packages.FirstOrDefault(pid => pid.PackageId == Convert.ToInt64(dataGridPackages.SelectedRows[0].Cells[0].Value));
            DataConnectionClass.PackageConnClass.DeletePackage(packageToBeRemoved);
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

        }


        public void EditPackage()
        {
            message = "EDIT";
        }
    }
}
