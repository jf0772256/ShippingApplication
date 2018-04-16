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

namespace shipapp
{
    /// <summary>
    /// This is the Main Menu for the shipping app:
    /// 1-Prmots the user to Log In.
    /// 2-Select Receiving for Daily Task.
    /// 3-Select History to view packages from the past.
    /// 4-Select Manage to add edit and delete from the database tables.
    /// 5-Select Settings to change the server connection information.
    /// </summary>
    public partial class MainMenu : Form
    {
        /// Class level variables
        // Helper variables
        int role;

        // Access the login form
        LogIn LogInForm { get; set; }

        #region MainMenu Setup
        /// <summary>
        /// Contructor
        /// -Grab the users info.
        /// -Fill all the data lsit
        /// -Set the user permissions
        /// -Back up to database if wither tuesday or thursday
        /// </summary>
        /// <param name="lif"></param>
        public MainMenu(LogIn lif)
        {
            InitializeComponent();

            // Grab User
            LogInForm = lif;
            lblUser.Text = DataConnectionClass.AuthenticatedUser.FirstName + " " + DataConnectionClass.AuthenticatedUser.LastName + " (" + DataConnectionClass.AuthenticatedUser.Level.Role_Title + ")";


            try
            {
                // Fill list
                DataConnectionClass.buildingConn.GetBuildingList();
                DataConnectionClass.CarrierConn.GetCarrierList();
                DataConnectionClass.EmployeeConn.GetAllAfaculty();
                DataConnectionClass.VendorConn.GetVendorList();
                DataConnectionClass.UserConn.GetManyUsers();
                DataConnectionClass.PackageConnClass.GetPackageList();
                DataConnectionClass.PackageConnClass.GetPackageHistoryList();
            }
            catch (Exception)
            {
                // Do nothing when errors but the lists will have to be pulled elsewhere
            }

            SetRole();

            // Only back up data if admin or super admin user.
            if (role == 1 || role == 0)
            {
                DataConnectionClass.Backup_DB.CheckToDoBackup();
            }
        }
        /// <summary>
        /// Set form permission base on user role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();

            // If Setup Admin
            if (role == 0)
            {
                btnDailyReceiving.Enabled = false;
                btnManage.Enabled = true;
                btnReports.Enabled = false;
                btnSettings.Enabled = true;
                btnDailyReceiving.BackColor = Color.LightPink;
                btnManage.BackColor = Color.White;
                btnReports.BackColor = Color.LightPink;
                btnSettings.BackColor = Color.White;
            }


            // If Admin
            if (role == 1)
            {
                // Do nothing
                btnDailyReceiving.Enabled = true;
                btnManage.Enabled = true;
                btnReports.Enabled = true;
                btnSettings.Enabled = true;
                btnDailyReceiving.BackColor = Color.White;
                btnManage.BackColor = Color.White;
                btnReports.BackColor = Color.White;
                btnSettings.BackColor = Color.White;
            }

            // If Dock Supervisor
            if (role == 2)
            {
                btnDailyReceiving.Enabled = true;
                btnManage.Enabled = true;
                btnReports.Enabled = true;
                btnSettings.Enabled = false;
                btnDailyReceiving.BackColor = Color.White;
                btnManage.BackColor = Color.White;
                btnReports.BackColor = Color.White;
                btnSettings.BackColor = Color.LightPink;
            }

            // If Supervisor
            if (role == 3)
            {
                btnDailyReceiving.Enabled = true;
                btnManage.Enabled = true;
                btnReports.Enabled = true;
                btnSettings.Enabled = false;
                btnDailyReceiving.BackColor = Color.White;
                btnManage.BackColor = Color.White;
                btnReports.BackColor = Color.White;
                btnSettings.BackColor = Color.LightPink;
            }

            // If Crew
            if (role == 4)
            {
                btnDailyReceiving.Enabled = true;
                btnManage.Enabled = false;
                btnReports.Enabled = false;
                btnSettings.Enabled = false;
                btnDailyReceiving.BackColor = Color.White;
                btnManage.BackColor = Color.LightPink;
                btnReports.BackColor = Color.LightPink;
                btnSettings.BackColor = Color.LightPink;
            }
        }
        /// <summary>
        /// Set the role of the user
        /// </summary>
        public void SetRole()
        {
            if (DataConnectionClass.AuthenticatedUser.Level.Role_Title == "Administrator")
            {
                role = 1;
            }
            else if (DataConnectionClass.AuthenticatedUser.Level.Role_Title == "Dock Supervisor")
            {
                role = 2;
            }
            else if (DataConnectionClass.AuthenticatedUser.Level.Role_Title == "Supervisor")
            {
                role = 3;
            }
            else if (DataConnectionClass.AuthenticatedUser.Level.Role_Title == "User")
            {
                role = 4;
            }
            else
            {
                role = 0;
            }
        }
        #endregion

        #region Login
        /// <summary>
        /// This label will indicate the current user who is logged in and thier permission level.
        /// -Upon click it will sign the user out  and load the login form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblUser_Click(object sender, EventArgs e)
        {
            DataConnectionClass.LogUserOut();
            GoToLogIn();
        }
        /// <summary>
        /// This icon will indicate the current user who is logged in and thier permission level.
        /// -Upon click it will sign the user out  and load the login form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DataConnectionClass.LogUserOut();
            GoToLogIn();
        }
        #endregion

        #region MainMenu Buttons
        /// <summary>
        /// When clicked switch to the Daily Receiving form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDailyReceiving_Click(object sender, EventArgs e)
        {
            GoToReceiving();
        }
        /// <summary>
        /// When clicked switch to the Report Creation form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReports_Click(object sender, EventArgs e)
        {
            GoToReports();
        }
        /// <summary>
        /// When clicked switch to the Manage Tables form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnManage_Click(object sender, EventArgs e)
        {
            GoToManage();
        }
        /// <summary>
        /// When clciked switch to the Settings form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSettings_Click(object sender, EventArgs e)
        {
            GoToSettings();
        }
        /// <summary>
        /// When this method fires load the receiving form.
        /// </summary>
        public void GoToReceiving()
        {
            Receiving receive = new Receiving();
            this.Hide();
            receive.label1.Text = DataConnectionClass.AuthenticatedUser.ToString() + " (" + DataConnectionClass.AuthenticatedUser.Level.ToString() + ")";
            Connections.DataConnections.DataConnectionClass.PackageConnClass.GetPackageList(receive);
            receive.Show();
            receive.FormClosed += new FormClosedEventHandler(receive_FormClosed);
        }
        /// <summary>
        /// When switching to main menu perform garbage collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void receive_FormClosed(object sender, FormClosedEventArgs e)
        {
            Receiving r = (Receiving)sender;
            r.Dispose();
            GC.Collect();
            this.Show();
        }
        /// <summary>
        /// When switching to main menu perform garbage collection
        /// </summary>
        public void GoToReports()
        {
            Reports report = new Reports();
            this.Hide();
            report.label1.Text = DataConnectionClass.AuthenticatedUser.ToString() + " (" + DataConnectionClass.AuthenticatedUser.Level.ToString() + ")";
            report.Show();
            report.FormClosed += new FormClosedEventHandler(report_FormClosed);
        }
        /// <summary>
        /// When switching to main menu perform garbage collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void report_FormClosed(object sender, FormClosedEventArgs e)
        {
            Reports r = (Reports)sender;
            r.Dispose();
            GC.Collect();
            this.Show();
        }
        /// <summary>
        /// When this method fires load the manage form.
        /// </summary>
        public void GoToManage()
        {
            Manage manage = new Manage();
            this.Hide();
            manage.label1.Text = DataConnectionClass.AuthenticatedUser.ToString() + " (" + DataConnectionClass.AuthenticatedUser.Level.ToString() + ")";
            manage.Show();
            manage.FormClosed += new FormClosedEventHandler(manage_FormClosed);
        }
        /// <summary>
        /// When switching to main menu perform garbage collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void manage_FormClosed(object sender, FormClosedEventArgs e)
        {
            Manage m = (Manage)sender;
            m.Dispose();
            GC.Collect();
            this.Show();
        }
        /// <summary>
        /// When this method fires load the setting form.
        /// </summary>
        public void GoToSettings()
        {
            Settings settings = new Settings();
            this.Hide();
            settings.Show();
            settings.FormClosed += new FormClosedEventHandler(settings_FormClosed);
        }
        /// <summary>
        /// When switching to main menu perform garbage collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            //to avoid memory leak, which was detected through the log in out process... all forms created must be disposed and resources freed
            Settings s = (Settings)sender;
            s.Dispose();
            GC.Collect();
            this.Show();
        }
        /// <summary>
        /// When this method fires load the login form.
        /// </summary>
        public void GoToLogIn()
        {
            LogInForm.txtBxUsername.Text = "";
            LogInForm.txtBxPassword.Text = "";
            LogInForm.Show();
            LogInForm.txtBxUsername.Focus();
            this.Dispose(true);
            GC.Collect();
        }
        #endregion

        #region MainMenu Functionality
        /// <summary>
        /// When the user closes the main menu exit the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}
