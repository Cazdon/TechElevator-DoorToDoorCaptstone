using DoorToDoorLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DoorToDoorLibrary.DAL
{
    public class DoorToDoorDAL : IDoorToDoorDAL
    {
        #region Properties and Variables

        private string _connectionString;

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
                cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);

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

        //public void RegisterNewUser(RegisterViewModel newuser)
        //{
        //    // Create a new connection object
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        // Open the connection
        //        conn.Open();
        //        string saveSurvey = "INSERT into Users (firstName,lastName,emailAddress)" +
        //            "VALUES (@,@FirstName,@LastName,@EmailAddress)";

        //        SqlCommand cmd = new SqlCommand(saveSurvey, conn);

        //        cmd.Parameters.AddWithValue("@firstName", newuser.FirstName);
        //        cmd.Parameters.AddWithValue("@lastName", newuser.LastName);
        //        cmd.Parameters.AddWithValue("@emailAddress", newuser.EmailAddress);
        //        //cmd.Parameters.AddWithValue("@password", newuser.Password);

        //        cmd.ExecuteScalar();
        //    }
        //}

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

        #endregion
    }
}
