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

        public ActionResult AddUser(string gebruikersnaam, string wachtwoord)
        {
            

        }
    }
}