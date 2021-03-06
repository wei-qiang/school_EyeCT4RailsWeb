﻿using EyeCT4RailsWeb.Data;
using EyeCT4RailsWeb.Logic;
using EyeCT4RailsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EyeCT4RailsWeb.Controllers
{
    public class HomeController : Controller
    {
        private GebruikerRepository GebruikerRepo = new GebruikerRepository(new GebruikerMSSQLContext());

        // GET: Home
        public ActionResult Index()
        {
            if (Session["gebruikersnaam"] != null)
            {
                return RedirectToAction("Index", "Remise");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string gebruikersnaam, string wachtwoord)
        {
            Gebruiker gebruiker = GebruikerRepo.LoginUser(gebruikersnaam, wachtwoord);

            if(gebruiker == null)
            {
                @ViewBag.error = "Gebruikersnaam of wachtwoord is onjuist. Probeer aub opnieuw in te loggen.";
                return View();
                
            }
            else
            {
                Session["gebruikersnaam"] = gebruikersnaam;
                Session["functie"] = gebruiker.Functie;
                Session.Timeout = 30;
                return RedirectToAction("Index", "Remise");
                //Hier moet nog het hele session gebeuren omheen gebouwd worden
            }
        }
    }
}