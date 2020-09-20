using upload.web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using upload.library;

namespace upload.web.Controllers.api
{
    public class UploadTestController : BaseApiController
    {
        public HttpResponseMessage Get()
        {
            List<string> list = OrganizationHelper.GetOrganizations();
            return Request.CreateResponse<List<string>>(HttpStatusCode.OK, list);
        }

        public HttpResponseMessage Get(string id)
        {
            return Request.CreateResponse<string>(HttpStatusCode.OK, "Made it to get with: " + id);
        }
    }
}
