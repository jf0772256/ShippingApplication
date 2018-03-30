using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models.ModelData;
using System.Windows.Forms;
using shipapp.Connections.HelperClasses;

namespace shipapp.Connections.DataConnections.Classes
{
    class BuildingConnClass : DatabaseConnection
    {
        object Sender { get; set; }
        public BuildingConnClass():base() { }
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
            }
        }
        public void WriteBuilding(BuildingClass building)
        {
            Write(building);
        }
        public void RemoveBuilding(BuildingClass building)
        {
            Delete(building);
        }
    }
}
