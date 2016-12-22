using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EyeCT4RailsWeb.Models
{
    public class Sector : IComparable<Sector>
    {
        public int ID { get; set; }
        public int Spoor { get; set; }
        public int Nummer { get; set; }
        public bool Blokkeren { get; set; }
        public bool InvertBlokkeren { get { return !Blokkeren; } }
        public int Lijn { get; set; }

        public Sector(int _ID, int _Spoor, int _Nummer, bool _Blokkeren)
        {
            ID = _ID;
            Spoor = _Spoor;
            Nummer = _Nummer;
            Blokkeren = _Blokkeren;
        }

        public bool BlokkeerSector(bool _Blokkeer)
        {
            if (this.Blokkeren == true)
            {
                return false;
            }
            Blokkeren = true;
            return true;
        }

        public int CompareTo(Sector other)
        {
            return ID.CompareTo(other.ID) * -1;
        }

        public override string ToString()
        {
            return ID.ToString();
        }
    }
}