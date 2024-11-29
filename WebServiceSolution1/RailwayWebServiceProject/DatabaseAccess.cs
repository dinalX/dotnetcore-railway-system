using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RailwayWebServiceProject.Models;

namespace WebService
{
    public class DatabaseAccess
    {
        private readonly string connectionString;

        public DatabaseAccess()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RailwayDb"].ConnectionString;
        }
        public string ConnectionString => connectionString;

        public List<Train> GetTrains()
        {
            var trains = new List<Train>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT TrainId, Name, SeatCount FROM Trains"; 
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int trainId = (int)reader["TrainId"];
                    int totalSeats = (int)reader["SeatCount"];

                    // Calculate booked seats
                    int bookedSeats = GetBookedSeats(trainId);

                    trains.Add(new Train
                    {
                        TrainId = trainId,
                        Name = reader["Name"].ToString(),
                        SeatCount = totalSeats,
                        AvailableSeats = totalSeats - bookedSeats 
                    });
                }
            }

            return trains;
        }

        private int GetBookedSeats(int trainId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Calculate the number of booked seats for the train
                string query = @"
                    SELECT SUM(SeatCount) 
                    FROM Bookings 
                    WHERE ScheduleId IN (SELECT ScheduleId FROM Schedules WHERE TrainId = @TrainId)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TrainId", trainId);

                connection.Open();
                var result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0; 
            }
        }

        public List<Station> GetStations()
        {
            var stations = new List<Station>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT StationId, Name FROM Stations"; 
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    stations.Add(new Station
                    {
                        StationId = (int)reader["StationId"],
                        Name = reader["Name"].ToString()
                    });
                }
            }

            return stations;
        }

        public List<Schedule> GetSchedules()
        {
            var schedules = new List<Schedule>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ScheduleId, TrainId, DepartureStationId, ArrivalStationId, DepartureTime, ArrivalTime FROM Schedules";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    schedules.Add(new Schedule
                    {
                        ScheduleId = (int)reader["ScheduleId"],
                        TrainId = (int)reader["TrainId"],
                        DepartureStationId = (int)reader["DepartureStationId"],
                        ArrivalStationId = (int)reader["ArrivalStationId"],
                        DepartureTime = (DateTime)reader["DepartureTime"],
                        ArrivalTime = (DateTime)reader["ArrivalTime"]
                    });
                }
            }

            return schedules;
        }

        public bool AddBooking(Booking booking)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Bookings (ScheduleId, NIC, SeatCount, PassengerName, ContactInfo) VALUES (@ScheduleId, @NIC, @SeatCount, @PassengerName, @ContactInfo)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ScheduleId", booking.ScheduleId);
                command.Parameters.AddWithValue("@NIC", booking.NIC);
                command.Parameters.AddWithValue("@SeatCount", booking.SeatCount);
                command.Parameters.AddWithValue("@PassengerName", booking.PassengerName);
                command.Parameters.AddWithValue("@ContactInfo", booking.ContactInfo);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0; 
            }
        }

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
        public int GetAvailableSeats(int scheduleId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT TrainId FROM Schedules WHERE ScheduleId = @ScheduleId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ScheduleId", scheduleId);

                connection.Open();
                var trainId = command.ExecuteScalar();

                if (trainId != null)
                {
                    query = "SELECT SeatCount FROM Trains WHERE TrainId = @TrainId";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TrainId", trainId);

                    var totalSeats = command.ExecuteScalar();

                    query = "SELECT ISNULL(SUM(SeatCount), 0) FROM Bookings WHERE ScheduleId = @ScheduleId";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ScheduleId", scheduleId);

                    var bookedSeats = command.ExecuteScalar();

                    return (totalSeats != null ? Convert.ToInt32(totalSeats) : 0) - Convert.ToInt32(bookedSeats);
                }
            }
            return 0;
        }
    }
}
