using shipapp.Connections.HelperClasses;
using shipapp.Models.ModelData;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace shipapp.Connections.DataConnections.Classes
{
    /// <summary>
    /// Building Class
    /// </summary>
    class BuildingConnClass : DatabaseConnection
    {
        /// <summary>
        /// Form Object
        /// </summary>
        object Sender { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public BuildingConnClass():base() { }
        /// <summary>
        /// Gets a list of buildings 
        /// </summary>
        /// <param name="sender">Form object</param>
        public async void GetBuildingList(object sender = null)
        {
            if (String.IsNullOrWhiteSpace(DataConnectionClass.ConnectionString))
            {
                return;
            }
            Sender = sender;
            SortableBindingList<BuildingClass> b = await Task.Run(() => Get_Building_List());
            if (Sender is Manage)
            {
                Manage t = (Manage)Sender;
                DataConnectionClass.DataLists.BuildingNames = b;
                BindingSource bs = new BindingSource
                {
                    DataSource = DataConnectionClass.DataLists.BuildingNames
                };
                t.dataGridView1.DataSource = bs;
                try
                {
                    t.dataGridView1.Columns["BuildingId"].Visible = false;
                }
                catch (Exception)
                {
                    //
                }
            }
            else if (Sender is ManageFaculty)
            {
                shipapp.ManageFaculty t = (shipapp.ManageFaculty)Sender;
                DataConnectionClass.DataLists.BuildingNames = b;
                foreach (BuildingClass bldg in DataConnectionClass.DataLists.BuildingNames)
                {
                    t.comboBox1.Items.Add(bldg.BuildingLongName);
                }
            }
            else
            {
                DataConnectionClass.DataLists.BuildingNames = b;
            }
        }
        /// <summary>
        /// Adds building to the database
        /// </summary>
        /// <param name="building">Building object to add</param>
        public void WriteBuilding(BuildingClass building)
        {
            Write(building);
        }
        /// <summary>
        /// Deletes building from the database
        /// </summary>
        /// <param name="building">Building object to remove</param>
        public void RemoveBuilding(BuildingClass building)
        {
            Delete(building);
        }
        /// <summary>
        /// Update building
        /// </summary>
        /// <param name="building">Building object to update</param>
        public void UpdateBuilding(BuildingClass building)
        {
            Update(building);
        }
    }
}