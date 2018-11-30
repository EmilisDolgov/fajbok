using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookAPI.Cassandra
{
    public class User
    {
        public string Email { get; set; }
        public string Birthdate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTimeOffset RegistredAt { get; set; }
    }
}