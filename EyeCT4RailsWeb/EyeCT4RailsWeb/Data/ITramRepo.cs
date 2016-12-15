using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EyeCT4RailsWeb.Models;

namespace EyeCT4RailsWeb.Data
{
    public interface ITramRepo
    {
        List<Tram> GetTrams();
        Tram GetTramByID(int ID);
        bool ChangeStatusTrams(string status, Tram tram);
        bool ChangeTramSector(Sector sector, Tram tram);
        bool ChangeSchoonmaakPrioriteitTram(int prioriteit, Tram tram);
        bool ChangeReparatiePrioriteitTram(int prioriteit, Tram tram);
        bool UpdateLaatsteGroteSchoonmaak(Tram tram);
        bool UpdateLaatsteKleineSchoonmaak(Tram tram);
        List<Tram> GetSchoonmaakTrams();
        List<Tram> GetReparartieTrams();
        bool LeaveRemise(int TramID);
    }
}
