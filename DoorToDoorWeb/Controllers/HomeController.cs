using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DoorToDoorWeb.Models;
using DoorToDoorLibrary.DAL;
using Microsoft.AspNetCore.Http;
using DoorToDoorLibrary.Exceptions;
using DoorToDoorLibrary.Logic;
using DoorToDoorLibrary.Models;

namespace DoorToDoorWeb.Controllers
{
    public class HomeController : AuthController
    {
        public HomeController(IDoorToDoorDAL db, IHttpContextAccessor httpContext) : base(db, httpContext)
        {

        }

        private ResetPasswordViewModel CreateResetPasswordViewModel()
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.EmailAddress = CurrentUser.EmailAddress;
            return model;
        }

        private ActionResult LoginRedirect()
        {
            ActionResult result = null;

            if (CurrentUser.UpdatePassword)
            {
                result = RedirectToAction("Reset", "Home");
            }
            else if (Role.IsAdministrator)
            {
                result = RedirectToAction("Home", "Administrator");
            }
            else if (Role.IsManager)
            {
                result = RedirectToAction("Home", "Manager");
            }
            else if (Role.IsSalesperson)
            {
                result = RedirectToAction("Home", "Salesperson");
            }

            return result;
        }

        private ProfileViewModel CreateProfileViewModel()
        {
            ProfileViewModel model = new ProfileViewModel();
            if (IsAuthenticated)
            {
                model.FirstName = CurrentUser.FirstName;
                model.LastName = CurrentUser.LastName;
                model.EmailAddress = CurrentUser.EmailAddress;
                model.UpdateProfile = new UpdateProfileViewModel();
                model.ResetPassword = new SelfResetPasswordViewModel();
                model.Role = Role.RoleName.ToString();
            }

            return model;
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
                    throw new Exception(" ");
                }

                LoginUser(model.EmailAddress, model.Password);

                result = LoginRedirect();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("invalid", ex.Message);

                result = View("Login");
            }

            return result;
        }

        [HttpGet]
        public ActionResult Reset()
        {
            ActionResult result = null;

            if (IsAuthenticated)
            {
                result = View(CreateResetPasswordViewModel());
            }
            else
            {
                result = RedirectToAction("Login");
            }

            return result;
        }

        [HttpPost]
        public ActionResult Reset(ResetPasswordViewModel model)
        {
            ActionResult result = View("Reset", CreateResetPasswordViewModel());

            if (IsAuthenticated && (model.EmailAddress.Equals(CurrentUser.EmailAddress)))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (!model.Password.Equals(model.ConfirmPassword))
                        {
                            throw new PasswordMatchException("The password and confirm password do not match.");
                        }

                        PasswordManager passHelper = new PasswordManager(model.Password);

                        bool passwordResetSuccess = _db.ResetPassword(model.EmailAddress, passHelper.Salt, passHelper.Hash);

                        if (passwordResetSuccess)
                        {
                            LoginUser(model.EmailAddress, model.Password);

                            result = RedirectToAction("ResetSuccess");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("invalid", ex.Message);
                }
            }
            else
            {
                result = RedirectToAction("Login");
            }

            return result;
        }

        [HttpGet]
        public ActionResult ResetSuccess()
        {
            ActionResult result = null;

            if (IsAuthenticated)
            {
                result = View();
            }
            else
            {
                result = RedirectToAction("Login");
            }

            return result;
        }

        [HttpPost]
        public ActionResult ResetRedirect()
        {
            ActionResult result = null;

            if (IsAuthenticated)
            {
                result = LoginRedirect();
            }
            else
            {
                result = RedirectToAction("Login");
            }

            return result;
        }

        [HttpGet]
        public ActionResult Logout()
        {
            LogoutUser();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return GetAuthenticatedView("Profile", CreateProfileViewModel());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
