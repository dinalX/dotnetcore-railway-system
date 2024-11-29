using System;
using System.Data.SqlClient;
using RailwayWebServiceProject.Models;

namespace RailwayWebServiceProject
{
    public class TrainManager
    {
        private readonly string connectionString;

        public TrainManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public string ConnectionString => connectionString;

        public int GetAvailableSeats(int scheduleId)
        {
            int trainId = GetTrainIdByScheduleId(scheduleId);
            if (trainId == 0)
            {
                throw new Exception("Schedule not found or does not have an associated TrainId.");
            }

            // Calculate available seats for the given schedule and train
            return CalculateAvailableSeats(scheduleId, trainId);
        }


        private int GetTrainIdByScheduleId(int scheduleId)
        {
            int trainId = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string trainQuery = "SELECT TrainId FROM Schedules WHERE ScheduleId = @ScheduleId";
                    SqlCommand trainCommand = new SqlCommand(trainQuery, connection);
                    trainCommand.Parameters.AddWithValue("@ScheduleId", scheduleId);

                    connection.Open();
                    object result = trainCommand.ExecuteScalar();

                    if (result != null)
                    {
                        trainId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new Exception("Error retrieving TrainId: " + ex.Message, ex);
            }

            return trainId;
        }

        private int CalculateAvailableSeats(int scheduleId, int trainId)
        {
            int availableSeats = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Query to calculate the available seats by subtracting the booked seats from the train's total seats
                string availableSeatsQuery = @"
            SELECT t.SeatCount - ISNULL((
                SELECT SUM(b.SeatCount) 
                FROM Bookings b 
                WHERE b.ScheduleId = @ScheduleId
            ), 0) AS AvailableSeats 
            FROM Trains t
            INNER JOIN Schedules s ON s.TrainId = t.TrainId
            WHERE t.TrainId = @TrainId AND s.ScheduleId = @ScheduleId";

                SqlCommand availableSeatsCommand = new SqlCommand(availableSeatsQuery, connection);
                availableSeatsCommand.Parameters.AddWithValue("@ScheduleId", scheduleId);
                availableSeatsCommand.Parameters.AddWithValue("@TrainId", trainId);

                connection.Open();
                object availableSeatsResult = availableSeatsCommand.ExecuteScalar();

                if (availableSeatsResult != null)
                {
                    availableSeats = Convert.ToInt32(availableSeatsResult);
                }
            }

            return availableSeats;
        }

    }
}
