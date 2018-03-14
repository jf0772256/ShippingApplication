using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shipapp
{
    public partial class Manage : Form
    {
        // Class level variables
        private int currentTable = 0;

        // Data list for tables
        private BindingList<Models.User> userList = new BindingList<Models.User>();
        private BindingList<Models.Vendors> vendorList = new BindingList<Models.Vendors>();
        private BindingList<Models.Faculty> facultyList = new BindingList<Models.Faculty>();
        //private BindingList<Models.b> buildingList = new BindingList<Models.User>(); -- Building list
        private BindingList<Models.Carrier> carrierList = new BindingList<Models.Carrier>();


        public Manage()
        {
            InitializeComponent();
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

            //var list = new BindingList<Models.Faculty> { };
            //list.Add(new Models.Faculty(0,"0","kalin", "bowden"));
            //list.Add(new Models.Faculty(0, "1", "jesse", "fender"));
            //list.Add(new Models.Faculty(0, "2", "tiffany", "ford"));

            //dataGridView1.DataSource = list;



            //list.Add(addFaculty.NewFaculty);
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
        }

        private void btnVendors_Click_1(object sender, EventArgs e)
        {
            currentTable = 2;
            //TODO Fill list with query from Database
            dataGridView1.DataSource = vendorList;
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
            dataGridView1.DataSource = carrierList;
        }

        private void btnOther_Click_1(object sender, EventArgs e)
        {
            currentTable = 6;
        }
    }
}

