﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EyeCT4RailsWeb.Models;

namespace EyeCT4RailsWeb.Data
{
    public interface ISectorRepo
    {
        List<Sector> GetAllSectors();
        List<Sector> GetSectorBySpoor(int spoor);
        Sector GetSectorByID(int spoornummer, int sectornummer);
        bool BlockSector(int spoorid, int sectornummer, int blokkeren);
        int GetSpoorLijn(int spoor);
        Sector GetSectorByLijn(int Lijn);
    }
}
