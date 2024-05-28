using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Supermarket.Services
{
    public class UserSession
    {
        private static UserSession _instance;
        private static readonly object _lock = new object();

        public int UserId { get; private set; }
        public string Username { get; private set; }
        public string UserType { get; private set; }

        private UserSession() { }

        public static UserSession Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new UserSession();
                    }
                    return _instance;
                }
            }
        }

        public void SetUser(int userId, string username, string userType)
        {
            UserId = userId;
            Username = username;
            UserType = userType;
        }
    }

}
