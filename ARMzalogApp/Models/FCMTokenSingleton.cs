using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Models
{
    public class FCMTokenSingleton
    {
        private static FCMTokenSingleton instance;
        private string fcmToken;

        private FCMTokenSingleton() { }

        public static FCMTokenSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FCMTokenSingleton();
                }
                return instance;
            }
        }

        public string FCMToken
        {
            get { return fcmToken; }
            set { fcmToken = value; }
        }
    }

}
