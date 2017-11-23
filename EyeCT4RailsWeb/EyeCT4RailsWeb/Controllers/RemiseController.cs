using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EyeCT4RailsWeb.Data;
using EyeCT4RailsWeb.Logic;
using EyeCT4RailsWeb.Models;

namespace EyeCT4RailsWeb.Controllers
{
    public class RemiseController : Controller
    {
        private SectorRepository sectorRepo = new SectorRepository(new SectorMSSQLContext());
        private TramRepository tramRepo = new TramRepository(new TramMSSQLContext(), new SectorMSSQLContext());

        // GET: Remise
        public ActionResult Index()
        {
            List<Tram> trams = new List<Tram>();
            trams = tramRepo.GetTrams();

            List<Sector> sectors = new List<Sector>();
            sectors = sectorRepo.GetAllSectors();

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("tramList", trams);
            dictionary.Add("sectorList", sectors);

            ViewBag.Error = TempData["error"];

            return View();
        }

        [HttpPost]
        public ActionResult Blokkeer(int blokkeer_spoor = 0, int blokkeer_sector = 0)
        {
            if (blokkeer_spoor > 0 && blokkeer_sector > 0)
            {
                sectorRepo.BlockSector(blokkeer_spoor, blokkeer_sector);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Verplaats(int tram_id = 0, int nieuwe_sector = 0, int nieuw_spoor = 0)
        {
            if (tram_id > 0 && nieuwe_sector > 0 && nieuw_spoor > 0)
            {
                Tram tram = tramRepo.GetTramByID(tram_id);
                
                Sector sector = sectorRepo.GetSectorByID(nieuw_spoor, nieuwe_sector);

                if (tram != null && sector != null)
                {
                    try
                    {
                        if (!tramRepo.ChangeTramSector(sector, tram))
                        {
                            TempData["error"] = "Tram kan niet verplaatst worden!";
                        }
                    }
                    catch
                    {
                        TempData["error"] = "Tram kan niet verplaatst worden!";
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Uitrijden(int tram_id = 0)
        {
            if (tram_id > 0)
            {
                Tram t = tramRepo.GetTramByID(tram_id);
                try
                {
                    tramRepo.LeaveRemise(t);
                }
                catch
                {
                    TempData["error"] = "Tram kan niet uitrijden!";
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Inrijden(int tram_id = 0)
        {
            if (tram_id > 0)
            {
                Tram t = tramRepo.GetTramByID(tram_id);
                tramRepo.DriveIntoRemise(t);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult OverzichtTrams()
        {
            return new RedirectResult("/Overzicht/Index");
        }

        public JsonResult GetRemiseTrams()
        {
            List<Tram> Trams = tramRepo.GetTrams();
            List<Tram> finalList = new List<Tram>();
            foreach (Tram tram in Trams)
            {
                if (tram.Sector.Spoor != 0 && tram.Sector.Nummer != 0)
                {
                    finalList.Add(tram);
                }
            }
            return Json(finalList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBlockedSectors()
        {
            List<Sector> Sectors = sectorRepo.GetAllSectors();
            List<Sector> finalList = new List<Sector>();
            foreach (Sector sector in Sectors)
            {
                if (sector.Blokkeren)
                {
                    finalList.Add(sector);
                }
            }
            return Json(finalList, JsonRequestBehavior.AllowGet);
        }
    }
}