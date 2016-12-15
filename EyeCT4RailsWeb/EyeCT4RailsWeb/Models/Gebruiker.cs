using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EyeCT4RailsWeb.Models
{
    public class Gebruiker
    {
        public string Gebruikersnaam { get; set; }
        public string Naam { get; set; }
        public Functie Functie { get; set; }


        public Gebruiker(string gebruikersnaam, string naam, Functie functie)
        {
            Gebruikersnaam = gebruikersnaam;
            Naam = naam;
            Functie = functie;
        }


        public override string ToString()
        {
            return Gebruikersnaam + Naam + Functie;
        }
    }
}