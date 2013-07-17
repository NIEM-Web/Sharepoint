using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Web;

namespace Niem.MyNiem.Webparts.SaveSearch
{
    public partial class SaveSearchUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSaveSearch.OnClientClick = "javascript:return ValidateTextBox('" + txtName.ClientID + "');";
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["saved"]))
                {
                    lblSearchMessage.Visible = false;
                    lnkBack.Visible = true;
                    showDelete();
                    
                }

            }
            catch (Exception)
            { }
        }

        void showDelete()
        {
            try
            {
                int itemid = int.Parse(Request.QueryString["saved"]);
                SPUser currentUser = SPContext.Current.Web.CurrentUser;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    try
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        using (SPWeb myNiemWeb = site.OpenWeb("/myniem"))
                        {
                            SPList savedSearchList = myNiemWeb.GetList("/myniem/Lists/SavedSearches");
                            SPListItem savedItem = savedSearchList.GetItemById(itemid);
                            SPFieldBoolean boolField = savedItem.Fields["AllUsers"] as SPFieldBoolean;
                            if ((bool)boolField.GetFieldValue(savedItem["AllUsers"].ToString()))
                            {
                                lnkDelete.Visible = false;
                            }
                            else
                                lnkDelete.Visible = true;


                        }
                        
                    }
                    catch (Exception)
                    {
                    }
                });

            }
            catch (Exception) { }


        }


        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int itemid =int.Parse(Request.QueryString["saved"]);
                SPUser currentUser = SPContext.Current.Web.CurrentUser;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    try
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        using (SPWeb myNiemWeb = site.OpenWeb("/myniem"))
                        {
                            SPList savedSearchList = myNiemWeb.GetList("/myniem/Lists/SavedSearches");
                            SPListItem savedItem = savedSearchList.GetItemById(itemid);
                            savedItem.Delete();
                            savedSearchList.Update();
                            
                            
                        }
                        Response.Redirect("/myniem/Pages/saved-searches.aspx");
                    }
                    catch (Exception ex)
                    {
                    }
                });

            }
            catch(Exception){}


        }

        public void btnSaveSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                SPUser currentUser = SPContext.Current.Web.CurrentUser;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    try
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        using (SPWeb myNiemWeb = site.OpenWeb("/myniem"))
                        {
                            SPList savedSearchList = myNiemWeb.GetList("/myniem/Lists/SavedSearches");
                            SPListItem newItem = savedSearchList.AddItem();
                            newItem["Title"] = txtName.Text.Trim();
                            newItem["SearchURL"] = HttpContext.Current.Request.Url.PathAndQuery;
                            newItem["User"] = currentUser;
                            newItem.Update();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                });
            }
        }
    }
}
