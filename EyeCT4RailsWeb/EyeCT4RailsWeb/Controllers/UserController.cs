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
    public class UserController : Controller
    {
        private GebruikerRepository GebruikerRepo = new GebruikerRepository(new GebruikerMSSQLContext());
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(string gebruikersnaam, string wachtwoord, string functie)
        {
            if(String.IsNullOrEmpty(gebruikersnaam) || String.IsNullOrEmpty(wachtwoord) || String.IsNullOrEmpty(functie))
            {
                ViewBag.error = "Je hebt niks ingevuld. Vul de velden in en probeer het opnieuw.";
                return View();
            }
            bool adding = GebruikerRepo.AddUser(gebruikersnaam, wachtwoord, functie);

            if (adding == true)
            {
                return RedirectToAction("Index","Remise");
            }
            else
            {
                ViewBag.error = "Er is iets fout gegaan. Probeer het opnieuw.";           
                return View(); // De view die een foutmelding geeft en het opnieuw laat proberen.
            }
        }
        
        
        public ActionResult Create()
        {
            return View();
        }
    }
}