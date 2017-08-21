using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI_1.Controllers
{

    // [Authorize]

    public class hwController : ApiController
    {
        #region initialize
        static List<Message> localValues = InitialMessages();
  
        private static List<Message> InitialMessages()
        {
            var ret = new List<Message>();

            ret.Add(new Message { id = 0, msg = "Hello World" });
            ret.Add(new Message { id = 1, msg = "It's a new world" });
            return ret;
        }

        public class Message
        {
            public int id;
            public string msg;
        }
        #endregion

        // RESTful verbs

        #region GET
        public IEnumerable<Message> Get()
        {
            return localValues;
        }
        #endregion
        // GET api/hw/1
        #region GET by id
        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage responseMsg = null;

            var retrievedMsg = (from c in localValues where c.id == id select c).FirstOrDefault();
            if (Request != null)
            {

                if(retrievedMsg == null)
                {
                    responseMsg = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Value does not exist");         
                }
                else
                {
                    responseMsg = Request.CreateResponse<string>(HttpStatusCode.OK, retrievedMsg.msg);

                }
            }
            else
            {
                responseMsg = new HttpResponseMessage();
                if (retrievedMsg == null)
                {
                    responseMsg.StatusCode = HttpStatusCode.NotFound;
                    responseMsg.ReasonPhrase = "Value does not exist";
                }
                else
                {
                    responseMsg.StatusCode = HttpStatusCode.OK;
                    var content = new StringContent(retrievedMsg.msg);
                    content.Headers.Expires = DateTime.Now.AddHours(4);
                    content.Headers.ContentType.MediaType = "text/plain";
                    responseMsg.Content = content;
                }
            }
            return responseMsg;
        }
        #endregion

        // POST api/hw/msg
        #region POST
        public HttpResponseMessage Post([FromBody]string newMsg)
        {
            int id = 0;
            HttpResponseMessage responseMsg = null;
            // get highest id
            var retrievedMsg = (from c in localValues where c.id > 0 select c).OrderByDescending(c => c.id).First();

            if (retrievedMsg != null)
                id = retrievedMsg.id + 1;
            if(Request != null)
            {
                try
                {
                    localValues.Add(new Message { id = id, msg = newMsg });
                    responseMsg = Request.CreateResponse(HttpStatusCode.Created);
                    responseMsg.Headers.Location = new Uri(Request.RequestUri + id.ToString());
                }
                catch(Exception ex)
                {
                    responseMsg = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Add message failed" + ex.Message);
                }
            }
            else
            {
                responseMsg = new HttpResponseMessage();
                try
                {
                    localValues.Add(new Message { id = id, msg = newMsg });
                    responseMsg.StatusCode = HttpStatusCode.Created;
                }
                catch (Exception ex)
                {
                    responseMsg.StatusCode = HttpStatusCode.InternalServerError;
                    responseMsg.ReasonPhrase = "Add message failed" + ex.Message;
                }
            }
            return responseMsg;
        }
        #endregion
        // PUT api/hw/msg at id
        #region PUT
        public HttpResponseMessage Put(int id, [FromBody]string newMsg)
        {
            HttpResponseMessage responseMsg = null;
            var retrievedMsg = (from c in localValues where c.id > 0 select c).OrderByDescending(c => c.id).First();
            if(Request != null)
            {
                if (retrievedMsg == null)
                {
                    responseMsg = Request.CreateErrorResponse(HttpStatusCode.NotFound, "id does not exist");
                }
                else
                {
                    retrievedMsg.msg = newMsg;
                    responseMsg = Request.CreateResponse(HttpStatusCode.OK, retrievedMsg.msg);
                }
            }
            else
            {
                responseMsg = new HttpResponseMessage();
                if (retrievedMsg == null)
                {
                    responseMsg.StatusCode = HttpStatusCode.NotFound;
                    responseMsg.ReasonPhrase = "Value does not exist";
                }
                else
                {
                    retrievedMsg.msg = newMsg;
                    responseMsg.StatusCode = HttpStatusCode.OK;
                    var content = new StringContent(retrievedMsg.msg);
                    content.Headers.Expires = DateTime.Now.AddHours(4);
                    content.Headers.ContentType.MediaType = "text/plain";
                    responseMsg.Content = content;
                }
            }
            return responseMsg;
        }
        #endregion
        // DELETE api/hw/
        #region DELETE
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage responseMsg = null;
            var retrievedMsg = (from c in localValues where c.id == id select c).FirstOrDefault();
            if(Request != null)
            { 
                if (retrievedMsg == null)
                {
                    responseMsg = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Value does not exist");
                }
                else
                {
                    localValues.Remove(retrievedMsg);
                    responseMsg = Request.CreateResponse(HttpStatusCode.OK, retrievedMsg.msg);
                }
            }
            else
            {
                responseMsg = new HttpResponseMessage();
                if (retrievedMsg == null)
                {
                    responseMsg.StatusCode = HttpStatusCode.NotFound;
                    responseMsg.ReasonPhrase = "Value does not exist";
                }
                else
                {
                    localValues.Remove(retrievedMsg);
                    responseMsg.StatusCode = HttpStatusCode.OK;
                    responseMsg.ReasonPhrase = "Successfully Deleted item " + retrievedMsg.id.ToString();
                }
            }
            return responseMsg;
        }
        #endregion
    }
}
