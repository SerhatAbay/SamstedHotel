using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamstedHotel.Model
{
    public class ReservationRoom
    {
        public int RoomID { get; set; }
        public int ReservationID { get; set; }

        public Room? Room { get; set; }
        public Reservation? Reservation { get; set; }

    }
}
