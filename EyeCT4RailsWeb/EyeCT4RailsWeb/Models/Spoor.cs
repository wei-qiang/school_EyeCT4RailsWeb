using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EyeCT4RailsWeb.Models
{
    public class Spoor
    {
        public int ID { get; set; }
        public List<Sector> sectoren { get; set; }

        public Spoor(int _id, List<Sector> _sectoren)
        {
            this.ID = _id;
            this.sectoren = _sectoren;
        }
    }
}