using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;
using RailwayWebServiceProject.Models;

namespace RailwayWebServiceProject
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AdminService : System.Web.Services.WebService
    {
        private readonly string connectionString;
        private TrainManager trainManager;

        public AdminService()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RailwayDb"].ConnectionString;
            trainManager = new TrainManager(connectionString);
        }

        [WebMethod]
        public string AddTrain(string name, int seatCount)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Trains (Name, SeatCount) VALUES (@Name, @SeatCount); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@SeatCount", seatCount);
                connection.Open();

                int trainId = Convert.ToInt32(command.ExecuteScalar());
                return $"Train added successfully with TrainId: {trainId}";
            }
        }

        [WebMethod]
        public void UpdateTrain(int trainId, string name, int seatCount)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Trains SET Name = @Name, SeatCount = @SeatCount WHERE TrainId = @TrainId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TrainId", trainId);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@SeatCount", seatCount);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public void DeleteTrain(int trainId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Trains WHERE TrainId = @TrainId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TrainId", trainId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public int GetAvailableSeats(int scheduleId)
        {
            return trainManager.GetAvailableSeats(scheduleId);
        }

        [WebMethod]
        public string AddStation(string name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Stations (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);

                connection.Open();

                int stationId = Convert.ToInt32(command.ExecuteScalar());
                return $"Station added successfully with StationId: {stationId}";
            }
        }

        [WebMethod]
        public void UpdateStation(int stationId, string name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Stations SET Name = @Name WHERE StationId = @StationId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StationId", stationId);
                command.Parameters.AddWithValue("@Name", name);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public void DeleteStation(int stationId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Stations WHERE StationId = @StationId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StationId", stationId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public string AddSchedule(int trainId, int departureStationId, int arrivalStationId, DateTime departureTime, DateTime arrivalTime)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Schedules (TrainId, DepartureStationId, ArrivalStationId, DepartureTime, ArrivalTime) 
                                 VALUES (@TrainId, @DepartureStationId, @ArrivalStationId, @DepartureTime, @ArrivalTime); 
                                 SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TrainId", trainId);
                command.Parameters.AddWithValue("@DepartureStationId", departureStationId);
                command.Parameters.AddWithValue("@ArrivalStationId", arrivalStationId);
                command.Parameters.AddWithValue("@DepartureTime", departureTime);
                command.Parameters.AddWithValue("@ArrivalTime", arrivalTime);

                connection.Open();

                int scheduleId = Convert.ToInt32(command.ExecuteScalar());
                return $"Schedule added successfully with ScheduleId: {scheduleId}";
            }
        }

        [WebMethod]
        public void UpdateSchedule(int scheduleId, int trainId, int departureStationId, int arrivalStationId, DateTime departureTime, DateTime arrivalTime)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Schedules SET TrainId = @TrainId, DepartureStationId = @DepartureStationId, 
                                 ArrivalStationId = @ArrivalStationId, DepartureTime = @DepartureTime, ArrivalTime = @ArrivalTime 
                                 WHERE ScheduleId = @ScheduleId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ScheduleId", scheduleId);
                command.Parameters.AddWithValue("@TrainId", trainId);
                command.Parameters.AddWithValue("@DepartureStationId", departureStationId);
                command.Parameters.AddWithValue("@ArrivalStationId", arrivalStationId);
                command.Parameters.AddWithValue("@DepartureTime", departureTime);
                command.Parameters.AddWithValue("@ArrivalTime", arrivalTime);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public void DeleteSchedule(int scheduleId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Schedules WHERE ScheduleId = @ScheduleId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ScheduleId", scheduleId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public List<Booking> GetBookings()
        {
            var bookings = new List<Booking>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT BookingId, ScheduleId, NIC, SeatCount, PassengerName, ContactInfo FROM Bookings";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    bookings.Add(new Booking
                    {
                        BookingId = (int)reader["BookingId"],
                        ScheduleId = (int)reader["ScheduleId"],
                        NIC = reader["NIC"].ToString(),
                        SeatCount = (int)reader["SeatCount"],
                        PassengerName = reader["PassengerName"].ToString(),
                        ContactInfo = reader["ContactInfo"].ToString()
                    });
                }
            }

            return bookings;
        }

        [WebMethod]
        public void DeleteBooking(int bookingId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Bookings WHERE BookingId = @BookingId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookingId", bookingId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
