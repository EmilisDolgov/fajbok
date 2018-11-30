using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookAPI.Cassandra
{
    public class UserByFirstThreeLetters
    {
        public string PartName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}