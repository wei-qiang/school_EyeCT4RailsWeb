﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EyeCT4RailsWeb.Logic;
using EyeCT4RailsWeb.Data;

namespace EyeCT4RailsWeb.Models
{
    public class Tram
    {
        public int ID { get; set; }
        public string Tramtype { get; set; }
        //public int Lengte { get; set; }
        public int Lijn { get; set; }
        public Sector Sector { get; set; }
        public Status Status { get; set; }
        public int PrioriteitReparatie { get; set; }
        public int PrioriteitSchoonmaak { get; set; }


        private SectorRepository sectorRepo = new SectorRepository(new SectorMSSQLContext());
        // private TramRepository tramRepo;

        /// <summary>
        /// deze constructor maakt maakt een instantie van tram aan.
        /// </summary>
        public Tram(int _ID, string _Tramtype, int _Lijn, Sector _Sector, Status _Status, int _PrioriteitReparatie, int _PrioriteitSchoonmaak)
        {
            ID = _ID;
            Tramtype = _Tramtype;
            Lijn = _Lijn;
            Sector = _Sector;
            Status = _Status;
            PrioriteitReparatie = _PrioriteitReparatie;
            PrioriteitSchoonmaak = _PrioriteitSchoonmaak;
        }

        /// <summary>
        /// constructor van tram die geen sector heeft
        /// </summary>
        public Tram(int _ID, string _Tramtype, int _Lijn, Status _Status, int _PrioriteitReparatie, int _PrioriteitSchoonmaak)
        {
            ID = _ID;
            Tramtype = _Tramtype;
            Lijn = _Lijn;
            Status = _Status;
            PrioriteitReparatie = _PrioriteitReparatie;
            PrioriteitSchoonmaak = _PrioriteitSchoonmaak;
        }

        public Tram(int _ID, string _Tramtype)
        {
            ID = _ID;
            Tramtype = _Tramtype;
        }

        /// <summary>
        /// deze constructor maakt een instantie van de tram aan en haalt zelf de sector klasse op als je alleen het nummer doorgeeft
        /// </summary>
        public Tram(int _ID, string _Tramtype, int _Lijn, int _Sector, Status _Status, int _PrioriteitReparatie, int _PrioriteitSchoonmaak)
        {
            ID = _ID;
            Tramtype = _Tramtype;
            Lijn = _Lijn;
            this.Sector = sectorRepo.GetSectorByID(_Sector);
            Status = _Status;
            PrioriteitReparatie = _PrioriteitReparatie;
            PrioriteitSchoonmaak = _PrioriteitSchoonmaak;
        }

        /*
        /// <summary>
        /// Wijzigt de status van de tram
        /// </summary>
        /// <param name="_Status"></param>
        /// <param name="_Prioriteit"></param>
        /// <returns></returns>
        public bool StatusWijzigen(Status _Status, int _Prioriteit)
        {
            if (Status == _Status)
            {
                if(Status == Status.Defect && PrioriteitReparatie == _Prioriteit)
                {
                    return false;
                }
                else if(Status == Status.Schoonmaak && PrioriteitSchoonmaak == _Prioriteit)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            Status = _Status;
            tramRepo.ChangeStatusTram(_Status, this, _Prioriteit);
            if(Status == Status.Defect)
            {
                PrioriteitReparatie = _Prioriteit;
            }
            else if(Status == Status.Schoonmaak)
            {
                PrioriteitSchoonmaak = _Prioriteit;
            } 
            return true;
        }
        */
        public override string ToString()
        {
            return ID + " " + Tramtype;
        }
    }
}