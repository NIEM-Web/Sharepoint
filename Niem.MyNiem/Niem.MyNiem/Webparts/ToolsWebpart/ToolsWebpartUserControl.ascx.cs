using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Portal.WebControls;
using System.Web;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.UserProfiles;

namespace Niem.MyNiem.Webparts.ToolsWebpart
{
    public partial class ToolsWebpartUserControl : UserControl
    {
        #region properties
        public string ToolsContentType
        {
            get;
            set;
        } 
        #endregion

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            toolsList.ItemCreated += new RepeaterItemEventHandler(toolsList_ItemCreated);
            toolsList.ItemCommand += new RepeaterCommandEventHandler(toolsList_ItemCommand);
            try
            {
                if (!Page.IsPostBack)
                {
                    DataView dtResults = GetData();
                    toolsList.DataSource = dtResults;
                    toolsList.DataBind();
                    if (toolsList.Items.Count == 0)
                        ShowNoRecords();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Item Command
        /// <summary>
        /// Item Command event for delete.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        void toolsList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                string[] ListGuidItemID = e.CommandArgument.ToString().Split(':');
                switch(e.CommandName)
                {
                    case "Delete":
                        //code to remove row and save to profile
                        UpdateItemsInProfile(ListGuidItemID[0], ListGuidItemID[1]);
                       // DataTable dtResults = GetData();
                        toolsList.DataSource = GetData();
                        toolsList.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }  
        #endregion

        void ShowNoRecords()
        {
            Controls.Add(new LiteralControl("Go to the <a href=\"/tools-catalog/Pages/tools.aspx\">Tools Catalog</a> and \"like\" your favorite items."));
        }

        #region GetData
        /// <summary>
        /// Gets data using SPSiteDataQuery.
        /// </summary>
        /// <returns></returns>
        protected DataView GetData()
        {
            DataTable dtResults = null;
            DataView dView = null;
            using (SPWeb toolsWeb = SPContext.Current.Site.OpenWeb("/tools-catalog"))
            {
                SPSiteDataQuery Query = new SPSiteDataQuery();
                Query.Lists = "<Lists ServerTemplate=\"100\" />";
                //"<Lists BaseType=\"1\" />";
                Query.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"ID\" /><FieldRef Name=\"_Comments\" /><FieldRef Name=\"ContentTypeId\" /><FieldRef Name='MPD_x0020_Classes' /><FieldRef Name='IEPD_x0020_Lifecycle_x0020_Phase' /><FieldRef Name='Artifacts_x0020_Produced' /><FieldRef Name='EMail' /><FieldRef Name='EncodedAbsUrl' /><FieldRef Name='URL' /><FieldRef Name='FileRef' /><FieldRef Name='Latest_x0020_Verison' /><FieldRef Name='Created' /><FieldRef Name='AverageRating' nullable='true'/>";
                //content type & property
                Query.Query = "<Where><Eq><FieldRef Name='ContentType' /><Value Type='Computed'>" + ToolsContentType + "</Value></Eq></Where>";
                Query.Webs = "<Webs Scope=\"Recursive\" />";
                dtResults = toolsWeb.GetSiteData(Query);
                dtResults = GetLikedTools(dtResults);
                LogText("dtResults1:"+ dtResults.Rows.Count.ToString());
                dtResults = AddDisplayLinks(dtResults);
                LogText("dtResults2:" + dtResults.Rows.Count.ToString());

                dtResults = GetUserTools(dtResults);
                LogText("dtResults3:" + dtResults.Rows.Count.ToString());

                dView = new DataView(dtResults);
                dView.Sort = "Created desc";
            }
            return dView;
        }
        #endregion

        void LogText(string message)
        {
           // System.IO.File.AppendAllText("C:\\temp\\log.txt", message + "\r\n");
        }
        protected string CheckNull(object value)
        {
            try
            {
                return string.IsNullOrEmpty(value.ToString()) ? "" : "version <strong>" + value.ToString() + "</strong>";
            }
            catch (Exception) { return string.Empty; }

        }
        DataTable GetLikedTools(DataTable dTable)
        {
            dTable.Rows.Clear();
            if (SPContext.Current.Web.CurrentUser == null || string.IsNullOrEmpty(SPContext.Current.Web.CurrentUser.LoginName))
            {


            }
            else
            {
                try
                {
                    
                    {
                        string siteURL = HttpContext.Current.Request.Url.AbsoluteUri;
                        using (SPSite site = new SPSite(siteURL))
                        {
                            using (SPWeb web = site.RootWeb)
                            {
                                SPList list = web.Lists["LikeStatus"];
                                SPQuery query = new SPQuery();

                                query.Query = "<Where><And>" +
                                                    "<Eq><FieldRef Name='CType'/><Value Type='Text'>Tools</Value></Eq>" +
                                                    "<Eq><FieldRef Name='SPUser'/><Value Type='Text'>" + SPContext.Current.Web.CurrentUser.LoginName + "</Value></Eq>" +
                                               "</And></Where>";


                                SPListItemCollection items = list.GetItems(query);
                                LogText("Liked Tools:" + items.Count.ToString());
                                foreach (SPListItem item in items)
                                {
                                    try
                                    {
                                        using (SPWeb itemWeb = site.OpenWeb(new Guid(item["WebID"].ToString())))
                                        {
                                            SPList itemList = itemWeb.Lists[new Guid(item["ListID"].ToString())];
                                            SPItem toolsItem = itemList.GetItemById(int.Parse(item["ItemID"].ToString()));
                                            
                                            DataRow dRow = dTable.NewRow();
                                            try{dRow["Title"] = toolsItem["Title"].ToString(); }catch(Exception){}
                                            try{dRow["ID"] = toolsItem["ID"].ToString();}catch(Exception){}
                                            try{dRow["_Comments"] = toolsItem["_Comments"].ToString();}catch(Exception){}
                                            try{dRow["ContentTypeId"] = toolsItem["ContentTypeId"].ToString();}catch(Exception){}
                                            try{dRow["MPD_x0020_Classes"] = toolsItem["MPD_x0020_Classes"].ToString();}catch(Exception){}
                                            try{dRow["IEPD_x0020_Lifecycle_x0020_Phase"] = toolsItem["IEPD_x0020_Lifecycle_x0020_Phase"].ToString();}catch(Exception){}
                                            try{dRow["Artifacts_x0020_Produced"] = toolsItem["Artifacts_x0020_Produced"].ToString();}catch(Exception){}
                                            try{dRow["EMail"] = toolsItem["EMail"].ToString();}catch(Exception){}
                                            try{dRow["EncodedAbsUrl"] = toolsItem["EncodedAbsUrl"].ToString();}catch(Exception){}
                                            try{dRow["FileRef"] = toolsItem["FileRef"].ToString(); }catch(Exception){}
                                            try{dRow["WebId"] =item["WebID"].ToString();}catch(Exception){}
                                            try{dRow["ListId"] = item["ListID"].ToString();}catch(Exception){}
                                            try{dRow["ID"] = item["ItemID"].ToString();}catch(Exception){}
                                            try{dRow["ContentTypeId"] = toolsItem["ContentTypeId"].ToString();}catch(Exception){}
                                            try{dRow["Created"] = toolsItem["Created"].ToString();}catch(Exception){}
                                            try { dRow["AverageRating"] = toolsItem["AverageRating"]; }
                                            catch (Exception) { }
                                            try { dRow["URL"] = toolsItem["URL"]; }
                                            catch (Exception) { }
                                            try { dRow["Latest_x0020_Verison"] = toolsItem["Latest_x0020_Verison"]; }
                                            catch (Exception) { }
                                            

                                            dTable.Rows.Add(dRow);


                                        }
                                    }

                                    catch (Exception ex)
                                    { LogText(ex.Message); }

                                }
                                

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }

            
            dTable.Columns.Add("DateDiff", typeof(int));
            foreach (DataRow dRow in dTable.Rows)
            {
                
                try
                {
                    DateTime itemTime = DateTime.Parse(dRow["Created"].ToString());
                    dRow["DateDiff"] = DateTime.Now.Subtract(itemTime).Days;
                }
                catch (Exception)
                {
                    dRow["DateDiff"] = "999";
                }
            }
            return dTable;//.DefaultView.ToTable(true,{""}));
        }
        #region GetUserTools
        /// <summary>
        /// Gets the tools for the user in the user and removes them from the data table.
        /// </summary>
        /// <param name="dtResults"></param>
        /// <returns></returns>
        protected DataTable GetUserTools(DataTable dtResults)
        {

            ////opens the user profile information
            //Microsoft.SharePoint.SPServiceContext serviceContext = Microsoft.SharePoint.SPServiceContext.Current;
            //UserProfileManager upm = new Microsoft.Office.Server.UserProfiles.UserProfileManager(serviceContext);
            //UserProfile CurrentUser = upm.GetUserProfile(true);

            ////grabs Established Communities and Audiences user is interested in
            //ProfileValueCollectionBase values = CurrentUser.GetProfileValueCollection("UserTools");
            //foreach (string value in values)
            //{
            //    string[] ListGuidID = value.Split(':');
            //    for (int i = dtResults.Rows.Count - 1; i >= 0; i--)
            //    {
            //        DataRow Row = dtResults.Rows[i];
            //        if (ListGuidID[0] == Row["ListId"].ToString() && ListGuidID[1].ToString() == Row["ID"].ToString())
            //        {
            //            dtResults.Rows.Remove(Row);
            //        }
            //    }
            //}
            return dtResults;
        } 
        #endregion

        #region AddDisplayLinks
        /// <summary>
        /// Gets the display links for each item.
        /// </summary>
        /// <param name="dtResults"></param>
        /// <returns></returns>
        protected DataTable AddDisplayLinks(DataTable dtResults)
        {
            dtResults.Columns.Add("LinkField");
            foreach (DataRow Row in dtResults.Rows)
            {
                SPSite Site = SPContext.Current.Site;
                Guid WebGuid = new Guid(Row["WebId"].ToString());
                SPWeb Web = Site.OpenWeb(WebGuid);
                Guid ListGuid = new Guid(Row["ListId"].ToString());
                SPList List = Web.Lists[ListGuid];
                Row["LinkField"] = Web.Url+"/"+List.RootFolder.Url+"/DispForm.aspx?ID="+Row["ID"].ToString()+"&ContentTypeID="+Row["ContentTypeId"].ToString()+"&IsDlg=1";
            }
            return dtResults;
        } 
        #endregion

        #region UpdateItems in Profile
        /// <summary>
        /// Updates the profile data.
        /// </summary>
        protected void UpdateItemsInProfile(string ListGuid, string ID)
        {
            HttpContext currentContext = HttpContext.Current;
            try
            {
                SPSite Site = SPContext.Current.Site;
                string LoginName = SPContext.Current.Web.CurrentUser.LoginName;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite ProfileSite = new SPSite(Site.ID))
                    {
                        //fixes the context issue in SharePoint 2010 and 2007 when elevating privileges
                        //remember to set context back in finally statement
                        HttpContext.Current = null;
                        Microsoft.SharePoint.SPServiceContext serviceContext = SPServiceContext.GetContext(ProfileSite);
                        UserProfileManager upm = new Microsoft.Office.Server.UserProfiles.UserProfileManager(serviceContext);
                        UserProfile profile = upm.GetUserProfile(LoginName);
                        ProfileValueCollectionBase values = profile.GetProfileValueCollection("UserTools");
                        values.Add(ListGuid + ":" + ID);
                        profile.Commit();
                    }
                });
            }
            catch (Exception ex)
            {
            }
            finally
            {
                HttpContext.Current = currentContext;
            }
        } 
        #endregion

        #region ItemCreated
        /// <summary>
        /// Creates the ratings control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void toolsList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
           /* PlaceHolder ratingsPlaceHolder = ((PlaceHolder)e.Item.FindControl("ratingsPlaceHolder"));
            DataRowView drv = (DataRowView)e.Item.DataItem;
            int ID = Convert.ToInt32(drv["ID"].ToString());
            Guid ListGuid = new Guid(drv["ListId"].ToString());
            Guid WebGuid = new Guid(drv["WebId"].ToString());
            SPSite Site = SPContext.Current.Site;
            SPWeb Web = Site.OpenWeb(WebGuid);
            SPList List = Web.Lists[ListGuid];
            SPField Field = List.Fields.GetFieldByInternalName("AverageRating");

            AverageRatingFieldControl avgRatings = new AverageRatingFieldControl();
            avgRatings.ItemContext = SPContext.GetContext(HttpContext.Current, ID, ListGuid, Web);
            avgRatings.ListId = ListGuid;
            avgRatings.ItemId = ID;
            avgRatings.ControlMode = SPControlMode.Display;
            avgRatings.FieldName = "AverageRating";

            ratingsPlaceHolder.Controls.Add(avgRatings);*/
        } 
        #endregion

        
public string stripSpecialChar(string longChar)
{
	string cleanString = "";
	string[] allChars = longChar.Split('#');
    if (allChars.Length>1){
	for(int i=0; i<allChars.Length; i++)
           {
            	cleanString += allChars[i];
           }
	}
    if (cleanString.Length > 2)
    {
        return cleanString.Substring(1, cleanString.Length - 2);
    }
    else {
        return cleanString;
    }
}

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
    }
}
