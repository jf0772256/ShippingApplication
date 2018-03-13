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
    public partial class Manage : Form
    {
        // Class level variables
        


        public Manage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Faculty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            var list = new BindingList<Models.Faculty> { };
            list.Add(new Models.Faculty(0,"0","kalin", "bowden"));
            list.Add(new Models.Faculty(0, "1", "jesse", "fender"));
            list.Add(new Models.Faculty(0, "2", "tiffany", "ford"));

            dataGridView1.DataSource = list;

            AddFaculty addFaculty = new AddFaculty();
            addFaculty.ShowDialog();

            list.Add(addFaculty.NewFaculty);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Manage_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
