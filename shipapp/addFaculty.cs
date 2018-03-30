using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using shipapp.Models;
using shipapp.Models.ModelData;
using shipapp.Connections.DataConnections;

namespace shipapp
{
    /// <summary>
    /// This calss allows the user to add a faculty to the database
    /// </summary>
    public partial class AddFaculty : Form
    {
        // Class level variables
        private Faculty newFaculty;
        private string message;


        /// <summary>
        /// Public property for faculty
        /// </summary>
        internal Faculty NewFaculty
        {
            get => newFaculty;
            set => newFaculty = value;
        }


        /// <summary>
        /// Add a fauctly to the database
        /// </summary>
        public AddFaculty()
        {
            InitializeComponent();
            NewFaculty = new Faculty();
            DataConnectionClass.buildingConn.GetBuildingList();
            foreach (BuildingClass b in DataConnectionClass.DataLists.BuildingNames)
            {
                comboBox1.Items.Add(b.BuildingLongName);
            }
        }


        /// <summary>
        /// Add a fauctly to the database
        /// </summary>
        //public AddFaculty()
        //{
        //    InitializeComponent();
        //    NewFaculty = new Faculty();
        //    Connections.DataConnections.DataConnectionClass.buildingConn.GetBuildingList();
        //    Connections.DataConnections.DataConnectionClass.DataLists.BuildingNames.ForEach(b => comboBox1.Items.Add(b.BuildingLongName));
        //}


        /// <summary>
        /// When the user clicks this button it will check the data, add it to the DB, and close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Reset the background color
            ResetError();

            // Check that the appropriate data exist before writing to the DB.
            if (ValidateData())
            {
                NewFaculty.FirstName = txtFirstName.Text;
                NewFaculty.LastName = txtLastName.Text;
                NewFaculty.Faculty_PersonId = txtId2.Text;
                NewFaculty.Id = long.Parse(txtId1.Text);
                BuildingClass g = DataConnectionClass.DataLists.BuildingNames.FirstOrDefault(m => m.BuildingLongName == comboBox1.SelectedItem.ToString());
                NewFaculty.Building_Id = g.BuildingId;
                NewFaculty.Building_Name = g.BuildingShortName;
                NewFaculty.RoomNumber = textBox1.Text;
                
                // Add to DB
                DataConnectionClass.EmployeeConn.AddFaculty(NewFaculty);
                DataConnectionClass.DataLists.FacultyList.Add(NewFaculty);
                this.Close();
            }
            else
            {
                MessageBox.Show("All fields must have correct data!", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        /// <summary>
        /// Valiadate the data
        /// </summary>
        /// <returns></returns>
        private bool ValidateData()
        {
            // Method level variables
            bool pass = true;
            long num0 = 0;

            // Test Data
            if (txtFirstName.Text == "")
            {
                pass = false;
                txtFirstName.BackColor = Color.LightPink;
            }

            if (txtLastName.Text == "")
            {
                pass = false;
                txtLastName.BackColor = Color.LightPink;
            }

            if (!long.TryParse(txtId1.Text, out num0))
            {
                pass = false;
                txtId1.BackColor = Color.LightPink;
            }

            if (String.IsNullOrWhiteSpace(txtId2.Text))
            {
                pass = false;
                txtId2.BackColor = Color.LightPink;
            }

            //// TODO: add combobox selection checking
            //if (comboBox1.SelectedItem )
            //{

            //}

            return pass;
        }


        /// <summary>
        /// Reset the backcolor after errors
        /// </summary>
        private void ResetError()
        {
            txtFirstName.BackColor = Color.White;
            txtLastName.BackColor = Color.White;
            txtId1.BackColor = Color.White;
            txtId2.BackColor = Color.White;
        }
    }
}
