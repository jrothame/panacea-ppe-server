using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upload.library
{
    public static class OrganizationHelper
    {

        public static List<string> GetOrganizations()
        {
            List<string> list = new List<string>();
            list.Add("Panacea");
            list.Add("Integris");
            list.Add("Trinity System");

            return list;
        }
    }
}
