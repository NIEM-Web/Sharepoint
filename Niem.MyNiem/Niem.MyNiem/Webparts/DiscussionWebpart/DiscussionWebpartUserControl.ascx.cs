using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using Microsoft.SharePoint;
using Microsoft.Office.Server.Search.Query;
using Microsoft.Office.Server.Search.Administration;

namespace Niem.MyNiem.Webparts.DiscussionWebpart
{
    public partial class DiscussionWebpartUserControl : UserControl
    {
        protected void GetData()
        {
            DataTable dtResults = null;
            using (SPWeb Web = SPContext.Current.Site.RootWeb)
            {
                SPSiteDataQuery Query = new SPSiteDataQuery();
                Query.Lists = "<Lists ServerTemplate=\"108\" />";
                //"<Lists BaseType=\"1\" />";
                //Query.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"ID\" /><FieldRef Name=\"Location\" /><FieldRef Name=\"EventDate\" /><FieldRef Name=\"Description\" /><FieldRef Name=\"ContentTypeId\" />";
                //content type & property
                //Query.Query = "<Where><Eq><FieldRef Name='ContentType' /><Value Type='Computed'>" + ToolsContentType + "</Value></Eq></Where>";
                // Query.Webs = "<Webs Scope=\"Recursive\" />";
                Query.Webs = "<Webs Scope=\"Recursive\" />";
                dtResults = Web.GetSiteData(Query);

                //dtResults.Columns.Add("LinkField");
                ////fixes the links and only uses 150 chars of the PublishingPageContentField
                //foreach (DataRow Row in dtResults.Rows)
                //{

                //    SPSite Site = SPContext.Current.Site;
                //    Guid WebGuid = new Guid(Row["WebId"].ToString());
                //    using (SPWeb LinkWeb = Site.OpenWeb(WebGuid))
                //    {
                //        Guid ListGuid = new Guid(Row["ListId"].ToString());
                //        SPList List = LinkWeb.Lists[ListGuid];
                //        Row["LinkField"] = LinkWeb.Url + "/" + List.RootFolder.Url + "/DispForm.aspx?ID=" + Row["ID"].ToString() + "&ContentTypeID=" + Row["ContentTypeId"].ToString() + "&IsDlg=1";
                //    }
                //}
                dtResults.TableName = "dtResults";
                dtResults.WriteXml("C:\\temp\\events.xml");

            }

            //DataView dvResults = new DataView(dtResults);
            //return FilterSearchData(dvResults);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //GetData();
                //SPSiteDataQuery query = new SPSiteDataQuery();

                ////query.ViewFields = "<FieldRef Name=\"LinkDiscussionTitle\"><FieldRef Name=\"ID\">";

                //query.Lists = "<Lists ServerTemplate=\"108\">";
                //query.Webs = "<Webs Scope=\"SiteCollection\" />";
                //SPWeb web = SPContext.Current.Site.RootWeb;
                //DataTable table = web.GetSiteData(query);
                //table.TableName = "Discussions";
                //table.WriteXml("c:\\log.txt");
                DateTime recentDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy")).AddDays(-30); 
                SPUser CurrentUser = SPContext.Current.Web.CurrentUser;
                List<string> communitiesWebUrl = new List<string>();
                List<string> userCommunities = null;
                string html = string.Empty;
                string siteUrl = String.Empty;
                string webRelUrl = String.Empty;

                //html = "Current Site url: " + SPContext.Current.Site.Url + "<br/>";
                siteUrl = SPContext.Current.Site.Url;
                //html += "Current Web url: " + SPContext.Current.Web.Url + "<br/>";
                //html += "Initial current user: " + SPContext.Current.Web.CurrentUser + "<br/>";


                SPSecurity.RunWithElevatedPrivileges(delegate()
                {

                    //Rks: This was changed over from using SPContext.Current.Site.Id to using the url because
                    // The Site object was opening against the default zone and the permissions there were not working
                    //  properly.  using the siteurl zone means the objects are being opened in code against the same
                    //  zone that they are seeing through the browser.
                    using (SPSite site = new SPSite(siteUrl))
                    {




                        // Fetch using SPSiteDataQuery
                        using (SPWeb web = site.OpenWeb())
                        {

                            //Distinct webs from SiteDataQuery for responses during the last 7 days.
                            SPSiteDataQuery query = new SPSiteDataQuery();
                            query.Lists = "<Lists ServerTemplate=\"108\" />";  // Discussions.
                            //RKS - 2013-09-04 - this was commented out because the standard discussion list does not include the AverageRating field.  If this viewfield is not present, the discussion is not returned.
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
                            //RKS - 2013-09-04 - this was commented out because the standard discussion list does not include the AverageRating field.  If this viewfield is not present, the discussion is not returned.
                            //query.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"LinkDiscussionTitle\"/><FieldRef Name=\"DiscussionLastUpdated\"/><FieldRef Name=\"ItemChildCount\"/><FieldRef Name=\"AverageRating\"/>";   /* Title is LastName column */
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




                    
                        //html += "using Site url: " + site.Url + "<br/>";

                        bool discussionExist = false;
                        foreach (var communityWebUrl in communitiesWebUrl)
                        {

                            //html += "> " + communityWebUrl + "<br/>";


                            using (SPWeb communityWeb = site.OpenWeb(communityWebUrl))
                            {

                                //html += ">--- Url..: " + communityWeb.Url + "<br/>";
                                //html += ">--- Title: " + communityWeb.Title + "<br/>";
                                // html += ">--- CurrentUserWeb: " + communityWeb.CurrentUser + "<br/>";

                                if (communityWeb.DoesUserHavePermissions(CurrentUser.LoginName, SPBasePermissions.AddListItems))
                                {
                                    //html += ">--- Has Permissions to: " + communityWeb.Title + "<br/>";
                                    
                                    //html += "<p>" + communityWeb.Title + "</p>";
                                    List<SPList> discussionBoards = GetDiscussionBoards(communityWeb);

                                    foreach (SPList discussionBoard in discussionBoards)
                                    {
                                        discussionExist = true;
                                        html += "<tr><td><div><b>" + discussionBoard.Title + "</b></div><hr/>";
                                        string queryText = GetQueryForCurrentUserReplies(discussionBoard, CurrentUser, recentDate);
                                        SPView view = discussionBoard.DefaultView;
                                        view.Paged = false;
                                        if (queryText != null)
                                        {
                                            html += "<div>Discussions replied by me: </div>";

                                            SPQuery query = new SPQuery(view);
                                            //'query.Query = queryText;
                                            query.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"LinkDiscussionTitle\"/><FieldRef Name=\"DiscussionLastUpdated\"/><FieldRef Name=\"ItemChildCount\"/><FieldRef Name=\"AverageRating\"/>";   /* Title is LastName column */
                                            query.Query = queryText;
                                            //RKS: I removed this line of code because Replies are not Dicussions so the queryText was more appropriate
                                            //query.Query = "<Where><Geq><FieldRef Name = 'Modified' /><Value Type ='DateTime'>" + Microsoft.SharePoint.Utilities.SPUtility.CreateISO8601DateTimeFromSystemDateTime(recentDate) + "</Value></Geq></Where>";

                                            query.RowLimit = 3;
                                            // query.ViewFields = "<OrderBy><FieldRef Name='Modified' Ascending='False' /></OrderBy><Fieldref Name='Subject'/><Fieldref Name='Body'/><Fieldref Name='AverageRating'/>";
                                            query.ViewFields = "<FieldRef Name='LinkDiscussionTitle'/><FieldRef Name='DiscussionLastUpdated'/><FieldRef Name='ItemChildCount'/><FieldRef Name='AverageRating'/>";
                                            query.ViewFieldsOnly = true;
                                            string discussionHtml = discussionBoard.RenderAsHtml(query);
                                            html += discussionHtml;

                                        }
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
                                        }

                                        html += "</td></tr>";

                                    }
                                    

                                }
                                //else
                                //{
                                //    html += ">--- No Permissions to: " + communityWeb.Title + "<br/>";
                                //}
                            }
   

                        }

                        //removed pagination and context menu
                        discussionBoardsDiv.InnerHtml = "<table id='tblDiscussions'><tbody>" + html.Replace(@"""ms-bottompaging""", @"""ms-bottompaging"" style='display:none;'").Replace("OnItem(this)", "") + "</tbody></table>";
                        if (discussionExist == false)
                            ShowNoRecords();
                    
                    }
                });                

            }
            catch (Exception ex)
            {
                Controls.Add(new LiteralControl(ex.Message));
            }
        }

        void ShowNoRecords()
        {
            Controls.Add(new LiteralControl("Join a <a href=\"/communities/Pages/communities.aspx\">Community</a> or participate in an open forum."));
        }

        private SPListItem GetCurrentUserItem(SPList profilesList, SPUser CurrentUser)
        {
            SPListItem currentProfile = null;
            if (profilesList != null)
            {
                SPQuery query = new SPQuery();
                query.Query = string.Format(@"<Where>
                                    <Eq>
                                        <FieldRef Name='SPUser' LookupId='TRUE' />
                                        <Value Type='Integer'>
                                            {0}
                                        </Value>
                                    </Eq>
                                </Where>", CurrentUser.ID);
                query.RowLimit = 1;
                SPListItemCollection items = profilesList.GetItems(query);
                if (items.Count > 0)
                {
                    currentProfile = items[0];
                }
            }
            return currentProfile;
        }

        private string GetQueryForCurrentUserReplies(SPList discussionBoard, SPUser currentUser, DateTime recentDate)
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

        /// <summary>
        /// This returns back the posts from the past 7 days from the dicussion board.
        /// </summary>
        /// <param name="discussionBoard"></param>
        /// <param name="recentDate"></param>
        /// <returns></returns>
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
                        queryText = "<And>" + queryText + "<Eq><FieldRef Name='FileRef' /><Value Type='Text'>" + threads[0] + "</Value></Eq></And>";
                    }
                    else
                    {
                        string tempQuery = "<Eq><FieldRef Name='FileRef' /><Value Type='Text'>" + threads[0] + "</Value></Eq>";
                        for (int i = 1; i < threads.Count; i++)
                        {
                            tempQuery = "<Or>" +
                                                "<Eq><FieldRef Name='FileRef' /><Value Type='Text'>" + threads[i] + "</Value></Eq>" +
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

        private List<SPList> GetDiscussionBoards(SPWeb communityWeb)
        {
            SPListCollection lists = communityWeb.Lists;
            List<SPList> discussionBoards = new List<SPList>();
            foreach (SPList list in lists)
            {
                if (list.BaseTemplate == SPListTemplateType.DiscussionBoard)
                {
                    discussionBoards.Add(list);
                }
            }
            return discussionBoards;
        }


    }
}
