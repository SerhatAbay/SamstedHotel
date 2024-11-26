using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamstedHotel
{
    public class Reservation
    {
        private DateTime _startDate;
        private DateTime _endDate;

        public int ReservationID { get; set; }
        public int UserID { get; }

        public DateTime Created {  get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value < DateTime.Today)
                    throw new ArgumentException("Start dato skal være fra idag og frem .");
                _startDate = value;
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value <= _startDate)
                    throw new ArgumentException("Slutdato skal være efter startdato.");
                _endDate = value;
            }
        }

        public string BookingType { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

    }
}
