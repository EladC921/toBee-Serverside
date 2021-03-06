using System;
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

        // GET api/<controller>
        [HttpGet]
        [Route("api/Groups/GetGroupsOfUser")]
        public IHttpActionResult GetGroupsOfUser(int uid)
        {
            try
            {
                Group g = new Group();
                List<Group> groups = g.GetGroupsOfUser(uid);
                return Ok(groups);
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

        //POST api/<controller> User in Group
        [HttpPost]
        [Route("api/Groups/PostUserInGroup")]
        public IHttpActionResult PostUserInGroup(int gid, string nickname)
        {
            try
            {
                Group g = new Group();
                g = g.PostUserInGroup(gid, nickname);
                return Created(new Uri(Request.RequestUri.AbsoluteUri + gid), g);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        //DELETE api/<controller> User from Group
        public IHttpActionResult Delete(int gid, int uid)
        {
            try
            {
                Group g = new Group();
                int rowsChanged = g.DeleteUserFromGroup(gid, uid);
                if (rowsChanged > 0) return Ok();
                return Content(HttpStatusCode.NotFound, $"Group with gid={gid} or User with uid={uid} was not found for deleting!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}