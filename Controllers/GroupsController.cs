﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using toBee_Serverside.Models;

namespace toBee_Serverside.Controllers
{
    public class GroupsController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get(int gid)
        {
            try
            {

                Group g = new Group();
                g = g.GetGroup(gid);
                return Ok(g);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        //POST api/<controller>
        public IHttpActionResult Post([FromBody] Group newG)
        {
            try
            {
                newG = newG.PostGroup();
                return Created(new Uri(Request.RequestUri.AbsoluteUri + newG.Gid), newG);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}