using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookAPI.Cassandra
{
    public class UserByBirthdate
    {
        public int Month { get; set; }
        public int Day { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}