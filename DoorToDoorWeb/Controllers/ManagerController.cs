using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoorToDoorLibrary.DAL;
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

        [HttpGet]
        public IActionResult Home()
        {
            ActionResult result = GetAuthenticatedView("Home", CreateManagerSalespersonListViewModel());

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
            ActionResult result = null;

            if (Role.IsManager)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        result = View("Home", CreateManagerSalespersonListViewModel());
                    }
                    else
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

                        result = RedirectToAction("Home", CreateManagerSalespersonListViewModel());
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("invalid", ex.Message);

                    result = View("Home", CreateManagerSalespersonListViewModel());
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

                    result = RedirectToAction("Home", CreateManagerSalespersonListViewModel());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError($"resetFailed{userID}", ex.Message);

                    result = RedirectToAction("Home", CreateManagerSalespersonListViewModel());
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

