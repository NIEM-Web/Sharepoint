using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Data;
using System.Linq;
using System.Collections.Generic;

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
                SPUser CurrentUser = SPContext.Current.Web.CurrentUser;
                List<string> communitiesWebUrl = new List<string>();
                List<string> userCommunities = null;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {

                        using (SPWeb web = site.OpenWeb())
                        {
                            //    SPList profilesList = web.GetList("/Lists/Profiles");
                            //    SPListItem profileItem = GetCurrentUserItem(profilesList, CurrentUser);
                            //    if (profileItem != null)
                            //    {
                            //        string communities = Convert.ToString(profileItem["EstablishedCommunities_x003a_Tit"]);
                            //        if (communities != null)
                            //        {
                            //            var tempCommunities = communities.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                            //            tempCommunities = (from tempCommunity in tempCommunities
                            //                               select tempCommunity.Trim()).ToArray<string>();
                            //            userCommunities = new List<string>(tempCommunities);
                            //        }
                            //    }
                            SPList myNiemList = web.GetList("/Lists/MyNiem%20List");
                            SPListItemCollection items = myNiemList.GetItems("Title");
                            communitiesWebUrl = (from item in items.OfType<SPListItem>()
                                                 select Convert.ToString(item["Title"])).ToList<string>();
                        }
                        //if (userCommunities != null)
                        //{
                        string html = string.Empty;


                        bool discussionExist = false;
                        foreach (var communityWebUrl in communitiesWebUrl)
                        {
                            using (SPWeb communityWeb = site.OpenWeb(communityWebUrl))
                            {
                                

                                if (communityWeb.DoesUserHavePermissions(CurrentUser.LoginName, SPBasePermissions.AddListItems))
                                {
                                    //html += "<p>" + communityWeb.Title + "</p>";
                                    List<SPList> discussionBoards = GetDiscussionBoards(communityWeb);

                                    foreach (SPList discussionBoard in discussionBoards)
                                    {
                                        discussionExist = true;
                                        html += "<tr><td><div><b>" + discussionBoard.Title + "</b></div><hr/>";
                                        string queryText = GetQueryForCurrentUserReplies(discussionBoard, CurrentUser);
                                        SPView view = discussionBoard.DefaultView;
                                        view.Paged = false;
                                        if (queryText != null)
                                        {
                                            html += "<div>Discussions replied by me: </div>";
                                           
                                            SPQuery query = new SPQuery(view);
                                            query.Query = queryText;
                                            query.RowLimit = 3;
                                           // query.ViewFields = "<OrderBy><FieldRef Name='Modified' Ascending='False' /></OrderBy><Fieldref Name='Subject'/><Fieldref Name='Body'/><Fieldref Name='AverageRating'/>";
                                            query.ViewFields = "<FieldRef Name='LinkDiscussionTitle'/><FieldRef Name='DiscussionLastUpdated'/><FieldRef Name='ItemChildCount'/><FieldRef Name='AverageRating'/>";
                                            query.ViewFieldsOnly = true;
                                            string discussionHtml = discussionBoard.RenderAsHtml(query);
                                            html += discussionHtml;

                                        }
                                        string recentForumsQuery = GetQueryForRecentPosts(discussionBoard);
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
                            }
                            
                        }
                        
                        //removed pagination and context menu
                        discussionBoardsDiv.InnerHtml = "<table id='tblDiscussions'><tbody>" + html.Replace(@"""ms-bottompaging""", @"""ms-bottompaging"" style='display:none;'").Replace("OnItem(this)", "") + "</tbody></table>";
                        if (discussionExist == false)
                            ShowNoRecords();
                    }
                    //}
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

        private string GetQueryForCurrentUserReplies(SPList discussionBoard, SPUser currentUser)
        {
            string query = null;

            if (discussionBoard != null)
            {
                SPQuery qry = new SPQuery();
                qry.Query = string.Format(@"<Where>
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
                                   </Where>",currentUser.ID);
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


        private string GetQueryForRecentPosts(SPList discussionBoard)
        {
            string query = null;

            if (discussionBoard != null)
            {
               
                query = @"<Where>
                                            <Eq>
                                                <FieldRef Name='ContentType' />
                                                <Value Type='Computed'>Discussion</Value>
                                            </Eq>
                                           

                                   </Where>
                                    <OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy>";

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
