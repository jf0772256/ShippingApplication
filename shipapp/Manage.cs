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

namespace shipapp
{
    public partial class Manage : Form
    {
        // Class level variables
        private int currentTable = 0;
        private DataGridViewColumnHelper dgvch = new DataGridViewColumnHelper();

        // Data list for tables
        //Use Connections.DataConnections.DataConnectionClass.DataLists.{Name of binding list}


        public Manage()
        {
            InitializeComponent();
            dataGridView1.DataError += DataGridView1_DataError;
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
            
        }

        private void btnVendors_Click(object sender, EventArgs e)
        {
            
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
            Connections.DataConnections.DataConnectionClass.EmployeeConn.GetAllAfaculty();
            dataGridView1.DataSource = Connections.DataConnections.DataConnectionClass.DataLists.FacultyList;
            //adds combo columns
            dgvch.AddCustomColumn(dataGridView1, "Phone Numbers", "phone_number", 9);
            dgvch.AddCustomColumn(dataGridView1, "E-Mail Address", "email_address", 10);
            dgvch.AddCustomColumn(dataGridView1, "Address", "address", 11);
            //add values to drop downs
            for (int i = 0; i < Connections.DataConnections.DataConnectionClass.DataLists.FacultyList.Count; i++)
            {
                DataGridViewComboBoxCell tcel = (DataGridViewComboBoxCell)dataGridView1.Rows[i].Cells["phone_number"];
                Connections.DataConnections.DataConnectionClass.DataLists.FacultyList[i].Phone.ForEach(p => tcel.Items.Add(p.Phone_Number.ToString()));

                DataGridViewComboBoxCell ucel = (DataGridViewComboBoxCell)dataGridView1.Rows[i].Cells["email_address"];
                Connections.DataConnections.DataConnectionClass.DataLists.FacultyList[i].Email.ForEach(h => ucel.Items.Add(h.Email_Address.ToString()));

                DataGridViewComboBoxCell vcel = (DataGridViewComboBoxCell)dataGridView1.Rows[i].Cells["address"];
                Connections.DataConnections.DataConnectionClass.DataLists.FacultyList[i].Address.ForEach(a => vcel.Items.Add(a.GetBuildingDetails(true)));
            }
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
                Connections.DataConnections.DataConnectionClass.UserConn.GetManyUsers();
                dataGridView1.DataSource = Connections.DataConnections.DataConnectionClass.DataLists.UsersList;
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
                Connections.DataConnections.DataConnectionClass.EmployeeConn.GetAllAfaculty();
                dataGridView1.DataSource = Connections.DataConnections.DataConnectionClass.DataLists.FacultyList;
            }
            else if (currentTable == 4)
            {
                AddBuilding addbuilding = new AddBuilding();
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
            Connections.DataConnections.DataConnectionClass.UserConn.GetManyUsers();
            dataGridView1.DataSource = Connections.DataConnections.DataConnectionClass.DataLists.UsersList;
            //change header text for roles
            dataGridView1.Columns["Level"].HeaderText = "Role";
            int i = 0;
            // sets the value of the text to role title rather than the class namespace and name
            // see tostring override in roles to see how this was hanled, may need to change based on what we do for other classes
            for (i = 0; i < Connections.DataConnections.DataConnectionClass.DataLists.UsersList.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Level"].Value = Connections.DataConnections.DataConnectionClass.DataLists.UsersList[i].Level.ToString();
            }
        }

        private void btnVendors_Click_1(object sender, EventArgs e)
        {
            currentTable = 2;
            //TODO Fill list with query from Database
            dataGridView1.DataSource = Connections.DataConnections.DataConnectionClass.DataLists.Vendors;
        }

        private void btnBuildings_Click_1(object sender, EventArgs e)
        {
            currentTable = 4;
            //TODO Fill list with query from Database
            //dataGridView1.DataSource = buildingList;
        }

        private void btnCarriers_Click_1(object sender, EventArgs e)
        {
            currentTable = 5;
            //TODO Fill list with query from Database
            dataGridView1.DataSource = Connections.DataConnections.DataConnectionClass.DataLists.CarriersList;
        }

        private void btnOther_Click_1(object sender, EventArgs e)
        {
            currentTable = 6;
        }
    }
}

