using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EyeCT4RailsWeb.Models;

namespace EyeCT4RailsWeb.Data
{
    public interface IGebruiker
    {
        Gebruiker Verificatie(string gebruikersnaam, string wachtwoord);
        bool GebruikerToevoegen(string gebruikersnaam, string wachtwoord, string functie);
        List<string> allFunctions();        
    }
}
