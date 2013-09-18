using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint.Administration.Claims;


namespace MyNiemProviderWebparts.Webparts.MyNiemUserProfile
{
    public delegate void _OnProfileChange(string fname, string lname, string email, string org, string orgtype, string position);

    [ToolboxItemAttribute(false)]
    public class MyNiemUserProfile : WebPart
    {
        /// <summary>
        /// The account name to retrieve the user profile, like DOMAIN\USERNAME 
        /// </summary>
        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true), WebDisplayName("User profile account name"), WebDescription("The account name to retrieve the user profile, like DOMAIN\\USERNAME"), Microsoft.SharePoint.WebPartPages.SPWebCategoryName("User Profile Settings")]
        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }
        private string _accountName = "CONTOSO\\aaronp";
        public string Mode
        {
            get;
            set;
        }

        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/MyNiemProviderWebparts.Webparts/MyNiemUserProfile/MyNiemUserProfileUserControl.ascx";

        /// <summary>
        /// Retrieves the user profile manager to get user profile for accountname and render table.
        /// </summary>
        protected override void CreateChildControls()
        {
            SPClaimProviderManager mgr = SPClaimProviderManager.Local;
            if (mgr != null)
            {


                if (SPContext.Current.Web.CurrentUser != null)
                {
                    if (string.Compare(SPContext.Current.Web.CurrentUser.LoginName, @"Sharepoint\system", true) != 0)
                    {
                        SPClaim userLoginClaim = mgr.DecodeClaim(SPContext.Current.Web.CurrentUser.LoginName);
                        SPOriginalIssuerType issuerType = SPOriginalIssuers.GetIssuerType(userLoginClaim.OriginalIssuer);
                        bool readOnly = false;
                        if (issuerType == SPOriginalIssuerType.Forms)
                        {
                            readOnly = false;
                            /*ctrlUserProfile control = (ctrlUserProfile)Page.LoadControl(_ascxPath);
                            control.OnProfileChange = new _OnProfileChange(ChangeProfile);
                            Controls.Add(control);*/

                            //if (Mode != "EDIT")
                            //  DisplayProfile();

                        }
                        else
                        {
                            readOnly = true;
                            // Controls.Add(new Label() { Text = "Active Directory users are not allowed to make modifications to their accounts!" });
                        }

                        MyNiemUserProfileUserControl control = (MyNiemUserProfileUserControl)Page.LoadControl(_ascxPath);
                        //control.OnProfileChange = new _OnProfileChange(ChangeProfile);
                        control.ReadOnly = false; //readOnly;
                        Controls.Add(control);

                    }
                }
            }
        }


        void ChangeProfile(string fname, string lname, string email, string org, string orgtype, string position)
        {
            HttpContext tempCtx = HttpContext.Current;
            try
            {
                SPSite Site = SPContext.Current.Site;

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite ProfileSite = new SPSite(Site.ID))
                    {
                        //SPIisSettings settings = SPContext.Current.Site.WebApplication.IisSettings[SPContext.Current.Site.Zone];
                        HttpContext.Current = null;
                        Microsoft.SharePoint.SPServiceContext serviceContext = SPServiceContext.GetContext(ProfileSite);
                        UserProfileManager upm = new Microsoft.Office.Server.UserProfiles.UserProfileManager(serviceContext);


                        UserProfile profile = upm.GetUserProfile(Site.RootWeb.CurrentUser.LoginName); //_accountName);
                        /*    MembershipProvider prov=  System.Web.Security.Membership.Providers[settings.FormsClaimsAuthenticationProvider.MembershipProvider];
                            MembershipUser usr = prov.GetUser(SPContext.Current.Web.CurrentUser.LoginName, false);
                            if (usr != null)
                            {
                                usr.Email = email;
                           
                            
                                System.Web.Security.Membership.UpdateUser(usr);
                            }
                            */
                        //profile["Title"].Value = fname + " " + lname;
                        profile["FirstName"].Value = fname;
                        profile["LastName"].Value = lname;
                        profile["WorkEmail"].Value = email;
                        profile["Department"].Value = org;
                        profile["OrganizationType"].Value = orgtype;
                        profile["Position"].Value = position;

                        profile.Commit();
                    }


                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                HttpContext.Current = tempCtx;
            }

        }

        /// <summary>
        /// Get Current Logon user of the site
        /// </summary>
        /// <returns>Current User Login Name</returns>
        private string getCurrentUser()
        {

            SPUser sp_user = SPContext.Current.Web.CurrentUser;
            return sp_user.LoginName;

        }

        TextBox txtFirstname = new TextBox();
        TextBox txtLastname = new TextBox();
        TextBox txtOrganisation = new TextBox();
        TextBox txtEMail = new TextBox();
        Table tbl = new Table();
        Button btnSubmit = new Button();
        private void createEditProfile()
        {



            try
            {


                TableRow tr;
                TableCell tc;

                /******* Heading ****/
                tr = new TableRow();
                tc = new TableCell();
                tc.ColumnSpan = 2;
                tc.Controls.Add(new Label() { Text = "" + CurrentUser["UserName"].Value.ToString() });
                tr.Controls.Add(tc);
                tbl.Controls.Add(tr); //First row

                /******* First Name Label****/
                tr = new TableRow();
                tc = new TableCell();
                tc.Controls.Add(new Label() { Text = "First Name" });
                tr.Controls.Add(tc);

                /************ First Name Textbox ********/
                tc = new TableCell();

                txtFirstname.Text = "" + CurrentUser["FirstName"].Value;
                tc.Controls.Add(txtFirstname);
                tr.Controls.Add(tc);

                tbl.Controls.Add(tr); //First row


                /******* Last Name Label****/
                tr = new TableRow();
                tc = new TableCell();
                tc.Controls.Add(new Label() { Text = "Last Name" });
                tr.Controls.Add(tc);

                /************ Last Name Textbox ********/
                tc = new TableCell();
                //txtLastname = new TextBox();
                txtLastname.Text = "" + CurrentUser["LastName"].Value;
                tc.Controls.Add(txtLastname);
                tr.Controls.Add(tc);

                tbl.Controls.Add(tr); //Second Row



                /******* Email Label****/
                tr = new TableRow();
                tc = new TableCell();
                tc.Controls.Add(new Label() { Text = "Email" });
                tr.Controls.Add(tc);

                /************ Email Textbox ********/
                tc = new TableCell();
                //txtEMail = new TextBox();
                txtEMail.Text = "" + CurrentUser["WorkEmail"].Value;
                tc.Controls.Add(txtEMail);
                tr.Controls.Add(tc);

                tbl.Controls.Add(tr); //Third row


                /******* Organisation Label****/
                tr = new TableRow();
                tc = new TableCell();
                tc.Controls.Add(new Label() { Text = "Organisation" });
                tr.Controls.Add(tc);

                /************ Organisation Textbox ********/
                tc = new TableCell();
                //txtOrganisation = new TextBox();
                txtOrganisation.Text = "" + CurrentUser["Department"].Value;
                tc.Controls.Add(txtOrganisation);
                tr.Controls.Add(tc);

                tbl.Controls.Add(tr); //Fourth row                  

                /************ Organisation Textbox ********/
                tr = new TableRow();
                tc = new TableCell();
                //btnSubmit = new Button();
                btnSubmit.Text = "Submit";


                tc.Controls.Add(btnSubmit);
                btnSubmit.Click += new EventHandler(btnSubmit_Click);
                tr.Controls.Add(tc);
                tbl.Controls.Add(tr); //fifth row    
                this.Controls.Add(tbl);

                /**** LOAD UP **/
                txtFirstname.Text = "" + CurrentUser["FirstName"].Value;
                txtLastname.Text = "" + CurrentUser["LastName"].Value;
                txtEMail.Text = "" + CurrentUser["WorkEmail"].Value;
                txtOrganisation.Text = "" + CurrentUser["Department"].Value;


                Mode = "EDIT";



            }
            catch
            {
            }
            finally
            {
                //System.Security.CodeAccessPermission.RevertAssert();
            }



        }

        void btnSubmit_Click(object sender, EventArgs e)
        {

            CurrentUser["FirstName"].Value = txtFirstname.Text.Trim();
            CurrentUser["LastName"].Value = txtLastname.Text.Trim();
            CurrentUser["WorkEmail"].Value = txtEMail.Text.Trim();
            CurrentUser["Department"].Value = txtOrganisation.Text.Trim();

            CurrentUser.Commit();
            Mode = "DSP";
            this.Controls.Clear();
            CreateChildControls();
            // this.DisplayProfile();


        }
        UserProfile usr = null;
        private UserProfile CurrentUser
        {
            get
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    if (usr == null)
                    {
                        Microsoft.SharePoint.SPServiceContext serviceContext = Microsoft.SharePoint.SPServiceContext.Current;
                        UserProfileManager upm = new Microsoft.Office.Server.UserProfiles.UserProfileManager(serviceContext);
                        //ProfileSubtypePropertyManager pspm = upm.DefaultProfileSubtypeProperties;


                        usr = upm.GetUserProfile(true); //_accountName);
                    }
                    //else if( usr.SID = SPContext.Current.Web.CurrentUser.
                });
                return usr;



            }
        }
        Button btnEdit;
        private void DisplayProfile()
        {
            try
            {
                System.Security.PermissionSet ps = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
                ps.Assert();

                Microsoft.SharePoint.SPServiceContext serviceContext = Microsoft.SharePoint.SPServiceContext.Current;
                UserProfileManager upm = new Microsoft.Office.Server.UserProfiles.UserProfileManager(serviceContext);
                ProfileSubtypePropertyManager pspm = upm.DefaultProfileSubtypeProperties;

                UserProfile profile = upm.GetUserProfile(true); //_accountName);

                this.Controls.Add(new Literal() { Text = "<table border='0' > <tr><td>Displayname</td><td>Value</td></tr>" });

                string[] props = { "UserName", "FirstName", "LastName", "WorkEmail", "Department" };

                foreach (string prop in props)
                {

                    string text = string.Format("<tr><td>{0}</td><td>{1}</td></tr>",
                           profile[prop].ProfileSubtypeProperty.DisplayName,
                           profile[prop].Value);

                    this.Controls.Add(new Literal() { Text = text });

                }
                string txtEdit = "<tr><td colspan='2'>";
                btnEdit = new Button() { Text = "Edit" };

                this.Controls.Add(new Literal() { Text = txtEdit });
                this.Controls.Add(btnEdit);
                this.Controls.Add(new Literal() { Text = "</td></tr>" });
                btnEdit.Click += new EventHandler(btnEdit_Click);


                /*
                foreach (ProfileSubtypeProperty prop in pspm.PropertiesWithSection)
                {

                    if (prop.IsSection)
                        this.Controls.Add(new Literal() { Text = string.Format("<tr><td colspan='3'><b>Section: {0}</b></td></tr>", prop.DisplayName) });
                    else
                    {
                        string text = string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",
                            prop.DisplayName,
                            prop.Name,
                            profile[prop.Name].Value);

                        this.Controls.Add(new Literal() { Text = text });
                    }
                }*/
                this.Controls.Add(new Literal() { Text = "</table>" });
            }
            catch (Exception ex)
            {
                this.Controls.Add(new Literal() { Text = ex.ToString() });
            }
            finally
            {
                System.Security.CodeAccessPermission.RevertAssert();
            }

        }

        void btnEdit_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            Mode = "EDIT";
            CreateChildControls();
            this.createEditProfile();


        }
    }
}
