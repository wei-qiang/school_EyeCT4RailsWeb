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
            return View();
        }

        public ActionResult Login(string gebruikersnaam, string wachtwoord)
        {
            Gebruiker gebruiker = GebruikerRepo.LoginUser(gebruikersnaam, wachtwoord);

            if(gebruiker == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Overzicht");
                //Hier moet nog het hele session gebeuren omheen gebouwd worden
            }
        }
    }
}