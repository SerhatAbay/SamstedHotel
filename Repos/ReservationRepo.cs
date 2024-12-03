using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SamstedHotel.Model;

namespace SamstedHotel.Repos
{
    public class ReservationRepo
    {
        
        private string _connectionString;

        public ReservationRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Opret en reservation
        public void AddReservation(Reservation reservation)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Reservations (CustomerID, StartDate, EndDate, TotalAmount, Status) VALUES (@CustomerID, @StartDate, @EndDate, @TotalAmount, @Status)", connection);
                command.Parameters.AddWithValue("@CustomerID", reservation.CustomerID);
                command.Parameters.AddWithValue("@StartDate", reservation.StartDate);
                command.Parameters.AddWithValue("@EndDate", reservation.EndDate);
                command.Parameters.AddWithValue("@TotalAmount", reservation.TotalAmount);
                command.Parameters.AddWithValue("@Status", reservation.Status);

                command.ExecuteNonQuery();
            }
        }

        // Hent reservationer
        public List<Reservation> GetAllReservations()
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Reservations", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    reservations.Add(new Reservation
                    {
                        ReservationID = (int)reader["ReservationID"],
                        CustomerID = (int)reader["CustomerID"],
                        StartDate = (DateTime)reader["StartDate"],
                        EndDate = (DateTime)reader["EndDate"],
                        TotalAmount = (decimal)reader["TotalAmount"],
                        Status = (string)reader["Status"]
                    });
                }
            }

            return reservations;
        }

        // Opdater en reservation
        public void UpdateReservation(Reservation reservation)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Reservations SET StartDate = @StartDate, EndDate = @EndDate, TotalAmount = @TotalAmount, Status = @Status WHERE ReservationID = @ReservationID", connection);
                command.Parameters.AddWithValue("@StartDate", reservation.StartDate);
                command.Parameters.AddWithValue("@EndDate", reservation.EndDate);
                command.Parameters.AddWithValue("@TotalAmount", reservation.TotalAmount);
                command.Parameters.AddWithValue("@Status", reservation.Status);
                command.Parameters.AddWithValue("@ReservationID", reservation.ReservationID);

                command.ExecuteNonQuery();
            }
        }

        // Slet en reservation
        public void DeleteReservation(int reservationID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Reservations WHERE ReservationID = @ReservationID", connection);
                command.Parameters.AddWithValue("@ReservationID", reservationID);

                command.ExecuteNonQuery();
            }
        }

        // Hent en reservation ved ID
        public Reservation GetReservationById(int reservationID)
        {
            Reservation reservation = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Reservations WHERE ReservationID = @ReservationID", connection);
                command.Parameters.AddWithValue("@ReservationID", reservationID);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    reservation = new Reservation
                    {
                        ReservationID = (int)reader["ReservationID"],
                        CustomerID = (int)reader["CustomerID"],
                        StartDate = (DateTime)reader["StartDate"],
                        EndDate = (DateTime)reader["EndDate"],
                        TotalAmount = (decimal)reader["TotalAmount"],
                        Status = (string)reader["Status"]
                    };
                }
            }

            return reservation;
        }
    }
}
