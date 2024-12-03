using Microsoft.Data.SqlClient;
using SamstedHotel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamstedHotel.Repos
{
    public class RoomRepo : IRepository<Room>
    {
        private readonly string _connectionString;

        public RoomRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Henter alle værelser
        public IEnumerable<Room> GetAll()
        {
            var rooms = new List<Room>();
            string query = "SELECT * FROM Rooms";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rooms.Add(new Room
                        {
                            RoomID = (int)reader["RoomID"],
                            RoomName = reader["RoomName"].ToString(),
                            RoomTypeID = (int)reader["RoomTypeID"],
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }

            return rooms;
        }

        // Henter et værelse baseret på ID
        public Room GetById(int roomId)
        {
            Room room = null;
            string query = "SELECT * FROM Rooms WHERE RoomID = @RoomID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", roomId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        room = new Room
                        {
                            RoomID = (int)reader["RoomID"],
                            RoomName = reader["RoomName"].ToString(),
                            RoomTypeID = (int)reader["RoomTypeID"],
                            Status = reader["Status"].ToString()
                        };
                    }
                }
            }

            return room;
        }

        // Tilføjer et nyt værelse
        public void Add(Room entity)
        {
            string query = "INSERT INTO Rooms (RoomName, RoomTypeID, Status) VALUES (@RoomName, @RoomTypeID, @Status)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomName", entity.RoomName);
                command.Parameters.AddWithValue("@RoomTypeID", entity.RoomTypeID);
                command.Parameters.AddWithValue("@Status", entity.Status);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Opdaterer et værelse
        public void Update(Room entity)
        {
            string query = "UPDATE Rooms SET RoomName = @RoomName, RoomTypeID = @RoomTypeID, Status = @Status WHERE RoomID = @RoomID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", entity.RoomID);
                command.Parameters.AddWithValue("@RoomName", entity.RoomName);
                command.Parameters.AddWithValue("@RoomTypeID", entity.RoomTypeID);
                command.Parameters.AddWithValue("@Status", entity.Status);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Sletter et værelse
        public void Delete(Room entity)
        {
            string query = "DELETE FROM Rooms WHERE RoomID = @RoomID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", entity.RoomID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
