﻿using Extentions;
using shipapp.Connections.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using shipapp.Models;
using shipapp.Models.ModelData;
using System.Linq;

namespace shipapp
{
    public partial class PrintPreview : Form
    {
        // Class level variabels
        private string clerk = "Null!";
        private int identity = 0;
        private List<Log> logs;
        private List<MailingList> mailingLists;
        private List<Package> packages;
        private List<Vendors> vendors;
        private List<Carrier> carriers;
        private List<BuildingClass> buildings;
        private List<User> users;
        private List<AuditItem> auditItems;
        private List<Package> printPackages;
        private BindingSource bs = new BindingSource();


        /// <summary>
        /// Constructor: Set form accroding to list type
        /// </summary>
        /// <param name="list"></param>
        public PrintPreview(Object list, int identity, object packages)
        {
            InitializeComponent();
            CreateCorrectPrintForm(identity, list, packages);
        }
        /// <summary>
        /// Print the Delivery log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            // If list is from daily receiving
            if (identity == 1)
            {
                if (String.IsNullOrWhiteSpace(cmboClerk.SelectedItem.ToString()))
                {
                    MessageBox.Show("You must select a clerk to deliever the packages!", "Uh-oh!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    Print();
                    UpdatePackageListWithClerk();
                    this.Close();
                }
            }
            else
            {
                // Else print other types
                Print();
                this.Close();
            }
        }
        /// <summary>
        /// Print the selected list with correct formatting
        /// </summary>
        public void Print()
        {
            // Create print object
            DGVPrinterHelper.DGVPrinter printer = new DGVPrinterHelper.DGVPrinter();

            // Set the print obejct page settings 
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = true;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.FooterSpacing = 15;
            printer.PageSettings.Landscape = true;
            printer.PrintMargins = new System.Drawing.Printing.Margins(10, 45, 30, 20);
            printer.ShowTotalPageNumber = true;

            // Set column widths
            if (identity == 1)
            {
                // Set daily and clerk info
                printer.Title = "Delivery Log";
                printer.SubTitle = "Clerk: " + clerk;
                printer.SubTitle += ", Date: " + DateTime.Today.ToShortDateString();

                // Set page widths
                dataGridLog.Columns[0].Width = 35;
                dataGridLog.Columns[1].Width = 50;
                dataGridLog.Columns[2].Width = 35;
                dataGridLog.Columns[3].Width = 110;
                dataGridLog.Columns[4].Width = 35;
                dataGridLog.Columns[5].Width = 85;
                dataGridLog.Columns[6].Width = 125;
                dataGridLog.Columns[6].HeaderText = "Please Sign & Print";

                // Update the package clerk and status
                UpdatePackages();
            }
            else if (identity == 2)
            {
                // Set header
                printer.Title = "History";
                printer.SubTitle += "Date: " + DateTime.Today.ToShortDateString();

                // Set page widths
                dataGridLog.Columns[0].Width = 50;
                dataGridLog.Columns[1].Width = 30;
                dataGridLog.Columns[2].Width = 33;
                dataGridLog.Columns[3].Width = 33;
                dataGridLog.Columns[4].Width = 30;
                dataGridLog.Columns[5].Width = 50;
                dataGridLog.Columns[6].Width = 50;
                dataGridLog.Columns[7].Width = 50;
                dataGridLog.Columns[8].Width = 50;
                dataGridLog.Columns[9].Width = 33;
                dataGridLog.Columns[10].Width = 33;
                dataGridLog.Columns[11].Width = 42;
                dataGridLog.Columns[12].Width = 32;
            }
            else if (identity == 3)
            {
                // Set headers
                printer.Title = "Users";
                printer.SubTitle += "Date: " + DateTime.Today.ToShortDateString();

                // Set pages widths
                dataGridLog.Columns[0].Width = 50;
                dataGridLog.Columns[1].Width = 50;
                dataGridLog.Columns[2].Width = 50;
                dataGridLog.Columns[3].Width = 50;
                dataGridLog.Columns[4].Width = 50;
                dataGridLog.Columns[5].Width = 50;
            }
            else if (identity == 4)
            {
                // Set headers
                printer.Title = "Vendors";
                printer.SubTitle += "Date: " + DateTime.Today.ToShortDateString();
                printer.PageSettings.Landscape = false;

                // Set page widths
                dataGridLog.Columns[0].Width = 50;
                dataGridLog.Columns[1].Width = 50;
            }
            else if (identity == 5)
            {
                // Set headers
                printer.PrintRowHeaders = false;
                printer.PageSettings.Landscape = false;
                printer.PrintColumnHeaders = false;
                printer.PrintFooter = false;
                printer.PrintHeader = false;
                

                // Set page widths and font style
                dataGridLog.Font = new Font("Microsoft Sans Serif", 16,FontStyle.Regular);
                dataGridLog.Columns[0].Width = 0;
                dataGridLog.Columns[1].Width = 0;
                dataGridLog.Columns[2].Width = 0;
                dataGridLog.Columns[3].Width = 0;
            }
            else if (identity == 6)
            {
                // Set headers
                printer.Title = "Buildings";
                printer.SubTitle += "Date: " + DateTime.Today.ToShortDateString();
                printer.PageSettings.Landscape = false;

                // Set page widths
                dataGridLog.Columns[0].Width = 50;
                dataGridLog.Columns[1].Width = 50;
                dataGridLog.Columns[2].Width = 50;
            }
            else if (identity == 7)
            {
                // Set Headers
                printer.Title = "Carriers";
                printer.SubTitle += "Date: " + DateTime.Today.ToShortDateString();
                printer.PageSettings.Landscape = false;

                // Set page widths
                dataGridLog.Columns[0].Width = 50;
                dataGridLog.Columns[1].Width = 50;
            }
            else if (identity == 8)
            {
                // Set headers
                printer.Title = "Activity History";
                printer.SubTitle += "Date: " + DateTime.Today.ToShortDateString();
                printer.PageSettings.Landscape = false;

                // Set page widths
                dataGridLog.Columns[0].Width = 75;
                dataGridLog.Columns[1].Width = 10;
                dataGridLog.Columns[2].Width = 15;
            }
            else
            {
                MessageBox.Show("Something has gone wrong.\r\nPlease try again", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Print the Object
            printer.PrintDataGridView(dataGridLog);
        }
        /// <summary>
        ///  Make the PO pretty
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public string PadPO(int newValue)
        {
            if (newValue <= 9)
            {
                return "0000" + newValue.ToString();
            }
            else if (newValue > 9 && newValue < 100)
            {
                return "000" + newValue.ToString();
            }
            else if (newValue > 99 && newValue < 1000)
            {
                return "00" + newValue.ToString();
            }
            else if (newValue > 999 && newValue < 10000)
            {
                return "0" + newValue.ToString();
            }
            else
            {
                return newValue.ToString();
            }
        }
        /// <summary>
        /// Returns a string containing the clerk assigned to the log
        /// </summary>
        /// <returns></returns>
        public string UpdatePackageListWithClerk()
        {
            return clerk;
        }
        /// <summary>
        /// Close the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Select a clerk to deliver the package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmboClerk_SelectedIndexChanged(object sender, EventArgs e)
        {
            clerk = cmboClerk.SelectedItem.ToString();
        }
        /// <summary>
        /// Set print form to match entity
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="list"></param>
        /// <param name="packages"></param>
        public void CreateCorrectPrintForm(int identity, Object list, Object packages)
        {
            this.identity = identity;

            // Determine list type
            if (identity == 1)
            {
                // List is Package
                logs = (List<Log>)list;
                printPackages = (List<Package>)packages;
                bs.DataSource = logs;
                dataGridLog.DataSource = bs;
                cmboClerk.Show();
                btnPrint.Enabled = false;
            }
            else if (identity == 2)
            {
                // List is Package History TODO
                this.packages = (List<Package>)list;
                bs.DataSource = this.packages;
                dataGridLog.DataSource = bs;
                cmboClerk.Hide();

                // Set grid headers
                dataGridLog.Columns["PackageId"].Visible = false;
                dataGridLog.Columns["Package_PersonId"].Visible = false;
                dataGridLog.Columns["PONumber"].HeaderText = "PO Number";
                dataGridLog.Columns["PackageCarrier"].HeaderText = "Carrier";
                dataGridLog.Columns["PackageVendor"].HeaderText = "Vendor";
                dataGridLog.Columns["PackageDeliveredTo"].HeaderText = "Delivered To";
                dataGridLog.Columns["PackageDeleveredBy"].HeaderText = "Delivered By";
                dataGridLog.Columns["PackageSignedForBy"].HeaderText = "Signed For By";
                dataGridLog.Columns["PackageTrackingNumber"].HeaderText = "Tracking Number";
                dataGridLog.Columns["PackageReceivedDate"].HeaderText = "Received Date";
                dataGridLog.Columns["PackageDeliveredDate"].HeaderText = "Delivered Date";
                dataGridLog.Columns["Status"].HeaderText = "Delivery Status";
                dataGridLog.Columns["DelivBuildingShortName"].HeaderText = "Deliver To Short Name";
            }
            else if (identity == 3)
            {
                // List is Users
                users = (List<User>)list;
                bs.DataSource = users;
                dataGridLog.DataSource = bs;
                cmboClerk.Hide();

                // Set grid columns
                dataGridLog.Columns[0].Visible = false;
                dataGridLog.Columns[1].HeaderText = "First Name";
                dataGridLog.Columns[2].HeaderText = "Last Name";
                dataGridLog.Columns[3].HeaderText = "Role";
                dataGridLog.Columns[4].Visible = false;
                dataGridLog.Columns[6].Visible = false;
            }
            else if (identity == 4)
            {
                // List is Vendor
                vendors = (List<Vendors>)list;
                bs.DataSource = vendors;
                dataGridLog.DataSource = bs;
                cmboClerk.Hide();

                // Set grid columns
                dataGridLog.Columns[0].Visible = false;
                dataGridLog.Columns[1].HeaderText = "Vendor";
            }
            else if (identity == 5)
            {
                // List is Faculty
                mailingLists = (List<MailingList>)list;
                mailingLists = mailingLists.OrderBy(i => i.Name).ToList();
                bs.DataSource = mailingLists;
                dataGridLog.DataSource = bs;
                cmboClerk.Hide();

                // Set grid columns
                dataGridLog.Columns[0].HeaderText = "Name";
                dataGridLog.Columns[1].HeaderText = "Building";
                dataGridLog.Columns[2].HeaderText = "MailBox #";
                dataGridLog.Columns[3].HeaderText = "Spacing";
            }
            else if (identity == 6)
            {
                // List is Building
                buildings = (List<BuildingClass>)list;
                bs.DataSource = buildings;
                dataGridLog.DataSource = bs;
                cmboClerk.Hide();

                // Set grid columns
                dataGridLog.Columns[0].Visible = false;
                dataGridLog.Columns[1].HeaderText = "Long Name";
                dataGridLog.Columns[2].HeaderText = "Short Name";
            }
            else if (identity == 7)
            {
                // List is Carrier
                carriers = (List<Carrier>)list;
                bs.DataSource = carriers;
                dataGridLog.DataSource = bs;
                cmboClerk.Hide();

                // Set grid columns
                dataGridLog.Columns[0].Visible = false;
                dataGridLog.Columns[1].HeaderText = "Carrier";
            }
            else if (identity == 8)
            {
                // List is activity history
                auditItems = (List<AuditItem>)list;
                bs.DataSource = auditItems;
                dataGridLog.DataSource = bs;
                cmboClerk.Hide();

                // Set grid columns
                dataGridLog.Columns[0].HeaderText = "Item";
                dataGridLog.Columns[1].HeaderText = "Date";
                dataGridLog.Columns[2].HeaderText = "Time";
            }
            else
            {
                MessageBox.Show("Something went wrong\r\nTry again.", "Uh-oh!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        /// <summary>
        /// When printing a dailey receiving log, uodate the packages with clerk and new status
        /// </summary>
        public void UpdatePackages()
        {
            // Update each package
            for (int i = 0; i < printPackages.Count; i++)
            {
                printPackages[i].PackageDeleveredBy = clerk;
                printPackages[i].Status = (Package.DeliveryStatus)2;
                Connections.DataConnections.DataConnectionClass.PackageConnClass.UpdatePackage(printPackages[i]);
            }
        }
        /// <summary>
        /// When the form loads fill the clerk combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintPreview_Load(object sender, EventArgs e)
        {
            Connections.DataConnections.DataConnectionClass.DataLists.UsersList.ForEach(i => cmboClerk.Items.Add(i));
        }
        /// <summary>
        /// Check that a clerk has been selected before printing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmboClerk_SelectionChangeCommitted(object sender, EventArgs e)
        {
            btnPrint.Enabled = true;
        }
    }
}
