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
    public partial class AddBuilding : Form
    {
        public AddBuilding()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the user clicks this button it will check the data, add it to the DB, and close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox1.Text))
            {
                return;
            }
            if (String.IsNullOrWhiteSpace(textBox2.Text))
            {
                return;
            }
            //create new building
            Models.ModelData.BuildingClass building = new Models.ModelData.BuildingClass()
            {
                BuildingLongName = textBox1.Text,
                BuildingShortName = textBox2.Text
            };
            Connections.DataConnections.DataConnectionClass.buildingConn.WriteBuilding(building);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void AddBuilding_Load(object sender, EventArgs e)
        {

        }
    }
}
