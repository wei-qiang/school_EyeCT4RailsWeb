using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EyeCT4RailsWeb.Models;
using EyeCT4RailsWeb.Logic;
using EyeCT4RailsWeb.Data;
using System.Web.UI;

namespace EyeCT4RailsWeb.Controllers
{
    public class OverzichtController : Controller
    {
        private TramRepository tramRepo = new TramRepository(new TramMSSQLContext());
        // GET: Overzicht
        public ActionResult Index()
        {
            ViewBag.ViewMessage = Convert.ToString(TempData["Text"]);
            List<Tram> TramList = new List<Tram>();
            TramList = tramRepo.GetTrams();         
            return View(TramList);
        }

        [HttpPost]
        public ActionResult ChangeStatus(string status = "Remise", int tram_id = 0, int Prioriteit = 0)
        {
            Status s;
            if (tram_id > 0)
            {
                Tram t = new Tram(tram_id);
                try
                { 
                    s = (Status)Enum.Parse(typeof(Status), status);
                }
                catch (Exception e)
                {
                    TempData["Text"] = "Je hebt geen geldige status opgegeven.";
                    return RedirectToAction("Index");                 
                }
                if(tramRepo.ChangeStatusTram(s, t, Prioriteit))
                {
                    TempData["Text"] = "Het veranderen van de status is gelukt";
                }
                else
                {
                    TempData["Text"] = "Het veranderen van de status is Mislukt";
                }
            }
            else
            {
                TempData["Text"] = "Het veranderen van de status is Mislukt";
            }
            return RedirectToAction("Index");
        }
    }
}