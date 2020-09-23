using System;
using System.Collections.Generic;
using System.Web.Http;
using upload.web.Classes;

namespace upload.web.Controllers.api
{
    public class FilesController : BaseApiController
    {
        // GET: api/Files
        public IHttpActionResult Get()
        {
            IHttpActionResult result = null;

            List<HCPCSFile> files = new List<HCPCSFile>();
          

            try
            {
                files = ExtensionMethods.GetHCPCSFiles();

                //files were retrieved
                if (files.Count > 0)
                {
                    result = Ok(files);
                }
                else
                {
                    result = NotFound();
                }

                return result;
            }

            catch (Exception ex)
            {
                return InternalServerError(ex);

            }

          
        }

       

       
    }
}
