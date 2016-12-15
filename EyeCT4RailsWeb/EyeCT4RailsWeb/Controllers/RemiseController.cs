﻿using System;
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
        private TramRepository tramRepo = new TramRepository(new TramMSSQLContext());

        // GET: Remise
        public ActionResult Index()
        {
            /*
            List<Tram> trams = new List<Tram>();
            trams = tramRepo.GetTrams();

            List<Sector> sectors = new List<Sector>();
            sectors = sectorRepo.GetAllSectors();

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("tramList", trams);
            dictionary.Add("sectorList", sectors);
            */
            return View();
        }

        [HttpPost]
        public ActionResult Blokkeer(int blokkeer_spoor = 0)
        {
            if (blokkeer_spoor > 0)
            {
                //repo.BlockSector(Convert.ToInt32(blokkeer_spoor));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Verplaats(int originele_sector = 0, int nieuwe_sector = 0)
        {
            if (originele_sector > 0 && nieuwe_sector > 0)
            {
                Tram tram = null;

                List<Tram> trams = tramRepo.GetTrams();
                foreach (Tram tempTram in trams)
                {
                    if (tempTram.Sector.ID == originele_sector)
                    {
                        tram = tempTram;
                        break;
                    }
                }

                Sector sector = sectorRepo.GetSectorByID(nieuwe_sector);

                if (tram != null && sector != null)
                {
                    //tramRepo.ChangeTramSector(sector, tram);
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
                //tramRepo.LeaveRemise(t);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Inrijden(int tram_id = 0)
        {
            if (tram_id > 0)
            {
                Tram t = tramRepo.GetTramByID(tram_id);
                //tramRepo.DriveIntoRemise(t);
            }
            return RedirectToAction("Index");
        }
    }
}