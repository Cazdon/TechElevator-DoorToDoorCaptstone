using DoorToDoorLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DoorToDoorLibrary.Exceptions;

namespace DoorToDoorLibrary.DAL
{
    public class DoorToDoorDAL : IDoorToDoorDAL
    {
        #region Properties and Variables

        private string _connectionString;

        private const string _getLastIdSql = "SELECT CAST(SCOPE_IDENTITY() AS int);";

        #endregion

        #region Constructor

        public DoorToDoorDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region User Methods

        /// <summary>
        /// Finds a single user in the database using the user's Email Address
        /// </summary>
        /// <param name="emailAddress">The desired user's Email Address</param>
        /// <returns>UserItem containing the user's information</returns>
        public UserItem GetUserItem(string emailAddress)
        {
            UserItem user = null;
            const string sql = "SELECT * From [Users] WHERE emailAddress = @EmailAddress;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@EmailAddress", emailAddress.ToLower());

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = GetUserItemFromReader(reader);
                }
            }

            if (user == null)
            {
                throw new Exception("User does not exist.");
            }

            return user;
        }

        /// <summary>
        /// Generates a UserItem from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>UserItem containing the information for a particular user</returns>
        private UserItem GetUserItemFromReader(SqlDataReader reader)
        {
            UserItem item = new UserItem();

            item.Id = Convert.ToInt32(reader["id"]);
            item.FirstName = Convert.ToString(reader["firstName"]);
            item.LastName = Convert.ToString(reader["lastName"]);
            item.EmailAddress = Convert.ToString(reader["emailAddress"]);
            item.Hash = Convert.ToString(reader["hash"]);
            item.Salt = Convert.ToString(reader["salt"]);
            item.RoleId = Convert.ToInt32(reader["roleID"]);
            item.UpdatePassword = Convert.ToBoolean(reader["updatePassword"]);

            return item;
        }

        //public UserItem GetManagerID()
        //{

        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        conn.Open();

        //        string sql = "SELECT id FROM [Users] WHERE id IN(SELECT salespersonID FROM Admin_Saleperson WHERE managerID = @ManagerID);";

        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("@ManagerID", managerID);

        //        SqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            UserItem newuser = GetUserItemFromReader(reader);
        //            output.Add(newuser);
        //        }
        //    }

        //    return output;
        //}

        public void RegisterNewUser(UserItem item)
        {
            int ID = 0;

            const string sql = "INSERT INTO [Users] (firstName, lastName, emailAddress, hash, salt, roleID, updatePassword) " +
                               "VALUES (@FirstName, @LastName, @EmailAddress, @Hash, @Salt, @RoleId, 1);";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + " " + _getLastIdSql, conn);
                cmd.Parameters.AddWithValue("@FirstName",  item.FirstName);
                cmd.Parameters.AddWithValue("@LastName", item.LastName);
                cmd.Parameters.AddWithValue("@EmailAddress", item.EmailAddress.ToLower());
                cmd.Parameters.AddWithValue("@Hash", item.Hash);
                cmd.Parameters.AddWithValue("@Salt", item.Salt);
                cmd.Parameters.AddWithValue("@RoleId", item.RoleId);

                try
                {
                    ID = (int)cmd.ExecuteScalar();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Returns a list of all Manager-type users from the system for Admin use
        /// </summary>
        /// <returns>List of Mangaer users</returns>
        public IList<UserItem> GetAllManagers()
        {
            List<UserItem> output = new List<UserItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM [Users] WHERE roleID = (SELECT id FROM Roles WHERE [name] = 'Manager');";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UserItem newuser = GetUserItemFromReader(reader);
                    output.Add(newuser);
                }
            }

            return output;
        }

        /// <summary>
        /// Returns a list of all Salesperson-type users from the system for a particular Manager
        /// </summary>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <returns>List of Salespeople under the given Manager</returns>
        public IList<UserItem> GetMySalespeople(int managerID)
        {
            List<UserItem> output = new List<UserItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM [Users] WHERE id IN(SELECT salespersonID FROM Admin_Saleperson WHERE managerID = @ManagerID);";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UserItem newuser = GetUserItemFromReader(reader);
                    output.Add(newuser);
                }
            }

            return output;
        }


        /// <summary>
        /// Set's the user's Reset Password flag. Throws error if unsuccessful
        /// </summary>
        /// <param name="userId">User's Database ID</param>
        public void MarkResetPassword(int userId)
        {
            int numRows = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "UPDATE Users SET updatePassword = 1 WHERE id = @UserID;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                numRows = cmd.ExecuteNonQuery();
            }

            if (numRows != 1)
            {
                throw new MarkResetPasswordFailedException();
            }
        }

        public bool ResetPassword(string emailAddress, string salt, string hash)
        {
            int numRows = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "UPDATE Users SET salt = @Salt, hash = @Hash, updatePassword = 0 WHERE emailAddress = @EmailAddress;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Salt", salt);
                cmd.Parameters.AddWithValue("@Hash", hash);
                cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);

                numRows = cmd.ExecuteNonQuery();
            }

            return numRows == 1 ? true : false;
        }

        #endregion
    }
}
