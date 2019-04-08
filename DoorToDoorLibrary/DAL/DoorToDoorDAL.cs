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

        public UserItem GetUserItem(string emailAddress)
        {
            UserItem user = null;
            const string sql = "SELECT * From [Users] WHERE emailAddress = @EmailAddress;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
                var reader = cmd.ExecuteReader();

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

        //public List<RegisterViewModel> GetUsers()
        //{
        //    List<RegisterViewModel> output = new List<RegisterViewModel>();

        //    //Create a SqlConnection to our database
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        string getSurveySQLString = "select user.firstName, user.lastName" +
        //                                    "from Users " +
        //                                    "join Roles on Users.RoleID = Roles.id " +
        //                                    "group by Role.id, order by firstName";

        //        SqlCommand cmd = new SqlCommand(getSurveySQLString, connection);

        //        // Execute the query to the database
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        // The results come back as a SqlDataReader. Loop through each of the rows
        //        // and add to the output list
        //        while (reader.Read())
        //        {
        //            RegisterViewModel newuser = new RegisterViewModel();
        //            newuser.FirstName = Convert.ToString(reader["firstName"]);
        //            newuser.LastName = Convert.ToString(reader["lastName"]);
        //            newuser.EmailAddress = Convert.ToInt32(reader["emailAddress"]);
        //            newuser.Password = Convert.ToInt32(reader["password"]);
        //            output.Add(newuser);
        //        }
        //    }
        //    return output;
        //}

        #endregion
    }
}
