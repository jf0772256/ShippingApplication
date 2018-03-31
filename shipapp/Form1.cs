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
    /// 2-Select Receiving.
    /// 3-Select Reports.
    /// 4-Select Manage Tables.
    /// 5-Select Settings.
    /// </summary>
    public partial class MainMenu : Form
    {
        // Class level variables
        bool isLoggedIn = false;
        int role;


        LogIn LogInForm { get; set; }


        public MainMenu(LogIn lif)
        {
            InitializeComponent();
            LogInForm = lif;
            lblUser.Text = DataConnectionClass.AuthenticatedUser.FirstName + " " + DataConnectionClass.AuthenticatedUser.LastName + " (" + DataConnectionClass.AuthenticatedUser.Level.Role_Title + ")";
            try
            {
                DataConnectionClass.buildingConn.GetBuildingList();
                DataConnectionClass.CarrierConn.GetCarrierList();
                DataConnectionClass.EmployeeConn.GetAllAfaculty();
                DataConnectionClass.VendorConn.GetVendorList();
                DataConnectionClass.UserConn.GetManyUsers();
                DataConnectionClass.PackageConnClass.GetPackageList();
            }
            catch (Exception)
            {
                // do nothing when errors but the lists will have to be pulled elsewhere
            }

            MessageBox.Show(DataConnectionClass.AuthenticatedUser.Level.Role_Title);
            SetRole();
        }


        /// <summary>
        /// When the Main Menu loads it will perform the folloing functions:
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
                btnManage.Enabled = false;
                btnReports.Enabled = false;
                btnSettings.Enabled = true;
                btnDailyReceiving.BackColor = Color.LightPink;
                btnManage.BackColor = Color.LightPink;
                btnReports.BackColor = Color.LightPink;
                btnSettings.BackColor = Color.White;
            }


            // If Danny
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

            // If Supervisor
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

            // If Crew
            if (role == 3)
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
        /// This Label will indicate the current user who is logged in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblUser_Click(object sender, EventArgs e)
        {
            DataConnectionClass.LogUserOut();
            GoToLogIn();
        }
        
        
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
        #endregion
        
        
        /// <summary>
        /// When the user closes the main menu exit the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        #region MainMeu ButtonFunctionality
        
        
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


        void receive_FormClosed(object sender, FormClosedEventArgs e)
        {
            Receiving r = (Receiving)sender;
            r.Dispose();
            GC.Collect();
            this.Show();
        }
        
        
        /// <summary>
        /// When this method fires load the reports form.
        /// </summary>
        public void GoToReports()
        {
            Reports report = new Reports();
            this.Hide();
            report.label1.Text = DataConnectionClass.AuthenticatedUser.ToString() + " (" + DataConnectionClass.AuthenticatedUser.Level.ToString() + ")";
            report.Show();

            report.FormClosed += new FormClosedEventHandler(report_FormClosed);
        }


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


        public void TestLoginTrue()
        {
            if (isLoggedIn)
            {

            }
        }


        public void SetRole()
        {
            if (DataConnectionClass.AuthenticatedUser.Level.Role_Title == "Administrator")
            {
                this.role = 1;
            }
            else if (DataConnectionClass.AuthenticatedUser.Level.Role_Title == "Supervisor")
            {
                role = 2;
            }
            else if (DataConnectionClass.AuthenticatedUser.Level.Role_Title == "User")
            {
                role = 3;
            }
            else
            {
                role = 0;
            }
        }
    }
}
