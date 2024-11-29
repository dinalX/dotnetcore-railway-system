using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RailwayWebServiceProject.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}