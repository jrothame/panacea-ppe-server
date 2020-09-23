using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace upload.web.Classes
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseApiController : ApiController
    {
        public HttpResponseMessage UnauthorizedMessage
        {
            get
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }

        public HttpResponseMessage ForbiddenMessage
        {
            get
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
    }
}