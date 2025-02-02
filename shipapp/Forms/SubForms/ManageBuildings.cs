﻿using shipapp.Connections.DataConnections;
using shipapp.Models.ModelData;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace shipapp
{
    /// <summary>
    /// Allow the addition and editing of buildings
    /// </summary>
    public partial class ManageBuilding : Form
    {
        // Class level variables
        private string message;
        private BuildingClass newBuilding;


        /// <summary>
        /// Set to Add
        /// </summary>
        /// <param name="message"></param>
        public ManageBuilding(string message)
        {
            InitializeComponent();
            this.message = message;
        }


        /// <summary>
        /// Set to Edit
        /// </summary>
        /// <param name="message"></param>
        /// <param name="buildingToBeEdited"></param>
        public ManageBuilding(string message, Object buildingToBeEdited)
        {
            InitializeComponent();
            this.message = message;
            newBuilding = (BuildingClass)buildingToBeEdited;
        }


        /// <summary>
        /// If message is edit set form to edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBuilding_Load(object sender, EventArgs e)
        {
            if (message == "EDIT")
            {
                this.Text = "Edit Building";
                btnAdd.Text = "EDIT";
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


        /// <summary>
        /// Validate data
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Reset error colors
        /// </summary>
        public void ResetError()
        {
            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;
        }


        /// <summary>
        /// Grab the data and add it to the database
        /// </summary>
        public void AddBuildingToDb()
        {
            newBuilding = new BuildingClass();
            newBuilding.BuildingLongName = textBox1.Text;
            newBuilding.BuildingShortName = textBox2.Text;
            DataConnectionClass.buildingConn.WriteBuilding(newBuilding);
            DataConnectionClass.AuditLogConnClass.AddRecordToAudit("added new building " + newBuilding.BuildingLongName);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        /// <summary>
        /// Grab the data and edit the building in the databse
        /// </summary>
        public void EditBuilding()
        {
            string oldname = newBuilding.BuildingLongName;
            newBuilding.BuildingLongName = textBox1.Text;
            newBuilding.BuildingShortName = textBox2.Text;
            DataConnectionClass.buildingConn.UpdateBuilding(newBuilding);
            DataConnectionClass.AuditLogConnClass.AddRecordToAudit("edited building from " + oldname + " to " + newBuilding.BuildingLongName);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
