using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.Office.Server.UserProfiles;
using System.Web;

namespace Niem.MyNiem.Webparts.ProfileWebpart
{
    public partial class ProfileWebpartUserControl : UserControl
    {
        #region properties
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

        #region Page_Load
        /// <summary>
        /// Page Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //oncheckbox change events
                cbAudience.SelectedIndexChanged += new EventHandler(cbAudience_SelectedIndexChanged);
                cbEstablishedCommunities.SelectedIndexChanged += new EventHandler(cbEstablishedCommunities_SelectedIndexChanged);
                if (!Page.IsPostBack)
                {
                    //grabs the established communities and audiences from the lists
                    cbAudience.DataSource = GetListData(SPContext.Current.Site.RootWeb, YourAudienceList);
                    cbAudience.DataTextField = "Title";
                    cbAudience.DataValueField = "ID";
                    cbAudience.DataBind();
                    cbEstablishedCommunities.DataSource = GetListData(SPContext.Current.Site.RootWeb, EstablishedCommunitiesList);
                    cbEstablishedCommunities.DataTextField = "Title";
                    cbEstablishedCommunities.DataValueField = "ID";
                    cbEstablishedCommunities.DataBind();


                    //opens the user profile information
                    //Microsoft.SharePoint.SPServiceContext serviceContext = Microsoft.SharePoint.SPServiceContext.Current;
                    //UserProfileManager upm = new Microsoft.Office.Server.UserProfiles.UserProfileManager(serviceContext);
                    //UserProfile CurrentUser = upm.GetUserProfile(true);

                    //gets the profile properties
                    string strFirstName = string.Empty;
                    string strLastName = string.Empty;
                    try { strFirstName = "" + CurrentUser["FirstName"].ToString(); }
                    catch (Exception) { }
                    try { strLastName = "" + CurrentUser["Title"].ToString(); }
                    catch (Exception) { }
                    lblName.Text = strFirstName + " " + strLastName;
                    string strEmail = string.Empty;
                    try { strEmail = "" + CurrentUser["Email"].ToString(); }
                    catch (Exception) { }
                    lblEmail.Text = "<a href='mailto:" + strEmail + "'>" + strEmail + "</a>";
                    try { lblOrganization.Text = "" + CurrentUser["Company"].ToString(); }
                    catch (Exception) { }
                    try { lblOrgType.Text = "" + CurrentUser["OrgType"].ToString(); }
                    catch (Exception) { }
                    try { lblPosition.Text = "" + CurrentUser["JobTitle"].ToString(); }
                    catch (Exception) { }
                                    
                    
                    //grabs Established Communities and Audiences user is interested in
                    //ProfileValueCollectionBase pvbCommunities = CurrentUser.GetProfileValueCollection("EstablishedCommunities");
                    SPFieldLookupValue group;
                    string lookedUpItemTitle = string.Empty;
                    try
                    {
                        group = new SPFieldLookupValue(CurrentUser["EstablishedCommunities"].ToString());
                        lookedUpItemTitle = group.LookupValue;
                        //Response.Write(lookedUpItemTitle);
                        SelectUserCheckboxes(lookedUpItemTitle.Split(';'), cbEstablishedCommunities);
                    }
                    catch (Exception) { }
                    //ProfileValueCollectionBase pvbYourAudience = CurrentUser.GetProfileValueCollection("YourAudience");
                    try
                    {
                        group = new SPFieldLookupValue(CurrentUser["YourAudienceList"].ToString());
                        lookedUpItemTitle = group.LookupValue;
                        SelectUserCheckboxes(lookedUpItemTitle.Split(';'), cbAudience);
                    }
                    catch (Exception) { }
                    //gets urls for modals
                    
                }
                
                

            }
            catch (Exception ex)
            {
            }
            try
            {/*
                Response.Write("<script>");
                Response.Write("var helloUserNameText = \"<em>My</em>NIEM: <span class='myNiemName'>Hello, ");
                try { Response.Write(CurrentUser["FirstName"].ToString() + " "); }
                catch (Exception) { }
                try { Response.Write(CurrentUser["Title"].ToString() + " "); }
                catch (Exception) { }
                Response.Write("</span>\";");
                Response.Write("</script>");*/
            }
            catch (Exception)
            { }
        } 
        #endregion

        #region SelectUserCheckboxes
        //grabs the items selected by the user
        protected void SelectUserCheckboxes(string[] values, CheckBoxList cbList)
        {

            foreach (string value in values)
            {
                try
                {
                    ListItem Item = cbList.Items.FindByValue(value.Replace('#', ' ').Trim());
                    Item.Selected = true;
                }
                catch (Exception) { }
            }
        } 
        #endregion

        #region Selected Index Established Communities
        /// <summary>
        /// Selected index changed for Established Communities.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cbEstablishedCommunities_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SaveUserProfileProperties(cbEstablishedCommunities, "EstablishedCommunities");
            }
            catch (Exception ex)
            {
            }
        } 
        #endregion

        #region Selected Index Audiences
        /// <summary>
        /// Selected index changed for Audiences.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cbAudience_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SaveUserProfileProperties(cbAudience, "YourAudienceList");
            }
            catch (Exception ex)
            {
            }
        } 
        #endregion

        #region SaveUserProfileProperties
        /// <summary>
        /// Saves to user profiles the settings for the user based on checkbox choices.
        /// </summary>
        /// <param name="cbList"></param>
        protected void SaveUserProfileProperties(CheckBoxList cbList, string PropertyName)
        {
            try
            {
                SPFieldLookupValueCollection fieldValues = new SPFieldLookupValueCollection();

                foreach (ListItem Item in cbList.Items)
                {
                    if (Item.Selected)
                        fieldValues.Add(new SPFieldLookupValue(int.Parse(Item.Value),Item.Text));  // add other field values or in loop
                }
                CurrentUser[PropertyName] = fieldValues;
                CurrentUser.Update();
                CurrentUser.ParentList.Update();
            }
            catch (Exception ex) {  }
            //HttpContext currentContext = HttpContext.Current;
            //try
            //{
            //    SPSite Site = SPContext.Current.Site;
            //    string LoginName = SPContext.Current.Web.CurrentUser.LoginName;
            //    SPSecurity.RunWithElevatedPrivileges(delegate()
            //    {
            //        using (SPSite ProfileSite = new SPSite(Site.ID))
            //        {
            //            //fixes the context issue in SharePoint 2010 and 2007 when elevating privileges
            //            //remember to set context back in finally statement
            //            HttpContext.Current = null;
            //            Microsoft.SharePoint.SPServiceContext serviceContext = SPServiceContext.GetContext(ProfileSite);
            //            UserProfileManager upm = new Microsoft.Office.Server.UserProfiles.UserProfileManager(serviceContext);
            //            UserProfile profile = upm.GetUserProfile(LoginName);
            //            ProfileValueCollectionBase values = profile.GetProfileValueCollection(PropertyName);
                       
            //            //add items
            //            bool FoundItem = false;
            //            foreach (ListItem Item in cbList.Items)
            //            {
            //                FoundItem = false;
            //                //checks if item exists in the list already
            //                if (values.Count != 0)
            //                {
            //                    for (int i = 0; i < values.Count; i++)
            //                    {
            //                        if (values[i].ToString() == Item.Value && Item.Selected)
            //                        {
            //                            FoundItem = true;
            //                        }
            //                    }
            //                    if (!FoundItem && Item.Selected)
            //                        values.Add(Item.Value);
            //                }
            //                else if (values.Count == 0 && Item.Selected)
            //                {
            //                    values.Add(Item.Value);
            //                }
            //            }

            //            //delete items
            //            bool Selected = false;
            //            for(int i = values.Count-1; i >= 0; i--)
            //            {
            //                foreach (ListItem Item in cbList.Items)
            //                {
            //                    if (values[i].ToString() == Item.Value && Item.Selected)
            //                    {
            //                        Selected = true;
            //                    }
            //                }
            //                if (Selected != true)
            //                {
            //                    values.RemoveAt(i);
            //                }
            //                else
            //                    Selected = false;
            //            }


            //            profile.Commit();
            //        }
            //    });
            //}
            //catch (Exception ex)
            //{
            //}
            //finally
            //{
            //    HttpContext.Current = currentContext;
            //}
        } 
        #endregion

        #region GetListData
        /// <summary>
        /// Gets the list data.
        /// </summary>
        protected SPListItemCollection  GetListData(SPWeb Web, string Listname)
        {
            //ListItemCollection ListItems = new ListItemCollection();
            SPList List = Web.Lists[Listname];
            SPQuery Query = new SPQuery();
            Query.Query = "<OrderBy><FieldRef Name='Title' /></OrderBy>";
            SPListItemCollection Items = List.GetItems(Query);
            return Items;
            //foreach (SPListItem Item in Items)
            //{
            //    ListItem cbItem = new ListItem(Item["Title"].ToString(), Item.ID.ToString());
            //    ListItems.Add(cbItem);
            //}
            //return ListItems;
        } 
        #endregion
    }
}
