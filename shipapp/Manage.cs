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
        private int role;
        private DataGridViewColumnHelper dgvch = new DataGridViewColumnHelper();
        private object objectToBeEditied;
        private BindingList<Faculty> faculties;
        private BindingList<Vendors> vendors;
        private BindingList<Carrier> carriers;
        private BindingList<BuildingClass> buildings;
        private BindingList<User> users;
        private char c = '\u2022';


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
            SetRole();

            //
            if (role == 1)
            {
                // Do nothing
                pcBxDelete.Enabled = true;
                pcBxEdit.Enabled = true;
                pictureBox1.Enabled = true;
                pcBxPrint.Enabled = true;

                pcBxDelete.BackColor = Color.Transparent;
                pcBxEdit.BackColor = Color.Transparent;
                pictureBox1.BackColor = Color.Transparent;
                pcBxPrint.Show();

                btnVendors.Enabled = true;
                btnFaculty.Show();
                btnFaculty.Enabled = true;
                btnBuildings.Show();
                btnBuildings.Enabled = true;
                btnCarriers.Show();
                btnCarriers.Enabled = true;
                btnOther.Enabled = true;
                btnOther.Show();
            }
            if (role == 2)
            {
                // Do nothing
                pcBxDelete.Enabled = false;
                pcBxEdit.Enabled = true;
                pictureBox1.Enabled = true;
                pcBxPrint.Enabled = true;

                pcBxDelete.BackColor = Color.Transparent;
                pcBxEdit.BackColor = Color.Transparent;
                pictureBox1.BackColor = Color.Transparent;
                pcBxPrint.Show();

                btnVendors.Enabled = true;
                btnFaculty.Show();
                btnFaculty.Enabled = true;
                btnBuildings.Show();
                btnBuildings.Enabled = true;
                btnCarriers.Show();
                btnCarriers.Enabled = true;
                btnOther.Enabled = true;
                btnOther.Show();
            }
            else if (role == 3)
            {

                pcBxDelete.Enabled = false;
                pcBxEdit.Enabled = false;
                pictureBox1.Enabled = false;
                pcBxPrint.Enabled = false;

                pcBxDelete.BackColor = Color.LightPink;
                pcBxEdit.BackColor = Color.LightPink;
                pictureBox1.BackColor = Color.LightPink;
                pcBxPrint.Hide();

                btnVendors.Enabled = true;
                btnFaculty.Show();
                btnFaculty.Enabled = true;
                btnBuildings.Show();
                btnBuildings.Enabled = true;
                btnCarriers.Show();
                btnCarriers.Enabled = true;
                btnOther.Enabled = true;
                btnOther.Show();
                // TODO: Restrict view of password
            }
            else if ( role == 0)
            {
                btnUsers.Enabled = true;
                btnUsers.BackColor = Color.White;
                btnVendors.Hide();
                btnVendors.Enabled = false;
                btnFaculty.Hide();
                btnFaculty.Enabled = false;
                btnBuildings.Hide();
                btnBuildings.Enabled = false;
                btnCarriers.Hide();
                btnCarriers.Enabled = false;
                btnOther.Enabled = false;
                btnOther.Hide();
            }
        }
        #region Table Buttons
        private void btnUsers_Click(object sender, EventArgs e)
        {
            ClearBackColor();
            btnUsers.BackColor = SystemColors.ButtonHighlight;
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = DataConnectionClass.DataLists.UsersList;
            dataGridView1.Update();
            if (role == 2)
            {
                pictureBox1.Enabled = false;
            }
        }
        private void btnVendors_Click(object sender, EventArgs e)
        {
            ClearBackColor();
            btnVendors.BackColor = SystemColors.ButtonHighlight;
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = DataConnectionClass.DataLists.Vendors;
        }
        /// <summary>
        /// Faculty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            ClearBackColor();
            btnFaculty.BackColor = SystemColors.ButtonHighlight;
            currentTable = 3;
            ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            DataConnectionClass.EmployeeConn.GetAllAfaculty(this);
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
                AddEntity(e);
        }
        #endregion


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void btnUsers_Click_1(object sender, EventArgs e)
        {
            ClearBackColor();
            btnUsers.BackColor = SystemColors.ButtonHighlight;
            currentTable = 1;
            //TODO Fill list with query from Database
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            DataConnectionClass.UserConn.GetManyUsers(this);

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
            ClearBackColor();
            btnVendors.BackColor = SystemColors.ButtonHighlight;
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
            ClearBackColor();
            btnBuildings.BackColor = SystemColors.ButtonHighlight;
            currentTable = 4;
            DataConnectionClass.buildingConn.GetBuildingList(this);
        }


        private void btnCarriers_Click_1(object sender, EventArgs e)
        {
            ClearBackColor();
            btnCarriers.BackColor = SystemColors.ButtonHighlight;
            currentTable = 5;
            //TODO Fill list with query from Database
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            ColumnDirection = new ListSortDirection[] { ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending, ListSortDirection.Descending };
            DataConnectionClass.CarrierConn.GetCarrierList(this);
        }


        private void btnOther_Click_1(object sender, EventArgs e)
        {
            ClearBackColor();
            btnOther.BackColor = SystemColors.ButtonHighlight;
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
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DeleteEntity();
            }
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
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    User userToBeEdited = DataConnectionClass.DataLists.UsersList.FirstOrDefault(uid => uid.Id == Convert.ToInt64(dataGridView1.SelectedRows[0].Cells[0].Value));
                    AddUser addUser = new AddUser(message, userToBeEdited);
                    addUser.ShowDialog();
                    DataConnectionClass.UserConn.GetManyUsers(this);
                }
                else
                {
                    MessageBox.Show("Sorry, you must select the row to proceed.","Row to modify is null");
                }
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
                AddCarrier addCarrier = new AddCarrier(message, carrierToBeEdited);
                addCarrier.ShowDialog();
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
            if (dataGridView1.SelectedRows.Count > 0)
            {
                EditEntity();
            }
        }


        public void SetRole()
        {
            if (DataConnectionClass.AuthenticatedUser.Level.Role_Title == "Administrator")
            {
                role = 1;
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


        private void pictureBox8_Click(object sender, EventArgs e)
        {
            SignOut();
        }


        private void label1_Click(object sender, EventArgs e)
        {
            SignOut();
        }


        public void SignOut()
        {
            MessageBox.Show(DataConnectionClass.AuthenticatedUser.LastName + ", " + DataConnectionClass.AuthenticatedUser.FirstName + "\r\n" + DataConnectionClass.AuthenticatedUser.Level.Role_Title + "\r\n\r\nTo Logout exit to the Main Menu.");
        }


        
        /// <summary>
        /// Print the selected report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pcBxPrint_Click(object sender, EventArgs e)
        {
            PrintReport();
        }


        /// <summary>
        /// Print the selected report
        /// </summary>
        public void PrintReport()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Print();
            }
        }


        /// <summary>
        /// Display a list of objects that match the text in the search bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchData();
        }


        public void SearchData()
        {
            // Determine Table
            // -- If column selected and search bar not equal null or whitespace, else do nothing
            // -- -- Query database and return results
        }


        private void dataGridView1_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("It worked: " + dataGridView1.SelectedCells[0].ColumnIndex + "\r\n" + dataGridView1.Columns[dataGridView1.SelectedCells[0].ColumnIndex].DataPropertyName);
            lblSearch.Text = dataGridView1.Columns[dataGridView1.SelectedCells[0].ColumnIndex].DataPropertyName;
        }


        public void Print()
        {
            if (currentTable == 1)
            {
                CreateLogList();
                PrintPreview printPreview = new PrintPreview(users, currentTable + 2, null);
                printPreview.ShowDialog();
            }
            else if (currentTable == 2)
            {
                CreateLogList();
                PrintPreview printPreview = new PrintPreview(vendors, currentTable + 2, null);
                printPreview.ShowDialog();
            }
            else if (currentTable == 3)
            {
                CreateLogList();
                PrintPreview printPreview = new PrintPreview(faculties, currentTable + 2, null);
                printPreview.ShowDialog();
            }
            else if (currentTable == 4)
            {
                CreateLogList();
                PrintPreview printPreview = new PrintPreview(buildings, currentTable + 2, null);
                printPreview.ShowDialog();
            }
            else if (currentTable == 5)
            {
                CreateLogList();
                PrintPreview printPreview = new PrintPreview(carriers, currentTable + 2, null);
                printPreview.ShowDialog();
            }
            else if (currentTable == 6)
            {
                //// TODO History
                //PrintPreview printPreview = new PrintPreview();
                //printPreview.ShowDialog();
            }
            else
            {
                MessageBox.Show("It seems something went wrong.\r\nPlease try again.", "Uh-oh!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        /// <summary>
        /// Fill a list with the selected items
        /// </summary>
        public void CreateLogList()
        {
            // Create new list object
            if (currentTable == 1)
            {
                users = new BindingList<User>();

                // Fill list with logs
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    // Convert packages to logs
                    users.Add((User)dataGridView1.SelectedRows[i].DataBoundItem);
                }
            }
            else if (currentTable == 2)
            {
                vendors = new BindingList<Vendors>();

                // Fill list with logs
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    // Convert packages to logs
                    vendors.Add((Vendors)dataGridView1.SelectedRows[i].DataBoundItem);
                }
            }
            else if (currentTable == 3)
            {
                faculties = new BindingList<Faculty>();

                // Fill list with logs
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    // Convert packages to logs
                    faculties.Add((Faculty)dataGridView1.SelectedRows[i].DataBoundItem);
                }
            }
            else if (currentTable == 4)
            {
                buildings = new BindingList<BuildingClass>();

                // Fill list with logs
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    // Convert packages to logs
                    buildings.Add((BuildingClass)dataGridView1.SelectedRows[i].DataBoundItem);
                }
            }
            else if (currentTable == 5)
            {
                carriers = new BindingList<Carrier>();

                // Fill list with logs
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    // Convert packages to logs
                    carriers.Add((Carrier)dataGridView1.SelectedRows[i].DataBoundItem);
                }
            }
            else if (currentTable == 6)
            {
                //// TODO History
                //list = new BindingList<Log>();

                // Fill list with logs
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    // Convert packages to logs
                    users.Add((User)dataGridView1.SelectedRows[i].DataBoundItem);
                }
            }
            else
            {
                MessageBox.Show("It seems something went wrong.\r\nTry again", "Uh-oh!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void AddEntity(EventArgs e)
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
                DataConnectionClass.UserConn.GetManyUsers(this);
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
                AddFaculty addFaculty = new AddFaculty(message);
                addFaculty.ShowDialog();
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
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
                AddCarrier addCarrier = new AddCarrier(message);
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


        public string ValueToPassWord(String password)
        {
            
            for (int i = 0; i < password.Length; i++)
            {
                 password.ToCharArray()[i] = c;
            }

            return password;
        }


        public void ClearBackColor()
        {
            btnUsers.BackColor = SystemColors.ButtonFace;
            btnVendors.BackColor = SystemColors.ButtonFace;
            btnFaculty.BackColor = SystemColors.ButtonFace;
            btnCarriers.BackColor = SystemColors.ButtonFace;
            btnBuildings.BackColor = SystemColors.ButtonFace;
            btnOther.BackColor = SystemColors.ButtonFace;
        }
    }
}

