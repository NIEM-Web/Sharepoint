using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;
using System.Web;

namespace lmd.NIEM.FarmSolution.ControlTemplates
{
    public partial class UserStatusLink : UserControl
    {
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
                        //LogText("Current UserId:" + userID);
                        using (SPWeb rootWeb = new SPSite(currentSite).RootWeb)
                        {
                            rootWeb.AllowUnsafeUpdates = true;
                            SPList profiles = rootWeb.Lists["Profiles"];
                            SPQuery query = new SPQuery();
                            query.Query = "<Where><Eq><FieldRef Name='SPUser' LookupId= 'TRUE'  />" +
                                            "<Value Type='User'>" + userID + "</Value>" + "</Eq></Where>";
                            SPListItemCollection items = profiles.GetItems(query);
                            SPFieldChoice orgTypes = (SPFieldChoice)profiles.Fields["OrgType"];

                            //LogText("query:" + query.Query);
                            //LogText("Users count:" + items.Count.ToString());
                            if (items.Count > 0)
                                usr = items[0];
                            else
                            {
                                usr = null;
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
        string GetFullNameFromProfile()
        {
            
            string FullName = string.Empty;
            try
            {
                System.Security.PermissionSet ps = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
                ps.Assert();

                Microsoft.SharePoint.SPServiceContext serviceContext = Microsoft.SharePoint.SPServiceContext.Current;
                UserProfileManager upm = new Microsoft.Office.Server.UserProfiles.UserProfileManager(serviceContext);
                ProfileSubtypePropertyManager pspm = upm.DefaultProfileSubtypeProperties;

                UserProfile profile = upm.GetUserProfile(true);
                FullName = profile["FirstName"].Value + " " + profile["LastName"].Value;
            }
            catch (Exception) { }
            return FullName;
        }
        string GetUserFullName(string siteURL, int id, string loginName)
        {
            string strFullName = string.Empty;
            SPSecurity.RunWithElevatedPrivileges(delegate() 
            { 
              using (SPSite site = new SPSite(siteURL))
              {
                  try
                  {
                      ServerContext serverContext = ServerContext.GetContext(SPContext.Current.Site);
                      UserProfileManager profileManager = new UserProfileManager(serverContext);
                      SPUser spUser = site.RootWeb.Users.GetByID(id);
                      UserProfile profile = profileManager.GetUserProfile(spUser.LoginName);
                      string firstName = profile["FirstName"].Value.ToString();
                      string LastName = profile["LastName"].Value.ToString();
                      strFullName = firstName + " " + LastName;
                  }
                  catch (Exception) {
                      string[] nameparts = loginName.Split('|');
                      strFullName = nameparts[nameparts.Length - 1];

                  }
              } 
            });
            return strFullName;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SPContext.Current.Web.CurrentUser == null) //User is anonymous
            {
                loginUtilAnony.Visible = true;
                loginUtil.Visible = false;

            }
            else
            {
                loginUtil.Visible = true;
                loginUtilAnony.Visible = false;
                string FullName =GetFullNameFromProfile();//GetUserFullName(SPContext.Current.Site.Url,SPContext.Current.Web.CurrentUser.ID,SPContext.Current.Web.CurrentUser.LoginName);
                if(string.IsNullOrEmpty(FullName.Trim()))
                {
                        string[] nameparts = SPContext.Current.Web.CurrentUser.LoginName.Split('|');
                        FullName = nameparts[nameparts.Length - 1];
                 }
                lblUserName.Text = FullName;
                bool showProfileEdit = false;
                try
                {
                    if (string.IsNullOrEmpty(CurrentUser["JobTitle"].ToString().Trim()))
                    {
                        showProfileEdit = true;
                    }
                }
                catch (Exception) { showProfileEdit = true; }

                if (showProfileEdit)
                    showProfileScript.Visible = true;
                try
                {
                    bool inGroup = false;
                    foreach (SPGroup grp in SPContext.Current.Web.CurrentUser.Groups)
                    {
                        if (grp.Name.Contains("NIEM Committee"))
                        {
                            inGroup = true;
                            break;
                        }
                    }
                    if (inGroup)
                    {
                        hlCommittees.Visible = true;
                    }
                    else
                    {
                        hlCommittees.Visible = false;
                        lblDiv.Visible = false;
                    }
                }
                catch (Exception) { hlCommittees.Visible = lblDiv.Visible = false; }
            }

        }
    }
}
