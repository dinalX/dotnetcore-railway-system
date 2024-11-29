using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace UserWebApp.Models
{
    public class Passenger
    {
        public int PassengerId { get; set; }
        public string NIC { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
    }
}
