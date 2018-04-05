using DGVPrinterHelper;
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
        private BindingList<Log> logs = new BindingList<Log>();


        public PrintPreview()
        {
            InitializeComponent();
        }


        private void PrintPreview_Load(object sender, EventArgs e)
        {
            GetLog();
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
            PrintLog();
        }


        public void PrintLog()
        {
            // Create print object
            DGVPrinter printer = new DGVPrinter();

            // Set the print obejct page settings
            printer.Title = "Deleivery Log";
            printer.SubTitle = "Deleivery Person Goes Here";
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = true;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            //printer.PrintMargins = new System.Drawing.Printing.Margins(1, 1, 1, 1);
            printer.Footer = "Footer";
            printer.FooterSpacing = 15;
            printer.PageSettings.Landscape = true;
            printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.Porportional;

            // Print the Object
            printer.PrintDataGridView(dataGridLog);
        }


        public void GetLog()
        {
            for (int i = 0; i < 25; i++)
            {
                Log log = new Log();

                log.Po = "0000" + 1;
                log.TrackingNumber = "" + (i + 1) + (i - 2) + (i + 3) + (i - 4) + (i + 5) + (i - 6) + (i + 7) + (i - 8) + (i + 9);
                log.Carrier = "UPS";
                log.Vendor = "Amazon";
                log.Recipiant = "Ford, Tiffany";
                log.Building = "NKM";
                log.Signature = "";

                logs.Add(log);
            }
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
    }
}
