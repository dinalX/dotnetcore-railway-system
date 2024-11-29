using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserWebApp.Models;

namespace UserWebApp.Models
{
    public class SearchViewModel
    {
        public DateTime TravelDate { get; set; }
        public int StartStationId { get; set; }
        public int DestinationStationId { get; set; }
        public List<Station> Stations { get; set; }
        public List<Train> AvailableTrains { get; set; }
    }
}
