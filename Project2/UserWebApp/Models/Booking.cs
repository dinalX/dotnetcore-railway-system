using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace UserWebApp.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int ScheduleId { get; set; }
        public string NIC { get; set; }
        public int SeatCount { get; set; }
    }
}
