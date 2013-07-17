
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Office.Server.UserProfiles;
using System.Text;
using Microsoft.SharePoint;
using System.Data;
using System.Web;

namespace Niem.MyNiem.Webparts.EventsWebpart
{
    public partial class EventsWebpartUserControl : UserControl
    {
        #region properties
        public string EventsContentType
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
        #endregion

        protected void DataPager_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            DataView dvResults = FilterSearchData(GetData());

            lvResources.DataSource = dvResults;
            lvResources.DataBind();
        }
        #region Page_Load
        /// <summary>
        /// Page load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            try
            {
                //GetCalendarData();
                DataView dvResults = GetData();
                lvResources.DataSource = dvResults;
                lvResources.DataBind();
                if (lvResources.Items.Count == 0)
                    ShowNoRecords();

                if (!Page.IsPostBack)
                {
                    //databind to drop down lists
                    //ddlAudience.DataSource = GetListData(SPContext.Current.Site.RootWeb, YourAudienceList);
                    //ddlAudience.DataBind();
                    //ddlCommunities.DataSource = GetListData(SPContext.Current.Site.RootWeb, EstablishedCommunitiesList);
                    //ddlCommunities.DataBind();

                    //grabs categories
                    SPContentType ctResources = SPContext.Current.Site.RootWeb.ContentTypes[EventsContentType];
                }
            }
            catch (Exception ex)
            {
                LogText(ex.Message);
            }
        }

        void LogText(string text)
        {
           // Response.Write(text + "<br/>");
        }
        #endregion

        void ShowNoRecords()
        {
            Controls.Add(new LiteralControl("No records found by latest query."));
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
                DataView dvResults = (DataView)eventsList.DataSource;

                //check freetext & check drop downs
                lvResources.DataSource = FilterSearchData(dvResults);
                lvResources.DataBind();
                if (lvResources.Items.Count == 0)
                    ShowNoRecords();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

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

            //communities
            //if (sbQuery.Length == 0 && ddlCommunities.SelectedItem.Text != "All")
            //{
            //    sbQuery.Append("Category_x0020_Domains='" + ddlCommunities.SelectedItem.Text + "'");
            //}
            //else if (sbQuery.Length != 0 && ddlCommunities.SelectedItem.Text != "All")
            //{
            //    sbQuery.Append(" AND Category_x0020_Domains='" + ddlCommunities.SelectedItem.Text + "'");
            //}

            ////audiences
            //if (sbQuery.Length == 0 && ddlAudience.SelectedItem.Text != "All")
            //{
            //    sbQuery.Append("Category_x0020_Subject_x0020_Are='" + ddlAudience.SelectedItem.Text + "'");
            //}
            //else if (sbQuery.Length != 0 && ddlAudience.SelectedItem.Text != "All")
            //{
            //    sbQuery.Append(" AND Category_x0020_Subject_x0020_Are='" + ddlAudience.SelectedItem.Text + "'");
            //}

            //freetext fields
            if (!string.IsNullOrEmpty(txtSearch.Text) && sbQuery.Length == 0)
            {
                sbQuery.Append("(Title LIKE '%" + txtSearch.Text + "%' OR Description LIKE '%" + txtSearch.Text + "%')");
            }
            else if (!string.IsNullOrEmpty(txtSearch.Text) && sbQuery.Length != 0)
            {
                sbQuery.Append(" AND (Title LIKE '%" + txtSearch.Text + "%' OR Description LIKE '%" + txtSearch.Text + "%')");
            }

            dvResults.RowFilter = sbQuery.ToString();

            return dvResults;
        }
        #endregion

        protected DataView GetData()
        {
            DataTable dtResults = null;
            using (SPWeb Web = SPContext.Current.Site.RootWeb)
            {
                SPSiteDataQuery Query = new SPSiteDataQuery();
                Query.Lists = "<Lists ServerTemplate=\"106\" />";
                //"<Lists BaseType=\"1\" />";
                Query.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"ID\" /><FieldRef Name=\"Location\" /><FieldRef Name=\"EventDate\" /><FieldRef Name=\"Description\" /><FieldRef Name=\"ContentTypeId\" />";
                //content type & property
                //Query.Query = "<Where><Eq><FieldRef Name='ContentType' /><Value Type='Computed'>" + ToolsContentType + "</Value></Eq></Where>";
                Query.Query = @"<Where>
                                    <Geq>
                                        <FieldRef Name='EventDate' /><Value IncludeTimeValue='False' Type='DateTime'>"+DateTime.Now.ToString("yyyy-MM-dd") + @" </Value>
                                     </Geq>
                                </Where>";

               // Query.Webs = "<Webs Scope=\"Recursive\" />";
                Query.Webs = "<Webs Scope=\"Recursive\" />";
                dtResults = Web.GetSiteData(Query);

                dtResults.Columns.Add("LinkField");
                //fixes the links and only uses 150 chars of the PublishingPageContentField
                foreach (DataRow Row in dtResults.Rows)
                {

                    SPSite Site = SPContext.Current.Site;
                    Guid WebGuid = new Guid(Row["WebId"].ToString());
                    using (SPWeb LinkWeb = Site.OpenWeb(WebGuid))
                    {
                        Guid ListGuid = new Guid(Row["ListId"].ToString());
                        SPList List = LinkWeb.Lists[ListGuid];
                        Row["LinkField"] = LinkWeb.Url + "/" + List.RootFolder.Url + "/DispForm.aspx?ID=" + Row["ID"].ToString() + "&ContentTypeID=" + Row["ContentTypeId"].ToString() + "&IsDlg=1";
                    }
                }
                
               // dtResults.TableName = "dtResults";
               //dtResults.WriteXml("C:\\temp\\events.xml");
                
            }
            
          DataView   dvResults = new DataView(dtResults);
          return FilterSearchData(dvResults);
        }

        #region GetData
        /// <summary>
        /// Gets data using SPSiteDataQuery.
        /// </summary>
        /// <returns></returns>
        protected DataView GetDataOld()
        {
            DataTable dtResults = null;
            DataView dvResults = null;
            using (SPWeb aboutNiemWeb = SPContext.Current.Site.OpenWeb("/aboutniem"))
            {
                SPSiteDataQuery Query = new SPSiteDataQuery();
                Query.Lists = "<Lists ServerTemplate=\"100\" />";
                //"<Lists BaseType=\"1\" />";
                Query.ViewFields = "<FieldRef Name='Title' /><FieldRef Name='ID' /><FieldRef Name='StartDate' /><FieldRef Name='Category_x0020_Domains' /><FieldRef Name='Category_x0020_Committee' /><FieldRef Name='Comments' /><FieldRef Name='EndDate' /><FieldRef Name='ContentTypeId' /><FieldRef Name='Category_x0020_Subject_x0020_Are' />";
                //content type & property
                Query.Query = "<Where><Eq><FieldRef Name='ContentType' /><Value Type='Computed'>" + EventsContentType + "</Value></Eq></Where>";
                Query.Webs = "<Webs Scope=\"Recursive\" />";
                dtResults = aboutNiemWeb.GetSiteData(Query);

                dtResults.Columns.Add("LinkField");
                dtResults.Columns.Add("EventDate");
                //fixes the links and only uses 150 chars of the PublishingPageContentField
                foreach (DataRow Row in dtResults.Rows)
                {
                    string[] CatDomains = Row["Category_x0020_Domains"].ToString().Split('#');
                    string[] CatAudiences = Row["Category_x0020_Subject_x0020_Are"].ToString().Split('#');
                    string[] CatCommittees = Row["Category_x0020_Committee"].ToString().Split('#');
                    Row["Category_x0020_Domains"] = CatDomains[1];
                    Row["Category_x0020_Subject_x0020_Are"] = CatAudiences[1];
                    Row["Category_x0020_Committee"] = CatCommittees[1];

                    DateTime dtStartDate = DateTime.Parse(Row["StartDate"].ToString());
                    DateTime dtEndDate = DateTime.Parse(Row["EndDate"].ToString());
                    string DateFormatted = dtStartDate.ToString("MMMM d, yyyy") + " until " + dtEndDate.ToString("MMMM d, yyyy");
                    //String.Format("{0:M d, yyyy}", Row["StartDate"].ToString()) + " until " + String.Format("{0:M d, yyyy}", Row["EndDate"].ToString());
                    Row["EventDate"] = DateFormatted;

                    Row["Comments"] = Row["Comments"].ToString().Substring(0, 150);

                    SPSite Site = SPContext.Current.Site;
                    Guid WebGuid = new Guid(Row["WebId"].ToString());
                    using (SPWeb LinkWeb = Site.OpenWeb(WebGuid))
                    {
                        Guid ListGuid = new Guid(Row["ListId"].ToString());
                        SPList List = LinkWeb.Lists[ListGuid];
                        Row["LinkField"] = LinkWeb.Url + "/" + List.RootFolder.Url + "/DispForm.aspx?ID=" + Row["ID"].ToString() + "&ContentTypeID=" + Row["ContentTypeId"].ToString() + "&IsDlg=1";
                    }
                }

                dvResults = new DataView(dtResults);

                /*
                ////gets profile data
                Microsoft.SharePoint.SPServiceContext serviceContext = Microsoft.SharePoint.SPServiceContext.Current;
                UserProfileManager upm = new Microsoft.Office.Server.UserProfiles.UserProfileManager(serviceContext);
                UserProfile CurrentUser = upm.GetUserProfile(true);

                ProfileValueCollectionBase pvbCommunities = CurrentUser.GetProfileValueCollection("EstablishedCommunities");
                ProfileValueCollectionBase pvbAudiences = CurrentUser.GetProfileValueCollection("YourAudience");

                //grabs established communities and your audiences for filtering
                StringBuilder sbQuery = new StringBuilder();
                foreach (var value in pvbAudiences)
                {
                    if (sbQuery.Length == 0)
                    {
                        sbQuery.Append("Category_x0020_Subject_x0020_Are='" + value.ToString() + "'");
                    }
                    else
                    {
                        sbQuery.Append(" OR Category_x0020_Subject_x0020_Are='" + value.ToString() + "'");
                    }
                }

                foreach (var value in pvbCommunities)
                {
                    if (sbQuery.Length == 0)
                    {
                        sbQuery.Append("Category_x0020_Domains='" + value.ToString() + "'");
                    }
                    else
                    {
                        sbQuery.Append(" OR Category_x0020_Domains='" + value.ToString() + "'");
                    }
                }

                dvResults.RowFilter = sbQuery.ToString();
                LogText("Returns fine");
                 * 
                 **/
            }
            return dvResults;
        }
        #endregion

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
    }
}
