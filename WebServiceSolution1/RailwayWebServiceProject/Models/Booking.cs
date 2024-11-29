using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RailwayWebServiceProject.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int ScheduleId { get; set; }
        public string NIC { get; set; }
        public int SeatCount { get; set; }
        public string PassengerName { get; set; }
        public string ContactInfo { get; set; }
    }
}