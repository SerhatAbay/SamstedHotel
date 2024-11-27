using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamstedHotel
{
    public class Customer
    {
       public int CustomerID { get; set; }
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        public string CustomerAddress { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerZip { get; set; }
        public string CustomerCountry { get; set; }


    }
}
