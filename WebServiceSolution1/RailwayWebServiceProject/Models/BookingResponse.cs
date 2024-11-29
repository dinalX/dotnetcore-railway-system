using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RailwayWebServiceProject.Models
{
    public class BookingResponse
    {
        public bool Success { get; set; }
        public int? BookingId { get; set; } 
        public string Message { get; set; }
    }
}