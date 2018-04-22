using shipapp.Connections.HelperClasses;
using shipapp.Models;
using shipapp.Models.ModelData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shipapp.Connections.DataConnections.Classes
{
    /// <summary>
    /// User connection
    /// </summary>
    class UserConnClass:DatabaseConnection
    {
        /// <summary>
        /// Form Sender object
        /// </summary>
        object Sender { get; set; }
        /// <summary>
        /// Instance of the user athuentication class
        /// </summary>
        public Authenticating Authenticate { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public UserConnClass():base() { Authenticate = new Authenticating(); }
        /// <summary>
        /// gets user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User object</returns>
        public User Get1User(long id)
        {
            return GetUser(id);
        }
        /// <summary>
        /// Gets single user by user name
        /// </summary>
        /// <param name="username">user name</param>
        /// <returns>User object</returns>
        public User Get1User(string username)
        {
            return GetUser(username);
        }
        /// <summary>
        /// writes single user
        /// </summary>
        /// <param name="user">User to be added</param>
        public void Write1User(User user)
        {
            Write(user);
        }
        /// <summary>
        /// updates given user in database
        /// </summary>
        /// <param name="u">User to be updated</param>
        public void Update1User(User u)
        {
            Update(u);
        }
        /// <summary>
        /// gets list of users from the database
        /// </summary>
        /// <param name="sender">Form object</param>
        public async void GetManyUsers(object sender = null)
        {
            if (String.IsNullOrWhiteSpace(DataConnectionClass.ConnectionString))
            {
                return;
            }
            Sender = sender;
            SortableBindingList<User> users = await Task.Run(() => GetUserList());
            if (Sender is Manage)
            {
                Manage t = (Manage)Sender;
                DataConnectionClass.DataLists.UsersList = users;
                BindingSource bs = new BindingSource
                {
                    DataSource = DataConnectionClass.DataLists.UsersList
                };
                t.dataGridView1.DataSource = bs;
                try
                {
                    t.dataGridView1.Columns["Id"].Visible = false;
                    t.dataGridView1.Columns["PassWord"].Visible = false;
                    t.dataGridView1.Columns["Person_Id"].Visible = false;
                }
                catch (Exception)
                {
                    //
                }
            }
            else
            {
                DataConnectionClass.DataLists.UsersList = users;
            }
        }
        /// <summary>
        /// The authenticated user class gets test data
        /// </summary>
        /// <param name="tester">User being attepted to be logged in</param>
        /// <returns>Boolean</returns>
        public bool CheckAuth(User tester)
        {
            if (tester.Username == Authenticate.UserName && tester.PassWord == Authenticate.Password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Removes a user from the database
        /// </summary>
        /// <param name="u">user being removed</param>
        public void DeleteUser(User u)
        {
            Delete(u);
        }
        /// <summary>
        /// Get Notes from  the database by id
        /// </summary>
        /// <param name="pid">Person Id</param>
        /// <returns>List of Note object</returns>
        public List<Note> GetNotesList(string pid)
        {
            return GetNotesListById(pid);
        }
    }
    /// <summary>
    /// Authenticating Class sued during athenticating users
    /// </summary>
    class Authenticating
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Authenticating() { }
        /// <summary>
        /// Athenticating User Name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Authenticating User Password
        /// </summary>
        public string Password { get; set; }
    }
}
