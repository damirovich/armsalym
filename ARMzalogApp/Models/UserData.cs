using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Models
{
    public static class UserData
    {
        private static User _currentUser;

        public static User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = new User();
                }
                return _currentUser;
            }
        }

        public static void SetUserData(User user)
        {
            _currentUser = user;
        }
    }
}
