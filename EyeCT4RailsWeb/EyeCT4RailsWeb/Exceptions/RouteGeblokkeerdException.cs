using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EyeCT4RailsWeb.Exceptions
{
    public class RouteGeblokkeerdException: Exception
    {
        public RouteGeblokkeerdException(string message)
           : base(message)
        {
        }
    }
}