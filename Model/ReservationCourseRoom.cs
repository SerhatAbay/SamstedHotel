using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamstedHotel.Model
{
    public class ReservationCourseRoom
    {
        public int ReservationID { get; set; }
        public string CourseRoomName { get; set; } = string.Empty;

        // Optional: Navigation properties
        public Reservation? Reservation { get; set; }
        public CourseRoom? CourseRoom { get; set; }
    }
}
