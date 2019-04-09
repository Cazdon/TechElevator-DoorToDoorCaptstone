using DoorToDoorLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.DAL
{
    public interface IDoorToDoorDAL
    {
        /// <summary>
        /// Finds a single user in the database using the user's Email Address
        /// </summary>
        /// <param name="emailAddress">The desired user's Email Address</param>
        /// <returns>UserItem containing the user's information</returns>
        UserItem GetUserItem(string emailAddress);

        /// <summary>
        /// Returns a list of all Manager-type users from the system for Admin use
        /// </summary>
        /// <returns>List of Mangaer users</returns>
        IList<UserItem> GetAllManagers();

        /// <summary>
        /// Returns a list of all Salesperson-type users from the system for a particular Manager
        /// </summary>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <returns>List of Salespeople under the given Manager</returns>
        IList<UserItem> GetMySalespeople(int managerID);
    }
}
