using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoorToDoorLibrary.DAL;
using DoorToDoorLibrary.DatabaseObjects;
using DoorToDoorWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoorToDoorWeb.Controllers
{
    public class SalespersonController : AuthController
    {
        public SalespersonController(IDoorToDoorDAL db, IHttpContextAccessor httpContext) : base(db, httpContext)
        {

        }

        private SalespersonHousesListViewModel CreateSalespersonHousesListViewModel()
        {
            SalespersonHousesListViewModel houseListModel = new SalespersonHousesListViewModel();
            houseListModel.Houses = _db.GetAassignedHouses(CurrentUser.Id);

            return houseListModel;
        }

        [HttpGet]
        public IActionResult Home()
        {
            ActionResult result = GetAuthenticatedView("Home");

            if (Role.IsSalesperson)
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
            ActionResult result = GetAuthenticatedView("Houses", CreateSalespersonHousesListViewModel());

            if (Role.IsSalesperson)
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

            if (!Role.IsSalesperson)
            {
                result = RedirectToAction("Login", "Home");
            }
            else if (house.AssignedSalespersonID != CurrentUser.Id)
            {
                ModelState.AddModelError("not-your-house", "You do not have permission to see this house");
                result = View("Houses", CreateSalespersonHousesListViewModel());
            }

            return result;
        }
    }
}