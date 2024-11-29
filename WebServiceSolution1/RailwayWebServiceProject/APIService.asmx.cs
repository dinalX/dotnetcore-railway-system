using RailwayWebServiceProject.Models;
using System;
using System.Collections.Generic;
using System.Web.Services;
using WebService;

namespace RailwayWebServiceProject
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class APIService : System.Web.Services.WebService
    {
        private readonly DatabaseAccess dbAccess;

        public APIService()
        {
            dbAccess = new DatabaseAccess();
        }

        [WebMethod]
        public List<Schedule> SearchTrains(int departureStationId, int arrivalStationId, DateTime date)
        {
            var schedules = dbAccess.GetSchedules();

            return schedules
                .FindAll(s => s.DepartureStationId == departureStationId &&
                              s.ArrivalStationId == arrivalStationId &&
                              s.DepartureTime.Date == date.Date);
        }

        [WebMethod]
        public int GetAvailableSeats(int scheduleId)
        {
            return dbAccess.GetAvailableSeats(scheduleId);
        }

        [WebMethod]
        public bool BookSeats(int scheduleId, string nic, int seatCount, string passengerName, string contactInfo)
        {
            if (seatCount > 5)
            {
                return false; 
            }

            int availableSeats = dbAccess.GetAvailableSeats(scheduleId);
            if (availableSeats >= seatCount)
            {
                var booking = new Booking
                {
                    ScheduleId = scheduleId,
                    NIC = nic,
                    SeatCount = seatCount,
                    PassengerName = passengerName,
                    ContactInfo = contactInfo
                };

                return dbAccess.AddBooking(booking);
            }

            return false;
        }

        [WebMethod]
        public List<Booking> GetBookings()
        {
            return dbAccess.GetBookings();
        }
    }
}
