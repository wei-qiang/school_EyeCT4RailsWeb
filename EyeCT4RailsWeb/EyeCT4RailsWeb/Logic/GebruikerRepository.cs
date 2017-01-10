using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EyeCT4RailsWeb.Data;
using EyeCT4RailsWeb.Models;

namespace EyeCT4RailsWeb.Logic
{
    public class GebruikerRepository
    {
        private IGebruiker context;

        public GebruikerRepository(IGebruiker context)
        {
            this.context = context;
        }

        public Gebruiker LoginUser(string gebruikersnaam, string wachtwoord)
        {
            return context.Verificatie(gebruikersnaam, wachtwoord);
        }

        public bool AddUser(string gebruikersnaam, string wachtwoord, string functie)
        {
            return context.GebruikerToevoegen(gebruikersnaam, wachtwoord, functie);
        }

        public List<string> GetAllFunctions()
        {
            return context.allFunctions();
        }
    }
}