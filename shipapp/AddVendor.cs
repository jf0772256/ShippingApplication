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
    public partial class AddVendor : Form
    {
        public AddVendor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Whene the user clicks this button it will check the data, add it to the DB, and close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetError();

            if (ValidateData())
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("All fields must have correct data!", "Uh-oh", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        /// <summary>
        /// Reset the back color after an error
        /// </summary>
        private void ResetError()
        {
           
        }

        /// <summary>
        /// Test the data before writing it to the database
        /// </summary>
        /// <returns></returns>
        private bool ValidateData()
        {
            // Method level variables
            bool pass = true;

            // Test data

            return pass;
        }
    }
}
