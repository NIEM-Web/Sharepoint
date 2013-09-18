using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.Office.Server.UserProfiles;
using System.Collections.Generic;
using System.Linq;

namespace lmd.NIEM.FarmSolution.ExportUsersToExcel
{
    public partial class ExportUsersToExcelUserControl : UserControl
    {
        private const string DisplayMessage = "Showing {0} of {1}";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (SPContext.Current.Web.CurrentUser != null)
                {
                    List<UserDetail> userDetails = Utility.GetUserDetails();

                    List<UserDetail> trimmedUserDetails = userDetails.Take(5).ToList();

                    if (trimmedUserDetails.Count > 0)
                    {
                        gvUsers.DataSource = trimmedUserDetails;
                        gvUsers.DataBind();
                        lblMessage.Text = string.Format(DisplayMessage, trimmedUserDetails.Count, userDetails.Count);
                    }
                }
                else
                {
                    this.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }

        }
    }
}
