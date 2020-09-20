<%@ Page Language="C#" %>

<%
        // Set up variables
        string path = Server.MapPath("~/index.html");
        string application_path = Request.ApplicationPath;
        if (!application_path.EndsWith("/"))
            application_path += "/";
        string new_base_href = string.Format("<base href=\"{0}\">", application_path);
        string html = "";

        // Read in index.html
        using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
        {
            html = sr.ReadToEnd();
        }

        // Locate and replace base tag
        int start_index = html.IndexOf("<base");
        if (start_index > -1)
        {
            int end_index = html.IndexOf(">", start_index);
            string old_base_href = html.Substring(start_index, end_index - start_index + 1);
            html = html.Replace(old_base_href, new_base_href);
        }

        // Prevent Caching
        Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.Cache.SetExpires(DateTime.Now);
        Response.Cache.SetValidUntilExpires(true);

        // Write Response
        Response.Write(html);
        Response.End();



%>
