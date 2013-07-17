using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Data;

namespace Niem.MyNiem.Webparts.SavedSearches
{
    public partial class SavedSearchesUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && SPContext.Current != null && SPContext.Current.Web.CurrentUser != null)
            {
                DataTable searches = GetData();
                if (searches != null && searches.Rows.Count > 0)
                {
                    rptDropDown.DataSource = searches;
                    rptDropDown.DataBind();
                }
            }
        }

        protected DataTable GetData()
        {
            DataTable searches = null;
            SPUser currentUser = SPContext.Current.Web.CurrentUser;
            SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    try
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        using (SPWeb myNiemWeb = site.OpenWeb("/myniem"))
                        {
                            SPList spList = myNiemWeb.GetList("/myniem/Lists/SavedSearches");
                            if (spList != null)
                            {
                                SPQuery qry = new SPQuery();
                                qry.Query =
                                string.Format(@"   <Where>
                                      <Or>
                                         <Eq>
                                                <FieldRef Name='AllUsers' />
                                                <Value Type='Boolean'>1</Value>
                                         </Eq>
                                         <Eq>
                                                <FieldRef Name='User' LookupId='True' />
                                                <Value Type='Integer'>{0}</Value>
                                         </Eq>
                                      </Or>
                                   </Where>", currentUser.ID);
                                qry.ViewFields = "<FieldRef Name='Title' /><FieldRef Name='SearchURL' />";
                                SPListItemCollection listItems = spList.GetItems(qry);
                                searches = listItems.GetDataTable();
                            }
                        }
                    }
                    catch
                    {
                    }
                });
            return searches;
        }
    }
}
