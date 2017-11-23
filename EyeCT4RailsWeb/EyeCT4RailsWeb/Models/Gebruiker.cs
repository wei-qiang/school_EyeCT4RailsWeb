using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EyeCT4RailsWeb.Models
{
    public class Gebruiker
    {
        public string Gebruikersnaam { get; set; }
        public Functie Functie { get; set; }
        public List<string> AllFunctiesFromGebruikers { get; set; }

        public Gebruiker()
        {

        }
        public Gebruiker(string gebruikersnaam, Functie functie)
        {
            Gebruikersnaam = gebruikersnaam;
            Functie = functie;
        }


        public override string ToString()
        {
            return Gebruikersnaam + Functie;
        }
    }
}