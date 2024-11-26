using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SamstedHotel
{
    using BCrypt.Net;

    public class User
    {

        private string _passwordHash;
        public int UserId { get; set; } // Auto-implemented property

        public string UserName { get; set; } // Auto-implemented property

        public int RoleId { get; set; } // Auto-implemented property

        // Navigation property for rollen
        public Role Role { get; set; }

        public string PasswordHash
        {
            get => _passwordHash; // Giver adgang til at læse _passwordHash
            private set // giver adgang til at skrive i klassen
            {
                if (string.IsNullOrEmpty(_passwordHash))
                    throw new ArgumentException("Password hash cannot be empty.");
                _passwordHash = value;
            }
        }
        public void SetPassword(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                throw new ArgumentException("Password cannot be empty.");

            // Use BCrypt to hash the password
            string hashedPassword = BCrypt.HashPassword(plainPassword);
            PasswordHash = hashedPassword; // Store in the backing field
        }
        public bool ValidatePassword(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                throw new ArgumentException("Password cannot be empty.");

            // Use BCrypt to compare the plain password with the stored hash
            return BCrypt.Verify(plainPassword, _passwordHash);
        }




        // Constructor for initialization
        public User(int userId, string userName, string passwordHash, int roleId)
        {
            UserId = userId;
            UserName = userName;
            PasswordHash = passwordHash;
            RoleId = roleId;
        }

     
    }
}
  


