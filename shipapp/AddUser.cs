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
    public partial class AddUser : Form
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void AddUser_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Models.User newUser = new Models.User();
            newUser.Id = long.Parse(txtId.Text);
            newUser.FirstName = txtFirstName.Text;
            newUser.LastName = txtLastName.Text;
            newUser.Level = new Models.ModelData.Role() {Role_id = 1};
            newUser.Username = txtUsername.Text;
            newUser.PassWord = txtPassword.Text;
            newUser.Person_Id = txtBoxPersonId.Text;

            Connections.DataConnections.DataConnectionClass.UserConn.Write1User(newUser);
        }
    }
}
