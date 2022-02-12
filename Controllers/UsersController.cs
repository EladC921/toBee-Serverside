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

        //POST api/<controller>
        public IHttpActionResult Post([FromBody] User newU)
        {
            try
            {
                newU.PostUser();
                return Created(new Uri(Request.RequestUri.AbsoluteUri + newU.Uid), newU);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}