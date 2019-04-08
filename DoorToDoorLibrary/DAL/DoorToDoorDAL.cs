using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DoorToDoorWeb.Models;
namespace DoorToDoorLibrary.DAL
{
    public class DoorToDoorDAL : IDoorToDoorDAL
    {
        #region Properties and Variables
        private static string connectionString;
        #endregion

        #region Constructor
        public DoorToDoorDAL(string connString)
        {
            connectionString = connString;
        }
        #endregion

        #region Methods
      

        public RegisterViewModel MapToRegisterViewModel(SqlDataReader reader)
        {
            return new RegisterViewModel()
            {
                FirstName = Convert.ToString(reader["firstName"]),
                LastName = Convert.ToString(reader["lastName"]),
                EmailAddress = Convert.ToString(reader["emailAddress"]),
                //Password = Convert.ToString(reader["password"]),
            };

        }
        
        public void RegisterNewUser(RegisterViewModel newuser)
        {
            // Create a new connection object
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Open the connection
                conn.Open();
                string saveSurvey = "INSERT into Users (firstName,lastName,emailAddress)" +
                    "VALUES (@,@FirstName,@LastName,@EmailAddress)";

                SqlCommand cmd = new SqlCommand(saveSurvey, conn);

                cmd.Parameters.AddWithValue("@firstName", newuser.FirstName);
                cmd.Parameters.AddWithValue("@lastName", newuser.LastName);
                cmd.Parameters.AddWithValue("@emailAddress", newuser.EmailAddress);
                //cmd.Parameters.AddWithValue("@password", newuser.Password);

                cmd.ExecuteScalar();
            }
        }

        public List<RegisterViewModel> GetUsers()
        {
            List<RegisterViewModel> output = new List<RegisterViewModel>();

            //Create a SqlConnection to our database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string getSurveySQLString = "select user.firstName, user.lastName" +
                                            "from Users " +
                                            "join Roles on Users.RoleID = Roles.id " +
                                            "group by Role.id, order by firstName";

                SqlCommand cmd = new SqlCommand(getSurveySQLString, connection);

                // Execute the query to the database
                SqlDataReader reader = cmd.ExecuteReader();

                // The results come back as a SqlDataReader. Loop through each of the rows
                // and add to the output list
                while (reader.Read())
                {
                    RegisterViewModel newuser = new RegisterViewModel();
                    newuser.FirstName = Convert.ToString(reader["firstName"]);
                    newuser.LastName = Convert.ToString(reader["lastName"]);
                    newuser.EmailAddress = Convert.ToInt32(reader["emailAddress"]);
                    newuser.Password = Convert.ToInt32(reader["password"]);
                    output.Add(newuser);
                }
            }
            return output;
        }
        #endregion
    }
}
