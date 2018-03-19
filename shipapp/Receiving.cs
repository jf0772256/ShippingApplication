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
    /// <summary>
    /// This class will alow the user to do the following:
    /// 1. Add and track freigth as it comes in.
    /// 2. Print out daily daily logs.
    /// 3. Change the status of current items.
    /// 4. Push the added/updated/deleted items to the DB.
    /// 5. Sign in and Out.
    /// </summary>
    public partial class Receiving : Form
    {
        //Class levle variables
        private BindingList<Models.Package> packages;



        public Receiving()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Receiving_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }

        /// <summary>
        /// When the use clicks back go back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// TODO: Add entity selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            AddNote note = new AddNote();
            note.Show();
        }

        public void AddPackage()
        {

        }

        public void EditPackage()
        {

        }

        public void DeletePackage()
        {

        }
    }
}
