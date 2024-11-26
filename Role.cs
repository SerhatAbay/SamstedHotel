using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamstedHotel
{
    public class Role
    {
        public int RoleID { get; } 

        public string RoleName { get; set; } 

        public string Description { get; set; } 

        public Role(int roleId, string roleName, string description)
        {
            RoleID = roleId;
            RoleName = roleName;
            Description = description;
        }
    }
}
