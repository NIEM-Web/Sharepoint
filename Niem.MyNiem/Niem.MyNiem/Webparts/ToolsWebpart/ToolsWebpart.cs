using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Niem.MyNiem.Webparts.ToolsWebpart
{
    [ToolboxItemAttribute(false)]
    public class ToolsWebpart : WebPart
    {
        #region properties
        //properties
        protected string _ContentTypeTools;
        protected string _ExceptionList;
        [WebBrowsable(true),
       Category("Tools Properties"),
       Personalizable(PersonalizationScope.Shared),
       WebDisplayName("Content Type:")]
        public string ContentTypeTools
        {
            get
            {
                if (_ContentTypeTools == null)
                    _ContentTypeTools = "Tools";
                return _ContentTypeTools;
            }
            set
            {
                _ContentTypeTools = value;
            }
        }
        [WebBrowsable(true),
        Category("Tools Properties"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Exception List:")]
        public string ExceptionList
        {
            get
            {
                if (_ExceptionList == null)
                    _ExceptionList = "Exception List";
                return _ExceptionList;
            }
            set
            {
                _ExceptionList = value;
            }
        } 
        #endregion

        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/Niem.MyNiem.Webparts/ToolsWebpart/ToolsWebpartUserControl.ascx";

        protected override void CreateChildControls()
        {
           Control control = Page.LoadControl(_ascxPath);
           if (control != null)
           {
               ((ToolsWebpartUserControl)control).ToolsContentType = ContentTypeTools;
           }
           Controls.Add(control);
        }
    }
}
