using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models;

namespace shipapp.Connections.DataConnections.Classes
{
    class UserConnClass:DatabaseConnection
    {
        public Authenticating Authenticate { get; set; }
        public UserConnClass():base() { Authenticate = new Authenticating(); }
        public User Get1User(long id)
        {
            return GetUser(id);
        }
        public User Get1User(string username)
        {
            return GetUser(username);
        }
        public void Write1User(User user)
        {
            Write_User_To_Database(user);
        }
        public void Update1User(long id, string[] columnNames, string[] newValues)
        {
            Update_User(id, columnNames, newValues);
        }
        public void GetManyUsers()
        {
            GetUserList();
        }
        public void WriteManyUsers(BindingList<User> users)
        {
            throw new NotImplementedException();
        }
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
        public void DeleteUser(User u)
        {
            throw new NotImplementedException();
        }
    }
    class Authenticating
    {
        public Authenticating() { }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
