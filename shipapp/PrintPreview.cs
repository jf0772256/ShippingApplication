using shipapp.Connections.HelperClasses;
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
    public partial class PrintPreview : Form
    {
        // Class level variabels
        private string clerk = "Null!";
        private BindingList<Log> logs;


        public PrintPreview(Object logs)
        {
            InitializeComponent();
            this.logs = (BindingList<Log>)logs;
        }


        private void PrintPreview_Load(object sender, EventArgs e)
        {
            dataGridLog.DataSource = logs;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        /// <summary>
        /// Print the log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            //
            if (String.IsNullOrWhiteSpace(cmboClerk.SelectedItem.ToString()))
            {
                MessageBox.Show("You must select a clerk to deleiver the packages!", "Uh-oh!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                PrintLog();
                UpdatePackageListWithClerk();
                this.Close();
            }
        }


        public void PrintLog()
        {
            // Create print object
            DGVPrinter printer = new DGVPrinter();

            // Set the print obejct page settings
            printer.Title = "Delivery Log";
            printer.SubTitle = "Clerk: " + clerk;
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = true;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "Footer";
            printer.FooterSpacing = 15;
            printer.PageSettings.Landscape = true;
            printer.PrintMargins = new System.Drawing.Printing.Margins(10, 45, 30, 20);
            printer.ShowTotalPageNumber = true;

            // Set column widths
            dataGridLog.Columns[0].Width = 35;
            dataGridLog.Columns[1].Width = 50;
            dataGridLog.Columns[2].Width = 35;
            dataGridLog.Columns[3].Width = 110;
            dataGridLog.Columns[4].Width = 35;
            dataGridLog.Columns[5].Width = 85;
            dataGridLog.Columns[6].Width = 125;

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
    }
}
