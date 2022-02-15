using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using toBee_Serverside.Models;

namespace toBee_Serverside.Controllers
{
    public class UsersController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get(int uid)
        {
            try
            {
                User u = new User();
                u = u.GetUser(uid);
                return Ok(u);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }
        
        // GET api/<controller>
        public IHttpActionResult Get(string mail)
        {
            try
            {
                User u = new User();
                u = u.GetUserByEmail(mail);
                return Ok(u);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        //POST api/<controller>
        public IHttpActionResult Post([FromBody] User newU)
        {
            try
            {
                int rowsAffected = newU.PostUser();
                return Created(new Uri(Request.RequestUri.AbsoluteUri), rowsAffected);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        //Put api/<controller>
        [HttpPut]
        [Route("api/Users/EditUserProfile")]
        public IHttpActionResult EditUserProfile([FromBody] User editedU)
        {
            try
            {
                editedU = editedU.EditUserProfile();
                return Ok(editedU);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        } 
        
        //Put api/<controller>
        [HttpPut]
        [Route("api/Users/EditUserProfilePic")]
        public IHttpActionResult EditUserProfilePic(string imgURL, int uid)
        {
            try
            {
                User u = new User();
                u = u.EditUserProfilePic(imgURL, uid);
                return Ok(u);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}