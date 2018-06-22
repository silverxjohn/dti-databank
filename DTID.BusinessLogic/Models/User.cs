using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class User
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
