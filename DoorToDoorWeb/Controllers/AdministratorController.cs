using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoorToDoorLibrary.DAL;
using DoorToDoorLibrary.Models;
using DoorToDoorWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoorToDoorWeb.Controllers
{
    public class AdministratorController : AuthController
    {
        public AdministratorController(IDoorToDoorDAL db, IHttpContextAccessor httpContext) : base(db, httpContext)
        {

        }

        [HttpGet]
        public IActionResult Home()
        {
            ManagerListViewModel managerList = new ManagerListViewModel();
            managerList.Managers = _db.GetAllManagers();

            ActionResult result = GetAuthenticatedView("Home", managerList);

            if (Role.IsAdministrator)
            {
                return result;
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}