using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Web;

namespace Niem.MyNiem
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:WelcomeCU runat=server></{0}:WelcomeCU>")]
    public class WelcomeCU : Panel
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

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            if (SPContext.Current.Web.Title == "MyNiem")
            {
                try
                {
                    if (CurrentUser != null)
                    {
                        try
                        {
                            Controls.Add(new LiteralControl(CurrentUser["FirstName"].ToString()));
                        }
                        catch (Exception) { }
                        Controls.Add(new LiteralControl(" "));
                        try
                        {
                            Controls.Add(new LiteralControl(CurrentUser["Title"].ToString()));
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception) { }
            }

        }
    }

    
}
