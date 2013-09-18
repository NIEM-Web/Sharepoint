using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using lmd.NIEM.FarmSolution.NiemUserProfileWebPart;

namespace lmd.NIEM.FarmSolution
{
     
    public partial class ctrlUserProfile : UserControl
    {
        public _OnProfileChange OnProfileChange;
        private UserProfile usr;
        public bool ReadOnly { get; set; }
        public  UserProfile CurrentUser
        {
            get
            {
                if (usr == null)
                {
                 
                    
                    Microsoft.SharePoint.SPServiceContext serviceContext = Microsoft.SharePoint.SPServiceContext.Current;
                    UserProfileManager upm = new Microsoft.Office.Server.UserProfiles.UserProfileManager(serviceContext);
                    //ProfileSubtypePropertyManager pspm = upm.DefaultProfileSubtypeProperties;
                    usr = upm.GetUserProfile(true);
                }

                return usr;



            }
            set { usr = value; }
        }

        void LoadUserProfile()
        {
            string[] namepart = CurrentUser.DisplayName.Split('|'); 
            lblUser.Text = namepart[namepart.Length-1];
            txtFirstname.Text = "" + CurrentUser["FirstName"].Value;
            txtLastName.Text = "" + CurrentUser["LastName"].Value;
            txtemail.Text = "" + CurrentUser["WorkEmail"].Value;
            txtOrg.Text = "" + CurrentUser["Department"].Value;
            profileID.Value = CurrentUser.RecordId.ToString();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblMessage.Text = "";
                LoadUserProfile();

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

                if (btnEdit.Text == "Edit")
                {
                    txtFirstname.ReadOnly =   txtLastName.ReadOnly= txtemail.ReadOnly = txtOrg.ReadOnly= false;
          
                    
                    
                   

                    btnEdit.Text = "Save";
                       // CurrentUser["FirstName"].Value = txtFirstname.Text.Trim();
                   // CurrentUser["LastName"].Value = txtLastName.Text.Trim();
                    //CurrentUser["WorkEmail"].Value = txtEMail.Text.Trim();
                    //CurrentUser["Department"].Value = txtOrganisation.Text.Trim();

                   
                    
                }
                else
                {
                    if (OnProfileChange != null)
                        OnProfileChange(txtFirstname.Text, txtLastName.Text, txtemail.Text,txtOrg.Text, long.Parse(profileID.Value));
                    //txtFirstname.ReadOnly = txtLastName.ReadOnly = txtemail.ReadOnly = txtOrg.ReadOnly = true;
                    //CurrentUser["FirstName"].Value = txtFirstname.Text.Trim();
                    //CurrentUser["LastName"].Value = txtLastName.Text.Trim();
                    //CurrentUser["WorkEmail"].Value = txtEMail.Text.Trim();
                    //CurrentUser["Department"].Value = txtOrganisation.Text.Trim();
                    LoadUserProfile();
                    lblMessage.Text = "Your profile information was successfully updated.";
                    //btnEdit.Text = "Edit";
                }
                    // implementation details omitted
                });
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
