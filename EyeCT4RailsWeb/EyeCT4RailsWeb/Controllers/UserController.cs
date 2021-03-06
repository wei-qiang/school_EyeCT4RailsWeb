﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EyeCT4RailsWeb.Models;
using EyeCT4RailsWeb.Logic;
using EyeCT4RailsWeb.Data;

namespace EyeCT4RailsWeb.Controllers
{
    public class UserController : Controller
    {
        private GebruikerRepository GebruikerRepo = new GebruikerRepository(new GebruikerMSSQLContext());
        private Gebruiker model = new Gebruiker();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(string gebruikersnaam, string wachtwoord, string functie)
        {
            if(Session["gebruikersnaam"] != null)
            {
                model.AllFunctiesFromGebruikers = GebruikerRepo.GetAllFunctions();
                if (String.IsNullOrEmpty(gebruikersnaam) || String.IsNullOrEmpty(wachtwoord) || String.IsNullOrEmpty(functie))
                {
                    ViewBag.error = "Je hebt niks ingevuld. Vul de velden in en probeer het opnieuw.";
                    
                    return View(model);
                }
                bool adding = GebruikerRepo.AddUser(gebruikersnaam, wachtwoord, functie);

                if (adding == true)
                {
                    ViewBag.error = null;
                    return RedirectToAction("Index", "Remise");
                }
                else
                {
                    ViewBag.error = "Er is iets fout gegaan. Probeer het opnieuw.";
                    return View(model); // De view die een foutmelding geeft en het opnieuw laat proberen.
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        
        public ActionResult Create()
        {
            if(Session["gebruikersnaam"] != null)
            {
                model.AllFunctiesFromGebruikers = GebruikerRepo.GetAllFunctions();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            if (Session["gebruikersnaam"] != null || Session["functie"] != null)
            {
                Session["gebruikernaam"] = null;
                Session["functie"] = null;
            }
            return RedirectToAction("Index", "Home");
        }
    }
}