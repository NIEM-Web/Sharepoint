using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace lmd.NIEM.FarmSolution.HttpModules
{
    public class PageNotFoundHttpModule : IHttpModule
    {
        private HttpApplication app;
        private string pageNotFoundUrl = "/pages/page-not-found.aspx";

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            app = context;
            app.PreSendRequestContent += new EventHandler(app_PreSendRequestContent);
        }

        void app_PreSendRequestContent(object sender, EventArgs e)
        {
            HttpResponse res = app.Response;
            HttpRequest req = app.Request;

            if (res.StatusCode == 404 &&
                !req.Url.AbsolutePath.Equals(pageNotFoundUrl, StringComparison.InvariantCultureIgnoreCase))
            {
                app.Server.TransferRequest(String.Format("{0}?url={1}", pageNotFoundUrl, req.Url.ToString()));
            }
        }
    }
}
