using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cassandra;
using Cassandra.Mapping;
using FacebookAPI.Cassandra;

namespace FacebookAPI.Controllers
{
    public class UsersController : ApiController
    {
        private readonly ISession _session;
        private readonly IMapper _mapper;
        public UsersController()
        {
            _session = CassandraContext.OpenConnection();
            _mapper = new Mapper(_session);
        }
        [HttpGet]
        [Route("api/users")]
        public IHttpActionResult GetAll()
        {
            var users = _mapper.Fetch<User>("SELECT * FROM user");
            return Ok(users);
        }
        [HttpGet]
        [Route("api/users/{letters}")]
        public IHttpActionResult GetByFirstThreeLetters(string letters)
        {
            var users = _mapper.Fetch<UserByFirstThreeLetters>("SELECT * FROM user_by_three_letters WHERE partname =?", letters);
            return Ok(users);
        }
        [HttpPost]
        [Route("api/users")]
        public IHttpActionResult Store(User user)
        {
            var userStm = _session.Prepare("INSERT INTO user (email, birthdate, firstname, lastname, password, phonenumber, registredat) VALUES (?,?,?,?,?,?,?)");
            var userSearchStm = _session.Prepare("INSERT INTO user_by_three_letters (partname, email, fullname) VALUES (?,?,?)");
            var userByBirthdate = _session.Prepare("INSERT INTO user_by_birthdate (month, day, email, firstname, lastname) VALUES (?,?,?,?,?)");
            var batch = new BatchStatement();
            var date = user.Birthdate.Split('-');
            var birthdate = new LocalDate(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]));
            batch.Add(userStm.Bind(user.Email, birthdate, user.FirstName, user.LastName, user.Password, user.PhoneNumber, DateTimeOffset.Now));
            var partName = new string(user.FirstName.Take(3).ToArray());
            batch.Add(userSearchStm.Bind(partName, user.Email, user.FirstName + " " + user.LastName));
            batch.Add(userByBirthdate.Bind(birthdate.Month, birthdate.Day, user.Email, user.FirstName, user.LastName));
            _session.Execute(batch);
            return Ok();
        }
        [HttpGet]
        [Route("api/users/date/{datestr}/")]
        public IHttpActionResult GetByDate(string dateStr)
        {
            var dateArr = dateStr.Split('-');
            var month = Convert.ToInt32(dateArr[0]);
            var day = Convert.ToInt32(dateArr[1]);
            var birthdayPerson = _mapper.Fetch<UserByBirthdate>("SELECT * FROM user_by_birthdate WHERE day=? AND month = ?", day, month);
            return Ok(birthdayPerson);

        }
    }
}
