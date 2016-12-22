using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EyeCT4RailsWeb.Data;
using EyeCT4RailsWeb.Models;

namespace EyeCT4RailsWeb.Logic
{
    public class SectorRepository
    {
        private readonly ISectorRepo _sectorRepo;

        public SectorRepository(ISectorRepo sectorRepo)
        {
            _sectorRepo = sectorRepo;
        }

        public List<Sector> GetAllSectors()
        {
            return _sectorRepo.GetAllSectors();
        }

        public Sector GetSectorByID(int spoornummer, int sectornummer)
        {
            Sector sector = _sectorRepo.GetSectorByID(spoornummer, sectornummer);
            sector.Lijn = GetSpoorLijn(sector.Spoor);
            return sector;
        }

        public List<Sector> GetSectorBySpoor(int spoor)
        {
            return _sectorRepo.GetSectorBySpoor(spoor);
        }

        public bool BlockSector(int spoorid, int sectornummer)
        {
            int blokkeren = 0;
            Sector sector = GetSectorByID(spoorid, sectornummer);
            if (sector.Blokkeren == false)
            {
                blokkeren = 1;
            }

            return _sectorRepo.BlockSector(spoorid, sectornummer, blokkeren);
        }

        public Sector GetSectorByLijn(int lijn)
        {
            return _sectorRepo.GetSectorByLijn(lijn);
        }

        public int GetSpoorLijn(int spoor)
        {
            return _sectorRepo.GetSpoorLijn(spoor);
        }
    }
}