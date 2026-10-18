using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public static int AddNewUser(stUserInfo userInfo)
        {
            SqlConnection connection = new SqlConnection();
            string Query = @"Insert into Users(Username ,PasswordHash , Role , IsActive)
                                    Value (@Username , @PasswordHash , @Role , @IsActive)";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@Username", userInfo.Username);
            command.Parameters.AddWithValue("@PasswordHash", userInfo.PasswordHash);
            command.Parameters.AddWithValue("@Role", userInfo.Role);
            command.Parameters.AddWithValue("@IsActive", userInfo.IsActive);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int InsertID))
                {
                    return InsertID;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Message: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return -1;
        }

        public static bool UpdateUser(stUserInfo userInfo)
        {
            SqlConnection connection = new SqlConnection();
            string Query = @"Update Users
                                set  Username =@Username
                                    ,PasswordHash =@PasswordHash 
                                    , Role  =  @Role
                                    ,IsActive = @IsActive
                                where UserID = @UserID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@Username", userInfo.Username);
            command.Parameters.AddWithValue("@PasswordHash", userInfo.PasswordHash);
            command.Parameters.AddWithValue("@Role", userInfo.Role);
            command.Parameters.AddWithValue("@IsActive", userInfo.IsActive);
            command.Parameters.AddWithValue("@UserID" , userInfo.UserID);

            int rowAffected = 0;
            try
            {
                connection.Open();

                rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Message: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return (rowAffected > 0);
        }
    }
}
