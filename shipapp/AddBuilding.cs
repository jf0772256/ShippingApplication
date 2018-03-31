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
    /// <summary>
    /// Allow the addition and editing of buildings
    /// </summary>
    public partial class AddBuilding : Form
    {
        // Class level variables
        private string message;
        private Models.ModelData.BuildingClass newBuilding;


        public AddBuilding(string message)
        {
            InitializeComponent();
            this.message = message;
        }


        public AddBuilding(string message, Object buildingToBeEdited)
        {
            InitializeComponent();
            this.message = message;
            newBuilding = (Models.ModelData.BuildingClass)buildingToBeEdited;
        }


        private void AddBuilding_Load(object sender, EventArgs e)
        {
            if (message == "EDIT")
            {
                textBox1.Text = newBuilding.BuildingLongName;
                textBox2.Text = newBuilding.BuildingShortName;
            }
        }


        /// <summary>
        /// When the user clicks this button it will check the data, add it to the DB, and close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetError();

            if (ValidateData() && message == "ADD")
            {
                AddBuildingToDb();
            }
            else if (ValidateData() && message == "EDIT")
            {
                EditBuilding();
            }
        }


        public bool ValidateData()
        {
            // Method level variables
            bool pass = true;
            string message = "Make sure all fields have correct data.\r\n";

            if (String.IsNullOrWhiteSpace(textBox1.Text))
            {
                pass = false;
                message = "\t-Must have a Long Name\r\n";
                textBox1.BackColor = Color.LightPink;
            }

            if (String.IsNullOrWhiteSpace(textBox2.Text))
            {
                pass = false;
                message = "\t-Must have a Short Name\r\n";
                textBox2.BackColor = Color.LightPink;
            }

            if (!pass)
            {
                MessageBox.Show(message, "Uh-oh!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            return pass;
        }


        public void ResetError()
        {
            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;
        }


        public void AddBuildingToDb()
        {
            newBuilding = new Models.ModelData.BuildingClass();

            newBuilding.BuildingLongName = textBox1.Text;
            newBuilding.BuildingShortName = textBox2.Text;
            Connections.DataConnections.DataConnectionClass.buildingConn.WriteBuilding(newBuilding);
            Connections.DataConnections.DataConnectionClass.DataLists.BuildingNames.Add(newBuilding);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        public void EditBuilding()
        {
            newBuilding.BuildingLongName = textBox1.Text;
            newBuilding.BuildingShortName = textBox2.Text;
            Connections.DataConnections.DataConnectionClass.buildingConn.WriteBuilding(newBuilding);
            Connections.DataConnections.DataConnectionClass.DataLists.BuildingNames.Add(newBuilding);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
