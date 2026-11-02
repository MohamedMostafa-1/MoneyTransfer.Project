using MoneyTransfer.DataAccessLayer;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTransfer.BusinessLayer
{
    internal class clsAuthentication
    {
       
        public static bool  Login(string Username , string Password , ref string MessageError)
        {
            bool LoginStatus = false;

            clsUser User = clsUser.Find(Username);

            if (User != null)
            {
                if (clsUserData.IsUserExistAndActive(Username))
                {
                    if (clsPasswordHelper.VerifyPassword(Password))
                    {
                        clsCurrentUser currentUser = new clsCurrentUser(User);
                        LoginStatus = true;
                    }
                    else
                    {
                        MessageError = "Password Not Correct >_<";
                        LoginStatus = false;
                    }
                }
                else
                {
                    MessageError = "User is  Not Active >_<";
                    LoginStatus = false;
                }
            }
            else
            {
                MessageError = "User is Not Exist >_<";
                LoginStatus = false;
            }
            return LoginStatus;
        }

        public static bool Logout()
        {
            return (clsCurrentUser.Logout());
        }
    }
}
