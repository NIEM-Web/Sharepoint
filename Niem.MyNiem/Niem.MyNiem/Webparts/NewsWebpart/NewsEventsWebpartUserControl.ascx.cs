using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using Microsoft.Office.Server.UserProfiles;
using System.Text;
using Microsoft.SharePoint;
using System.Data;
using System.Collections.Generic;

namespace Niem.MyNiem.Webparts.NewsEventsWebpart
{
    public partial class NewsEventsWebpartUserControl : UserControl
    {
        #region properties
        public string NewsContentType
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
                            {
                                usr = items[0];
                            }
                            else
                            {
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
                                rootWeb.AllowUnsafeUpdates = false;
                            }

                        }

                       
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

        DataView GetLikedNews(DataView dTable)
        {
            if (SPContext.Current.Web.CurrentUser == null || string.IsNullOrEmpty(SPContext.Current.Web.CurrentUser.LoginName))
            {


            }
            else
            {
                try
                {
                    if (ddlAudience.SelectedItem.Text == "All" || ddlCommunities.SelectedItem.Text == "All")
                    {
                        string siteURL = HttpContext.Current.Request.Url.AbsoluteUri;
                        using (SPSite site = new SPSite(siteURL))
                        {
                            using (SPWeb web = site.RootWeb)
                            {
                                SPList list = web.Lists["LikeStatus"];
                                SPQuery query = new SPQuery();

                                query.Query = "<Where><And>" +
                                                    "<Eq><FieldRef Name='CType'/><Value Type='Text'>News</Value></Eq>" +
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


                                            foundRows = dTable.Table.Select("FileRef = '" + resourceItem["FileRef"].ToString() + "'");
                                           
                                            if (foundRows.Length == 0)
                                            {

                                                DataRow dRow = dTable.Table.NewRow();
                                                try { dRow["Title"] = resourceItem["Title"].ToString(); }
                                                catch (Exception) { }
                                                try { dRow["ID"] = resourceItem["ID"].ToString(); }
                                                catch (Exception) { }
                                                try { dRow["ArticleStartDate"] = resourceItem["ArticleStartDate"].ToString(); }
                                                catch (Exception) { }
                                                try { dRow["ArticleStartDate"] = resourceItem["ArticleStartDate"].ToString(); }
                                                catch (Exception) { }
                                                try { dRow["Category_x0020_Committee"] = resourceItem["Category_x0020_Committee"].ToString(); }
                                                catch (Exception) { }
                                                try { dRow["PublishingPageContent"] = dRow["PublishingPageContent"].ToString().Substring(0, 150); }
                                                catch (Exception) { }
                                                try { dRow["FileRef"] = resourceItem["FileRef"].ToString(); }
                                                catch (Exception) { }
                                                try { dRow["ContentTypeId"] = resourceItem["ContentTypeId"].ToString(); }
                                                catch (Exception) { }
                                                try
                                                {
                                                    string[] CatDomains = resourceItem["Category_x0020_Domains"].ToString().Split('#');
                                                    if (CatDomains.Length > 1)
                                                    {
                                                        dRow["Category_x0020_Domains"] = CatDomains[1];
                                                    }

                                                }
                                                catch (Exception) { }
                                                try
                                                {
                                                    string[] CatAudiences = resourceItem["Category_x0020_Subject_x0020_Area_x002F_Audience"].ToString().Split('#');
                                                    if (CatAudiences.Length > 1)
                                                    {
                                                        dRow["Category_x0020_Subject_x0020_Area_x002F_Audience"] = CatAudiences[1];
                                                    }

                                                }
                                                catch (Exception) { }

                                                if (ddlCommunities.SelectedItem.Text == "All" || ddlAudience.SelectedItem.Text == "All")
                                                {
                                                    if (string.IsNullOrEmpty(txtSearch.Text.Trim()) || dTable.Table.Select("Title LIKE '%" + txtSearch.Text + "%'").Length > 0)
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
            return dTable;//.DefaultView.ToTable(true,{""}));
        }
        void ShowNoRecords()
        {
            Controls.Add(new LiteralControl("Go to <a href=\"/news/Pages/news.aspx\">News</a> and \"like\" your favorite items." ));
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


                if (!Page.IsPostBack)
                {
                    //databind to drop down lists
                    ddlAudience.DataSource = GetListData(SPContext.Current.Site.RootWeb, YourAudienceList);
                    ddlAudience.DataBind();
                    ddlCommunities.DataSource = GetListData(SPContext.Current.Site.RootWeb, EstablishedCommunitiesList);
                    ddlCommunities.DataBind();

                    //grabs categories
                    SPContentType ctResources = SPContext.Current.Site.RootWeb.ContentTypes[NewsContentType];
                }
                DataView dvResults = GetData();
                lvResources.DataSource = dvResults;
                lvResources.DataBind();

                if (lvResources.Items.Count == 0)
                    ShowNoRecords();
            }
            catch (Exception ex)
            {
                LogText("PageLoad:" + ex.Message);
            }
        }
        #endregion

        protected void DataPager_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            DataView dvResults = GetData();// FilterSearchData(GetData());

            lvResources.DataSource = dvResults;
            lvResources.DataBind();
        }

        #region Button Click
        /// <summary>
        /// Filter even to search data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    DataView dvResults = GetData();

            //    //check freetext & check drop downs
            //    lvResources.DataSource = dvResults;// FilterSearchData(dvResults);
            //    lvResources.DataBind();

            //    if (lvResources.Items.Count == 0)
            //        ShowNoRecords();
            //}
            //catch (Exception ex)
            //{
            //}
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
            return FilterDataAccordingToUsers(dvResults);
        }
        #endregion

        void LogText(string text)
        {
           // Response.Write(text + "<br/>");
        }

        //New version of get Data LCG
        protected DataView GetData()
        {   
            DataTable dtResults = null;
            DataView dvResults = null;
            //List<CamlQueryElements> query = new System.Collections.Generic.List<CamlQueryElements>();
           
            using (SPWeb Web = SPContext.Current.Site.OpenWeb("/news"))
            {
                SPQuery Query = new SPQuery();
                Query.ViewFields = "<FieldRef Name='Title' /><FieldRef Name='ID' /><FieldRef Name='ArticleStartDate' /><FieldRef Name='ArticleByLine' /><FieldRef Name='Category_x0020_Domains' /><FieldRef Name='Category_x0020_Committee' /><FieldRef Name='PublishingPageContent' /><FieldRef Name='FileRef' /><FieldRef Name='ContentTypeId' /><FieldRef Name='Category_x0020_Subject_x0020_Area_x002F_Audience' />";
               
                Query.Query = BuildQuery();
               dtResults = Web.Lists["Pages"].GetItems(Query).GetDataTable();
            }

            dvResults = dtResults.DefaultView;
            dvResults.Sort = "ArticleStartDate desc";
            dvResults.Table.Columns.Add("DateDiff", typeof(int));
            
            foreach (DataRow dRow in dvResults.Table.Rows)
            {
                try
                {
                    DateTime itemTime = DateTime.Parse(dRow["ArticleStartDate"].ToString());
                    dRow["DateDiff"] = DateTime.Now.Subtract(itemTime).Days;
                }
                catch (Exception)
                {
                    dRow["DateDiff"] = "999";
                }

                try
                {
                    dRow["PublishingPageContent"] = dRow["PublishingPageContent"].ToString().Substring(0, 150);
                }
                catch {}
            }


            return dvResults;
        }
        
        private const string CategoryDomains = "Category_x0020_Domains";
        private const string CategoryAudience = "Category_x0020_Subject_x0020_Area_x002F_Audience";
        private const string EstablishedCommunities = "EstablishedCommunities_x003a_Tit"; //Column used to fetch personal communities / domains.
        private const string EstablilshedAudiences = "YourAudienceList_x003a_Title"; //column used to fetch personal audiences

        private string BuildQuery()
        {

            Criteria contentTypeCriteria = Criteria.Eq("Content Type", "Computed", NewsContentType);
            List<Expression> criteria = new List<Expression>();

            if (ddlCommunities.SelectedItem.Text != "All")
            {
                criteria.Add(Criteria.Eq(CategoryDomains, "LookupMulti", ddlCommunities.SelectedItem.Text));
            }
            else
            {
                CreateCriteria(EstablishedCommunities, CategoryDomains, ref criteria);
            }
            if (ddlAudience.SelectedItem.Text != "All")
            {
                criteria.Add(Criteria.Eq(CategoryAudience, "LookupMulti", ddlAudience.SelectedItem.Text));
            }
            else
            {
                CreateCriteria(EstablilshedAudiences, CategoryAudience, ref criteria);
            }

            Or freeTextCriteria = null;
            //check free text
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                freeTextCriteria = Operator.Or(
                    Criteria.Contains("Title", "Text", txtSearch.Text),
                    Criteria.Contains("PublishingPageContent", "Note", txtSearch.Text)
                    );
            }

            Expression expression = null;

            switch (criteria.Count)
            {
                default:
                    Operator criteriaJoin = new Or();
                    criteriaJoin.AddRange((IEnumerable<Expression>)criteria);
                    expression = criteriaJoin;
                    break;
                case 0:
                    expression = contentTypeCriteria;//Operator.And(contentTypeCriteria);
                    break;
                case 1 :
                    expression = criteria[0];//Operator.And(contentTypeCriteria, criteria[0]);
                    break;
            }

            if (freeTextCriteria != null)
            {
                expression = expression * (freeTextCriteria);
            }
            string result = String.Format("<Where>{0}</Where>", expression.GetCAML());
            return(result);
        }

        private string BuildNewsQuery(List<CamlQueryElements> query)
        {
            string result = string.Empty;
            List<CamlQueryElements> newQuery = new List<CamlQueryElements>();

            if (ddlCommunities.SelectedItem.Text != "All")
            {
                query.Add(new CamlQueryElements
                {
                    ComparisonOperators = "Eq",
                    FieldName = "Category_x0020_Domains",
                    FieldValue = ddlCommunities.SelectedItem.Text,
                    FieldType = "LookupMulti",
                    LogicalJoin = "Or"
                });
            }
            else
            {
                CreateFilter("EstablishedCommunities_x003a_Tit", "Category_x0020_Domains", ref query);
            }

            if (ddlAudience.SelectedItem.Text != "All")
            {
                query.Add(new CamlQueryElements
                {
                    ComparisonOperators = "Eq",
                    FieldName = "Category_x0020_Subject_x0020_Area_x002F_Audience",
                    FieldValue = ddlAudience.SelectedItem.Text,
                    FieldType = "LookupMulti",
                    LogicalJoin = "Or"
                });
            }
            else
            {
                CreateFilter("YourAudienceList_x003a_Title", "Category_x0020_Subject_x0020_Area_x002F_Audience", ref query);
            }

            //add in free text search
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                query.Add(new CamlQueryElements
                {
                    FieldName = "PublishingPageContent",
                    FieldType = "Note",
                    FieldValue = txtSearch.Text,
                    ComparisonOperators = "Contains",
                    LogicalJoin = "And"
                });
            }

            query.Add(new CamlQueryElements
            {
                ComparisonOperators = "Eq",
                FieldName = "ContentType",
                FieldValue = "News Article",
                LogicalJoin = "And"
            });

            result = CamlQuery.GenerateQuery(query);
            return (result);
        }

        private void CreateCriteria(string userFilter, string fieldName, ref List<Expression> criteria)
        {
            //List<Expression> criteria = new List<Expression>();
            string[] filters = new string[] { "" };

            if (!string.IsNullOrEmpty(userFilter))
            {

                SPFieldLookupValue domains = new SPFieldLookupValue(CurrentUser[userFilter].ToString());
                string lookupValue = domains.LookupValue;
                if (string.IsNullOrEmpty(lookupValue))
                {
                    return;
                }

                filters = lookupValue.Replace("#", string.Empty).Split(';');

                foreach (string filter in filters)
                {
                    //test ids. the look up values sometimes have their numeric id associated
                    //we don't want to add those values to our query.
                    if (System.Text.RegularExpressions.Regex.IsMatch(filter, @"^\d+$"))
                    {
                        continue;
                    }

                    criteria.Add(Criteria.Eq(fieldName, "LookupMulti", filter));
                 
                }
            }
        }

        private void CreateFilter(string userFilter, string fieldName, ref List<CamlQueryElements> query)
        {
            string[] filters = new string[] { "" };
            
            if (!string.IsNullOrEmpty(userFilter))
            {
               
                SPFieldLookupValue domains = new SPFieldLookupValue(CurrentUser[userFilter].ToString());
                string lookupValue = domains.LookupValue;
                if (string.IsNullOrEmpty(lookupValue))
                {
                    return;
                }

                filters = lookupValue.Replace("#",string.Empty).Split(';');

                foreach(string filter in filters)
                {
                    //test ids. the look up values sometimes have their numeric id associated
                    //we don't want to add those values to our query.
                    if (System.Text.RegularExpressions.Regex.IsMatch(filter, @"^\d+$"))
                    {
                        continue;
                    }
                    query.Add(new CamlQueryElements
                    {
                        ComparisonOperators = "Eq",
                        FieldName = fieldName,
                        FieldValue = filter,
                        FieldType = "LookupMulti",
                        LogicalJoin = "Or"
                    });
                }
            }
        }


        #region GetData (original)
        /// <summary>
        /// Gets data using SPSiteDataQuery.
        /// </summary>
        /// <returns></returns>
        protected DataView GetDataOriginal()
        {
            DataTable dtResults = null;
            DataView dvResults = null;
       
            using (SPWeb Web = SPContext.Current.Site.OpenWeb("/news"))
            {
                //SPSiteDataQuery Query = new SPSiteDataQuery();
                SPQuery Query = new SPQuery();
                //Query.Lists = "<Lists BaseType='1' />";
                //"<Lists BaseType=\"1\" />";
                Query.ViewFields = "<FieldRef Name='Title' /><FieldRef Name='ID' /><FieldRef Name='ArticleStartDate' /><FieldRef Name='ArticleByLine' /><FieldRef Name='Category_x0020_Domains' /><FieldRef Name='Category_x0020_Committee' /><FieldRef Name='PublishingPageContent' /><FieldRef Name='FileRef' /><FieldRef Name='ContentTypeId' /><FieldRef Name='Category_x0020_Subject_x0020_Area_x002F_Audience' />";
                //content type & property
                Query.Query = "<Where><And><Eq><FieldRef Name='ContentType' /><Value Type='Computed'>" + NewsContentType + "</Value></Eq><Leq><FieldRef Name='Created' /><Value Type='DateTime'><Today Offset='-7' /></Value></Leq></And></Where>";
                //Query.Query = "<Where><Eq><FieldRef Name='ContentType' /><Value Type='Computed'>" + NewsContentType + "</Value></Eq></Where>";
                //Query.Webs = "<Webs Scope=\"SiteCollection\" />";
                dtResults = Web.Lists["Pages"].GetItems(Query).GetDataTable();// Web.GetSiteData(Query);
                //dtResults.WriteXml("C:\\log.txt",true);
                // LogText("dtResults:" + dtResults.Rows.Count.ToString());
                // foreach (DataColumn dc in dtResults.Columns)
                //   LogText(dc.ColumnName);
                //dtResults.Columns.Add("LinkField");
                //fixes the links and only uses 150 chars of the PublishingPageContentField


                foreach (DataRow Row in dtResults.Rows)
                {
                    try
                    {
                        try
                        {
                            string[] CatDomains = Row["Category_x0020_Domains"].ToString().Split('#');
                            Row["Category_x0020_Domains"] = CatDomains[1];
                        }
                        catch (Exception) { }
                        try
                        {
                            string[] CatAudiences = Row["Category_x0020_Subject_x0020_Area_x002F_Audience"].ToString().Split('#');
                            Row["Category_x0020_Subject_x0020_Area_x002F_Audience"] = CatAudiences[1];
                        }
                        catch (Exception) { }
                        try
                        {
                            string[] CatCommittees = Row["Category_x0020_Committee"].ToString().Split('#');
                            Row["Category_x0020_Committee"] = CatCommittees[1];
                        }
                        catch (Exception) { }



                        try
                        {
                            string DateFormatted = String.Format("{0:MM,dd yyyy}", Row["ArticleStartDate"].ToString());
                            Row["ArticleStartDate"] = DateFormatted;
                        }
                        catch (Exception) { }
                        try
                        {
                            Row["PublishingPageContent"] = Row["PublishingPageContent"].ToString().Substring(0, 150);
                        }
                        catch (Exception) { }

                        try
                        {
                            //  Row["LinkField"] = LinkWeb.Url + "/" + List.RootFolder.Url + "/Forms/DispForm.aspx?ID=" + Row["ID"].ToString() + "&ContentTypeID=" + item["ContentTypeId"].ToString() + "&IsDlg=1";
                        }
                        catch (Exception) { }
                        //fixes issue with link
                        try
                        {
                            //string[] LinkFix = item["FileRef"].ToString().Split('#');
                            //Row["FileRef"] = Web.Site.Url + "/" + LinkFix[1];
                            //Row["LinkField"] = item["FileRef"];
                        }
                        catch (Exception) { }
                    }
                    catch (Exception ex)
                    {
                        LogText(ex.Message);
                    }
                }

                dvResults = new DataView(dtResults);
                // LogText("dvResults:"+dvResults.Count.ToString());
                ////gets profile data
                //Microsoft.SharePoint.SPServiceContext serviceContext = Microsoft.SharePoint.SPServiceContext.Current;
                //UserProfileManager upm = new Microsoft.Office.Server.UserProfiles.UserProfileManager(serviceContext);
                //UserProfile CurrentUser = upm.GetUserProfile(true);
                // foreach (SPField field in CurrentUser.Fields)
                //   LogText(field.InternalName);
                //should have community and interest

                dvResults = FilterDataAccordingToUsers(dvResults);
            }
            //LogText("sbQuery:"+sbQuery.ToString());
           
            return dvResults;
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
                //ProfileValueCollectionBase pvbYourAudience = CurrentUser.GetProfileValueCollection("YourAudience");
                try
                {
                    group = new SPFieldLookupValue(CurrentUser["YourAudienceList_x003a_Title"].ToString());
                    lookedUpItemTitle = group.LookupValue;
                    pvbAudiences = lookedUpItemTitle.Split(';');
                }
                catch (Exception) { }
            //if (!Page.IsPostBack)
            {
                
                //grabs established communities and your audiences for filtering
                StringBuilder sbQuery = new StringBuilder();
                foreach (string value in pvbAudiences)
                {
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {

                        if (sbQuery.Length == 0)
                        {
                            if (ddlCommunities.SelectedItem.Text != "All")
                            {
                                sbQuery.Append("Category_x0020_Domains='" + ddlCommunities.SelectedItem.Text + "'");
                            }
                            else
                            {

                                sbQuery.Append("Category_x0020_Domains='" + value.Replace('#', ' ').Trim() + "'");
//DW Remove extra filter        sbQuery.Append(" OR Category_x0020_Subject_x0020_Area_x002F_Audience='" + value.Replace('#', ' ').Trim() + "'");

                            }
                        }
                        else
                        {
                            if (ddlCommunities.SelectedItem.Text != "All")
                            {
                                sbQuery.Append(" AND Category_x0020_Domains='" + ddlCommunities.SelectedItem.Text + "'");
                            }
                            else
                            {
                                sbQuery.Append(" OR Category_x0020_Domains'" + value.Replace('#', ' ').Trim() + "'");
                                //sbQuery.Append(" OR Category_x0020_Subject_x0020_Area_x002F_Audience='" + value.Replace('#', ' ').Trim() + "'");
                            }
                        }
                    }
                }



                foreach (var value in pvbCommunities)
                {
                    if (!string.IsNullOrEmpty(value.Trim()))
                    {
                        if (sbQuery.Length == 0)
                        {
                            if (ddlAudience.SelectedItem.Text != "All")
                            {
                                sbQuery.Append("Category_x0020_Subject_x0020_Area_x002F_Audience='" + ddlAudience.SelectedItem.Text + "'");
                            }
                            else
                            {
                                sbQuery.Append("Category_x0020_Subject_x0020_Area_x002F_Audience='" + value.Replace('#', ' ').Trim() + "'");
//Remove extra filter           sbQuery.Append(" OR Category_x0020_Domains='" + value.Replace('#', ' ').Trim() + "'");
                            }
                        }
                        else
                        {
                            if (ddlAudience.SelectedItem.Text != "All")
                            {
                                sbQuery.Append(" AND Category_x0020_Subject_x0020_Area_x002F_Audience='" + ddlAudience.SelectedItem.Text + "'");
                            }
                            else
                            {
                                sbQuery.Append(" OR Category_x0020_Subject_x0020_Area_x002F_Audience='" + value.Replace('#', ' ').Trim() + "'");
//Remove extra filter           sbQuery.Append(" OR Category_x0020_Domains='" + value.Replace('#', ' ').Trim() + "'");
                            }
                        }
                    }
                    
                }

                if (sbQuery.Length == 0)
                {
                    dvResults.Table.Rows.Clear();
                    sbQuery.Length = 0;
                    try
                    {
                        dvResults = GetLikedNews(dvResults);
                    }
                    catch (Exception) { }
                }
                else
                {
                    if (ddlAudience.SelectedItem.Text == "All" && ddlCommunities.SelectedItem.Text == "All")
                    {
                        try
                        {

                            dvResults.RowFilter = sbQuery.ToString();
                            dvResults = GetLikedNews(new DataView(dvResults.ToTable()));
                            sbQuery.Length = 0;

                        }
                        catch (Exception) { }
                    }
                }

                //freetext fields
                if (!string.IsNullOrEmpty(txtSearch.Text) && sbQuery.Length == 0)
                {
                    sbQuery.Append("(Title LIKE '%" + txtSearch.Text + "%' OR PublishingPageContent LIKE '%" + txtSearch.Text + "%')");
                }
                else if (!string.IsNullOrEmpty(txtSearch.Text) && sbQuery.Length != 0)
                {
                    sbQuery.Append(" AND (Title LIKE '%" + txtSearch.Text + "%' OR PublishingPageContent LIKE '%" + txtSearch.Text + "%')");
                }


               // LogText(sbQuery.ToString());
                ViewState["InitialQuery"] = sbQuery.ToString();
                dvResults.RowFilter = sbQuery.ToString();
            }
            //else
            //{
            //    //grabs established communities and your audiences for filtering
            //    StringBuilder sbQuery = new StringBuilder();
            //    if (sbQuery.Length == 0)
            //    {
            //        if (ddlCommunities.SelectedItem.Text != "All")
            //        {
            //            sbQuery.Append("Category_x0020_Domains='" + ddlCommunities.SelectedItem.Text + "'");
            //        }
            //        if (ddlAudience.SelectedItem.Text != "All")
            //        {
            //            sbQuery.Append("Category_x0020_Subject_x0020_Area_x002F_Audience='" + ddlAudience.SelectedItem.Text + "'");
            //        }
            //    }
            //    else
            //    {
            //        if (ddlCommunities.SelectedItem.Text != "All")
            //        {
            //            sbQuery.Append(" AND Category_x0020_Domains='" + ddlCommunities.SelectedItem.Text + "'");
            //        }

            //        if (ddlAudience.SelectedItem.Text != "All")
            //        {
            //            sbQuery.Append(" AND Category_x0020_Subject_x0020_Area_x002F_Audience='" + ddlAudience.SelectedItem.Text + "'");
            //        }
            //    }


            //    //freetext fields
            //    if (!string.IsNullOrEmpty(txtSearch.Text) && sbQuery.Length == 0)
            //    {
            //        sbQuery.Append("(Title LIKE '%" + txtSearch.Text + "%' OR PublishingPageContent LIKE '%" + txtSearch.Text + "%')");
            //    }
            //    else if (!string.IsNullOrEmpty(txtSearch.Text) && sbQuery.Length != 0)
            //    {
            //        sbQuery.Append(" AND (Title LIKE '%" + txtSearch.Text + "%' OR PublishingPageContent LIKE '%" + txtSearch.Text + "%')");
            //    }
            //    //ViewState["InitialQuery"] = sbQuery.ToString();
            //    string query = sbQuery.ToString().Trim();
            //    if (!string.IsNullOrEmpty(query))
            //    {
            //        dvResults.RowFilter = "(" + ViewState["InitialQuery"].ToString() + ")AND(" +query +")";
            //    }
            //    else
            //    {
            //        dvResults.RowFilter = ViewState["InitialQuery"].ToString();
            //    }
            //}
            dvResults.Sort = "ArticleStartDate desc";
            dvResults.Table.Columns.Add("DateDiff", typeof(int));
            foreach (DataRow dRow in dvResults.Table.Rows)
            {
                
                try
                {
                    DateTime itemTime = DateTime.Parse(dRow["ArticleStartDate"].ToString());
                    dRow["DateDiff"] = DateTime.Now.Subtract(itemTime).Days;
                }
                catch (Exception)
                {
                    dRow["DateDiff"] = "999";
                }
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
