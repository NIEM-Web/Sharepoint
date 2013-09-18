using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Web;

namespace NIEM_Like_Solution.ControlTemplates.NIEM_Like_Solution
{
    public partial class NIEM_Like_Control : UserControl
    {
        private string url;
        public string URL
        {
            set
            {
                
                if (!string.IsNullOrEmpty(value.Trim()))
                {
                    value = value.Replace("2;#", "").Replace("%20", " ");
                    if (!value.Contains("://"))
                    {
                        string newURL = SPContext.Current.Site.Url;
                        if (value.StartsWith("/"))
                            newURL = newURL + value;
                        else
                            newURL = newURL + "/" + value;

                        url = newURL;
                        hdnUrl.Value = url;
                    }
                    else
                        url = value;
                }
                else
                    url = value;

            }
            get { return url; }
        }

        private string webid;
        public string WebID
        {
            set {
                if (!string.IsNullOrEmpty(value.Trim()))
                {
                    webid = value;
                    hdnWebId.Value = webid;
                }
                else
                    webid = value;
            }
            get { return webid; }
        }

        private string listid;
        public string ListID
        {
            set {
                if (!string.IsNullOrEmpty(value.Trim()))
                {
                    listid = value;
                    hdnListId.Value = listid;
                }
                else
                    listid = value; 
            }
            get { return listid; }
        }

        private string itemid;
        public string ItemID
        {
            set {
                if (!string.IsNullOrEmpty(value.Trim()))
                {
                    itemid = value;
                    hdnItemId.Value = itemid;
                }
                else
                    itemid = value;
            }
            get { return itemid; }
        }
        private string ctype;
        public string CType
        {
            set
            {
                if (!string.IsNullOrEmpty(value.Trim()))
                {
                    ctype = value;
                    hdnCType.Value = ctype;
                }
                else
                    ctype = value;
            }
            get { return ctype; }
        }


        private string user;
        public string User
        {
            set { user = value; }
            get { return user; }
        }
        void LogText(string message)
        {
            //System.IO.File.AppendAllText("C:\\temp\\log.txt", message + "\r\n");
        }

        void SetLinkText()
        {
            URL = hdnUrl.Value;
            WebID = hdnWebId.Value;
            ListID = hdnListId.Value;
            ItemID = hdnItemId.Value;
            CType = hdnCType.Value;
            try
            {
                string siteURL = HttpContext.Current.Request.Url.AbsoluteUri;
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.RootWeb)
                    {
                        SPList list = web.Lists["LikeStatus"];
                        SPQuery query = new SPQuery();
                        if (!string.IsNullOrEmpty(URL))
                        {
                            query.Query = "<Where><And>" +
                                                "<Eq><FieldRef Name='Title'/><Value Type='Text'>" + URL + "</Value></Eq>" +
                                                "<Eq><FieldRef Name='SPUser'/><Value Type='Text'>" + SPContext.Current.Web.CurrentUser.LoginName + "</Value></Eq>" +
                                           "</And></Where>";
                        }
                        else
                        {
                            query.Query = "<Where><And>" +
                                                "<Eq><FieldRef Name='WebID'/><Value Type='Text'>" + WebID + "</Value></Eq>" +
                                                "<Eq><FieldRef Name='ListID'/><Value Type='Text'>" + ListID + "</Value></Eq>" +
                                                "</And><And><Eq><FieldRef Name='ItemID'/><Value Type='Text'>" + ItemID + "</Value></Eq>" +
                                                "<Eq><FieldRef Name='SPUser'/><Value Type='Text'>" + SPContext.Current.Web.CurrentUser.LoginName + "</Value></Eq>" +
                                           "</And></Where>";
                        }
                        SPListItemCollection items = list.GetItems(query);
                        if (items.Count > 0)
                            lnkLike.Text = "Unlike";
                        else
                            lnkLike.Text = "Like";

                    }
                }
            }
            catch (Exception ex)
            {
                LogText(ex.Message);
            }
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {

           

           
            


        }


        protected void lnkLike_Click(object sender, EventArgs e)
        {
            if (lnkLike.Text == "Like")
                DoLike();
            else
                DoUnLike();

            Response.Redirect(Request.RawUrl, true);
        }

        void DoUnLike()
        {
            URL = hdnUrl.Value;
            WebID = hdnWebId.Value;
            ListID = hdnListId.Value;
            ItemID = hdnItemId.Value;
            try
            {
                using (SPSite site = SPContext.Current.Site)
                {
                    using (SPWeb web = site.RootWeb)
                    {
                        SPList list = web.Lists["LikeStatus"];
                        SPQuery query = new SPQuery();
                        if (!string.IsNullOrEmpty(URL))
                        {
                            query.Query = "<Where><And>" +
                                                "<Eq><FieldRef Name='Title'/><Value Type='Text'>" + URL + "</Value></Eq>" +
                                                "<Eq><FieldRef Name='SPUser'/><Value Type='Text'>" + SPContext.Current.Web.CurrentUser.LoginName + "</Value></Eq>" +
                                           "</And></Where>";
                        }
                        else
                        {
                            query.Query = "<Where><And>" +
                                                "<Eq><FieldRef Name='WebID'/><Value Type='Text'>" + WebID + "</Value></Eq>" +
                                                "<Eq><FieldRef Name='ListID'/><Value Type='Text'>" + ListID + "</Value></Eq>" +
                                                "</And><And><Eq><FieldRef Name='ItemID'/><Value Type='Text'>" + ItemID + "</Value></Eq>" +
                                                "<Eq><FieldRef Name='SPUser'/><Value Type='Text'>" + SPContext.Current.Web.CurrentUser.LoginName + "</Value></Eq>" +
                                           "</And></Where>";
                        }
                        SPListItemCollection items = list.GetItems(query);
                        foreach (SPListItem item in items)
                        {
                            item.Delete();

                        }
                        list.Update();

                    }
                }
            }
            catch (Exception ex)
            {
                // Controls.Add(new LiteralControl(ex.Message));
            }

            SetLinkText();

        }
        void DoLike()
        {
            URL = hdnUrl.Value;
            WebID = hdnWebId.Value;
            ListID = hdnListId.Value;
            ItemID = hdnItemId.Value;
            CType = hdnCType.Value;
            SPListItem item = null;
            SPSite site = null;
            SPWeb web = null;
            SPList list = null;
            LogText("1 URL:" + URL);

            if (!string.IsNullOrEmpty(URL.Trim()))
            {
                //string siteURL = SPContext.Current.Site.MakeFullUrl(URL);
                site = new SPSite(URL);
                LogText("site:" + site.Url);
                web = site.OpenWeb();
                LogText("web:" + web.Url);
                item = web.GetListItem(URL);
                LogText("item:" + item.Url);
                list = item.ParentList;
                LogText("list:" + list.Title);
                //  Controls.Add(new LiteralControl(item.ID.ToString()));

                LogText("2 site.RootWeb:" + site.RootWeb.Url);
                LogText("3 ItemID:" + item.ID);
                using (SPWeb rootWeb = new SPSite(URL).RootWeb)
                {
                    SPList likeList = rootWeb.Lists["LikeStatus"];
                    SPListItem newItem = likeList.AddItem();
                    newItem["Title"] = web.Url + "/" + item.Url;
                    newItem["WebID"] = web.ID.ToString();
                    newItem["ListID"] = list.ID.ToString();
                    newItem["ItemID"] = item.ID;
                    newItem["SPUser"] = SPContext.Current.Web.CurrentUser.LoginName;
                    newItem["CType"] = CType;
                    newItem.Update();
                    likeList.Update();

                }

            }
            else
            {
                site = SPContext.Current.Site;

                web = site.OpenWeb(new Guid(WebID));
                LogText("5: web:" + web.Url);
                list = web.Lists[new Guid(ListID)];
                LogText("6: list:" + list.DefaultViewUrl);
                item = list.GetItemById(int.Parse(ItemID));
                LogText("7: item:" + item.Url);
                // Controls.Add(new LiteralControl(item.ID.ToString()));

                LogText("2 site.RootWeb:" + site.RootWeb.Url);
                LogText("3 ItemID:" + item.ID);
                string siteURL = HttpContext.Current.Request.Url.AbsoluteUri;
                using (SPWeb rootWeb = new SPSite(siteURL).RootWeb)
                {
                    SPList likeList = rootWeb.Lists["LikeStatus"];
                    SPListItem newItem = likeList.AddItem();
                    newItem["Title"] = item.Url;
                    newItem["WebID"] = web.ID.ToString();
                    newItem["ListID"] = list.ID.ToString();
                    newItem["ItemID"] = item.ID;
                    newItem["SPUser"] = SPContext.Current.Web.CurrentUser.LoginName;
                    newItem.Update();
                    likeList.Update();


                }
            }

            SetLinkText();



        }

        protected void lnkLike_PreRender(object sender, EventArgs e)
        {

            if (SPContext.Current.Web.CurrentUser == null || SPContext.Current.Web.CurrentUser.LoginName == string.Empty)
            {
                lnkLike.Visible = false;

            }
            else
            {
                SetLinkText();
               // Controls.Add(new LiteralControl("URL:" + URL));
            }
        }
    }
}
