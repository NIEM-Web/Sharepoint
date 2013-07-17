using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;
using Microsoft.SharePoint.Utilities;

namespace Niem.MyNiem.Webparts.CrossSiteListWebpart
{
    [ToolboxItemAttribute(false)]
    public class CrossSiteListWebpart : WebPart
    {
        #region Properties
        //Properties
        protected string _ListName;
        protected string _WebUrl;
        protected string _ViewName;
        protected string _ExceptionList;

        [WebBrowsable(true),
        Category("List Properties"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("List Name:")]
        public string ListName
        {
            get
            {
                if (_ListName == null)
                    _ListName = "Discussion";
                return _ListName;
            }
            set
            {
                _ListName = value;
            }
        }
        [WebBrowsable(true),
        Category("List Properties"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Web Url:")]
        public string WebUrl
        {
            get
            {
                if (_WebUrl == null)
                    _WebUrl = SPContext.Current.Site.Url;
                //"http://mossloverserver:57/";
                return _WebUrl;
            }
            set
            {
                _WebUrl = value;
            }
        }
        [WebBrowsable(true),
        Category("List Properties"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("View Name:")]
        public string ViewName
        {
            get
            {
                if (_ViewName == null)
                    _ViewName = "Subject";
                return _ViewName;
            }
            set
            {
                _ViewName = value;
            }
        }
        [WebBrowsable(true),
        Category("List Properties"),
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

        #region CreateChildControls
        protected LiteralControl lcScript;

        protected override void CreateChildControls()
        {
            try
            {
                //if (WebUrl.Contains(SPContext.Current.Site.Url))
                //{
                using (SPSite Site = new SPSite(WebUrl))
                {
                    using (SPWeb Web = Site.OpenWeb())
                    {
                        Microsoft.SharePoint.WebPartPages.XsltListViewWebPart view = new Microsoft.SharePoint.WebPartPages.XsltListViewWebPart();
                        SPList List = Web.Lists[ListName];
                        if (List != null)
                        {
                            view.ListId = List.ID;
                            view.WebId = Web.ID; ;
                            view.ListName = List.ID.ToString();
                            view.ListUrl = Web.Url + "/" + List.RootFolder.Url;
                            view.TitleUrl = "/" + List.RootFolder.Url;
                            SPView View = List.Views[ViewName];
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<View Name=\"{" + View.ID.ToString() + "}\" MobileView=\"TRUE\" Type=\"HTML\" DisplayName=\"Subject\" Url=\"" + View.Url + "\" Level=\"1\" BaseViewID=\"3\" ContentTypeID=\"0x012001\" ImageUrl=\"/_layouts/images/vwdisc.png\">");
                            sb.Append("<Query>");
                            sb.Append("<Where>");
                            sb.Append("<Leq>");
                            sb.Append("<FieldRef Name='Created' />");
                            sb.Append("<Value Type='DateTime'>");
                            sb.Append("<Today Offset='-7' /></Value>");
                            sb.Append("</Leq>");
                            sb.Append("</Where>");
                            sb.Append("</Query>");
                            sb.Append("<ViewFields>");
                            sb.Append("<FieldRef Name=\"Title\" Explicit=\"TRUE\"/>");
                            sb.Append("<FieldRef Name=\"LinkDiscussionTitle\"/>");
                            sb.Append("<FieldRef Name=\"DiscussionLastUpdated\"/>");
                            sb.Append("<FieldRef Name=\"ItemChildCount\"/>");
                            sb.Append("<FieldRef Name=\"AverageRating\"/>");
                            sb.Append("</ViewFields>");
                            sb.Append("<RowLimit Paged=\"TRUE\">5</RowLimit>");
                            sb.Append("<Toolbar Type=\"Freeform\"/>");
                            sb.Append("</View>");
                            string xmldifinition = sb.ToString();
                            view.XmlDefinition = xmldifinition;
                            view.ChromeType = PartChromeType.None;
                            view.ViewContentTypeId = "0x012001";
                            view.GhostedXslLink = "main.xsl";
                            view.NoDefaultStyle = "TRUE";
                            view.ViewFlag = "8";
                            StringBuilder sbParam = new StringBuilder();
                            sbParam.Append("<ParameterBinding Name=\"dvt_sortdir\" Location=\"Postback;Connection\"/>");
                            sbParam.Append("<ParameterBinding Name=\"dvt_sortfield\" Location=\"Postback;Connection\"/>");
                            sbParam.Append("<ParameterBinding Name=\"dvt_startposition\" Location=\"Postback\" DefaultValue=\"\"/>");
                            sbParam.Append("<ParameterBinding Name=\"dvt_firstrow\" Location=\"Postback;Connection\"/>");
                            sbParam.Append("<ParameterBinding Name=\"OpenMenuKeyAccessible\" Location=\"Resource(wss,OpenMenuKeyAccessible)\" />");
                            sbParam.Append("<ParameterBinding Name=\"open_menu\" Location=\"Resource(wss,open_menu)\" />");
                            sbParam.Append("<ParameterBinding Name=\"select_deselect_all\" Location=\"Resource(wss,select_deselect_all)\" />");
                            sbParam.Append("<ParameterBinding Name=\"idPresEnabled\" Location=\"Resource(wss,idPresEnabled)\" />");
                            sbParam.Append("<ParameterBinding Name=\"NoAnnouncements\" Location=\"Resource(wss,noXinviewofY_LIST)\" />");
                            sbParam.Append("<ParameterBinding Name=\"NoAnnouncementsHowTo\" Location=\"Resource(core,noXinviewofY_DEFAULT)\" />");
                            sbParam.Append("<ParameterBinding Name=\"AddNewAnnouncement\" Location=\"Resource(wss,addnewitem)\" />");
                            sbParam.Append("<ParameterBinding Name=\"MoreAnnouncements\" Location=\"Resource(wss,moreItemsParen)\" />");
                            view.ParameterBindings = sb.ToString();

                            string DiscussionUrl = Web.Url + "/" + List.RootFolder.Url + "/Flat.aspx";
                            lcScript = new LiteralControl();
                            lcScript.ID = "lcEncodedUrls";
                            lcScript.Text = "<div id='EncodedInfo' style='display:none;'>" + DiscussionUrl + ";" + SPContext.Current.Web.ServerRelativeUrl + "/pages/discussions.aspx</div>";
                            Controls.Add(lcScript);

                            Controls.Add(view);
                        }
                    }
                }
                //}
            }
            catch (SPException ex)
            {
            }
        }
        #endregion
    }
}
