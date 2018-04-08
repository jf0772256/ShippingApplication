using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using shipapp.Connections.HelperClasses;
using shipapp.Models;
using shipapp.Models.ModelData;
using shipapp.Connections.DataConnections;

namespace shipapp
{
    /// <summary>
    /// This form will attomaticly load with all past packages aand allow the
    /// user to add past package to the daily receiving,
    /// </summary>
    public partial class Reports : Form
    {
        // Class level variabels
        private DataGridViewColumnHelper dgvch = new DataGridViewColumnHelper();
        private int role;
        private ListSortDirection[] ColumnDirection { get; set; }


        /// <summary>
        /// Main constructor
        /// </summary>
        public Reports()
        {
            InitializeComponent();
        }


        /// <summary>
        /// When the back button is clicked, close this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Upon form load, do this
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reports_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            GetPackages();
            SetRole();
            //BindingSource bs = new BindingSource
            //{
            //    DataSource = DataConnectionClass.DataLists.PackageHistory
            //};
            //datGridHistory.DataSource = bs;
            // Set form according to the role
            if (role == 1)
            {
                pcBxAddToDaily.Enabled = true;
                pcBxAddToDaily.Show();
            }
            else if (role == 2)
            {
                pcBxAddToDaily.Enabled = true;
                pcBxAddToDaily.Show();
            }
            else if (role == 3)
            {
                pcBxAddToDaily.Enabled = false;
                pcBxAddToDaily.Hide();
            }
            else
            {
                pcBxAddToDaily.Enabled = false;
                pcBxAddToDaily.Hide();
            }
        }


        /// <summary>
        /// Alert user if an atempt to logout occurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            SignOut();
        }


        /// <summary>
        /// Alert user if an atempt to logout occurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            SignOut();
        }


        /// <summary>
        /// If the user attempts to log out. inform them to go back to the main menu
        /// </summary>
        public void SignOut()
        {
            MessageBox.Show(DataConnectionClass.AuthenticatedUser.LastName + ", " + DataConnectionClass.AuthenticatedUser.FirstName + "\r\n" + DataConnectionClass.AuthenticatedUser.Level.Role_Title + "\r\n\r\nTo Logout exit to the Main Menu.");
        }


        /// <summary>
        /// When the user clicks the button, add all selected packages to the daily receiving list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pcBxAddToDaily_Click(object sender, EventArgs e)
        {
            AddPackageToDaily();
        }


        /// <summary>
        /// Set role to match the user
        /// </summary>
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


        /// <summary>
        /// Add selected packages to daily receiving
        /// </summary>
        public void AddPackageToDaily()
        {

        }


        /// <summary>
        /// Fill the lsit with packages
        /// </summary>
        public void GetPackages()
        {
            DataConnectionClass.PackageConnClass.GetPackageHistoryList(this);
        }


        /// <summary>
        /// Refreash list
        /// </summary>
        public void Refreash()
        {
            DataConnectionClass.PackageConnClass.GetPackageList(this);
        }

        /// <summary>
        /// Refreash the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pcBxRefreash_Click(object sender, EventArgs e)
        {
            GetPackages();
        }
    }
}
