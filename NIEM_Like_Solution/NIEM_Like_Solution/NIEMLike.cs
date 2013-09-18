using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Web;
using System.Web.UI;
using System.ComponentModel;

namespace NIEM_Like_Solution
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:NIEMLike runat=server></{0}:NIEMLike>")]
    public class NIEMLike : LinkButton
    {
        private HiddenField hdnUrl;
        private HiddenField hdnWebId;
        private HiddenField hdnListId;
        private HiddenField hdnItemId;
        private HiddenField hdnCType;
        
        
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("URL."),
        Localizable(true)
        ]
        public virtual string URL
        {
            set
            {
                CheckControls();
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

                        
                        hdnUrl.Value = newURL;
                    }
                    else
                        hdnUrl.Value = value;
                }
                else
                    hdnUrl.Value = value;

            }
            get { CheckControls(); return hdnUrl.Value; }

            
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("WebID."),
        Localizable(true)
        ]
        public virtual string WebID
        {
            set
            {
                CheckControls(); 
                hdnWebId.Value  = value;
            }
            get { CheckControls(); return hdnWebId.Value; }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("ListID."),
        Localizable(true)
        ]
        public virtual string ListID
        {
            set
            {
                CheckControls();
                hdnListId.Value = value;
            }
            get { CheckControls(); return hdnListId.Value; }

        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("ItemID."),
        Localizable(true)
        ]
        public virtual string ItemID
        {
            set
            {
                CheckControls();
                hdnItemId.Value = value;
            }
            get { CheckControls(); return hdnItemId.Value; }

        }

        [
       Bindable(true),
       Category("Appearance"),
       DefaultValue(""),
       Description("CType."),
       Localizable(true)
       ]
        public virtual string CType
        {
            set
            {
                CheckControls();
                hdnCType.Value = value;
            }
            get { CheckControls(); return hdnCType.Value; }

        }

        void CheckControls()
        {
            if (hdnUrl == null || hdnWebId == null)
            {
                hdnUrl = new HiddenField();
                hdnWebId = new HiddenField();
                hdnListId = new HiddenField();
                hdnItemId = new HiddenField();
                hdnCType = new HiddenField();
                
            }
        }

        protected override void OnClick(EventArgs e)
        {
            if (this.Text == "Like")
                DoLike();
            else
                DoUnLike();

            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl, true);
        }

        protected override void CreateChildControls()
        {
            if (!Page.IsPostBack)
            {
                this.EnableViewState = true;
                CheckControls();
                
                Controls.Add(hdnUrl);
                Controls.Add(hdnWebId);
                Controls.Add(hdnListId);
                Controls.Add(hdnItemId);
                Controls.Add(hdnCType);
                base.CreateChildControls();
            }
            

            if (SPContext.Current.Web.CurrentUser == null || SPContext.Current.Web.CurrentUser.LoginName == string.Empty)
            {
                this.Visible = false;
                //this.Text = "No User";
                

            }
            else
            {
                SetLinkText();
              // Controls.Add(new LiteralControl("URL:" + URL));
            }
        }

        void LogText(string message)
        {
            //System.IO.File.AppendAllText("C:\\temp\\log.txt", message + "\r\n");
        }

        void SetLinkText()
        {
            
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
                            this.Text = "Unlike";
                        else
                            this.Text = "Like";

                    }
                }
            }
            catch (Exception ex)
            {
                LogText(ex.Message);
            }
        }

        
       

        void DoUnLike()
        {
            
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

        //protected override void RenderContents(HtmlTextWriter writer)
        //{
        //    writer.WriteEncodedText(Text);

        //    string displayUserName = DefaultUserName;
        //    if (Context != null)
        //    {
        //        string userName = Context.User.Identity.Name;
        //        if (!String.IsNullOrEmpty(userName))
        //        {
        //            displayUserName = userName;
        //        }
        //    }

        //    if (!String.IsNullOrEmpty(displayUserName))
        //    {
        //        writer.Write(", ");
        //        writer.WriteEncodedText(displayUserName);
        //    }

        //    writer.Write("!");
        //}

    }


}

