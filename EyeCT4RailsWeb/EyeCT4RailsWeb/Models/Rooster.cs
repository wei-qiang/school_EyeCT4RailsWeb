using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EyeCT4RailsWeb.Models
{
    public class Rooster
    {
        public int ID { get; set; }
        public Dag Dag { get; set; }
        public DateTime TijdVertrek { get; set; }
        public DateTime TijdAankomst { get; set; }
        public string TramType { get; set; }
        public RoosterType Type { get; set; }

        public Rooster(int _ID, DateTime _TijdVertrek/*, DateTime _TijdAankomst, Dag _Dag*/)
        {
            ID = _ID;
            TijdVertrek = _TijdVertrek;
            //TijdAankomst = _TijdAankomst;
            //Dag = _Dag;
        }

        public Rooster(int _ID, Dag _Dag, DateTime _TijdAankomst, string _TramType, RoosterType _Type)
        {
            ID = _ID;
            Dag = _Dag;
            TijdAankomst = _TijdAankomst;
            TramType = _TramType;
            Type = _Type;
        }
    }
}