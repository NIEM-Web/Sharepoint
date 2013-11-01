using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Niem.MyNiem.Webparts.NewsEventsWebpart
{
    [ToolboxItemAttribute(false)]
    public class NewsEventsWebpart : WebPart
    {
        #region properties
        //properties
        protected string _ContentTypeNews;
        protected string _ExceptionList;
        protected string _YourAudienceList;
        protected string _EstablishedCommunitiesList;
        [WebBrowsable(true),
       Category("News/Events Properties"),
       Personalizable(PersonalizationScope.Shared),
       WebDisplayName("Your Audience Listname:")]
        public string YourAudienceList
        {
            get
            {
                if (_YourAudienceList == null)
                    _YourAudienceList = "Audience";
                return _YourAudienceList;
            }
            set
            {
                _YourAudienceList = value;
            }
        }
        [WebBrowsable(true),
       Category("News/Events Properties"),
       Personalizable(PersonalizationScope.Shared),
       WebDisplayName("Communities Listname:")]
        public string EstablishedCommunitiesList
        {
            get
            {
                if (_EstablishedCommunitiesList == null)
                    _EstablishedCommunitiesList = "Communities";
                return _EstablishedCommunitiesList;
            }
            set
            {
                _EstablishedCommunitiesList = value;
            }
        }
        [WebBrowsable(true),
       Category("News/Events Properties"),
       Personalizable(PersonalizationScope.Shared),
       WebDisplayName("Content Type:")]
        public string ContentTypeNews
        {
            get
            {
                if (_ContentTypeNews == null)
                    _ContentTypeNews = "News Article";
                return _ContentTypeNews;
            }
            set
            {
                _ContentTypeNews = value;
            }
        }
        [WebBrowsable(true),
        Category("News/Events Properties"),
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
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/Niem.MyNiem.Webparts/NewsEventsWebpart/NewsEventsWebpartUserControl.ascx";

        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(_ascxPath);
            if (control != null)
            {
                ((NewsEventsWebpartUserControl)control).NewsContentType = ContentTypeNews;
                ((NewsEventsWebpartUserControl)control).EstablishedCommunitiesList = EstablishedCommunitiesList;
                ((NewsEventsWebpartUserControl)control).YourAudienceList = YourAudienceList;
            }
            Controls.Add(control);
        }
    }
}
