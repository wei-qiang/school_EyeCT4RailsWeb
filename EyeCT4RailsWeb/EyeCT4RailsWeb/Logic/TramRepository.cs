using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EyeCT4RailsWeb.Models;
using EyeCT4RailsWeb.Data;
using EyeCT4RailsWeb.Exceptions;

namespace EyeCT4RailsWeb.Logic
{
    public class TramRepository
    {
        private ITramRepo _tramRepo;
        private SectorRepository sectorRepository;


        public TramRepository(ITramRepo tramrepo, ISectorRepo sectorrepo)
        {
            _tramRepo = tramrepo;
            sectorRepository = new SectorRepository(sectorrepo);
        }

        public TramRepository(ITramRepo tramrepo)
        {
            _tramRepo = tramrepo;
        }

        public List<Tram> GetTrams()
        {
            List<Tram> tramList;
            tramList = _tramRepo.GetTrams();
            return tramList;
        }

        public Tram GetTramByID(int nummer)
        {
            return _tramRepo.GetTramByID(nummer);
        }

        public bool ChangeStatusTram(Status status, Tram tram, int prioriteit)
        {
            bool gelukt = false;
            string _status = status.ToString();
            try
            {
                if (status == Status.Defect)
                {

                    gelukt = _tramRepo.ChangeReparatiePrioriteitTram(prioriteit, tram);
                    tram.PrioriteitReparatie = prioriteit;
                }
                else if (status == Status.Schoonmaak)
                {
                    gelukt = _tramRepo.ChangeSchoonmaakPrioriteitTram(prioriteit, tram);
                    tram.PrioriteitSchoonmaak = prioriteit;
                }

                tram.Status = status;

                gelukt = _tramRepo.ChangeStatusTrams(_status, tram);
            }
            catch (Exception e)
            {
                throw e;
            }

            return gelukt;
        }

        public bool DriveIntoRemise(Tram tram)
        {
            Sector sector = sectorRepository.GetSectorByLijn(tram.Lijn);
            Sector FreeSector = GetReachableSectorOfSpoor(sector.Spoor);
            if (tram.Sector != null)
            {
                throw (new TramBestaatException("Tram staat al op remise."));
            }
            try
            {
                _tramRepo.ChangeTramSector(FreeSector, tram);
                sectorRepository.BlockSector(FreeSector.Spoor, FreeSector.Nummer);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// verplaats tram van sector
        /// </summary>
        /// <param name="sector"></param>
        /// <param name="tram"></param>
        /// <returns></returns>
        public bool ChangeTramSector(Sector sector, Tram tram)
        {
            //check of de sector vrij is
            if (sector.Blokkeren == false)
            {
                //check of de bestemming op hetzelfde spoor staan
                if (sector.Spoor == tram.Sector.Spoor)
                {
                    if (CheckReachableSectorFromTram(tram, sector))
                    {
                        sectorRepository.BlockSector(tram.Sector.Spoor, tram.Sector.Nummer);
                        tram.Sector = sector;
                        _tramRepo.ChangeTramSector(sector, tram);
                        sectorRepository.BlockSector(sector.Spoor, sector.Nummer);
                        return true;
                    }
                    return false;
                }
                else
                {
                    Sector FreeSector = null;
                    //check of de tram op het spoor mag staan
                    if (tram.Lijn != sector.Lijn)
                    {
                        //return false;
                    }
                    //check of de tram al op een sector staat
                    if (tram.Sector != null)
                    {
                        //haalt bereikbare spoor op
                        FreeSector = GetReachableSectorOfSpoor(tram.Sector.Spoor);

                        //kijk of de tram de spoor kan verlaten
                        if (FreeSector != null)
                        {
                            string maxspoorsector = Convert.ToString(tram.Sector.Spoor) + Convert.ToString(sectorRepository.GetSectorBySpoor(tram.Sector.Spoor).Count);
                            if (maxspoorsector == Convert.ToString(tram.Sector.Spoor) + Convert.ToString(tram.Sector.Nummer))
                            {
                                FreeSector = GetReachableSectorOfSpoor(sector.Spoor);
                            }
                        }
                        else if (convertSectorSpoorNummer(FreeSector.Spoor, FreeSector.Nummer) == convertSectorSpoorNummer(tram.Sector.Spoor, tram.Sector.Nummer) + 1)
                        {
                            FreeSector = GetReachableSectorOfSpoor(sector.Spoor);
                        }
                        else
                        {
                            throw (new RouteGeblokkeerdException("De tram kan het spoor niet verlaten."));
                        }

                        //kijk of de weg naar het sector vanaf het spoor vrij is
                        if (convertSectorSpoorNummer(sector.Spoor, sector.Nummer) >= convertSectorSpoorNummer(FreeSector.Spoor, FreeSector.Nummer))
                        {
                            sectorRepository.BlockSector(tram.Sector.Spoor, tram.Sector.Nummer);
                            _tramRepo.ChangeTramSector(sector, tram);
                            tram.Sector.ID = sector.ID;
                            tram.Sector.Nummer = sector.Nummer;
                            tram.Sector.Spoor = sector.Spoor;
                            sectorRepository.BlockSector(tram.Sector.Spoor, tram.Sector.Nummer);
                            return true;
                        }
                    }

                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool LeaveRemise(Tram tram)
        {
            Sector FreeSector = GetReachableSectorOfSpoor(tram.Sector.Spoor);
            //kijk of de tram de spoor kan verlaten
            if (convertSectorSpoorNummer(FreeSector.Spoor, FreeSector.Nummer) == convertSectorSpoorNummer(tram.Sector.Spoor, tram.Sector.Nummer) + 1)
            {
                sectorRepository.BlockSector(tram.Sector.Spoor, tram.Sector.Nummer);
                _tramRepo.LeaveRemise(tram.TramNummer);
                return true;
            }
            else
            {
                throw (new RouteGeblokkeerdException("De weg is geblokkeerd."));
            }
        }

        /// <summary>
        /// haalt een bereikbare sector op van gegeven spoor
        /// </summary>
        /// <param name="spoor"></param>
        /// <returns>bereikbare sector</returns>
        public Sector GetReachableSectorOfSpoor(int spoor)
        {
            Sector FreeSector = default(Sector);
            //haalt een lijst op van sectors van het spoor waar de tram op staat
            List<Sector> sectors = new List<Sector>();
            sectors = sectorRepository.GetSectorBySpoor(spoor);
            sectors.Sort();
            foreach (Sector s in sectors)
            {
                if (s.Blokkeren == false)
                {
                    FreeSector = s;
                }
                else
                {
                    break;
                }
            }
            return FreeSector;
        }

        /// <summary>
        /// Kijkt of een sector die op hetzelfde spoor van tram bereikbaar is
        /// </summary>
        /// <param name="tram">tram die je wilt verplaatsen</param>
        /// <param name="sector">sector die je wilt bereiken</param>
        /// <returns>true als het kan, falase als het niet kan</returns>
        private bool CheckReachableSectorFromTram(Tram tram, Sector sector)
        {
            //haalt een lijst op van sectors van het spoor waar de tram op staat
            List<Sector> sectors = sectorRepository.GetSectorBySpoor(tram.Sector.Spoor);
            if (convertSectorSpoorNummer(sector.Spoor, sector.Nummer) < convertSectorSpoorNummer(tram.Sector.Spoor, tram.Sector.Nummer))
            {
                for (int i = 0; i < sectors.Count; i++)
                {
                    if (Convert.ToInt32(Convert.ToString(sectors[i].Spoor) + Convert.ToString(sectors[i].Nummer)) < Convert.ToInt32(Convert.ToString(sector.Spoor) + Convert.ToString(sector.Nummer)) || Convert.ToInt32(Convert.ToString(sectors[i].Spoor) + Convert.ToString(sectors[i].Nummer)) >= Convert.ToInt32(Convert.ToString(tram.Sector.Spoor) + Convert.ToString(tram.Sector.Nummer)))
                    {
                        sectors.Remove(sectors[i]);
                        i = -1;
                    }
                }
            }
            else
            {
                for (int i = 0; i < sectors.Count; i++)
                {
                    if (Convert.ToInt32(Convert.ToString(sectors[i].Spoor) + Convert.ToString(sectors[i].Nummer)) > Convert.ToInt32(Convert.ToString(sector.Spoor) + Convert.ToString(sector.Nummer)) || Convert.ToInt32(Convert.ToString(sectors[i].Spoor) + Convert.ToString(sectors[i].Nummer)) <= Convert.ToInt32(Convert.ToString(tram.Sector.Spoor) + Convert.ToString(tram.Sector.Nummer)))
                    {
                        sectors.Remove(sectors[i]);
                        i = -1;
                    }
                }
            }
            //checkt of alle sectors tussen de tram en bestemming vrij zijn
            foreach (Sector s in sectors)
            {
                if (s.Blokkeren == true)
                {
                    return false;
                }
            }
            return true;
        }

        public List<Tram> getTramsMetStatus(Status status)
        {
            if (status == Status.Schoonmaak)
            {
                return _tramRepo.GetSchoonmaakTrams();
            }
            if (status == Status.Defect)
            {
                return _tramRepo.GetReparartieTrams();
            }
            return null;
        }

        private int convertSectorSpoorNummer(int spoor, int nummer)
        {
            return Convert.ToInt32(Convert.ToString(spoor) + Convert.ToString(nummer)); 
        }
    }
}