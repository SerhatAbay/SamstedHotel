using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SamstedHotel
{
    public class ReservationRepo : IRepository<Reservation>
    {
        private readonly string _connectionString;

        public ReservationRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        // Hent alle ambulancer
        public IEnumerable<Reservation> GetAll()
        {
            var reservations = new List<Reservation>();
            string query = "SELECT * FROM RESERVATIONS";


            //Dette skal ændres/slettes når view bruges
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservations.Add(new Reservation
                        {
                            ReservationID = (int)reader["ReservationID"],
                            CustomerID = (int)reader["CustomerID"],
                            Status = (string)reader["Status"],
                            ReservationType = (string)reader["ReservationType"],
                            Created = (DateTime)reader["Created"],
                            Updated = (DateTime)reader["Updated"],
                            StartDate = (DateTime)reader["Startdate"],
                            EndDate = (DateTime)reader["Enddate"],
                            TotalAmount = (Decimal)reader["Totalamount"]
                        });
                    }
                }
            }

            return reservations;
        }
        // Tilføjer en ny kunde
        public void Add(Reservation reservation)
        {
            string query = "INSERT INTO Reservations (ReservationID, KundeID, Status, ReservationType, Created, Updated, Startdate, Enddate, Totalamount) " +
                           "VALUES (@ReservationID, @CustomerID, @Status, @ReservationType, @Created, @Updated, @Startdate, Enddate, @Totalamount@)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReservationID", reservation.ReservationID);
                command.Parameters.AddWithValue("@CustomerID", reservation.CustomerID);
                command.Parameters.AddWithValue("@Status", reservation.Status);
                command.Parameters.AddWithValue("@ReservationType", reservation.ReservationType);
                command.Parameters.AddWithValue("@Created", reservation.Created);
                command.Parameters.AddWithValue("@Updated", reservation.Updated);
                command.Parameters.AddWithValue("@Startdate", reservation.StartDate);
                command.Parameters.AddWithValue("@Enddate", reservation.EndDate);
                command.Parameters.AddWithValue("@Totalamount", reservation.TotalAmount);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Opdater en ambulance
        public void Update(Reservation reservation)
        {
            string query = "INSERT INTO Reservations (ReservationID, KundeID, Status, ReservationType, Created, Updated, Startdate, Enddate, Totalamount) " +
                         "VALUES (@ReservationID, @CustomerID, @Status, @ReservationType, @Created, @Updated, @Startdate, Enddate, @Totalamount@)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReservationID", reservation.ReservationID);
                command.Parameters.AddWithValue("@CustomerID", reservation.CustomerID);
                command.Parameters.AddWithValue("@Status", reservation.Status);
                command.Parameters.AddWithValue("@ReservationType", reservation.ReservationType);
                command.Parameters.AddWithValue("@Created", reservation.Created);
                command.Parameters.AddWithValue("@Updated", reservation.Updated);
                command.Parameters.AddWithValue("@Startdate", reservation.StartDate);
                command.Parameters.AddWithValue("@Enddate", reservation.EndDate);
                command.Parameters.AddWithValue("@Totalamount", reservation.TotalAmount);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        // Hent ambulance ved ID
        public Reservation GetById(int id)
        {
            Reservation reservation = null;
            string query = "SELECT * FROM Reservation WHERE ReservationID = @ReservationID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReservationID", id);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reservation = new Reservation
                        {
                            ReservationID = (int)reader["ReservationID"],
                            CustomerID = (int)reader["CustomerID"],
                            Status = (string)reader["Status"],
                            ReservationType = (string)reader["ReservationType"],
                            Created = (DateTime)reader["Created"],
                            Updated = (DateTime)reader["Updated"],
                            StartDate = (DateTime)reader["Startdate"],
                            EndDate = (DateTime)reader["Enddate"],
                            TotalAmount = (Decimal)reader["Totalamount"]
                        };
                    }
                }
            } 
                    return reservation;
                }

                        // Slet en ambulance
                        public void Delete(int id)
                        {
                            string query = "DELETE FROM Reservations WHERE ReservationID = @ReservationID";

                            using (SqlConnection connection = new SqlConnection(_connectionString))
                            {
                                SqlCommand command = new SqlCommand(query, connection);
                                command.Parameters.AddWithValue("@ReservationID", id);
                                connection.Open();
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                } 