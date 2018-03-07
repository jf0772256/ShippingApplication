using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using shipapp.Connections.DataConnections;

namespace shipapp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            TestConnClass tc = new TestConnClass();
            tc.Testing();
        }
    }
}
