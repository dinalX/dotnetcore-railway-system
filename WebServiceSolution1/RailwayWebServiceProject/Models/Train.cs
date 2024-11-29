using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RailwayWebServiceProject.Models
{
    public class Train
    {
        public int TrainId { get; set; }
        public string Name { get; set; }
        public int SeatCount { get; set; }
        public int AvailableSeats { get; internal set; }
    }
}
