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
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
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
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
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

        public static bool DeleteUser(int UserID)
        {

            string Query = @"Update Users 
                                set IsActive = 0
                                Where UserID = @UserID
                                        And IsActive = 1";

            int rowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;

                    connection.Open();

                    rowAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Message: " + ex.Message);
            }
            //finally
            //{
            //    connection.Close();
            //}
            return (rowAffected > 0);
        }
        public static bool FindUserByID(int UserID, ref stUserInfo UserInfo)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"Select * from Users
                                Where UserID = @UserID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;

                    UserInfo.UserID = UserID;
                    UserInfo.Username =(string) reader["Username"];
                    UserInfo.PasswordHash =(string) reader["PasswordHash"];
                    UserInfo.Role =(string) reader["Role"];
                    UserInfo.IsActive =(bool) reader["IsActive"];
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
            return isFound;
        }
        public static bool FindUserByUsername(string Username, ref stUserInfo UserInfo)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"Select * from Users
                                Where Username = @Username";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@Username", Username);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;

                    UserInfo.UserID = (int)reader["UserID"];
                    UserInfo.Username = Username;
                    UserInfo.PasswordHash =(string) reader["PasswordHash"];
                    UserInfo.Role =(string) reader["Role"];
                    UserInfo.IsActive =(bool) reader["IsActive"];
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
            return isFound;
        }

        public static bool IsUserExist(int UserID)
        {
            string Query = @"Select found = 1 From User Where  UserID = @UserID";

            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using( SqlCommand command = new SqlCommand(Query, connection)) 
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                        IsFound = true;
                    else
                        IsFound = false;
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Message: " + ex.Message);
            }

            return IsFound;
        }
    }
}
