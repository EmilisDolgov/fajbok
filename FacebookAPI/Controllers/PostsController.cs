using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using FacebookAPI.Cassandra;

namespace FacebookAPI.Controllers
{
    public class PostsController : ApiController
    {
        private readonly ISession _session;
        private readonly IMapper _mapper;
        public PostsController()
        {
            _session = CassandraContext.OpenConnection();
            _mapper = new Mapper(_session);
        }
        [HttpGet]
        [Route("api/posts")]
        public IHttpActionResult GetAll()
        {
            
            var posts = _mapper.Fetch<Post>("SELECT * FROM post");
            return Ok(posts);
        }
        [HttpPost]
        [Route("api/posts")]
        public IHttpActionResult Store(Post post)
        {
            var timestamp = DateTimeOffset.Now;
            post.Post_id = TimeUuid.NewId(timestamp);
            post.PostedAt = timestamp;
            var postStm = _session.Prepare("INSERT INTO post (email, post_id, content, likes, postedat) VALUES (?,?,?,?,?);");
            var batch = new BatchStatement();
            batch.Add(postStm.Bind(post.Email, post.Post_id, post.Content, post.Likes, post.PostedAt));
            _session.Execute(batch);
            return Ok();
        }
        [HttpGet]
        [Route("api/posts/{email}/")]
        public IHttpActionResult GetByEmail(string email)
        {
            var posts = _mapper.Fetch<Post>("SELECT * FROM post WHERE email = ?;", email);
            return Ok(posts);
        }
        [HttpPut]
        [Route("api/posts/{email}/{id}/user/{uEmail}/")]
        public IHttpActionResult LikePost(string email, Guid id, string uEmail)
        {
            var likeStm = _session.Prepare("UPDATE post SET likes = likes + ['"+ uEmail +"'] WHERE email = ? and post_id = ?;");
            var stm = likeStm.Bind(email, id);
            _session.Execute(stm);
            return Ok();
        }

    }
}
