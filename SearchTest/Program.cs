using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Data;
using System.Web;
using System.Web.Security;
using System.IO;
using System.Linq;
using System.Xml;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;
using Microsoft.Office.Server.Search.Query;
using Microsoft.Office.Server.Search.Administration;



namespace SearchTest
{
    class Program
    {
        public const string _COMMANDLINEUSAGE = "Usage: SearchTest.exe /all siteCollectionUrl ";

       #region Main

        static void Main(string[] args)
        {

            string url;
            string html = String.Empty;

            try
            {
                url = args[0];
            }
            catch
            {
                Console.WriteLine(_COMMANDLINEUSAGE);
                return;
            }


            if (IsValid(url))
            {


                using (SPSite site = new SPSite(url))                
                using (SPWeb web = site.OpenWeb(url.Replace(site.Url, "")))
                {

                    ConsoleOut(String.Format("Begin operation against site collection: {0}", site.Url));

                    ConsoleOut(FixFor_GettingDistinctWebs(site));
                    //ConsoleOut(FixFor_GetQueryForRecentPosts(site));
                    //ConsoleOut(FixFor_GetQueryForCurrentUserReplies(site));
                    //ConsoleOut(EnumerateDicussionListsInSiteCollection(site));
                    //ConsoleOut(EnumerateDistinctWebsWithDiscussions(site));
                    //ConsoleOut(UseSearchForGettingDiscussions(site));

                         
                    
                    ConsoleOut(String.Format("End operation against site collection: ", site.Url));

                }
            }

        }

        #endregion

        #region ConsoleErrorOut

        private static void ConsoleErrorOut(string message, Exception ex)
        {
            Console.WriteLine(String.Format("{0} :: {1}", message, ex.ToString()));
            Console.Error.WriteLine(String.Format("{2} {0} :: {1}", message, ex.ToString(), DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")));
        }

        #endregion

        #region ConsoleOut

        private static void ConsoleOut(string message)
        {
            Console.WriteLine("{0}  {1}", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), message);
        }

        #endregion

        #region IsValid

        static bool IsValid(string url)
        {
            if (url == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region UseSearchForForumInfo
        private static string UseSearchForGettingDiscussions(SPSite site)
        {
           string html = String.Empty;
                    
            // Using search for gathering discussions 
            //<div>Recent Discussions: </div>
            //"<FieldRef Name='LinkDiscussionTitle'/><FieldRef Name='DiscussionLastUpdated'/><FieldRef Name='ItemChildCount'/><FieldRef Name='AverageRating'/>";
            ResultTableCollection searchResults = ExecuteKeywordQuery(site, "ContentType:Discussion");

            if (searchResults.Exists(ResultType.RelevantResults))
            {
                ResultTable searchResult = searchResults[ResultType.RelevantResults];
                DataTable result = new DataTable();
                result.TableName = "Result";
                result.Load(searchResult, LoadOption.OverwriteChanges);

                foreach (DataRow row in result.Rows)
                {
                    string test = "/communities/biometrics/biometrics-community";

                    using (SPWeb lWeb = site.OpenWeb(test))
                    {
                    }
                    html += "WorkId = " + row["WorkId"].ToString() + "\n";
                    html += "Rank = " + row["Rank"].ToString() + "\n";
                    html += "Title = " + row["Title"].ToString() + "\n";
                    html += "Author = " + row["Author"].ToString() + "\n";
                    html += "Size = " + row["Size"].ToString() + "\n";
                    html += "Path = " + row["Path"].ToString() + "\n";
                    html += "Description = " + row["Description"].ToString() + "\n";
                    html += "Write = " + row["Write"].ToString() + "\n";
                    html += "SiteName = " + row["SiteName"].ToString() + "\n";
                    html += "CollapsingStatus = " + row["CollapsingStatus"].ToString() + "\n";
                    html += "HitHighlightedSummary = " + row["HitHighlightedSummary"].ToString() + "\n";
                    html += "HitHighlightedProperties = " + row["HitHighlightedProperties"].ToString() + "\n";
                    html += "ContentClass = " + row["ContentClass"].ToString() + "\n";
                    html += "IsDocument = " + row["IsDocument"].ToString() + "\n";
                    html += "PictureThumbnailURL = " + row["PictureThumbnailURL"].ToString() + "\n";
                    html += "ServerRedirectedURL = " + row["ServerRedirectedURL"].ToString() + "\n";
                    html += "======================================================";
                    //html += "<tr><td><div><b>" + discussionBoard.Title + "</b></div><hr/>";

                }
                    
                /* - Example of a hit and waht is returned by the search results


                        row["SiteName"].ToString()	"http://sp2010app:8001/communities/biometrics/biometrics-community/Lists/Biometric"	string
                WorkId = 782
                Rank = 100000000
                Title = Another Biometrics Forum test2
                Author = System.String[]
                Size = 0
                Path = http://sp2010app:8001/communities/biometrics/biometrics-community/Lists/Biometric/Another Biometrics Forum test2
                Description = The NIEM Web site is designed to develop, disseminate and support enterprise-wide information exchange standards and processes that can enable jurisdictions to effectively share critical information in emergency situations, as well as support the day-to-day operations of agencies throughout the nation.
                Write = 3/20/2012 8:35:22 PM
                SiteName = http://sp2010app:8001/communities/biometrics/biometrics-community/Lists/Biometric
                CollapsingStatus = 0
                HitHighlightedSummary = ​This is from a Biometrics member. <ddd/> The NIEM Web site is designed to develop, disseminate and support enterprise-wide information exchange standards and processes that can enable jurisdictions to <ddd/> 
                HitHighlightedProperties = <HHTitle>Another Biometrics Forum test2</HHTitle><HHUrl>http://sp2010app:8001/communities/biometrics/biometrics-community/Lists/Biometric/Another Biometrics Forum test2</HHUrl>
                ContentClass = STS_ListItem_DiscussionBoard
                IsDocument = False
                PictureThumbnailURL = 
                ServerRedirectedURL = 
                "	string
                    * */

                /*
                        WorkId
                        Rank
                        Title
                        Author
                        Size
                        Path
                        Description
                        Write
                        SiteName
                        CollapsingStatus
                        HitHighlightedSummary
                        HitHighlightedProperties
                        ContentClass
                        IsDocument
                        PictureThumbnailURL
                        ServerRedirectedURL

                        ContentType:Discuss and Write>=2013-08-01
                 */
            }

            return html;

        }

        private static ResultTableCollection ExecuteKeywordQuery(SPSite site, string queryText)
        {
            SearchServiceApplicationProxy proxy = (SearchServiceApplicationProxy)SearchServiceApplicationProxy.GetProxy(SPServiceContext.GetContext(site));
            KeywordQuery query = new KeywordQuery(proxy);
            query.ResultsProvider = Microsoft.Office.Server.Search.Query.SearchProvider.Default;
            query.SelectProperties.Add("ItemID");
            query.QueryText = queryText;
            query.ResultTypes |= ResultType.RelevantResults;
            query.RowLimit = 0;
            ResultTableCollection searchResults = query.Execute();
            return searchResults;
            //if (searchResults.Exists(ResultType.RelevantResults))
            //{
            //    ResultTable searchResult = searchResults[ResultType.RelevantResults];
            //    DataTable result = new DataTable();
            //    result.TableName = "Result";
            //    result.Load(searchResult, LoadOption.OverwriteChanges);
            //    FillResultsGrid(result);
            //}
        }
        #endregion

        private static string EnumerateDistinctWebsWithDiscussions(SPSite site)
        {

            string html2 = String.Empty;
            
            using (SPWeb web = site.RootWeb)
            {
                //Distinct webs from SiteDataQuery for responses during the last 7 days.
                // Fetch using SPSiteDataQuery
                string today = DateTime.Now.ToString("MM/dd/yyyy");
                SPSiteDataQuery query = new SPSiteDataQuery();
                query.Lists = "<Lists ServerTemplate=\"108\" />";  // Discussions.
                query.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"LinkDiscussionTitle\"/><FieldRef Name=\"DiscussionLastUpdated\"/><FieldRef Name=\"ItemChildCount\"/><FieldRef Name=\"AverageRating\"/>";    //Title is LastName column 
                query.Webs = "<Webs Scope=\"SiteCollection\" />";
                query.Query = "<Where><Geq><FieldRef Name = 'Modified' /><Value Type ='DateTime'>" + Microsoft.SharePoint.Utilities.SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(today).AddDays(-30)) + "</Value></Geq></Where>";
                DataTable dataTable = web.GetSiteData(query);
                //48

                List<string> communitiesWebUrl = new List<string>();
                string webRelUrl = String.Empty;
                foreach (DataRow row in dataTable.Rows)
                {
                    webRelUrl = site.AllWebs[new Guid(row["WebId"].ToString())].ServerRelativeUrl;
                    if (!communitiesWebUrl.Contains(webRelUrl))
                    {
                        communitiesWebUrl.Add(webRelUrl);
                        html2 += webRelUrl + "\n";
                    }

                }

                SPUser currentUser = web.CurrentUser;

                //Distinct webs from SiteDataQuery for responses during the last 7 days.
                // Fetch using SPSiteDataQuery
                SPSiteDataQuery query2 = new SPSiteDataQuery();
                query2.Lists = "<Lists ServerTemplate=\"108\" />";  // Discussions.
                query2.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"LinkDiscussionTitle\"/><FieldRef Name=\"DiscussionLastUpdated\"/><FieldRef Name=\"ItemChildCount\"/><FieldRef Name=\"AverageRating\"/>";   // Title is LastName column 
                query2.Webs = "<Webs Scope=\"SiteCollection\" />";
                query2.Query = string.Format(@"<Where>
                                <And>
                                    <Eq>
                                        <FieldRef Name='ContentType' />
                                        <Value Type='Computed'>Message</Value>
                                    </Eq>
                                    <Eq>
                                        <FieldRef Name='Author' LookupId='True' />
                                        <Value Type='Integer'>{0}</Value>
                                    </Eq>
                                </And>
                            </Where>", currentUser.ID);
                DataTable dataTable2 = web.GetSiteData(query2);
                //48

                List<string> communitiesWebUrl2 = new List<string>();
                string webRelUrl2 = String.Empty;
                foreach (DataRow row in dataTable.Rows)
                {
                    webRelUrl2 = site.AllWebs[new Guid(row["WebId"].ToString())].ServerRelativeUrl;
                    if (!communitiesWebUrl2.Contains(webRelUrl))
                    {
                        communitiesWebUrl2.Add(webRelUrl);
                        html2 += webRelUrl + "\n";
                    }

                }
            }
            return html2;

        }

        private static string EnumerateDicussionListsInSiteCollection(SPSite site)
        {
            string html = String.Empty;
            string messageBody = String.Empty;

            foreach (SPWeb dWeb in site.AllWebs)
            {
                foreach (SPList dlist in dWeb.Lists)
                {
                    if (dlist.Title.ToUpper().Contains("FORUM") || dlist.Title.ToUpper().Contains("DISCUSSION"))
                    {
                        if (dlist.ItemCount > 0)
                        {


                            foreach (SPListItem discussion in dlist.Folders)
                            {

                                //discussion.Title is discussion title
                                SPQuery qry = new SPQuery();
                                qry.Folder = discussion.Folder;
                                SPListItemCollection messages = dlist.GetItems(qry);

                                if (messages.Count > 0)
                                {
                                    messageBody = discussion["MessageBody"].ToString().Replace("\n", "");
                                    messageBody = messageBody.Substring(messageBody.IndexOf(">") + 1, messageBody.Length - ((messageBody.IndexOf(">") + 1))).Replace("</div>", "");
                                    if (messageBody.Length > 100)
                                    {
                                        messageBody = messageBody.Substring(0, 100);
                                    }
                                    html += dWeb.ServerRelativeUrl + "\t" + dlist.Title + "\tDiscussion\t" + discussion["Editor"].ToString() + "\t" + discussion["Modified"].ToString() + "\t" + messageBody + "\n";
                                    foreach (SPListItem message in messages)
                                    {
                                        messageBody = message["MessageBody"].ToString().Replace("\n", "");
                                        messageBody = messageBody.Substring(messageBody.IndexOf(">") + 1, messageBody.Length - ((messageBody.IndexOf(">") + 1))).Replace("</div>", "");
                                        if (messageBody.Length > 100)
                                        {
                                            messageBody = messageBody.Substring(0, 100);
                                        }
                                        html += dWeb.ServerRelativeUrl + "\t" + dlist.Title + "\tReply\t" + message["Editor"].ToString() + "\t" + message["Modified"].ToString() + "\t" + messageBody + "\n";
                                    }
                                }
                                else
                                {
                                    messageBody = String.Empty;
                                    if (discussion["MessageBody"] != null)
                                    {
                                        messageBody = discussion["MessageBody"].ToString().Replace("\n", "");
                                        messageBody = messageBody.Substring(messageBody.IndexOf(">") + 1, messageBody.Length - ((messageBody.IndexOf(">") + 1))).Replace("</div>", "");
                                        if (messageBody.Length > 100)
                                        {
                                            messageBody = messageBody.Substring(0, 100);
                                        }
                                    }
                                    html += dWeb.ServerRelativeUrl + "\t" + dlist.Title + "\tDiscussion (no replies)\t" + discussion["Editor"].ToString() + "\t" + discussion["Modified"].ToString() + "\t" + messageBody + "\n";
                                }

                            }

                        }
                        else
                        {
                            html += dWeb.ServerRelativeUrl + "\t" + dlist.Title + "\tNo Dicussions\t\t\n";
                        }
                    }
                }
            }
            return html;

        }

        private static string FixFor_GettingDistinctWebs(SPSite site)
        {

            string webRelUrl = String.Empty;
            DateTime recentDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy")).AddDays(-30); 
            List<string> communitiesWebUrl = new List<string>();

            // Fetch using SPSiteDataQuery
            using (SPWeb web = site.OpenWeb())
            {
                SPUser CurrentUser = web.CurrentUser;

                //Distinct webs from SiteDataQuery for responses during the last 7 days.
                SPSiteDataQuery query = new SPSiteDataQuery();
                query.Lists = "<Lists ServerTemplate=\"108\" />";  // Discussions.
                //query.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"LinkDiscussionTitle\"/><FieldRef Name=\"DiscussionLastUpdated\"/><FieldRef Name=\"ItemChildCount\"/><FieldRef Name=\"AverageRating\"/>";   /* Title is LastName column */
                query.Webs = "<Webs Scope=\"SiteCollection\" />";
                query.Query = "<Where><Geq><FieldRef Name = 'Modified' /><Value Type ='DateTime'>" + Microsoft.SharePoint.Utilities.SPUtility.CreateISO8601DateTimeFromSystemDateTime(recentDate) + "</Value></Geq></Where>";

                DataTable dataTable = web.GetSiteData(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    webRelUrl = site.AllWebs[new Guid(row["WebId"].ToString())].ServerRelativeUrl;
                    if (!communitiesWebUrl.Contains(webRelUrl))
                    {
                        communitiesWebUrl.Add(webRelUrl);
                    }

                }


                //Distinct webs from SiteDataQuery for responses to my posts.  I got this CAML from Chris' code.
                // Fetch using SPSiteDataQuery
                query = new SPSiteDataQuery();
                query.Lists = "<Lists ServerTemplate=\"108\" />";  // Discussions.
                query.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"LinkDiscussionTitle\"/><FieldRef Name=\"DiscussionLastUpdated\"/><FieldRef Name=\"ItemChildCount\"/><FieldRef Name=\"AverageRating\"/>";   /* Title is LastName column */
                query.Webs = "<Webs Scope=\"SiteCollection\" />";
                query.Query = string.Format(@"<Where>
                                        <And>
                                            <Eq>
                                                <FieldRef Name='ContentType' />
                                                <Value Type='Computed'>Message</Value>
                                            </Eq>
                                            <Eq>
                                                <FieldRef Name='Author' LookupId='True' />
                                                <Value Type='Integer'>{0}</Value>
                                            </Eq>
                                        </And>
                                   </Where>", CurrentUser.ID);

                dataTable = web.GetSiteData(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    webRelUrl = site.AllWebs[new Guid(row["WebId"].ToString())].ServerRelativeUrl;
                    if (!communitiesWebUrl.Contains(webRelUrl))
                    {
                        communitiesWebUrl.Add(webRelUrl);
                    }

                }

            }
            return communitiesWebUrl.ToArray().ToString();
        }

        private static string FixFor_GetQueryForCurrentUserReplies(SPSite site)
        {
            string html = string.Empty;
            string webUrl = "/communities/health/health-community";
            string listTitle = "Health Forum";
            DateTime recentDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy")).AddDays(-1); 

            using (SPWeb web = site.AllWebs[webUrl])
            {
                SPUser CurrentUser = web.CurrentUser;

                SPList discussionBoard = web.Lists[listTitle];


                string queryText = GetQueryForCurrentUserReplies(discussionBoard, CurrentUser, recentDate);
                SPView view = discussionBoard.DefaultView;
                view.Paged = false;
                if (queryText != null)
                {
                    html += "<div>Discussions replied by me: </div>";

                    SPQuery query = new SPQuery(view);
                    //'query.Query = queryText;
                    query.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"LinkDiscussionTitle\"/><FieldRef Name=\"DiscussionLastUpdated\"/><FieldRef Name=\"ItemChildCount\"/><FieldRef Name=\"AverageRating\"/>";   /* Title is LastName column */
                    query.Query = queryText;// "<Where><Geq><FieldRef Name = 'Modified' /><Value Type ='DateTime'>" + Microsoft.SharePoint.Utilities.SPUtility.CreateISO8601DateTimeFromSystemDateTime(recentDate) + "</Value></Geq></Where>";

                    query.RowLimit = 3;
                    // query.ViewFields = "<OrderBy><FieldRef Name='Modified' Ascending='False' /></OrderBy><Fieldref Name='Subject'/><Fieldref Name='Body'/><Fieldref Name='AverageRating'/>";
                    query.ViewFields = "<FieldRef Name='LinkDiscussionTitle'/><FieldRef Name='DiscussionLastUpdated'/><FieldRef Name='ItemChildCount'/><FieldRef Name='AverageRating'/>";
                    query.ViewFieldsOnly = true;
                    string discussionHtml = discussionBoard.RenderAsHtml(query);
                    html += discussionHtml;

                }

            }

            return html;

        }

        private static string FixFor_GetQueryForRecentPosts(SPSite site)
        {
            
            string html = string.Empty;
            string webUrl = "/communities/cbrn/cbrn-community";
            string listTitle = "CBRN Forum";
            //string webUrl = "/communities/canada";
            //string listTitle = "Canada COE Forum";
            DateTime recentDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy")).AddDays(-30);

            using (SPWeb web = site.AllWebs[webUrl])
            {
                SPUser CurrentUser = web.CurrentUser;

                SPList discussionBoard = web.Lists[listTitle];
                SPView view = discussionBoard.DefaultView;

                string recentForumsQuery = GetQueryForRecentPosts(discussionBoard, recentDate);
                if (recentForumsQuery != null)
                {
                    html += "<div>Recent Discussions: </div>";
                    SPQuery query = new SPQuery(view);
                    query.Query = recentForumsQuery;
                    query.ViewFields = "<FieldRef Name='LinkDiscussionTitle'/><FieldRef Name='DiscussionLastUpdated'/><FieldRef Name='ItemChildCount'/><FieldRef Name='AverageRating'/>";
                    query.ViewFieldsOnly = true;
                    query.RowLimit = 3;
                    string discussionHtml = discussionBoard.RenderAsHtml(query);
                    html += discussionHtml;

                    SPListItemCollection listItems = discussionBoard.GetItems(query);

                }
            }
            return html;

        }

        private static string GetQueryForRecentPosts(SPList discussionBoard, DateTime recentDate)
        {
            // All this is currently built, then you will see a recent discussion table appear even if there is no recent discussion
            //            string query = null;

            //            if (discussionBoard != null)
            //            {

            //                query = String.Format(@"<Where>
            //                                          <And>
            //                                            <Eq>
            //                                                <FieldRef Name='ContentType' />
            //                                                <Value Type='Computed'>Discussion</Value>
            //                                            </Eq>
            //                                           <Geq><FieldRef Name = 'Modified' /><Value Type ='DateTime'>{0}</Value></Geq>
            //                                          </And>
            //                                        </Where>
            //                                        <OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy>", Microsoft.SharePoint.Utilities.SPUtility.CreateISO8601DateTimeFromSystemDateTime(recentDate));

            //            }
            //            return query;

            // This is simlar to GetQueryForCurrentUserReplies but we are not looking for replies made by the current user
            string query = null;

            if (discussionBoard != null)
            {
                SPQuery qry = new SPQuery();
                qry.Query = string.Format(@"<Where>
                                              <Or>
                                                <And>
                                                    <Eq>
                                                        <FieldRef Name='ContentType' />
                                                        <Value Type='Computed'>Message</Value>
                                                    </Eq>
                                                    <Geq>
                                                        <FieldRef Name = 'Modified' />
                                                        <Value Type ='DateTime'>{0}</Value>
                                                    </Geq>
                                                </And>
                                                <And>
                                                    <Eq>
                                                        <FieldRef Name='ContentType' />
                                                        <Value Type='Computed'>Discussion</Value>
                                                    </Eq>
                                                    <Geq><FieldRef Name = 'Modified' /><Value Type ='DateTime'>{0}</Value></Geq>
                                                </And>
                                              </Or>
                                   </Where>", Microsoft.SharePoint.Utilities.SPUtility.CreateISO8601DateTimeFromSystemDateTime(recentDate));
                //qry.ViewFields = @"<FieldRef Name='ReplyNoGif' />";
                qry.ViewAttributes = "Scope='RecursiveAll'";
                SPListItemCollection listItems = discussionBoard.GetItems(qry);
                if (listItems.Count > 0)
                {
                    string queryText = @"<Eq>
                                             <FieldRef Name='ContentType' />
                                             <Value Type='Computed'>Discussion</Value>
                                          </Eq>";
                    List<string> threads = new List<string>();
                    foreach (SPListItem message in listItems)
                    {

                        string thread = String.Empty;

                        if (message.ContentType.Name == "Message")
                        {
                            thread = Convert.ToString(message["ReplyNoGif"]).TrimStart('/');
                        }
                        else if (message.ContentType.Name == "Discussion")
                        {
                            thread = Convert.ToString(message["FileRef"]).TrimStart('/');
                        }
                        thread = "/" + thread;

                        if (!threads.Contains(thread))
                        {
                            threads.Add(thread);
                        }

                    }

                    if (threads.Count == 1)
                    {
                        queryText = "<And>" + queryText + "<Eq><FieldRef Name='FileRef' /><Value Type='Text'>" +  threads[0] + "</Value></Eq></And>";
                    }
                    else
                    {
                        string tempQuery = "<Eq><FieldRef Name='FileRef' /><Value Type='Text'>" + threads[0] + "</Value></Eq>";
                        for (int i = 1; i < threads.Count; i++)
                        {
                            tempQuery = "<Or>" +
                                                "<Eq><FieldRef Name='FileRef' /><Value Type='Text'>" +  threads[i] + "</Value></Eq>" +
                                                tempQuery +
                                        "</Or>";
                        }
                        queryText = "<And>" + tempQuery + "</And>";
                    }
                    queryText = "<Where>" + queryText + "</Where>";
                    query = queryText;
                }
            }
            return query;
        }

        private static string GetQueryForCurrentUserReplies(SPList discussionBoard, SPUser currentUser, DateTime recentDate)
        {
            string query = null;

            if (discussionBoard != null)
            {
                SPQuery qry = new SPQuery();
                qry.Query = string.Format(@"<Where>
                                        <And>
                                            <And>
                                                <Eq>
                                                    <FieldRef Name='ContentType' />
                                                    <Value Type='Computed'>Message</Value>
                                                </Eq>
                                                <Eq>
                                                    <FieldRef Name='Author' LookupId='True' />
                                                    <Value Type='Integer'>{0}</Value>
                                                </Eq>
                                            </And>
                                            <Geq>
                                                <FieldRef Name = 'Modified' />
                                                <Value Type ='DateTime'>{1}</Value>
                                            </Geq>
                                        </And>
                                   </Where>", currentUser.ID, Microsoft.SharePoint.Utilities.SPUtility.CreateISO8601DateTimeFromSystemDateTime(recentDate));
                //qry.ViewFields = @"<FieldRef Name='ReplyNoGif' />";
                qry.ViewAttributes = "Scope='RecursiveAll'";
                SPListItemCollection listItems = discussionBoard.GetItems(qry);
                if (listItems.Count > 0)
                {
                    string queryText = @"<Eq>
                                             <FieldRef Name='ContentType' />
                                             <Value Type='Computed'>Discussion</Value>
                                          </Eq>";
                    List<string> threads = new List<string>();
                    foreach (SPListItem message in listItems)
                    {
                        string thread = Convert.ToString(message["ReplyNoGif"]);
                        //thread = thread.Substring(0, 46);
                        if (!threads.Contains(thread))
                        {
                            threads.Add(thread);

                        }
                    }

                    if (threads.Count == 1)
                    {
                        queryText = "<And>" + queryText + "<Eq><FieldRef Name='FileRef' /><Value Type='Text'>" + "/" + threads[0] + "</Value></Eq></And>";
                    }
                    else
                    {
                        string tempQuery = "<Eq><FieldRef Name='FileRef' /><Value Type='Text'>" + "/" + threads[0] + "</Value></Eq>";
                        for (int i = 1; i < threads.Count; i++)
                        {
                            tempQuery = "<Or>" +
                                                "<Eq><FieldRef Name='FileRef' /><Value Type='Text'>" + "/" + threads[i] + "</Value></Eq>" +
                                                tempQuery +
                                        "</Or>";
                        }
                        queryText = "<And>" + tempQuery + "</And>";
                    }
                    queryText = "<Where>" + queryText + "</Where>";
                    query = queryText;
                }
            }
            return query;
        }

    }
}

