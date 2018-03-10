using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using shipapp.Connections.DataConnections;
using shipapp.Models;

namespace shipapp
{
    /// <summary>
    /// Thic class handles the logging in of users
    /// </summary>
    public partial class LogIn : Form
    {
        // Class level variables
        private string testUsername = "admin";
        private string testPassword = "admin";
        private MainMenu Main { get; set; }

        public LogIn()
        {
            InitializeComponent();
            DataConnectionClass.GetDatabaseData();
            //DataConnectionClass.TestConn.ResetAllDatabaseTables();
        }

        /// <summary>
        /// When the form starts these functions will run.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogIn_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        #region Login User and Password fields
        private void txtBxUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBxPassword_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

        /// <summary>
        /// When this button is clicked verify the user and either
        /// Allow or Disallow login. If failed promted for another 
        /// attempt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            UserAuthenticating();
        }


        /// <summary>
        /// When the user attempts to login:
        /// On succed, login and proceed to main menu.
        /// On fail, promt the user to tray again.
        /// STUB: TODO; Remove this line
        /// </summary>
        public void UserAuthenticating()
        {
            // This is just a stub.
            // Will need to match field vs those on database. 
            DataConnectionClass.UserConn.Authenticate.UserName = txtBxUsername.Text;
            DataConnectionClass.UserConn.Authenticate.Password = txtBxPassword.Text;
            
            // Auth system for hardcoded admin value
            if (txtBxUsername.Text == testUsername)
            {
                if (txtBxPassword.Text == testPassword)
                {
                    DataConnectionClass.AuthenticatedUser = new User()
                    {
                        FirstName = "Super",
                        LastName = "Admin",
                        Level = new Models.ModelData.Role()
                        {
                            Role_id = 0,
                            Role_Title = "Super Admin"
                        }
                    };
                    OnLoginSucceed();
                }
                else
                {
                    MessageBox.Show("Incorrect Username or Password.\r\nPlease try again...", "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //auth for users in database
            else
            {
                User Valid = DataConnectionClass.UserConn.Get1User(DataConnectionClass.UserConn.Authenticate.UserName);
                DataConnectionClass.SuccessAuthenticating = DataConnectionClass.UserConn.CheckAuth(Valid);
                if (DataConnectionClass.SuccessAuthenticating)
                {
                    DataConnectionClass.AuthenticatedUser = Valid;
                    OnLoginSucceed();
                }
                else
                {
                    MessageBox.Show("Incorrect Username or Password.\r\nPlease try again...", "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void OnLoginSucceed()
        {
            Main = new MainMenu(this);
            GC.Collect();
            Main.Show();
            this.Hide();
        }
    }
}
