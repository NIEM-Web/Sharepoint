using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Niem.MyNiem.Webparts.SaveSearch
{
    [ToolboxItemAttribute(false)]
    public class SaveSearch : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/Niem.MyNiem.Webparts/SaveSearch/SaveSearchUserControl.ascx";

        protected override void CreateChildControls()
        {
            if (SPContext.Current.Web.CurrentUser != null)
            {
                Control control = Page.LoadControl(_ascxPath);
                Controls.Add(control);
            }
        }
    }
}
