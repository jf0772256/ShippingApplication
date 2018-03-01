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
    /// Thic class handles the logging in of users
    /// </summary>
    public partial class LogIn : Form
    {
        // Class level variables
        private string testUsername = "admin";
        private string testPassword = "admin";


        public LogIn()
        {
            InitializeComponent();
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
        /// </summary>
        public void UserAuthenticating()
        {
            // This is just a stub.
            // Will need to match field vs those on database. 
            if (txtBxUsername.Text == testUsername)
            {
                if (txtBxPassword.Text == testPassword)
                {
                    OnLoginSucceed();
                }
                else
                {
                    MessageBox.Show("Incorrect Username or Password.\r\nPlease try again...", "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Incorrect Username or Password.\r\nPlease try again...", "Try Again", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        public void OnLoginSucceed()
        {
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Hide();
            
        }
    }
}
