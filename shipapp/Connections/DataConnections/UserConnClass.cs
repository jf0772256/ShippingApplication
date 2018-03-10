using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models;

namespace shipapp.Connections.DataConnections
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
        public void Update1User(long id, string newPassword)
        {

        }
        public BindingList<User> GetManyUsers()
        {
            throw new NotImplementedException();
        }
        public void WriteManuUsers(BindingList<User> users)
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
    }
    class Authenticating
    {
        public Authenticating() { }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
