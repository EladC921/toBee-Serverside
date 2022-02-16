using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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

        ////Put api/<controller>
        //[HttpPut]
        //[Route("api/Users/EditUserProfilePic")]
        //public IHttpActionResult EditUserProfilePic([FromBody] User u)
        //{
        //    try
        //    {
        //        u = u.EditUserProfilePic();
        //        return Ok(u);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.BadRequest, ex);
        //    }
        //}

        //    [HttpPost]
        //    [Route("api/Users/Uploadpictures")]
        //    public Task<HttpResponseMessage> Uploadpictures()
        //    {
        //        string outputForNir = "start---";
        //        List<string> savedFilePath = new List<string>();
        //        if (!Request.Content.IsMimeMultipartContent())
        //        {
        //            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //        }

        //        //Where to put the picture on server  ...MapPath("~/TargetDir")
        //        string rootPath = HttpContext.Current.Server.MapPath("~/Assets");
        //        var provider = new MultipartFileStreamProvider(rootPath);
        //        var task = Request.Content.ReadAsMultipartAsync(provider).
        //            ContinueWith<HttpResponseMessage>(t =>
        //            {
        //                if (t.IsCanceled || t.IsFaulted)
        //                {
        //                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
        //                }
        //                foreach (MultipartFileData item in provider.FileData)
        //                {
        //                    try
        //                    {
        //                        outputForNir += " ---here";
        //                        string name = item.Headers.ContentDisposition.FileName.Replace("\"", "");
        //                        outputForNir += " ---here2=" + name;

        //                        //need the guid because in react native in order to refresh an inamge it has to have a new name
        //                        string newFileName = Path.GetFileNameWithoutExtension(name) + "_" + CreateDateTimeWithValidChars() + Path.GetExtension(name);
        //                        //string newFileName = Path.GetFileNameWithoutExtension(name) + "_" + Guid.NewGuid() + Path.GetExtension(name);
        //                        //string newFileName = name + "" + Guid.NewGuid();
        //                        outputForNir += " ---here3" + newFileName;

        //                        //delete all files begining with the same name
        //                        string[] names = Directory.GetFiles(rootPath);
        //                        foreach (var fileName in names)
        //                        {
        //                            if (Path.GetFileNameWithoutExtension(fileName).IndexOf(Path.GetFileNameWithoutExtension(name)) != -1)
        //                            {
        //                                File.Delete(fileName);
        //                            }
        //                        }

        //                        //File.Move(item.LocalFileName, Path.Combine(rootPath, newFileName));
        //                        File.Copy(item.LocalFileName, Path.Combine(rootPath, newFileName), true);
        //                        File.Delete(item.LocalFileName);
        //                        outputForNir += " ---here4";

        //                        Uri baseuri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty));
        //                        outputForNir += " ---here5";
        //                        string fileRelativePath = "~/Assets/" + newFileName;
        //                        outputForNir += " ---here6 imageName=" + fileRelativePath;
        //                        Uri fileFullPath = new Uri(baseuri, VirtualPathUtility.ToAbsolute(fileRelativePath));
        //                        outputForNir += " ---here7" + fileFullPath.ToString();
        //                        savedFilePath.Add(fileFullPath.ToString());

        //                        // ~~~~~~~~~~~~~~~~~~~ Save the file path to the SQL Server ~~~~~~~~~~~~~~~~~~~
        //                        string[] subs = name.Split('_', '.');
        //                        string uidStr = subs[0];
        //                        int uid = Convert.ToInt32(uidStr);
        //                        try
        //                        {
        //                            User u = new User
        //                            {
        //                                Uid = uid,
        //                                ImgURL = savedFilePath[0]
        //                            };
        //                            u = u.EditUserProfilePic();
        //                            return Request.CreateResponse(HttpStatusCode.Created, u);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        outputForNir += " ---excption=" + ex.Message;
        //                        string message = ex.Message;
        //                    }
        //                }

        //                return Request.CreateResponse(HttpStatusCode.Created, savedFilePath[0]);
        //            });
        //        return task;
        //    }

        //    private string CreateDateTimeWithValidChars()
        //    {
        //        return DateTime.Now.ToString().Replace('/', '_').Replace(':', '-').Replace(' ', '_');
        //    }
    }
}