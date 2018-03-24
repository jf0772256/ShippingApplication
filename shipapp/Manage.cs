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
    public partial class Manage : Form
    {
        // Class level variables
        private int currentTable = 0;
        private DataGridViewColumnHelper dgvch = new DataGridViewColumnHelper();

        // Data list for tables
        //Use Connections.DataConnections.DataConnectionClass.DataLists.{Name of binding list}

        private ListSortDirection[] ColumnDirection { get; set; }

        public Manage()
        {
            InitializeComponent();
            dataGridView1.DataError += DataGridView1_DataError;
            dataGridView1.ColumnHeaderMouseClick += DataGridView1_ColumnHeaderMouseClick;

        }

        private void DataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //data sort
                if (ColumnDirection.Length > 0 && ColumnDirection[e.ColumnIndex] == ListSortDirection.Descending)
                {
                    dataGridView1.Sort(dataGridView1.Columns[e.ColumnIndex], ListSortDirection.Ascending);
                    ColumnDirection[e.ColumnIndex] = ListSortDirection.Ascending;
                }
                else if (ColumnDirection.Length > 0 && ColumnDirection[e.ColumnIndex] == ListSortDirection.Ascending)
                {
                    dataGridView1.Sort(dataGridView1.Columns[e.ColumnIndex], ListSortDirection.Descending);
                    ColumnDirection[e.ColumnIndex] = ListSortDirection.Descending;
                }
                // reset column values lost during sort
                if (currentTable == 1)
                {
                    dataGridView1.Columns["Level"].HeaderText = "Role";
                    for (int i = 0; i < DataConnectionClass.DataLists.UsersList.Count; i++)
                    {
                        long a = Convert.ToInt64(dataGridView1.Rows[i].Cells[0].Value);
                        User res = DataConnectionClass.DataLists.UsersList.FirstOrDefault(m => m.Id == a);
                        dataGridView1.Rows[i].Cells["note_count"].Value = res.Notes.Count.ToString();
                        dataGridView1.Rows[i].Cells["Level"].Value = res.Level.ToString();
                    }
                }
                else if (currentTable == 2)
                {

                }
                else if (currentTable == 3)
                {
                    for (int i = 0; i < DataConnectionClass.DataLists.FacultyList.Count; i++)
                    {
                        long a = Convert.ToInt64(dataGridView1.Rows[i].Cells[0].Value);
                        Faculty res = DataConnectionClass.DataLists.FacultyList.FirstOrDefault(m => m.Id == a);
                    }
                }
                else if (currentTable == 4)
                {

                }
                else if (currentTable == 5)
                {

                }
                else if (currentTable == 6)
                {

                }
                else
                {
                    throw new ArgumentOutOfRangeException("Current table value is out of range");
                }
            }
            catch (Exception)
            {
                //do nothing but quietly handle error
            }
        }

        /// <summary>
        /// used to hide data conversion errors even though they are resolved through the getStrings and toStrings methods
        /// </summary>
        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //
        }

        /// <summary>
        /// Close the Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Manage_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }


        #region Table Buttons
        private void btnUsers_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
        }

        private void btnVendors_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            Connections.DataConnections.DataConnectionClass.VendorConn.GetVendorList();
            ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            dataGridView1.DataSource = Connections.DataConnections.DataConnectionClass.DataLists.Vendors;
        }

        /// <summary>
        /// Faculty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            currentTable = 3;
            //TODO Fill list with query from Database
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear(); ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            DataConnectionClass.EmployeeConn.GetAllAfaculty();
            dataGridView1.DataSource = DataConnectionClass.DataLists.FacultyList;
        }
        private void btnBuildings_Click(object sender, EventArgs e)
        {
            
        }
        private void btnCarriers_Click(object sender, EventArgs e)
        {
            
        }
        private void btnOther_Click(object sender, EventArgs e)
        {
            
        }
        #endregion // When the user clicks one of these button they will assign the active table and fiil the grid with data.


        #region Grid Buttons

        /// <summary>
        /// Allow the application to know what table to add and
        /// bring the appropriate form to the front.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (currentTable == 0)
            {
                MessageBox.Show("You must select a table before you can add an item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (currentTable == 1)
            {
                AddUser addUser = new AddUser();
                addUser.ShowDialog();
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
                DataConnectionClass.UserConn.GetManyUsers();
                dataGridView1.DataSource = DataConnectionClass.DataLists.UsersList;
                dgvch.AddCustomColumn(dataGridView1, "Note Count", "note_count", "", 10);
                for (int i = 0; i < DataConnectionClass.DataLists.UsersList.Count; i++)
                {
                    dataGridView1.Rows[i].Cells["note_count"].Value = DataConnectionClass.DataLists.UsersList[i].Notes.Count.ToString();
                }
            }
            else if (currentTable == 2)
            {
                AddVendor addVendor = new AddVendor();
                addVendor.ShowDialog();
            }
            else if (currentTable == 3)
            {
                AddFaculty addFaculty = new AddFaculty();
                addFaculty.ShowDialog();
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
                DataConnectionClass.EmployeeConn.GetAllAfaculty();
                dataGridView1.DataSource = DataConnectionClass.DataLists.FacultyList;
            }
            else if (currentTable == 4)
            {
                AddBuilding addbuilding = new AddBuilding();
                DialogResult dr = addbuilding.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    addbuilding.Dispose();
                    GC.Collect();
                }
            }
            else if (currentTable == 5)
            {
                AddCarrier addCarrier = new AddCarrier();
            }
            else if (currentTable == 6)
            {
                MessageBox.Show("This button is not set to a existing table! Please select another table.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnUsers_Click_1(object sender, EventArgs e)
        {
            currentTable = 1;
            //TODO Fill list with query from Database
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            DataConnectionClass.UserConn.GetManyUsers();
            dataGridView1.DataSource = DataConnectionClass.DataLists.UsersList;
            //change header text for roles
            dataGridView1.Columns["Level"].HeaderText = "Role";
            dgvch.AddCustomColumn(dataGridView1, "Note Count", "note_count", "", 10);
            int i = 0;
            // sets the value of the text to role title rather than the class namespace and name
            // see tostring override in roles to see how this was hanled, may need to change based on what we do for other classes
            for (i = 0; i < DataConnectionClass.DataLists.UsersList.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Level"].Value = DataConnectionClass.DataLists.UsersList[i].Level.ToString();
                dataGridView1.Rows[i].Cells["note_count"].Value = DataConnectionClass.DataLists.UsersList[i].Notes.Count.ToString();
            }
        }

        private void btnVendors_Click_1(object sender, EventArgs e)
        {
            currentTable = 2;
            //TODO Fill list with query from Database
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            DataConnectionClass.VendorConn.GetVendorList();
            dataGridView1.DataSource = DataConnectionClass.DataLists.Vendors;
            dgvch.AddCustomColumn(dataGridView1, "Note Count", "note_count", "", 9);
            for (int i = 0; i < DataConnectionClass.DataLists.Vendors.Count; i++)
            {
                dataGridView1.Rows[i].Cells["note_count"].Value = DataConnectionClass.DataLists.Vendors[i].Notes.Count.ToString();
            }
        }

        private void btnBuildings_Click_1(object sender, EventArgs e)
        {
            currentTable = 4;
            DataConnectionClass.buildingConn.GetBuildingList();
            //TODO Fill list with query from Database
            dataGridView1.DataSource = DataConnectionClass.DataLists.BuildingNames;
        }

        private void btnCarriers_Click_1(object sender, EventArgs e)
        {
            currentTable = 5;
            //TODO Fill list with query from Database
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            DataConnectionClass.CarrierConn.GetCarrierList();
            dataGridView1.DataSource = DataConnectionClass.DataLists.CarriersList;
            dgvch.AddCustomColumn(dataGridView1, "Note Count", "note_count", "", 9);
            for (int i = 0; i < DataConnectionClass.DataLists.CarriersList.Count; i++)
            {
                dataGridView1.Rows[i].Cells["note_count"].Value = DataConnectionClass.DataLists.CarriersList[i].Notes.Count.ToString();
            }
        }

        private void btnOther_Click_1(object sender, EventArgs e)
        {
            currentTable = 6;
        }
    }
}

