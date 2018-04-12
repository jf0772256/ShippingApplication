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
using Extentions;

namespace shipapp
{
    /// <summary>
    /// This class allows the user to add a faculty to the database
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
        public AddFaculty(string message)
        {
            InitializeComponent();
            NewFaculty = new Faculty();
            this.message = message;
        }
        /// <summary>
        /// Add a fauctly to the database
        /// </summary>
        public AddFaculty(string message, Object facultyToBeEdited)
        {
            InitializeComponent();
            NewFaculty = (Faculty)facultyToBeEdited;
            this.message = message;

            if (message == "EDIT")
            {
                txtFirstName.Text = newFaculty.FirstName;
                txtLastName.Text = newFaculty.LastName;
                txtId2.Text = newFaculty.Faculty_PersonId;
                //comboBox1.SelectedIndex = (int)newFaculty.Building_Id;
            }
        }
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
            if (ValidateData() && message == "ADD")
            {
                AddFacultyToDb();
            }
            else if (ValidateData() && message == "EDIT")
            {
                EditFaculty();
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
            if (String.IsNullOrWhiteSpace(txtId2.Text))
            {
                pass = false;
                txtId2.BackColor = Color.LightPink;
            }
            return pass;
        }
        /// <summary>
        /// Reset the backcolor after errors
        /// </summary>
        private void ResetError()
        {
            txtFirstName.BackColor = Color.White;
            txtLastName.BackColor = Color.White;
            txtId2.BackColor = Color.White;
        }
        private void AddFaculty_Load(object sender, EventArgs e)
        {
            DataConnectionClass.buildingConn.GetBuildingList(this);
        }
        public void AddFacultyToDb()
        {
            NewFaculty.FirstName = txtFirstName.Text;
            NewFaculty.LastName = txtLastName.Text;
            NewFaculty.Faculty_PersonId = txtId2.Text;
            BuildingClass g = DataConnectionClass.DataLists.BuildingNames.FirstOrDefault(m => m.BuildingLongName == comboBox1.SelectedItem.ToString());
            NewFaculty.Building_Id = g.BuildingId;
            NewFaculty.Building_Name = g.BuildingShortName;
            NewFaculty.RoomNumber = txtRoomNumber.Text;

            // Add to DB
            DataConnectionClass.EmployeeConn.AddFaculty(NewFaculty);
            DataConnectionClass.DataLists.FacultyList.Add(NewFaculty);
            this.Close();
        }
        public void EditFaculty()
        {
            NewFaculty.FirstName = txtFirstName.Text;
            NewFaculty.LastName = txtLastName.Text;
            NewFaculty.Faculty_PersonId = txtId2.Text;
            BuildingClass g = DataConnectionClass.DataLists.BuildingNames.FirstOrDefault(m => m.BuildingLongName == comboBox1.SelectedItem.ToString());
            NewFaculty.Building_Id = g.BuildingId;
            NewFaculty.Building_Name = g.BuildingShortName;
            NewFaculty.RoomNumber = txtRoomNumber.Text;

            // Add to DB
            DataConnectionClass.EmployeeConn.UpdateFaculty(NewFaculty);
            DataConnectionClass.DataLists.FacultyList.Add(NewFaculty);
            this.Close();
        }
        private void txtFirstName_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtFirstName.Text) && message != "EDIT")
            {
                if (txtFirstName.Text.Length < 4)
                {
                    DataConnectionClass.CreatePersonId(txtFirstName.Text.ToLower().Substring(0, txtFirstName.Text.Length));
                    txtId2.Text = DataConnectionClass.PersonIdGenerated;
                }
                else
                {
                    DataConnectionClass.CreatePersonId(txtFirstName.Text.ToLower().Substring(0, 4));
                    txtId2.Text = DataConnectionClass.PersonIdGenerated;
                }
            }
        }
        private void txtLastName_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtFirstName.Text) && !String.IsNullOrWhiteSpace(txtLastName.Text) && message != "EDIT")
            {
                string pidstring = "";
                if (txtFirstName.Text.Length < 4)
                {
                    pidstring = txtFirstName.Text.ToLower().Substring(0, txtFirstName.Text.Length);
                }
                else
                {
                    pidstring = txtFirstName.Text.ToLower().Substring(0, 4);
                }
                if (txtLastName.Text.Length < 4)
                {
                    pidstring += txtLastName.Text.ToLower().Substring(0, txtLastName.Text.Length);
                }
                else
                {
                    pidstring += txtLastName.Text.ToLower().Substring(0, 4);
                }
                DataConnectionClass.CreatePersonId(pidstring);
                txtId2.Text = DataConnectionClass.PersonIdGenerated;
            }
            else if (!String.IsNullOrWhiteSpace(txtLastName.Text) && message != "EDIT")
            {
                if (txtLastName.Text.Length < 4)
                {
                    DataConnectionClass.CreatePersonId(txtLastName.Text.ToLower().Substring(0, txtLastName.Text.Length));
                    txtId2.Text = DataConnectionClass.PersonIdGenerated;
                }
                else
                {
                    DataConnectionClass.CreatePersonId(txtLastName.Text.ToLower().Substring(0, 4));
                    txtId2.Text = DataConnectionClass.PersonIdGenerated;
                }
            }
        }
    }
}
