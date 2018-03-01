﻿using System;
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


        public MainMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the Main Menu loads it will perform the folloing functions:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        /// <summary>
        /// This Label will indicate the current user who is logged in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblUser_Click(object sender, EventArgs e)
        {
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
            receive.Show();

            receive.FormClosed += new FormClosedEventHandler(receive_FormClosed);

        }

        void receive_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        /// <summary>
        /// When this method fires load the reports form.
        /// </summary>
        public void GoToReports()
        {
            Reports report = new Reports();
            this.Hide();
            report.Show();

            report.FormClosed += new FormClosedEventHandler(report_FormClosed);
        }

        void report_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        /// <summary>
        /// When this method fires load the manage form.
        /// </summary>
        public void GoToManage()
        {
            Manage manage = new Manage();
            this.Hide();
            manage.Show();

            manage.FormClosed += new FormClosedEventHandler(manage_FormClosed);
        }

        void manage_FormClosed(object sender, FormClosedEventArgs e)
        {
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
            this.Show();
        }

        /// <summary>
        /// When this method fires load the login form.
        /// </summary>
        public void GoToLogIn()
        {

        }
        #endregion

        public void TestLoginTrue()
        {
            if (isLoggedIn)
            {

            }
        }
    }
}
