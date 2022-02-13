using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using toBee_Serverside.Models;

namespace toBee_Serverside.Controllers
{
    public class TasksController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        [Route("api/Tasks/GetTasksOfGroup")]
        public IHttpActionResult GetTasksOfGroup(int gid)
        {
            try
            {
                Task t = new Task();
                List<Task> tasks = t.GetTasksOfGroup(gid);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        // GET api/<controller>
        [HttpGet]
        [Route("api/Tasks/GetProfileTasksOfUser")]
        public IHttpActionResult GetProfileTasksOfUser(int uid)
        {
            try
            {
                Task t = new Task();
                List<Task> tasks = t.GetProfileTasksOfUser(uid);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }
        
        // GET api/<controller>
        [HttpGet]
        [Route("api/Tasks/GetAvailableTasksInAllGroups")]
        public IHttpActionResult GetAvailableTasksInAllGroups(int uid)
        {
            try
            {
                Task t = new Task();
                List<Task> tasks = t.GetAvailableTasksInAllGroups(uid);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }
             
        // GET api/<controller>
        [HttpGet]
        [Route("api/Tasks/GetTasksOfRegUserInAllGroups")]
        public IHttpActionResult GetTasksOfRegUserInAllGroups(int uid)
        {
            try
            {
                Task t = new Task();
                List<Task> tasks = t.GetTasksOfRegUserInAllGroups(uid);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        //POST api/<controller>
        public IHttpActionResult Post([FromBody] Task newT)
        {
            try
            {
                List<Task> tasks = newT.PostTask();
                return Created(new Uri(Request.RequestUri.AbsoluteUri), tasks);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}