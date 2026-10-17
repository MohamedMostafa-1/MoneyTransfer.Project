using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTransfer.DataAccessLayer
{
    public struct stUserInfo
    {
        public int UserID;
        public string Username;
        public string PasswordHash;
        public string Role;
        public bool IsActive;
    }
    public class clsUserData
    {
        public static DataTable GetAllUsers()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"Select * From Users";

            SqlCommand command = new SqlCommand(Query ,connection);
            DataTable dtUsers = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtUsers.Load(reader);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Message: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dtUsers;
        } 
    }
}
