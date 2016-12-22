using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EyeCT4RailsWeb.Models;
using EyeCT4RailsWeb.Logic;
using EyeCT4RailsWeb.Data;

namespace EyeCT4RailsWeb.Controllers
{
    public class OverzichtController : Controller
    {
        // GET: Overzicht
        public ActionResult Index()
        {
            TramRepository tramRepo = new TramRepository(new TramMSSQLContext());
            List<Tram> TramList = new List<Tram>();
            TramList = tramRepo.GetTrams();         
            return View(TramList);
        }
    }
}