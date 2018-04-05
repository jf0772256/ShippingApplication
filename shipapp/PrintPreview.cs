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
            printer.Title = "Delivery Log";
            printer.SubTitle = "Delivery Person Goes Here";
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = true;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            //printer.PrintMargins = new System.Drawing.Printing.Margins(1, 1, 1, 1);
            printer.Footer = "Footer";
            printer.FooterSpacing = 15;
            printer.PageSettings.Landscape = true;
            //printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.Porportional;
            //printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;
            printer.PrintMargins = new System.Drawing.Printing.Margins(10, 45, 30, 20);
            printer.ShowTotalPageNumber = true;
            //dataGridLog.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
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


        public void GetLog()
        {
            for (int i = 0; i < 25; i++)
            {
                Log log = new Log();

                log.Po = PadPO(i);
                log.TrackingNumber = "" + (i + 1) + (i - 2) + (i + 3) + (i - 4) + (i + 5) + (i - 6) + (i + 7) + (i - 8) + (i + 9);
                log.Carrier = "UPS";
                log.Vendor = "Amazon";
                log.Recipiant = "Ford, Tiffany";
                log.Building = "NKM";
                log.Signature = "";

                logs.Add(log);
            }
        }

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
