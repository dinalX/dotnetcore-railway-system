using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using RailwayWebServiceProject.Models;
using WebService;

namespace RailwayWebServiceProject
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class ClientService : System.Web.Services.WebService
    {
        private static DatabaseAccess dbAccess = new DatabaseAccess();
        private TrainManager trainManager;

        // Constructor to initialize TrainManager
        public ClientService()
        {
            trainManager = new TrainManager(dbAccess.ConnectionString); 
        }

        [WebMethod]
        public List<Schedule> SearchTrains(int departureStationId, int arrivalStationId, DateTime date)
        {
            try
            {
                var allSchedules = dbAccess.GetSchedules();

                var result = allSchedules
                    .Where(s => s.DepartureStationId == departureStationId &&
                                s.ArrivalStationId == arrivalStationId &&
                                s.DepartureTime.Date == date.Date)
                    .Select(s => new Schedule
                    {
                        ScheduleId = s.ScheduleId,
                        TrainId = s.TrainId,
                        DepartureStationId = s.DepartureStationId,
                        ArrivalStationId = s.ArrivalStationId,
                        DepartureTime = s.DepartureTime,
                        ArrivalTime = s.ArrivalTime,
                        TrainName = dbAccess.GetTrains().FirstOrDefault(t => t.TrainId == s.TrainId)?.Name,
                        AvailableSeats = GetAvailableSeats(s.ScheduleId) 
                    })
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("An error occurred while searching trains.", ex);
            }
        }

        [WebMethod]
        public List<Station> GetStations()
        {
            return dbAccess.GetStations();
        }

        [WebMethod]
        public int GetAvailableSeats(int scheduleId)
        {
            try
            {
                // Check if trainManager is initialized
                if (trainManager == null)
                {
                    throw new InvalidOperationException("TrainManager is not initialized.");
                }

                int availableSeats = trainManager.GetAvailableSeats(scheduleId);

                // Add logging to check the available seats value
                if (availableSeats < 0)
                {
                    throw new Exception("Available seats calculation resulted in a negative value.");
                }

                return availableSeats;
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                throw new Exception("An error occurred while retrieving available seats.", ex);
            }
        }

        [WebMethod]
        public BookingResponse AddBooking(int scheduleId, string nic, int seatCount, string passengerName, string contactInfo)
        {
            var response = new BookingResponse();

            try
            {
                if (seatCount > 5)
                {
                    response.Success = false;
                    response.Message = "Cannot book more than 5 seats.";
                    return response;
                }

                var schedule = dbAccess.GetSchedules().FirstOrDefault(s => s.ScheduleId == scheduleId);
                if (schedule != null)
                {
                    var train = dbAccess.GetTrains().FirstOrDefault(t => t.TrainId == schedule.TrainId);
                    if (train != null && GetAvailableSeats(scheduleId) >= seatCount)
                    {
                        var booking = new Booking
                        {
                            ScheduleId = scheduleId,
                            NIC = nic,
                            SeatCount = seatCount,
                            PassengerName = passengerName,
                            ContactInfo = contactInfo
                        };

                        bool isBooked = dbAccess.AddBooking(booking);

                        if (isBooked)
                        {
                            response.Success = true;
                            response.BookingId = booking.BookingId;
                            response.Message = "Booking successful.";
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Failed to add booking.";
                        }
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Insufficient seats available.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Invalid schedule.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                response.Success = false;
                response.Message = "An error occurred during booking: " + ex.Message;
            }

            return response;
        }
        


    }
}
