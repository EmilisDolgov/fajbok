using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookAPI.Cassandra
{
    public class Comment
    {
        public Guid Post_id { get; set; }
        public Guid Comment_id { get; set; }
        public string Content { get; set; }
        public List<Guid> Likes { get; set; }
        public DateTimeOffset PostedAt { get; set; }
        public string Email { get; set; }
    }
}