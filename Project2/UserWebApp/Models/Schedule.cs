using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace UserWebApp.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int TrainId { get; set; }
        public int DepartureStationId { get; set; }
        public int ArrivalStationId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
