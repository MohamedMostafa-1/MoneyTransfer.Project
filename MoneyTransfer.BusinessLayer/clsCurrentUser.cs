using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MoneyTransfer.BusinessLayer
{
    internal class clsCurrentUser
    {
        public static clsUser User  { get; private set; }
        public static bool IsAuthenticated => User != null;
        
        public static void SetUser(clsUser UserInfo)
        {
            User = UserInfo;
        }
        public static bool Logout()
        {
            User = null;
            return true;
        }


        //public clsCurrentUser(clsUser UserInfo)
        //{
        //    User = UserInfo;
        //}
    }
}
