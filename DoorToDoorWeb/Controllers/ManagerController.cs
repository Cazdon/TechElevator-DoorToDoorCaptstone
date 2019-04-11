﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoorToDoorLibrary.DAL;
using DoorToDoorLibrary.DatabaseObjects;
using DoorToDoorLibrary.Logic;
using DoorToDoorLibrary.Models;
using DoorToDoorWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoorToDoorWeb.Controllers
{
    public class ManagerController : AuthController
    {
        public ManagerController(IDoorToDoorDAL db, IHttpContextAccessor httpContext) : base(db, httpContext)
        {

        }

        private ManagerSalespersonListViewModel CreateManagerSalespersonListViewModel()
        {
            ManagerSalespersonListViewModel salespeoplelist = new ManagerSalespersonListViewModel();
            salespeoplelist.Salespeople = _db.GetMySalespeople(CurrentUser.Id);
            salespeoplelist.Register = new RegisterViewModel();

            return salespeoplelist;
        }


        private ManagerDashboardViewModel CreateManagerDashboardViewModel()
        {
            ManagerDashboardViewModel dashboard = new ManagerDashboardViewModel();
            dashboard.Transactions = _db.GetSalesmanTransactionData(CurrentUser.Id);
            dashboard.TotalSales = _db.GetTotalSales(CurrentUser.Id);

            return dashboard;
        }

        private ManagerHousesListViewModel CreateManagerHousesListViewModel()
        {
            ManagerHousesListViewModel houseListModel = new ManagerHousesListViewModel();
            houseListModel.Houses = _db.GetAllHouses(CurrentUser.Id);
            houseListModel.CreatedHouse = new CreateHouseViewModel();
            houseListModel.PossibleSalespeople = _db.GetMySalespeopleOptions(CurrentUser.Id);

            return houseListModel;

        }

        [HttpGet]
        public IActionResult Home()
        {
            ActionResult result = GetAuthenticatedView("Home", CreateManagerDashboardViewModel());

            if (Role.IsManager)
            {
                return result;
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpGet]
        public IActionResult Salespeople()
        {
            ActionResult result = GetAuthenticatedView("Salespeople", CreateManagerSalespersonListViewModel());

            if (Role.IsManager)
            {
                return result;
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpGet]
        public IActionResult Houses()
        {
            ActionResult result = GetAuthenticatedView("Houses", CreateManagerHousesListViewModel());

            if (Role.IsManager)
            {
                return result;
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult RegisterSalesperson(ManagerSalespersonListViewModel model)
        {
            ActionResult result = View("Salespeople", CreateManagerSalespersonListViewModel()); ;

            if (Role.IsManager)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        User newUser = new User()
                        {
                            FirstName = model.Register.FirstName,
                            LastName = model.Register.LastName,
                            EmailAddress = model.Register.EmailAddress,
                            Password = model.Register.Password,
                            ConfirmPassword = model.Register.ConfirmPassword,
                            RoleId = (int)RoleManager.eRole.Salesperson
                        };

                        int newSalespersonID = RegisterUser(newUser);

                        _db.PairManagerWithSalesperson(CurrentUser.Id, newSalespersonID);

                        result = RedirectToAction("Salespeople", CreateManagerSalespersonListViewModel());
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("invalid", ex.Message);
                }
            }
            else
            {
                result = RedirectToAction("Login", "Home");
            }

            return result;
        }

        [HttpPost]
        public ActionResult ResetPassword(int userID)
        {
            ActionResult result = null;

            if (Role.IsManager)
            {
                try
                {

                    _db.MarkResetPassword(userID);

                    TempData["resetSuccess"] = true;

                    result = RedirectToAction("Salespeople", CreateManagerSalespersonListViewModel());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError($"resetFailed{userID}", ex.Message);

                    result = RedirectToAction("Salespeople", CreateManagerSalespersonListViewModel());
                }
            }
            else
            {
                result = RedirectToAction("Login", "Home");
            }

            return result;
        }

        [HttpPost]
        public ActionResult CreateHouse(ManagerHousesListViewModel model)
        {
            ActionResult result = View("Houses", CreateManagerHousesListViewModel());

            if (Role.IsManager)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        HouseItem newHouse = new HouseItem()
                        {
                            Street = model.CreatedHouse.Street,
                            City = model.CreatedHouse.City,
                            District = model.CreatedHouse.District,
                            ZipCode = model.CreatedHouse.ZipCode,
                            Country = model.CreatedHouse.Country,
                            ManagerID = CurrentUser.Id,
                            AssignedSalespersonID = model.CreatedHouse.AssignedSalespersonID
                        };

                        _db.CreateHouse(newHouse);

                        result = RedirectToAction("Houses", CreateManagerHousesListViewModel());
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("invalid", ex.Message);
                }
            }
            else
            {
                result = RedirectToAction("Login", "Home");
            }

            return result;
        }
    }
}
