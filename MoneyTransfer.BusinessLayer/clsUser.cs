using MoneyTransfer.DataAccessLayer;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTransfer.BusinessLayer
{
    public class clsUser
    {
        private int _UserID { get; set; }
        private string _Username {get; set;}
        private string _PasswordHash {get; set;}
        private string _Role {get; set;}
        private bool IsActive {get; set;}

        private enum enMode { AddNewUser = 0, UpdateUser = 1 };
        private enMode _Mode;
        private enum enPermission
        {
            All = -1,
            ManageUser = 1,
            OpenShift = 2,
            ReviewShift = 4,
            ManageTransaction = 8,
            ManageWallets = 16,
            ManageAccountBalances = 32
        }

        public clsUser() {
            _UserID = -1;
            _Username = "";
            _PasswordHash = "";
            _Role = "";
            IsActive = false;
            _Mode = enMode.AddNewUser;
        }
        private clsUser(clsUserData.stUserInfo UserInfo)
        {
            _UserID = UserInfo.UserID;
            _Username = UserInfo.Username;
            _PasswordHash = UserInfo.PasswordHash;
            _Role = UserInfo.Role;
            IsActive = UserInfo.IsActive;
            _Mode = enMode.UpdateUser;
        }


        
    }
}
