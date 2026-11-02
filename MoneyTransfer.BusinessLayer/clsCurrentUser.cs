using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MoneyTransfer.BusinessLayer
{
    internal class CurrentUser
    {
        public static clsUser User  { get; private set; }
        //public static bool IsAuthenticated => User != null;
        public static  void setUser (clsUser user)
        {
            User = user;
        }
        public static bool Logout()
        {
            User = null;
            return true;
        }
    }
}
