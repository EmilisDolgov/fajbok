using Cassandra;
using FacebookAPI.Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookAPI
{
    public class CassandraContext
    {
        public static ISession OpenConnection()
        {
            var cluster = Cluster.Builder()
                     .AddContactPoints("localhost")
                     .Build();
            var session = cluster.Connect("facebook");
            return session;
        }
    }
}
