using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EyeCT4RailsWeb.Exceptions
{
    public class TramBestaatException: Exception
    {
        public TramBestaatException(string message)
            : base(message)
        {
        }
    }
}