using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DoorToDoorWeb.Models;
using DoorToDoorLibrary.DAL;
using Microsoft.AspNetCore.Http;

namespace DoorToDoorWeb.Controllers
{
    public class HomeController : AuthController
    {
        public HomeController(IDoorToDoorDAL db, IHttpContextAccessor httpContext) : base(db, httpContext)
        {

        }

        [HttpGet]
        public ActionResult Login()
        {
            if (IsAuthenticated)
            {
                LogoutUser();
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            ActionResult result = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                LoginUser(model.EmailAddress, model.Password);

                if (Role.IsAdministrator)
                {
                    result = RedirectToAction("Index", "Administrator");
                }
                else if (Role.IsManager)
                {
                    result = RedirectToAction("Index", "Manager");
                }
                else if (Role.IsSalesperson)
                {
                    result = RedirectToAction("Index", "Salesperson");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("invalid", ex.Message);

                result = View("Login");
            }

            return result;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
