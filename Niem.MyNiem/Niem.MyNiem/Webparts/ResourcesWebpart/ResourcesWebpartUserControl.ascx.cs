using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Portal.WebControls;
using System.Web;
using Microsoft.Office.Server.UserProfiles;
using System.Text;
using System.Collections.Generic;

namespace Niem.MyNiem.Webparts.ResourcesWebpart
{
    public partial class ResourcesWebpartUserControl : UserControl
    {
        #region properties
        public string ResourcesContentType
        {
            get;
            set;
        }
        public string YourAudienceList
        {
            get;
            set;
        }
        public string EstablishedCommunitiesList
        {
            get;
            set;
        }

        public Guid ResourceListId { get; set; }

        public Guid WebId { get; set; }

        #endregion

        private SPListItem usr;
        public SPListItem CurrentUser
        {
            get
            {
                try
                {
                    if (usr == null)
                    {
                        string currentSite = HttpContext.Current.Request.Url.AbsoluteUri;
                        string userID = SPContext.Current.Web.CurrentUser.ID.ToString();

                        using (SPWeb rootWeb = new SPSite(currentSite).RootWeb)
                        {
                            rootWeb.AllowUnsafeUpdates = true;
                            SPList profiles = rootWeb.Lists["Profiles"];
                            SPQuery query = new SPQuery();
                            query.Query = "<Where><Eq><FieldRef Name='SPUser' LookupId= 'TRUE'  />" +
                                            "<Value Type='User'>" + userID + "</Value>" + "</Eq></Where>";
                            SPListItemCollection items = profiles.GetItems(query);
                            SPFieldChoice orgTypes = (SPFieldChoice)profiles.Fields["OrgType"];


                            if (items.Count > 0)
                                usr = items[0];
                            else
                            {
                                // SPUser contextUser = SPContext.Current.Web.CurrentUser;
                                //Update Initial Data
                                //Microsoft.SharePoint.SPServiceContext serviceContext = Microsoft.SharePoint.SPServiceContext.Current;
                                //UserProfileManager upm = new Microsoft.Office.Server.UserProfiles.UserProfileManager(serviceContext);
                                ////ProfileSubtypePropertyManager pspm = upm.DefaultProfileSubtypeProperties;
                                //UserProfile usrProf = upm.GetUserProfile(true);
                                SPUser contextUser = SPContext.Current.Web.CurrentUser;

                                string[] nameparts = contextUser.Name.Split('|');

                                string[] splitName = nameparts[nameparts.Length - 1].Split(' ');
                                SPListItem newItem = profiles.AddItem();
                                if (splitName.Length > 1)
                                    newItem["Title"] = splitName[splitName.Length - 1];
                                else
                                    newItem["Title"] = string.Empty;
                                newItem["FirstName"] = splitName[0];
                                newItem["Email"] = contextUser.Email;
                                newItem["Company"] = "";
                                newItem["OrgType"] = orgTypes.DefaultValue;
                                SPFieldUserValueCollection userValue = new SPFieldUserValueCollection();
                                userValue.Add(new SPFieldUserValue(SPContext.Current.Web, contextUser.ID, contextUser.Name));
                                newItem["SPUser"] = userValue;
                                newItem.Update();
                                profiles.Update();
                                usr = newItem;
                                //ddlOrgType.SelectedValue = ""+CurrentUser["OrganizationType"].Value;
                                //txtPosition.Text = ""+CurrentUser["Position"].Value;
                                rootWeb.AllowUnsafeUpdates = false;
                            }

                        }

                        //Microsoft.SharePoint.SPServiceContext serviceContext = Microsoft.SharePoint.SPServiceContext.Current;
                        //UserProfileManager upm = new Microsoft.Office.Server.UserProfiles.UserProfileManager(serviceContext);
                        ////ProfileSubtypePropertyManager pspm = upm.DefaultProfileSubtypeProperties;
                        //usr = upm.GetUserProfile(true);
                    }
                }
                catch (Exception ex)
                {
                    //LogText(ex.Message);
                }

                return usr;



            }
            set { usr = value; }
        }
        private DataView _data;
        #region Page_Load
        /// <summary>
        /// Page_Load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //resourcesList.ItemDataBound += new RepeaterItemEventHandler(resourcesList_ItemDataBound);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            try
            {

                if (!Page.IsPostBack)
                {
                    //databind to drop down lists
                    ddlAudience.DataSource = GetListData(SPContext.Current.Site.RootWeb, YourAudienceList);
                    ddlAudience.DataBind();
                    ddlCommunities.DataSource = GetListData(SPContext.Current.Site.RootWeb, EstablishedCommunitiesList);
                    ddlCommunities.DataBind();

                    //grabs categories
                    SPContentType ctResources = SPContext.Current.Site.RootWeb.ContentTypes[ResourcesContentType];
                    SPFieldChoice Field = (SPFieldChoice)ctResources.Fields["Resource Category"];
                    ddlCategory.Items.Add("All");
                    foreach (string s in Field.Choices)
                    {
                        ListItem Item = new ListItem(s, s);
                        ddlCategory.Items.Add(Item);
                    }
                    lvResources.DataSource = GetData();
                    lvResources.DataBind();
                    if (lvResources.Items.Count == 0)
                    {
                        ShowNoRecords();
                    }
                }
               

            }
            catch (Exception ex)
            {
                Controls.Add(new LiteralControl(ex.Message));
            }
        }
        #endregion

        void ShowNoRecords()
        {
            Controls.Add(new LiteralControl("Go to <a href=\"/documentsdb/Pages/documents.aspx\">Resource Database</a> and \"like\" your favorite items."));
        }
        #region Button Click
        /// <summary>
        /// Filter even to search data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                lvResources.DataSource = GetData();
                lvResources.DataBind();
                if (lvResources.Items.Count == 0)
                    ShowNoRecords();

            }
            catch (Exception ex)
            {
                Controls.Add(new LiteralControl(ex.Message));
            }
        }
        #endregion

        protected void DataPager_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
       
            DataView dvResults = GetData();

            lvResources.DataSource = dvResults;
            lvResources.DataBind();
        }
        protected void DataPager_PreRender(object sender, EventArgs e)
        {
            //DataView dvResults = GetData();

            //lvResources.DataSource = dvResults;
            //lvResources.DataBind();
        }

        #region FilterSearchData
        /// <summary>
        /// Filters search results.
        /// </summary>
        /// <param name="dvResults"></param>
        /// <returns></returns>
        protected DataView FilterSearchData(DataView dvResults)
        {
            //grabs established communities and your audiences for filtering
            StringBuilder sbQuery = new StringBuilder();
            LogText("1");
            //communities
            if (sbQuery.Length == 0 && ddlCommunities.SelectedItem.Text != "All")
            {
                sbQuery.Append("Category_x0020_Domains='" + ddlCommunities.SelectedItem.Text + "'");
            }
            else if (sbQuery.Length != 0 && ddlCommunities.SelectedItem.Text != "All")
            {
                sbQuery.Append(" AND Category_x0020_Domains='" + ddlCommunities.SelectedItem.Text + "'");
            }
            LogText("2");
            //audiences
            if (sbQuery.Length == 0 && ddlAudience.SelectedItem.Text != "All")
            {
                sbQuery.Append("Category_x0020_Subject_x0020_Area_x002F_Audience='" + ddlAudience.SelectedItem.Text + "'");
            }
            else if (sbQuery.Length != 0 && ddlAudience.SelectedItem.Text != "All")
            {
                sbQuery.Append(" AND Category_x0020_Subject_x0020_Area_x002F_Audience='" + ddlAudience.SelectedItem.Text + "'");
            }
            LogText("3");
            //categories
            if (sbQuery.Length == 0 && ddlCategory.SelectedItem.Text != "All")
            {
                sbQuery.Append("FileDirRef='" + ddlCategory.SelectedItem.Text + "'");
            }
            else if (sbQuery.Length != 0 && ddlCategory.SelectedItem.Text != "All")
            {
                sbQuery.Append(" AND FileDirRef='" + ddlCategory.SelectedItem.Text + "'");
            }
            LogText("4");
            //freetext fields
            if (!string.IsNullOrEmpty(txtSearch.Text) && sbQuery.Length == 0)
            {
                sbQuery.Append("(Title LIKE '%" + txtSearch.Text + "%' OR _Comments LIKE '%" + txtSearch.Text + "%')");
            }
            else if (!string.IsNullOrEmpty(txtSearch.Text) && sbQuery.Length != 0)
            {
                sbQuery.Append(" AND (Title LIKE '%" + txtSearch.Text + "%' OR _Comments LIKE '%" + txtSearch.Text + "%')");
            }
            LogText("5");
            dvResults.RowFilter = sbQuery.ToString();
            LogText("6");
            return dvResults;
        }
        
        void LogText(string message)
        {
           //System.IO.File.AppendAllText("C:\\temp\\Resourceslog.txt", message + "\r\n");
        }
        #endregion

        #region ItemDataBound

        protected void lvResources_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem)
            {
                return;
            }
            ListViewDataItem item = e.Item as ListViewDataItem;

            PlaceHolder ratingsPlaceHolder = ((PlaceHolder)e.Item.FindControl("ratingsPlaceHolder"));
            DataRowView drv = (DataRowView)item.DataItem;
            int ID = Convert.ToInt32(drv["ID"].ToString());
            Guid ListGuid = ResourceListId;
            Guid WebGuid = WebId;
            SPSite Site = SPContext.Current.Site;
            using (SPWeb Web = SPContext.Current.Site.OpenWeb("documentsdb"))
            {
                //  SPWeb Web = Site.OpenWeb(WebGuid);
                SPList List = Web.Lists[ListGuid];
                SPField Field = List.Fields.TryGetFieldByStaticName("AverageRating");


                if (Field != null)
                {
                    AverageRatingFieldControl avgRatings = new AverageRatingFieldControl();
                    avgRatings.ItemContext = SPContext.GetContext(HttpContext.Current, ID, ListGuid, Web);
                    avgRatings.ListId = ListGuid;
                    avgRatings.ItemId = ID;
                    avgRatings.ControlMode = SPControlMode.Display;
                    avgRatings.FieldName = "AverageRating";
                    ratingsPlaceHolder.Controls.Add(avgRatings);
                }
            }
        }

        /// <summary>
        /// Item Created event to add ratings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void resourcesList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            PlaceHolder ratingsPlaceHolder = ((PlaceHolder)e.Item.FindControl("ratingsPlaceHolder"));
            DataRowView drv = (DataRowView)e.Item.DataItem;
            int ID = Convert.ToInt32(drv["ID"].ToString());
            Guid ListGuid = new Guid(drv["ListId"].ToString());
            Guid WebGuid = new Guid(drv["WebId"].ToString());
            SPSite Site = SPContext.Current.Site;
            SPWeb Web = Site.OpenWeb(WebGuid);
            SPList List = Web.Lists[ListGuid];
            SPField Field = List.Fields.TryGetFieldByStaticName("AverageRating");
            if (Field != null)
            {
                AverageRatingFieldControl avgRatings = new AverageRatingFieldControl();
                avgRatings.ItemContext = SPContext.GetContext(HttpContext.Current, ID, ListGuid, Web);
                avgRatings.ListId = ListGuid;
                avgRatings.ItemId = ID;
                avgRatings.ControlMode = SPControlMode.Display;
                avgRatings.FieldName = "AverageRating";
                ratingsPlaceHolder.Controls.Add(avgRatings);
            }

        }
        #endregion

        
        #region GetData (Original)
        /// <summary>
        /// Gets data using SPSiteDataQuery.
        /// </summary>
        /// <returns></returns>
        protected DataView GetData()
        {
            DataTable dtResults = null;
            using (SPWeb Web = SPContext.Current.Site.OpenWeb("documentsdb"))
            {
                SPList documents = Web.GetList("/documentsdb/documents");
                SPQuery query = new SPQuery();
                query.Query = "<Where><Neq><FieldRef Name='ContentType' /><Value Type='Computed'>Folder</Value></Neq></Where>";
                query.ViewFields = @"<FieldRef Name='Title' /><FieldRef Name='ID' /><FieldRef Name='_Comments' /><FieldRef Name='FileRef' /><FieldRef Name='Category_x0020_Domains' Nullable='TRUE' /><FieldRef Name='Category_x0020_Subject_x0020_Area_x002F_Audience' Nullable='TRUE'/><FieldRef Name='Flipbook_x0020_Url0' Nullable='TRUE'/><FieldRef Name='Pdf_x0020_Url' Nullable='TRUE'/><FieldRef Name='FileDirRef' Nullable='TRUE'/><FieldRef Name='FileLeafRef' Nullable='TRUE'/><FieldRef Name='PublishingRollupImage' Nullable='TRUE'/><FieldRef Name='AverageRating' Nullable='TRUE'/><FieldRef Name='Created' />";
                query.ViewAttributes = "Scope='RecursiveAll'";
                SPListItemCollection docs = documents.GetItems(query);
                dtResults = docs.GetDataTable();

                WebId = Web.ID; // Set this for use with ratings
                ResourceListId = documents.ID; // Set this for use with ratings
                                
                foreach (DataRow Row in dtResults.Rows)
                {
                    //fixes issue with link
                    string DirectoryLinkFix = Row["FileDirRef"].ToString();
                    if (DirectoryLinkFix.LastIndexOf('/') > 0)
                    {
                        DirectoryLinkFix = DirectoryLinkFix.Substring(DirectoryLinkFix.LastIndexOf('/') + 1);
                    }
                    Row["FileDirRef"] = DirectoryLinkFix;
                }
            }
         
            DataView dvResults = new DataView(dtResults);
            DataView dView = FilterDataAccordingToUsers(dvResults);
            
            return dView;// GetLikedResources(dTable);
        }
        #endregion

        DataView GetLikedResources(DataView  dTable)
        {
            if (SPContext.Current.Web.CurrentUser == null || string.IsNullOrEmpty(SPContext.Current.Web.CurrentUser.LoginName))
            {

               
            }
            else
            {
                try
                {
                    if(ddlAudience.SelectedItem.Text == "All" || ddlCommunities.SelectedItem.Text  == "All")
                    {
                    string siteURL = HttpContext.Current.Request.Url.AbsoluteUri;
                    using (SPSite site = new SPSite(siteURL))
                    {
                        using (SPWeb web = site.RootWeb)
                        {
                            SPList list = web.Lists["LikeStatus"];
                            SPQuery query = new SPQuery();

                            query.Query = "<Where><And>" +
                                                "<Eq><FieldRef Name='CType'/><Value Type='Text'>Resource</Value></Eq>" +
                                                "<Eq><FieldRef Name='SPUser'/><Value Type='Text'>" + SPContext.Current.Web.CurrentUser.LoginName + "</Value></Eq>" +
                                           "</And></Where>";


                            SPListItemCollection items = list.GetItems(query);
                            foreach (SPListItem item in items)
                            {
                                try
                                {
                                    using (SPWeb itemWeb = site.OpenWeb(new Guid(item["WebID"].ToString())))
                                    {
                                        SPList itemList = itemWeb.Lists[new Guid(item["ListID"].ToString())];
                                        SPItem resourceItem = itemList.GetItemById(int.Parse(item["ItemID"].ToString()));

                                        DataRow[] foundRows;


                                        foundRows = dTable.Table.Select("FileRef = '"+ resourceItem["FileRef"].ToString()+"'");
                                        if (foundRows.Length == 0)
                                        {

                                            DataRow dRow = dTable.Table.NewRow();
                                            try { dRow["Title"] = resourceItem["Title"].ToString(); }
                                            catch (Exception) { }
                                            try { dRow["ID"] = resourceItem["ID"].ToString(); }
                                            catch (Exception) { }
                                            try { dRow["_Comments"] = resourceItem["_Comments"].ToString(); }
                                            catch (Exception) { }
                                            try { dRow["FileRef"] = resourceItem["FileRef"].ToString(); }
                                            catch (Exception) { }
                                            try
                                            {
                                                string[] CatDomains = resourceItem["Category_x0020_Domains"].ToString().Split('#');
                                                if (CatDomains.Length > 1)
                                                {
                                                   // dRow["Category_x0020_Domains"] = CatDomains[1];
                                                }

                                            }
                                            catch (Exception) { }
                                            try
                                            {
                                                string[] CatAudiences = resourceItem["Category_x0020_Subject_x0020_Area_x002F_Audience"].ToString().Split('#');
                                                if (CatAudiences.Length > 1)
                                                {
                                                  //  dRow["Category_x0020_Subject_x0020_Area_x002F_Audience"] = CatAudiences[1];
                                                }

                                            }
                                            catch (Exception) { }
                                            try { dRow["Flipbook_x0020_Url0"] = resourceItem["Flipbook_x0020_Url0"].ToString(); }
                                            catch (Exception) { }
                                            try { dRow["Pdf_x0020_Url"] = resourceItem["Pdf_x0020_Url"].ToString(); }
                                            catch (Exception) { }
                                            try
                                            {
                                                string DirectoryLinkFix = resourceItem["FileDirRef"].ToString();
                                                if (DirectoryLinkFix.LastIndexOf('/') > 0)
                                                    DirectoryLinkFix = DirectoryLinkFix.Substring(DirectoryLinkFix.LastIndexOf('/') + 1);
                                                dRow["FileDirRef"] = DirectoryLinkFix;

                                            }
                                            catch (Exception) { }
                                            try { dRow["FileLeafRef"] = resourceItem["FileLeafRef"].ToString(); }
                                            catch (Exception) { }
                                            try { dRow["PublishingRollupImage"] = resourceItem["PublishingRollupImage"].ToString(); }
                                            catch (Exception) { }
                                            try { dRow["AverageRating"] = resourceItem["AverageRating"].ToString(); }
                                            catch (Exception) { }
                                            try { dRow["Created"] = resourceItem["Created"].ToString(); }
                                            catch (Exception) { }
                                            if (ddlCategory.SelectedItem.Text == "All" || ddlCategory.SelectedItem.Text == dRow["FileDirRef"].ToString())
                                            {
                                                if(string.IsNullOrEmpty(txtSearch.Text.Trim()) || dTable.Table.Select("Title LIKE '%" + txtSearch.Text + "%'").Length > 0)
                                                    dTable.Table.Rows.Add(dRow);
                                            }
                                        }
                                        
                                    }
                                }

                                catch (Exception)
                                { }

                            }
                            LogText("Liked News:" + items.Count.ToString());
                            LogText("Updated List:" + dTable.Table.Rows.Count.ToString());

                        }
                    }
                    }
                }
                catch (Exception ex)
                {
                    LogText(ex.Message);
                }
            }
            dTable.Sort = "Created desc";
            dTable.Table.Columns.Add("DateDiff",typeof(int));
            foreach (DataRow dRow in dTable.Table.Rows)
            {
                DateTime itemTime = DateTime.Parse(dRow["Created"].ToString());
                try
                {
                    dRow["DateDiff"] = DateTime.Now.Subtract(itemTime).Days;
                }
                catch (Exception)
                {
                    dRow["DateDiff"] = "999";
                }
            }
            return dTable;//.DefaultView.ToTable(true,{""}));
        }

        private DataView FilterDataAccordingToUsers(DataView dvResults)
        {
            
            SPFieldLookupValue group;
            string lookedUpItemTitle = string.Empty;
            string[] pvbCommunities = new string[] { "" };
            string[] pvbAudiences = new string[] { "" };
            try
            {
                group = new SPFieldLookupValue(CurrentUser["EstablishedCommunities_x003a_Tit"].ToString());
                lookedUpItemTitle = group.LookupValue;
                //Response.Write(lookedUpItemTitle);
                pvbCommunities = lookedUpItemTitle.Split(';');
            }
            catch (Exception) { }
            try
            {
                group = new SPFieldLookupValue(CurrentUser["YourAudienceList_x003a_Title"].ToString());
                lookedUpItemTitle = group.LookupValue;
                pvbAudiences = lookedUpItemTitle.Split(';');
            }
            catch (Exception) { }

            StringBuilder sbQuery = new StringBuilder();
            string orString = string.Empty;
            if (ddlAudience.SelectedItem.Text != "All")
            {
                sbQuery.Append("Category_x0020_Subject_x0020_Area_x002F_Audience like'%" + ddlAudience.SelectedItem.Text + "%'");
            }
            else
            {
                
                foreach (string value in pvbAudiences)
                {

                    if (String.IsNullOrEmpty(value))
                    {
                        continue;
                    }
                    string newValue = value.Replace("#", string.Empty);
                    //test ids. the look up values sometimes have their numeric id associated
                    //we don't want to add those values to our query.
                    if (System.Text.RegularExpressions.Regex.IsMatch(newValue, @"^\d+$"))
                    {
                        continue;
                    }
                    
                    orString = (sbQuery.Length > 0 ? " OR " : string.Empty);
                    sbQuery.Append(orString + "Category_x0020_Subject_x0020_Area_x002F_Audience like'%" + newValue + "%'");
                }
            }


            if (ddlCommunities.SelectedItem.Text != "All")
            {
                orString = (sbQuery.Length > 0 ? " OR " : string.Empty);
                sbQuery.Append(orString + "Category_x0020_Domains like'%" + ddlCommunities.SelectedItem.Text + "%'");
            }
            else
            {

                foreach (string value in pvbCommunities)
                {
                    if (String.IsNullOrEmpty(value))
                    {
                        continue;
                    }

                    string newValue = value.Replace("#", string.Empty);
                    //test ids. the look up values sometimes have their numeric id associated
                    //we don't want to add those values to our query.
                    if (System.Text.RegularExpressions.Regex.IsMatch(newValue, @"^\d+$"))
                    {
                        continue;
                    }

                    orString = (sbQuery.Length > 0 ? " OR " : string.Empty);
                    sbQuery.Append(orString + "Category_x0020_Domains like'%" + newValue + "%'");
                }
            }

            

            //grabs established communities and your audiences for filtering
            #region old audience filtering
//            foreach (string value in pvbAudiences)
//                {
//                    if (!string.IsNullOrEmpty(value.Trim()))
//                    {

//                        if (sbQuery.Length == 0)
//                        {
//                            if (ddlCommunities.SelectedItem.Text != "All")
//                            {
//                                //sbQuery.Append("Category_x0020_Domains='" + ddlCommunities.SelectedItem.Text + "'");
//                                sbQuery.Append("Category_x0020_Domains like'%" + ddlCommunities.SelectedItem.Text + "%'");
//                            }
//                            else
//                            {

////                                sbQuery.Append("Category_x0020_Domains='" + value.Replace('#', ' ').Trim() + "'");
//                                sbQuery.Append(" OR Category_x0020_Subject_x0020_Area_x002F_Audience='" + value.Replace('#', ' ').Trim() + "'");

//                            }
//                        }
//                        else
//                        {
//                            if (ddlCommunities.SelectedItem.Text != "All")
//                            {
//                                sbQuery.Append(" AND Category_x0020_Domains like '%" + ddlCommunities.SelectedItem.Text + "%'");
//                            }
//                            else
//                            {

//                                sbQuery.Append(" OR Category_x0020_Domains='" + value.Replace('#', ' ').Trim() + "'");
//                                sbQuery.Append(" OR Category_x0020_Subject_x0020_Area_x002F_Audience='" + value.Replace('#', ' ').Trim() + "'");

//                            }
//                        }
//                    }

//                }

            #endregion

            #region old communities filtering
            //foreach (var value in pvbCommunities)
            //    {
            //        if (!string.IsNullOrEmpty(value.Trim()))
            //        {
            //            if (sbQuery.Length == 0)
            //            {
            //                if (ddlAudience.SelectedItem.Text != "All")
            //                {
            //                    sbQuery.Append("Category_x0020_Subject_x0020_Area_x002F_Audience='" + ddlAudience.SelectedItem.Text + "'");
            //                }
            //                else
            //                {

            //                    sbQuery.Append("Category_x0020_Subject_x0020_Area_x002F_Audience='" + value.Replace('#', ' ').Trim() + "'");
            //                    sbQuery.Append(" OR Category_x0020_Domains='" + value.Replace('#', ' ').Trim() + "'");

            //                }
            //            }
            //            else
            //            {
            //                if (ddlAudience.SelectedItem.Text != "All")
            //                {
            //                    sbQuery.Append(" AND Category_x0020_Subject_x0020_Area_x002F_Audience='" + ddlAudience.SelectedItem.Text + "'");
            //                }
            //                else
            //                {

            //                    sbQuery.Append(" OR Category_x0020_Subject_x0020_Area_x002F_Audience='" + value.Replace('#', ' ').Trim() + "'");
            //                    sbQuery.Append(" OR Category_x0020_Domains='" + value.Replace('#', ' ').Trim() + "'");

            //                }
            //            }
            //        }

            //    }
            #endregion

            //categories
            if (sbQuery.Length == 0 && ddlCategory.SelectedItem.Text != "All")
            {
                sbQuery.Append(" FileDirRef='" + ddlCategory.SelectedItem.Text + "'");
            }
            else if (sbQuery.Length != 0 && ddlCategory.SelectedItem.Text != "All")
            {
                sbQuery.Append(" AND FileDirRef='" + ddlCategory.SelectedItem.Text + "'");
            }

            //freetext fields
            if (!string.IsNullOrEmpty(txtSearch.Text) && sbQuery.Length == 0)
            {
                sbQuery.Append(" (Title LIKE '%" + txtSearch.Text + "%' OR _Comments LIKE '%" + txtSearch.Text + "%')");
            }
            else if (!string.IsNullOrEmpty(txtSearch.Text) && sbQuery.Length != 0)
            {
                sbQuery.Append(" AND (Title LIKE '%" + txtSearch.Text + "%' OR _Comments LIKE '%" + txtSearch.Text + "%')");
            }

            LogText("Generated Query: " +sbQuery.ToString());
            if (sbQuery.Length == 0)
            {
                dvResults.Table.Rows.Clear();
                sbQuery.Length = 0;
                try
                {
                    dvResults = GetLikedResources(dvResults);
                }
                catch (Exception) { }
            }
            else
            {
                if (ddlAudience.SelectedItem.Text == "All" && ddlCommunities.SelectedItem.Text=="All")
                {
                    try
                    {

                        dvResults.RowFilter = sbQuery.ToString();
                        dvResults = GetLikedResources(new DataView(dvResults.ToTable()));
                        sbQuery.Length = 0;
                        
                    }
                    catch (Exception) { }
                }
            }
            

            //System.IO.File.WriteAllText("C:\\temp\\log.txt", sbQuery.ToString());
            LogText(sbQuery.ToString());
            dvResults.RowFilter = sbQuery.ToString();
            
            return dvResults;
        }
     
        #region GetListData
        /// <summary>
        /// Gets the list data.
        /// </summary>
        protected ListItemCollection GetListData(SPWeb Web, string Listname)
        {
            SPFieldLookupValue group;
            string lookedUpItemTitle = string.Empty;
            ListItemCollection ListItems = new ListItemCollection();
            ListItem AllItem = new ListItem("All");
            ListItems.Add(AllItem);

            string[] pvbCommunities = new string[] { "" };
            try
            {
                if (Listname == EstablishedCommunitiesList)
                {
                    group = new SPFieldLookupValue(CurrentUser["EstablishedCommunities_x003a_Tit"].ToString());
                }
                else
                    group = new SPFieldLookupValue(CurrentUser["YourAudienceList_x003a_Title"].ToString());

                lookedUpItemTitle = group.LookupValue;

                pvbCommunities = lookedUpItemTitle.Split(';');
                foreach (string itemstring in pvbCommunities)
                {
                    string value = itemstring.Replace('#', ' ').Trim();
                    try
                    {
                        int gotID = int.Parse(value);
                    }
                    catch (Exception)
                    {
                        ListItem cbItem = new ListItem(value, value);
                        ListItems.Add(cbItem);
                    }
                }
            }
            catch (Exception) { }
            //ListItemCollection ListItems = new ListItemCollection();
            //SPList List = Web.Lists[Listname];
            //SPQuery Query = new SPQuery();
            //Query.Query = "<OrderBy><FieldRef Name='Title' /></OrderBy>";
            //SPListItemCollection Items = List.GetItems(Query);
            //ListItem AllItem = new ListItem("All");
            //ListItems.Add(AllItem);
            //foreach (SPListItem Item in Items)
            //{
            //    ListItem cbItem = new ListItem(Item["Title"].ToString(), Item["Title"].ToString());
            //    ListItems.Add(cbItem);
            //}
            return ListItems;
        }
        #endregion

        public string getRating(double rating)
        {
            string html = string.Empty;

            if (rating != 0 && !Double.IsNaN(rating))
            {
                html = "<div style='width:80px;background-image:url(/_layouts/Images/Ratings.png);background-position:" + ((Math.Floor(rating) * 16) - 240) + "px 0px;height:16px;'></div>";

            }
            if (rating == 0 || Double.IsNaN(rating))
            {
                html = "<div style='width:80px;background-image:url(/_layouts/Images/Ratings.png);background-position:-80px 0px;height:16px'></div>";

            }
            if (Math.Floor(rating) != Math.Ceiling(rating) && rating != 0 && Double.IsNaN(rating))
            {
                html = "<div style='width:80px;background-image:url(/_layouts/Images/Ratings.png);background-position:" + (-(Math.Floor(rating) * 16) - 304) + "px 0px;height:16px'></div>";

            }
            return html;
        }
        public string pdfLink(string longLink)
        {
            string trueLink = "";
            string[] allChars = longLink.Split(',');
            if (allChars.Length > 1)
            {
                
                    trueLink += allChars[1];
                
            }
           
                return trueLink;
          
        }
    }
}
