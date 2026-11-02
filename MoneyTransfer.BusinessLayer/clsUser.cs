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
        public int UserID { get; private set; }
        public string Username {get; set;}
        public string PasswordHash {get; set;}
        public string Role {get; set;}
        public bool IsActive {get; set;}
        public int Permission { get; set;}

        private enum enMode { AddNewUser = 0, UpdateUser = 1 };
        private enMode _Mode;
        public enum enPermission
        {
            All = -1,
            ManageUser = 1,
            OpenShift = 2,
            ManageShifts = 4,
            ManageTransaction = 8,
            ManageWallets = 16,
            ManageAccountBalances = 32
        }

        public clsUser() {
            UserID = -1;
            Username = "";
            PasswordHash = "";
            Role = "";
            IsActive = false;
            Permission = -1;
            _Mode = enMode.AddNewUser;
        }
        private clsUser(clsUserData.stUserInfo UserInfo)
        {
            UserID = UserInfo.UserID;
            Username = UserInfo.Username;
            PasswordHash = UserInfo.PasswordHash;
            Role = UserInfo.Role;
            IsActive = UserInfo.IsActive;
            Permission = UserInfo.Permission;
            _Mode = enMode.UpdateUser;
        }
        private static clsUserData.stUserInfo stDALUser = new clsUserData.stUserInfo();
        public  bool CheckPermission(enPermission ePermission)
        {
            if (ePermission == enPermission.All)
                return true;

            return (Permission & (int)ePermission) == (int)ePermission;
         
        }
        public void ReadPermission(bool FullAccess , bool ManageUser, bool ManageTransaction, bool ManageWallets,
                                   bool ManegeAcountBalance, bool ManageShifts , bool OpenShift) {
            int Permission = 0;

            if (FullAccess)
            {
                Permission = (int)enPermission.All;

            }
            else
            {
                if (ManageUser)
                    Permission += (int)enPermission.ManageUser;

                if (ManageTransaction)
                    Permission += (int)enPermission.ManageTransaction;

                if (ManageShifts)
                    Permission += (int)enPermission.ManageShifts;

                if (ManegeAcountBalance)
                    Permission += (int)enPermission.ManageAccountBalances;

                if (ManageWallets)
                    Permission += (int)enPermission.ManageUser;

                if (OpenShift)
                    Permission += (int)enPermission.ManageUser;

               
            }

                this.Permission = Permission;
        }
        private void _FillDALUserByObject( ref clsUserData.stUserInfo stDALUser)
        {
            stDALUser.Username = this.Username;
            stDALUser.PasswordHash = this.PasswordHash;
            stDALUser.Role = this.Role;
            stDALUser.Permission = this.Permission;
            stDALUser.IsActive = this.IsActive;
            
        }
        private bool _AddNewUser()
        {
            _FillDALUserByObject(ref stDALUser);

            this.UserID= clsUserData.AddNewUser(stDALUser);
            stDALUser.UserID = this.UserID;

            return (stDALUser.UserID != -1);

        }
        private bool _UpdateUser()
        {
            if (!clsUserData.IsUserExist(UserID))
                return false;

            _FillDALUserByObject(ref stDALUser);

            return clsUserData.UpdateUser(stDALUser);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNewUser:
                    if (_AddNewUser())
                    {
                        _Mode = enMode.UpdateUser;
                        return true;
                    }
                    else
                        return false;
                    
                case enMode.UpdateUser:
                    return _UpdateUser();
                    
                   
 
            }
            return false;
        }

        public static clsUser Find(int UserID) 
        {
            //clsUserData.stUserInfo stUser = new clsUserData.stUserInfo();

            if (clsUserData.FindUserByID(UserID, ref stDALUser))
                return new clsUser(stDALUser);
            else
                return null;
        }
        public static clsUser Find(string Username) 
        {
            //clsUserData.stUserInfo stUser = new clsUserData.stUserInfo();

            if (clsUserData.FindUserByUsername(Username, ref stDALUser))
                return new clsUser(stDALUser);
            else
                return null;
        }
        public static bool DeactivateUser(int UserID)
        {
            if (!clsUserData.IsUserExist(UserID))
                return false;

            return clsUserData.DeactivateUser(UserID);
        }
        public static bool ActivateUser(int UserID)
        {
            if (!clsUserData.IsUserExist(UserID))
                return false;

            return clsUserData.ReActiveUser(UserID);
        }


        /*
            AddNewUser() Done
            UpdateUser() Done
            FindUser(id) Done
            FindUser(username) Done
            ActivateUser() Done
            DeactivateUser() Done
            Save()  Done
            CheckPermission() Done
            ReadPermission()  Done
            _FillDALUserByObject() Done
         */

    }
}
