using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MyNiemFix3.DummyWebPartToForceDeployToSelection
{
    [ToolboxItemAttribute(false)]
    public class DummyWebPartToForceDeployToSelection : WebPart
    {
        protected override void CreateChildControls()
        {
        }
    }
}
