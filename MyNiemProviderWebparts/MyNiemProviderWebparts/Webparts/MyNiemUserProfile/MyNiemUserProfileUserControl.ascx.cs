using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using System.Web;


namespace MyNiemProviderWebparts.Webparts.MyNiemUserProfile
{
    public partial class MyNiemUserProfileUserControl : UserControl
    {
        //public _OnProfileChange OnProfileChange;
        private SPListItem usr;
        public bool ReadOnly { get; set; }

        void LogText(string text)
        {
           // System.IO.File.AppendAllText("C:\\log.txt", text + "\n\r");
        }
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
                        LogText("Current UserId:"+ userID);
                        using (SPWeb rootWeb = new SPSite(currentSite).RootWeb)
                        {
                            rootWeb.AllowUnsafeUpdates = true;
                            SPList profiles = rootWeb.Lists["Profiles"];
                          SPQuery query = new SPQuery();
                            query.Query = "<Where><Eq><FieldRef Name='SPUser' LookupId= 'TRUE'  />" +
                                            "<Value Type='User'>" + userID   + "</Value>" + "</Eq></Where>";  
                            SPListItemCollection items = profiles.GetItems(query);
                            SPFieldChoice orgTypes =(SPFieldChoice) profiles.Fields["OrgType"];
                            
                            LogText("query:" + query.Query);
                            LogText("Users count:" + items.Count.ToString());
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
                                LogText("Got Profile:"+ contextUser.Name);
                                string[] nameparts =contextUser.Name.Split('|');

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
                    LogText(ex.Message);
                }

                return usr;



            }
            set { usr = value; }
        }

        void bindddlOrgType()
        {
            ddlOrgType.Items.Clear();
            string currentSite = HttpContext.Current.Request.Url.AbsoluteUri;
            using (SPWeb web = new SPSite(currentSite).RootWeb)
            {
                SPList profiles = web.Lists["Profiles"];
                SPFieldChoice orgTypes = (SPFieldChoice)profiles.Fields["OrgType"];
                foreach (string choice in orgTypes.Choices)
                {
                    ddlOrgType.Items.Add(choice);
                }

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
                    lblMessage.Text = "";
                    bindddlOrgType();
                    try { txtFirstname.Text = "" + CurrentUser["FirstName"].ToString(); }
                    catch (Exception) { }
                    try { txtLastName.Text = "" + CurrentUser["Title"].ToString(); }
                    catch (Exception) { }
                    try { txtemail.Text = "" + CurrentUser["Email"].ToString(); }
                    catch (Exception) { }
                    try { txtOrg.Text = "" + CurrentUser["Company"].ToString(); }
                    catch (Exception) { }
                    try { ddlOrgType.SelectedValue = "" + CurrentUser["OrgType"].ToString(); }
                    catch (Exception) { }
                    try { txtPosition.Text = "" + CurrentUser["JobTitle"].ToString(); }
                    catch (Exception) { }
                
                
                if (ReadOnly)
                {
                    lblMessage.Text = "Active Directory users are not allowed to make modifications to their accounts!";
                    btnEdit.Enabled = false;

                }
                else btnEdit.Enabled = true;

            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    lblMessage.Text = "";
                    if (ReadOnly)
                    {
                        lblMessage.Text = "Active Directory users are not allowed to make modifications to their accounts!";

                        return;
                    }


                    //if (OnProfileChange != null)
                    //    OnProfileChange(txtFirstname.Text, txtLastName.Text, txtemail.Text, txtOrg.Text, ddlOrgType.SelectedValue,txtPosition.Text);
                });

                try {CurrentUser["FirstName"] = txtFirstname.Text; }
                catch (Exception) { }
                try { CurrentUser["Title"] = txtLastName.Text; }
                catch (Exception) { }
                try { CurrentUser["Email"] = txtemail.Text; }
                catch (Exception) { }
                try { CurrentUser["Company"] =txtOrg.Text; }
                catch (Exception) { }
                try { CurrentUser["OrgType"] = ddlOrgType.SelectedValue; }
                catch (Exception) { }
                try { CurrentUser["JobTitle"] =  txtPosition.Text; }
                catch (Exception) { }
                CurrentUser.Update();
                CurrentUser.ParentList.Update();
                lblMessage.Text = "Profile has been updated.";
            }
            catch (Exception ex)
            {
                this.Controls.Add(new Literal() { Text = ex.ToString() });
            }
            finally
            {
                //System.Security.CodeAccessPermission.RevertAssert();
            }
        }
    }
}
