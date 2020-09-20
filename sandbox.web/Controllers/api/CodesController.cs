﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using upload.web.Classes;

namespace upload.web.Controllers.api
{
    public class CodesController : BaseApiController
    {
       
        public IHttpActionResult Get(string type, string id)
        {
  
            List<string> codes = new List<string>();
            List<HCPCSFile> files = new List<HCPCSFile>();

            IHttpActionResult result = null;

            try
            {
                switch (type)
                {
                    //Revenue Codes
                    case "rev":

                        codes = ExtensionMethods.GetRevCodes(id);

                        //codes were retrieved
                        if (codes.Count > 0)
                        {
                            result = Ok(codes);    
                        }

                        //codes were not retrieved
                        else
                        {
                            result = NotFound();
                           
                        }
                        break;
                    
                    //HCPCS Codes
                    case "hcpcs":

                        codes = ExtensionMethods.GetHCPCSCodes(id);

                        //codes were retrieved
                        if (codes.Count > 0)
                        {
                            result = Ok(codes);
                        }

                        //codes were not retrieved
                        else
                        {
                            result = NotFound();

                        }

                        break;

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