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

namespace shipapp
{
    public partial class AddFaculty : Form
    {
        // Class level variables
        private Models.Faculty newFaculty;

        internal Faculty NewFaculty { get => newFaculty; set => newFaculty = value; }

        public AddFaculty()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Models.Faculty faculty = new Models.Faculty();
            faculty.FirstName = txtFirstName.Text;
            faculty.LastName = txtLastName.Text;
            faculty.Faculty_PersonId = txtId2.Text;
            faculty.Id = long.Parse(txtId1.Text);

            Connections.DataConnections.DataConnectionClass.EmployeeConn.AddFaculty(faculty);
        }
    }
}
