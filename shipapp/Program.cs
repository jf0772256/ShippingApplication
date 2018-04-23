using System;
using System.Windows.Forms;
namespace shipapp
{
    /// <summary>
    /// Initial Program app class
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Creating the instance of the DataConnectionClass
        /// </summary>
        public static Connections.DataConnections.DataConnectionClass TheConnection = new Connections.DataConnections.DataConnectionClass();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}