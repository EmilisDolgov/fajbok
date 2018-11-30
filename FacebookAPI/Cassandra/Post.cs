using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookAPI.Cassandra
{
    public class Post
    {
        public string Email { get; set; }
        public Guid Post_id { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> Likes { get; set; }
        public DateTimeOffset PostedAt { get; set; }
    }
}