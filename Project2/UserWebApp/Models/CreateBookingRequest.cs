using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserWebApp.Models
{
    public class CreateBookingRequest
    {
        public int ScheduleId { get; set; }
        public string NIC { get; set; }
        public List<int> SeatNumbers { get; set; }
    }
}
