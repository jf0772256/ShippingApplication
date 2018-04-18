using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models;
using shipapp.Models.ModelData;
using System.Windows.Forms;
using shipapp.Connections.HelperClasses;
using Extentions;

namespace shipapp.Connections.DataConnections.Classes
{
    class EmployeeConnClass:DatabaseConnection
    {
        object Sender { get; set; }
        public EmployeeConnClass() : base() { }

        public Faculty GetFaculty(long id)
        {
            return Get_Faculty(id);
        }
        /// <summary>
        /// Adds new faculty to the database
        /// </summary>
        /// <param name="f">Faculty object</param>
        public void AddFaculty(Faculty f)
        {
            Write(f);
        }
        /// <summary>
        /// Updates currect faculty in the database, DO NOT MODIFY OR CHANGE THE IDS OR PERSION ID!!!
        /// </summary>
        /// <param name="f">Modified faculty object</param>
        public void UpdateFaculty(Faculty f)
        {
            Update(f);
        }
        public async void GetAllAfaculty(object sender = null)
        {
            if (String.IsNullOrWhiteSpace(DataConnectionClass.ConnectionString))
            {
                return;
            }
            Sender = sender;
            SortableBindingList<Faculty> fac = await Task.Run(() => Get_Faculty_List());
            SortableBindingList<BuildingClass> b = Get_Building_List();
            foreach (Faculty f in fac)
            {
                f.Building_Name = b.FirstOrDefault(i => f.Building_Id == i.BuildingId).ToString();
            }
            if (Sender is Manage)
            {
                Manage t = (Manage)Sender;
                DataConnectionClass.DataLists.FacultyList = fac;
                BindingSource bs = new BindingSource
                {
                    DataSource = DataConnectionClass.DataLists.FacultyList
                };
                t.dataGridView1.DataSource = bs;
                try
                {
                    t.dataGridView1.Columns["Id"].Visible = false;
                    t.dataGridView1.Columns["Faculty_PersonId"].Visible = false;
                    t.dataGridView1.Columns["Building_Id"].Visible = false;
                }
                catch (Exception)
                {
                    //
                }
            }
            else
            {
                DataConnectionClass.DataLists.FacultyList = fac;
            }
        }
        public void DeleteFaculty(Faculty f)
        {
            Delete(f);
        }
        public List<Note> GetNotesList(string pid)
        {
            return GetNotesListById(pid);
        }
    }
}
