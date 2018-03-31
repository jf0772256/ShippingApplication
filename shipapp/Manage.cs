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
    /// This class will allow the program to track and update table information
    /// Note 0: THe class keeps track of which table is active by testing the variable current table
    /// </summary>
    public partial class Manage : Form
    {
        // Class level variables
        private int currentTable = 0;
        private string message = "REST";
        private DataGridViewColumnHelper dgvch = new DataGridViewColumnHelper();
        private object objectToBeEditied;


        // Data list for tables
        //Use Connections.DataConnections.DataConnectionClass.DataLists.{Name of binding list}
        private ListSortDirection[] ColumnDirection { get; set; }
        public object ObjectToBeEditied { get => objectToBeEditied; set => objectToBeEditied = value; }

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
            btnUsers_Click_1(this, e);
        }
        #region Table Buttons
        private void btnUsers_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = DataConnectionClass.DataLists.UsersList;
            dataGridView1.Update();
        }
        private void btnVendors_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            //DataConnectionClass.VendorConn.GetVendorList();
            ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            dataGridView1.DataSource = DataConnectionClass.DataLists.Vendors;
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
            //dataGridView1.DataSource = null;
            //dataGridView1.Columns.Clear();
            ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            DataConnectionClass.EmployeeConn.GetAllAfaculty(this);
            //dataGridView1.DataSource = DataConnectionClass.DataLists.FacultyList;
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
            message = "ADD";

            if (currentTable == 0)
            {
                MessageBox.Show("You must select a table before you can add an item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (currentTable == 1)
            {
                AddUser addUser = new AddUser(message);
                addUser.ShowDialog();
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
                DataConnectionClass.UserConn.GetManyUsers(this);
                //dataGridView1.DataSource = DataConnectionClass.DataLists.UsersList;
                //dgvch.AddCustomColumn(dataGridView1, "Note Count", "note_count", "", 10);
                //for (int i = 0; i < DataConnectionClass.DataLists.UsersList.Count; i++)
                //{
                //    if (DataConnectionClass.DataLists.UsersList[i].Notes is null || DataConnectionClass.DataLists.UsersList[i].Notes.Count <= 0)
                //    {
                //        dataGridView1.Rows[i].Cells["note_count"].Value = 0;
                //    }
                //    else
                //    {
                //        dataGridView1.Rows[i].Cells["note_count"].Value = DataConnectionClass.DataLists.UsersList[i].Notes.Count.ToString();
                //    }
                //}
                dataGridView1.Update();
            }
            else if (currentTable == 2)
            {
                AddVendor addVendor = new AddVendor(message);
                addVendor.ShowDialog();
                btnVendors_Click_1(this, e);
            }
            else if (currentTable == 3)
            {
                AddFaculty addFaculty = new shipapp.AddFaculty(message);
                addFaculty.ShowDialog();
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            }
            else if (currentTable == 4)
            {
                AddBuilding addbuilding = new AddBuilding(message);
                DialogResult dr = addbuilding.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    addbuilding.Dispose();
                    GC.Collect();
                    btnBuildings_Click_1(this, e);
                }
            }
            else if (currentTable == 5)
            {
                AddCarrier addCarrier = new AddCarrier();
                addCarrier.ShowDialog();
                btnCarriers_Click_1(this, e);
            }
            else if (currentTable == 6)
            {
                MessageBox.Show("This button is not set to a existing table! Please select another table.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Reset message
            message = "REST";
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
            DataConnectionClass.UserConn.GetManyUsers(this);
            //dataGridView1.DataSource = DataConnectionClass.DataLists.UsersList;
            //change header text for roles
            try
            {
                dataGridView1.Columns["Level"].HeaderText = "Role";
                //dgvch.AddCustomColumn(dataGridView1, "Note Count", "note_count", "", 10);
                int i = 0;
                // sets the value of the text to role title rather than the class namespace and name
                // see tostring override in roles to see how this was hanled, may need to change based on what we do for other classes
                for (i = 0; i < DataConnectionClass.DataLists.UsersList.Count; i++)
                {
                    dataGridView1.Rows[i].Cells["Level"].Value = DataConnectionClass.DataLists.UsersList[i].Level.ToString();
                    //if (DataConnectionClass.DataLists.UsersList[i].Notes is null || DataConnectionClass.DataLists.UsersList[i].Notes.Count <= 0)
                    //{
                    //    dataGridView1.Rows[i].Cells["note_count"].Value = 0;
                    //}
                    //else
                    //{
                    //    dataGridView1.Rows[i].Cells["note_count"].Value = DataConnectionClass.DataLists.UsersList[i].Notes.Count.ToString();
                    //}
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                dataGridView1.Update();
            }
        }


        private void btnVendors_Click_1(object sender, EventArgs e)
        {
            currentTable = 2;
            //TODO Fill list with query from Database
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            DataConnectionClass.VendorConn.GetVendorList(this);
            //dataGridView1.DataSource = DataConnectionClass.DataLists.Vendors;
        }


        private void btnBuildings_Click_1(object sender, EventArgs e)
        {
            currentTable = 4;
            DataConnectionClass.buildingConn.GetBuildingList(this);
            //TODO Fill list with query from Database
            //dataGridView1.DataSource = DataConnectionClass.DataLists.BuildingNames;
        }


        private void btnCarriers_Click_1(object sender, EventArgs e)
        {
            currentTable = 5;
            //TODO Fill list with query from Database
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            DataConnectionClass.CarrierConn.GetCarrierList(this);
            //dataGridView1.DataSource = DataConnectionClass.DataLists.CarriersList;
        }


        private void btnOther_Click_1(object sender, EventArgs e)
        {
            currentTable = 6;
        }


        /// <summary>
        /// Delete an object from the designated table
        /// </summary>
        public void DeleteEntity()
        {
            // Use the proper delete for the table
            if (currentTable == 1)
            {
                // Delete selected user
                User userToBeDeleted = DataConnectionClass.DataLists.UsersList.FirstOrDefault(uid => uid.Id == Convert.ToInt64(dataGridView1.SelectedRows[0].Cells[0].Value));
                DataConnectionClass.UserConn.DeleteUser(userToBeDeleted); 
            }
            else if (currentTable == 2)
            {
                // Delete selected vendor
                Vendors vendorToBeDeleted = DataConnectionClass.DataLists.Vendors.FirstOrDefault(vid => vid.VendorId == Convert.ToInt64(dataGridView1.SelectedRows[0].Cells[0].Value));
                DataConnectionClass.VendorConn.DeleteVendor(vendorToBeDeleted);
            }
            else if (currentTable == 3)
            {
                // Delete selected faculty
                Faculty facultyToBeDeleted = DataConnectionClass.DataLists.FacultyList.FirstOrDefault(fid => fid.Id == Convert.ToInt64(dataGridView1.SelectedRows[0].Cells[0].Value));
                DataConnectionClass.EmployeeConn.DeleteFaculty(facultyToBeDeleted);
            }
            else if (currentTable == 4)
            {
                // Delete selected building
                BuildingClass buildingToBeDeleted = DataConnectionClass.DataLists.BuildingNames.FirstOrDefault(bid => bid.BuildingId == Convert.ToInt64(dataGridView1.SelectedRows[0].Cells[0].Value));
                DataConnectionClass.buildingConn.RemoveBuilding(buildingToBeDeleted);
            }
            else if (currentTable == 5)
            {
                // Delete selected carrier
                Carrier carrierToBeDeleted = DataConnectionClass.DataLists.CarriersList.FirstOrDefault(cid => cid.CarrierId == Convert.ToInt64(dataGridView1.SelectedRows[0].Cells[0].Value));
                DataConnectionClass.CarrierConn.DeleteCarrier(carrierToBeDeleted);
            }
            else if (currentTable == 6)
            {
                // TODO: 
                MessageBox.Show("This button does not yet have a function", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else// Provide an error message if no table is selected
            {
                MessageBox.Show("You must select a table before you can delete an item.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// When this event fires, delete the currently selected entity from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pcBxDelete_Click(object sender, EventArgs e)
        {
            DeleteEntity();
        }


        /// <summary>
        /// Send a user to the edit form then edit the user
        /// </summary>
        public void EditEntity()
        {
            // Set the message to edit
            message = "EDIT";

            // Edit the correct object
            if (currentTable == 1)
            {
                // Edit user object 
                User userToBeEdited = DataConnectionClass.DataLists.UsersList.FirstOrDefault(uid => uid.Id == Convert.ToInt64(dataGridView1.SelectedRows[0].Cells[0].Value));
                AddUser addUser = new AddUser(message, userToBeEdited);
                addUser.ShowDialog();
            }
            else if (currentTable == 2)
            {
                // Edit vendor object
                Vendors vendorToBeEdited = DataConnectionClass.DataLists.Vendors.FirstOrDefault(vid => vid.VendorId == Convert.ToInt64(dataGridView1.SelectedRows[0].Cells[0].Value));
                AddVendor addVendor = new AddVendor(message, vendorToBeEdited);
                addVendor.ShowDialog();
            }
            else if (currentTable == 3)
            {
                // Edit faculty object
                Faculty facultyToBeEdited = DataConnectionClass.DataLists.FacultyList.FirstOrDefault(fid => fid.Id == Convert.ToInt64(dataGridView1.SelectedRows[0].Cells[0].Value));
                AddFaculty addFaculty = new AddFaculty(message, facultyToBeEdited);
                addFaculty.ShowDialog();
            }
            else if (currentTable == 4)
            {
                // Edit building object
                BuildingClass buildingToBeEdited = DataConnectionClass.DataLists.BuildingNames.FirstOrDefault(bid => bid.BuildingId == Convert.ToInt64(dataGridView1.SelectedRows[0].Cells[0].Value));
                AddBuilding addBuilding = new AddBuilding(message, buildingToBeEdited);
                addBuilding.ShowDialog();
            }
            else if (currentTable == 5)
            {
                // Edit carrier object
                Carrier carrierToBeEdited = DataConnectionClass.DataLists.CarriersList.FirstOrDefault(cid => cid.CarrierId == Convert.ToInt64(dataGridView1.SelectedRows[0].Cells[0].Value));
                DataConnectionClass.CarrierConn.UpdateCarrier(carrierToBeEdited);
            }
            else if (currentTable == 6)
            {
                // Edit other object
                MessageBox.Show("This table is not setup yet", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Alert user that a table must be selected
                MessageBox.Show("Please select a table", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            message = "REST";
        }


        /// <summary>
        /// When this button fires grab the correct entity and edit it to the DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pcBxEdit_Click(object sender, EventArgs e)
        {
            EditEntity();
        }
    }
}