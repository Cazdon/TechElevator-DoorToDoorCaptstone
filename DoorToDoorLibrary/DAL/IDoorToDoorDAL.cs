﻿using DoorToDoorLibrary.DatabaseObjects;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.DAL
{
    public interface IDoorToDoorDAL
    {
        #region User Methods
        /// <summary>
        /// Finds a single user in the database using the user's Email Address
        /// </summary>
        /// <param name="emailAddress">The desired user's Email Address</param>
        /// <returns>UserItem containing the user's information</returns>
        UserItem GetUserItem(string emailAddress);

        /// <summary>
        /// Creates a new User in the database
        /// </summary>
        /// <param name="item">The user to be created</param>
        /// <returns>ID of the created User</returns>
        int RegisterNewUser(UserItem item);

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

        /// <summary>
        /// Returns a Select List of all Salesperson-type users from the system for a particular Manager
        /// </summary>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <returns>Select List of Salespeople under the given Manager</returns>
        IList<SelectListItem> GetMySalespeopleOptions(int managerID);

        /// <summary>
        /// Set's the user's Reset Password flag. Throws error if unsuccessful
        /// </summary>
        /// <param name="userId">User's Database ID</param>
        void MarkResetPassword(int userId, string newPassword);

        /// <summary>
        /// Reset's the given User's Password values
        /// </summary>
        /// <param name="emailAddress">Email Address of the User</param>
        /// <param name="salt">New Salt value for the User</param>
        /// <param name="hash">New Hash value for the User</param>
        /// <returns>True if successful, false if unsuccessful</returns>
        bool ResetPassword(string emailAddress, string salt, string hash);

        /// <summary>
        /// Pairs the current logged in Manager with the newly created Salesperson
        /// </summary>
        /// <param name="managerID">User ID of the Manager</param>
        /// <param name="SalespersonID">User ID of the Salesperson</param>
        void PairManagerWithSalesperson(int managerID, int SalespersonID);

        #endregion

        #region Houses Methods

        /// <summary>
        /// Returns a list of Houses associated to the given Manager
        /// </summary>
        /// <param name="managerID">User ID of the Manager</param>
        /// <returns>List of Houses associated to that Manager</returns>
        IList<HouseItem> GetAllHouses(int managerID);

        /// <summary>
        /// Returns a list of Houses associated to the given Salesperson
        /// </summary>
        /// <param name="managerID">User ID of the Manager</param>
        /// <returns>List of Houses associated to that Manager</returns>
        IList<HouseItem> GetAassignedHouses(int salespersonID);

        /// <summary>
        /// Retrieves a specific House from the Database
        /// </summary>
        /// <param name="houseID">Database ID of the House</param>
        /// <returns>HouseItem containing the House's information</returns>
        HouseItem GetHouse(int houseID);

        /// <summary>
        /// Creates a new House in the database
        /// </summary>
        /// <param name="item">The House to be created</param>
        /// <returns>ID of the created House</returns>
        int CreateHouse(HouseItem item);

        /// <summary>
        /// Returns a list of Notes associated with the given House
        /// </summary>
        /// <param name="houseID">Database ID of the House</param>
        /// <returns>List of NoteItems associated with that House</returns>
        IList<NoteItem> GetHouseNotes(int houseID);

        /// <summary>
        /// Creates a Note for a House
        /// </summary>
        /// <param name="note">The Note to be created</param>
        /// <returns>Database ID of the Note</returns>
        int AddHouseNote(NoteItem note);

        #endregion

        #region Manager Dashboard Methods

        /// <summary>
        /// Generates the top salesman based on amount of sales for current manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>A list of SalesmanCountItem</returns>
        IList<SalesmanSalesCountItem> GetTopSalesmenByQuantity(int managerID);

        /// <summary>
        /// Generates the top salesman based on total revenue of sales for current manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>A list of SalesRevenueItem</returns>
        IList<SalesmanRevenueItem> GetTopSalesmenByRevenue(int managerID);

        IList<HouseSalesCountItem> GetTopHouseByQuantity(int managerID);

        /// <summary>
        /// Generates the top house based on total revenue of sales for current manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>A list of HouseRevenueItems</returns>
        IList<HouseRevenueItem> GetTopHouseByRevenue(int managerID);

        /// <summary>
        /// Gets the total number of transactions that have taken place under this manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>The total number of transactions from a manager's salesmen as an Int</returns>
        int GetManagerTotalSales(int managerID);

        /// <summary>
        /// Gets the total amount of revenue generated under this manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>The total revenue from manager as an Int</returns>
        double GetManagerTotalRevenue(int managerID);

        #endregion

        #region Salesman Dashboard Methods

        /// <summary>
        /// Generates a list of houses that salesman has assigned.
        /// </summary>
        /// <param name="managerID"></param>
        IList<HouseDashboardItem> GetSalesmanDashboardHouses(int salesmanID);

        /// <summary>
        /// Generates a list of products the salesman has avaliable.
        /// </summary>
        /// <param name="managerID"></param>
        IList<ProductDashboardItem> GetSalesmanDashboardProducts(int salesmanID);

        /// <summary>
        /// Gets the total number of transactions that have taken place from this Salesman.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>The total number of transactions from a salesmen as an Int</returns>
        int GetSalesmanDashboardSales(int salesmanID);

        /// <summary>
        /// Gets the total amount of revenue generated from Salesman.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>The total revenue from Salesman as an Int</returns>
        double GetSalesmanDashboardTotalRevenue(int salemanID);

        #endregion

        #region Product Methods

        /// <summary>
        /// Retrieves a list of Products associated with the given Manager
        /// </summary>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <returns>List of Products associated with the manager</returns>
        IList<ProductItem> GetMyProducts(int managerID);

        /// <summary>
        /// Creates a Product in the Database with the given name and associates that Product with the given Manager
        /// </summary>
        /// <param name="productName">Name of the new Product</param>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <returns>Newly created Product's ID</returns>
        int CreateProduct(string productName, int managerID);

        #endregion

     
    }
}
