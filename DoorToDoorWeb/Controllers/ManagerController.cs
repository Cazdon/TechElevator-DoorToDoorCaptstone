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
            dashboard.SalesmanRankByQuantity = _db.GetTopSalesmenByQuantity(CurrentUser.Id);
            dashboard.SalesmanRankByRevenue = _db.GetTopSalesmenByRevenue(CurrentUser.Id);
            dashboard.HouseRankByQuantity = _db.GetTopHouseByQuantity(CurrentUser.Id);
            dashboard.HouseRankByRevenue = _db.GetTopHouseByRevenue(CurrentUser.Id);
            dashboard.TotalSales = _db.GetTotalSales(CurrentUser.Id);
            dashboard.TotalRevenue = _db.GetTotalRevenue(CurrentUser.Id);

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

        private ManagerProductsListViewModel CreateManagerProductsListViewModel()
        {
            ManagerProductsListViewModel houseListModel = new ManagerProductsListViewModel();
            houseListModel.Products = _db.GetMyProducts(CurrentUser.Id);
            houseListModel.CreatedProduct = new CreateProductViewModel();

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

        [HttpGet]
        public IActionResult HouseDetails(int houseID)
        {
            HouseItem house = _db.GetHouse(houseID);

            ActionResult result = GetAuthenticatedView("HouseDetails", house);

            if (!Role.IsManager)
            {
                result = RedirectToAction("Login", "Home");
            }
            else if (house.ManagerID != CurrentUser.Id)
            {
                ModelState.AddModelError("not-your-house", "You do not have permission to see this house");
                result = View("Houses", CreateManagerHousesListViewModel());
            }

            return result;
        }

        [HttpGet]
        public IActionResult Products()
        {
            ActionResult result = GetAuthenticatedView("Products", CreateManagerProductsListViewModel());

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
                    string newPassword = GenerateNewPassword();

                    _db.MarkResetPassword(userID, newPassword);

                    TempData["tempPassword"] = newPassword;

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

        [HttpPost]
        public ActionResult CreateProduct(ManagerProductsListViewModel model)
        {
            ActionResult result = View("Products", CreateManagerProductsListViewModel());

            if (Role.IsManager)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        _db.CreateProduct(model.CreatedProduct.Name, CurrentUser.Id);

                        result = RedirectToAction("Products", CreateManagerProductsListViewModel());
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

