using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamstedHotel
{
    public class CustomerRepo :IRepository<Customer>
    {

        private readonly string _connectionString;

        public CustomernRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IEnumerable<Customer> GetAll()
    }
}
