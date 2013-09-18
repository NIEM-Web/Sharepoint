using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Net;

namespace lmd.NIEM.FarmSolution.PageNotFoundWidgetWebpart
{
    [ToolboxItemAttribute(false)]
    public class PageNotFoundWidgetWebpart : WebPart
    {
        protected override void CreateChildControls()
        {
        }
        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
            Page.Response.StatusCode = (int)HttpStatusCode.NotFound;

        }

    }
}
