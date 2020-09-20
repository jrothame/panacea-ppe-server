using Newtonsoft.Json;
using sandbox.library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using upload.web.Classes;

namespace upload.web.Controllers.api
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]

    public class Data
    {
        public string contractid { get; set; }
        public string productid { get; set; }
        public string codedesc { get; set; }

    }

    
    public class ImportDataController : BaseApiController
    {
        [Route("api/ImportData")]
        [HttpPost]
        public HttpResponseMessage ImportData([FromBody] object data)
        {
            HttpResponseMessage result = null;

            try
            {
                var rows = JsonConvert.DeserializeObject<List<Data>>(data.ToString());

                if (rows.Count > 0)
                {
                    result = Request.CreateResponse(HttpStatusCode.OK, "Your data was processed successfully!");
                }

                else
                {
                    result = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Your data not received. Please try again.");
                }

            }

            catch (Exception ex)
            {
                result = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while processing data: " + ex.Message);
            }

            return result;

        }
    }
}
