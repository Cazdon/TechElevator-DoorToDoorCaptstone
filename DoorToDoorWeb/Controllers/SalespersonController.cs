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

        private SalespersonDashboardViewModel CreateSalespersonDashboardViewModel()
        {
            SalespersonDashboardViewModel dashboardViewModel = new SalespersonDashboardViewModel();

            return dashboardViewModel;
        }

        private SalespersonHousesListViewModel CreateSalespersonHousesListViewModel()
        {
            SalespersonHousesListViewModel houseListModel = new SalespersonHousesListViewModel();
            houseListModel.Houses = _db.GetAassignedHouses(CurrentUser.Id);

            return houseListModel;
        }

        private HouseDetailsViewModel CreateHouseDetailsViewModel(int houseID)
        {
            HouseDetailsViewModel model = new HouseDetailsViewModel();

            model.House = _db.GetHouse(houseID);
            model.Notes = _db.GetHouseNotes(houseID);
            model.AddNote = new AddHouseNoteViewModel();

            return model;
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
            HouseDetailsViewModel model = CreateHouseDetailsViewModel(houseID);

            ActionResult result = GetAuthenticatedView("HouseDetails", model);
            
            if (!Role.IsSalesperson)
            {
                result = RedirectToAction("Login", "Home");
            }
            else if (model.House.AssignedSalespersonID != CurrentUser.Id)
            {
                ModelState.AddModelError("not-your-house", "You do not have permission to see this house");
                result = View("Houses", CreateSalespersonHousesListViewModel());
            }

            return result;
        }

        [HttpPost]
        public ActionResult AddHouseNote(HouseDetailsViewModel model)
        {
            ActionResult result = View("HouseDetails", CreateHouseDetailsViewModel(model.AddNote.HouseID));

            if (Role.IsSalesperson)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        NoteItem newNote = new NoteItem()
                        {
                            HouseID = model.AddNote.HouseID,
                            UserID = CurrentUser.Id,
                            Note = model.AddNote.Note,
                            SubmittedDate = DateTime.Now
                        };

                        _db.AddHouseNote(newNote);

                        result = RedirectToAction("HouseDetails", new { houseID = model.AddNote.HouseID });
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