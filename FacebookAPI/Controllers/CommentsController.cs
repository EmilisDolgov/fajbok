using Cassandra;
using Cassandra.Mapping;
using FacebookAPI.Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FacebookAPI.Controllers
{
    public class CommentsController : ApiController
    {
        private readonly ISession _session;
        private readonly IMapper _mapper;
        public CommentsController()
        {
            _session = CassandraContext.OpenConnection();
            _mapper = new Mapper(_session);
        }
        [HttpPost]
        [Route("api/comments")]
        public IHttpActionResult Store(Comment comment)
        {
            var timestamp = DateTimeOffset.Now;
            comment.Comment_id = TimeUuid.NewId(timestamp);
            comment.PostedAt = timestamp;
            var commentStm = _session.Prepare("INSERT INTO comment (post_id, comment_id, content, likes, postedat, email) VALUES (?,?,?,?,?,?) IF NOT EXISTS;");
            var stm  = commentStm.Bind(comment.Post_id, comment.Comment_id, comment.Content, comment.Likes, comment.PostedAt, comment.Email);
            _session.Execute(stm);
            return Ok();
        }
        [HttpGet]
        [Route("api/comments/{postid}")]
        public IHttpActionResult GetComments(Guid postid)
        {
            var comments = _mapper.Fetch<Comment>("SELECT * FROM comment WHERE post_id = ?;", postid);
            return Ok(comments);
        }

  
    }
}
