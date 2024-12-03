using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamstedHotel.Model;

namespace SamstedHotel.Repos
{
    public class CourseRoomRepo : IRepository<CourseRoom>
    {
        private readonly string _connectionString;

        public CourseRoomRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<CourseRoom> GetAll()
        {
            var courseRooms = new List<CourseRoom>();
            string query = "SELECT * FROM CourseRooms";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courseRooms.Add(new CourseRoom
                        {
                            CourseRoomName = reader["CourseRoomName"].ToString(),
                            EventPackage = reader["EventPackage"].ToString(),
                            TotalPrice = (decimal)reader["TotalPrice"]
                        });
                    }
                }
            }

            return courseRooms;
        }

        public CourseRoom GetById(int courseRoomId)
        {
            CourseRoom courseRoom = null;
            string query = "SELECT * FROM CourseRooms WHERE CourseRoomID = @CourseRoomID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CourseRoomID", courseRoomId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        courseRoom = new CourseRoom
                        {
                            CourseRoomID = (int)reader["CourseRoomID"],
                            CourseRoomName = reader["CourseRoomName"].ToString(),
                            EventPackage = reader["EventPackage"].ToString(),
                            TotalPrice = (decimal)reader["TotalPrice"]
                        };
                    }
                }
            }

            return courseRoom;
        }

        public void Add(CourseRoom entity)
        {
            string query = "INSERT INTO CourseRooms (CourseRoomName, EventPackage, TotalPrice) VALUES (@CourseRoomName, @EventPackage, @TotalPrice)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CourseRoomName", entity.CourseRoomName);
                command.Parameters.AddWithValue("@EventPackage", entity.EventPackage);
                command.Parameters.AddWithValue("@TotalPrice", entity.TotalPrice);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(CourseRoom entity)
        {
            string query = "UPDATE CourseRooms SET CourseRoomName = @CourseRoomName, EventPackage = @EventPackage, TotalPrice = @TotalPrice WHERE CourseRoomID = @CourseRoomID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CourseRoomID", entity.CourseRoomID);
                command.Parameters.AddWithValue("@CourseRoomName", entity.CourseRoomName);
                command.Parameters.AddWithValue("@EventPackage", entity.EventPackage);
                command.Parameters.AddWithValue("@TotalPrice", entity.TotalPrice);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(CourseRoom entity)
        {
            string query = "DELETE FROM CourseRooms WHERE CourseRoomID = @CourseRoomID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CourseRoomID", entity.CourseRoomID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
