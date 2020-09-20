using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using upload.web.Classes;
using System.Web;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace upload.web.Controllers.api
{
    
    public class BatchController : BaseApiController
    {

        public class Batch
        {
            public string batchId { get; set; }
            public string url { get; set; }
            public string fileName { get; set; }
            public string created { get; set; }
            public string submitted { get; set; }

        }

        public class BatchData
        {
          
            public string contractid { get; set; }
            public string productid { get; set; }
            public string codedesc { get; set; }
            public string rev { get; set; }
            public string ip_ind { get; set; }
            public string op_ind { get; set; }
            public string sds_ind { get; set; }
            public string er_ind { get; set; }
            public string ex_ip_ind { get; set; }
            public string ex_op_ind { get; set; }
            public string ex_sds_ind { get; set; }
            public string ex_er_ind { get; set; }
            public string hcpcs { get; set; }
        }

        public const string SQLCONN = "Server=10.7.2.12;Database=zbp_dev;User Id=jrothamel;Password=AM34pe9x!@#$";


        [Route("api/batch")]
        [HttpPost]
        public async Task<HttpResponseMessage> InsertBatchAsync([FromBody] object data)
        {
            HttpResponseMessage result = null;
            
            try
            {
                var rows = JsonConvert.DeserializeObject<List<Batch>>(data.ToString());

                //download the original source file
                byte[] bytes =  await ExtensionMethods.DownloadFile(rows[0].url);

                if (ExtensionMethods.InsertBatch(bytes, rows))
                {

                    result = Request.CreateResponse(HttpStatusCode.OK, "The file was loaded into the database successfully!");
                }
        
            }

            catch(Exception ex)
            {
                result = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while loading the orignal file into the database: " + ex.Message);
            }

            return result;
        }



        [Route("api/batchData/{batchId}")]
        [HttpPost]
        public HttpResponseMessage InsertBatchData([FromBody] object data, string batchId)
        {
            HttpResponseMessage result = null;

            try
            {
                var rows = JsonConvert.DeserializeObject<List<BatchData>>(data.ToString());

                

                if (ExtensionMethods.InsertBatchData(batchId, rows))
                {

                    result = Request.CreateResponse(HttpStatusCode.OK, "The file was loaded into the database successfully!");
                }

            }

            catch (Exception ex)
            {
                result = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while loading the orignal file into the database: " + ex.Message);
            }

            return result;
        }


    }
}
