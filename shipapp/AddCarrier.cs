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
    public partial class AddCarrier : Form
    {
        public AddCarrier()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the user clicks the ADD button it will check the data, write the data to the DB, and then close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
