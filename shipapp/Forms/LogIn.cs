using shipapp.Connections.DataConnections;
using shipapp.Models;
using System;
using System.Windows.Forms;

namespace shipapp
{
    /// <summary>
    /// This class handles the logging in of users
    /// </summary>
    public partial class LogIn : Form
    {
        /// Class level variables
        // Hard coded admin username and password chosen by Danny Lane
        private string testUsername = "admin";
        private string testPassword = "admin";

        // Main Menu
        private MainMenu Main { get; set; }

        #region Login Setup
        /// <summary>
        /// Check the database and load the data
        /// </summary>
        public LogIn()
        {
            InitializeComponent();
            DataConnectionClass.GetDatabaseData();
            DataConnectionClass.TestConn.Checktables();
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
        #endregion

        #region LoggingIn
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
        /// </summary>
        public void UserAuthenticating()
        {
            // Grab the entered Username and password 
            DataConnectionClass.UserConn.Authenticate.UserName = txtBxUsername.Text;
            DataConnectionClass.UserConn.Authenticate.Password = txtBxPassword.Text;

            // Auth system for hardcoded admin value
            if (txtBxUsername.Text == testUsername && txtBxPassword.Text == testPassword)// Test for first time setup with Hard coded username
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
            // Test that user exist in database
            else
            {
                // Grab the username and test the password
                User Valid = DataConnectionClass.UserConn.Get1User(DataConnectionClass.UserConn.Authenticate.UserName);
                DataConnectionClass.SuccessAuthenticating = DataConnectionClass.UserConn.CheckAuth(Valid);
                if (DataConnectionClass.SuccessAuthenticating)
                {
                    DataConnectionClass.AuthenticatedUser = Valid;
                    OnLoginSucceed();
                }
                else
                {
                    MessageBox.Show("Incorrect Username or Password.\r\nPlease try again...", "Uh-oh!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        /// <summary>
        /// On login success, load the main menu and send it the user information
        /// </summary>
        public void OnLoginSucceed()
        {
            Main = new MainMenu(this);
            GC.Collect();
            Main.Show();
            this.Hide();
        }
        #endregion
    }
}
