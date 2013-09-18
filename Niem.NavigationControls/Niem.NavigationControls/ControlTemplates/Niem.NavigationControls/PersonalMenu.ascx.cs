using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;


namespace Niem.NavigationControls.ControlTemplates.Niem.NavigationControls
{
    public partial class PersonalMenu : UserControl
    {
        #region Properties
        //properties
        private string _MyNiemListName;
        private string _ErrorLog;

        public string MyNiemListName
        {
            get
            {
                return _MyNiemListName;
            }
            set
            {
                _MyNiemListName = "MyNiem List";
            }
        }

        public string ErrorLog
        {
            get
            {
                return _ErrorLog;
            }
            set
            {
                _ErrorLog = "Error Log List";
            }
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// Grabs the list information and renders the menu based on access.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    SPWeb Web = SPContext.Current.Site.RootWeb;
                    SPList List = Web.Lists[MyNiemListName];
                    LoadNavigation(List);
                }
            }
            catch (SPException ex)
            {
                UpdateErrorLog(ex);
            }
        } 
        #endregion

        #region LoadNavigation
        /// <summary>
        /// Loops through the list items in the MyNiem List and gets the navigation based on access.
        /// </summary>
        /// <param name="List"></param>
        protected void LoadNavigation(SPList List)
        {
            try
            {
                if (Page.User.Identity.IsAuthenticated)
                {
                    SPListItemCollection Items = List.Items;
                    var PersonalNav = LoginView1.FindControl("PersonalNav") as AspMenu;
                    MenuItemCollection MenuItems = new MenuItemCollection();

                    //grabs the items in the list then loops through the webs the user currently has access too
                    foreach (SPListItem Item in Items)
                    {
                        //old code to recursively check if the user has access to the subweb
                        //if (Web.Url.ToLower() == SPContext.Current.Web.Site.RootWeb.Url.ToLower() + "/" + Item["Title"].ToString().ToLower())
                        //    FoundWeb = true;
                        //else
                        //    FoundWeb = GetSubWebs(Web, SPContext.Current.Web.Site.RootWeb.Url.ToLower() + "/" + Item["Title"].ToString().ToLower(), FoundWeb);
                        //Guid SiteGuid = SPContext.Current.Site.ID;
                        string SiteURL = SPContext.Current.Web.Site.RootWeb.Url.ToLower();
                        SPUser CurrentUser = SPContext.Current.Web.CurrentUser;
                        SPSecurity.RunWithElevatedPrivileges(delegate()
                        {
                            #region OldCode
                            //    if (FoundWeb)
                            //    {
                            //        using (SPSite Site = new SPSite(SPContext.Current.Web.Site.RootWeb.Url.ToLower() + "/" + Item["Title"].ToString()))
                            //        {
                            //            using (SPWeb FoundSubWeb = Site.OpenWeb())
                            //            {
                            //                MenuItem MyNiemItem = new MenuItem(FoundSubWeb.Title, FoundSubWeb.Title, "", FoundSubWeb.Url);
                            //                int CompareNumber = 0;
                            //                for(int i=0;i<MenuItems.Count && CompareNumber>=0;i++)
                            //                {
                            //                    string CompareItem = MyNiemItem.Text;
                            //                    CompareNumber = CompareItem.CompareTo(MenuItems[i].Text);
                            //                    if(CompareNumber < 0)
                            //                        MenuItems.AddAt(i, MyNiemItem);
                            //                }
                            //                if(CompareNumber>=0)
                            //                    MenuItems.Add(MyNiemItem);
                            //                FoundWeb = false;
                            //            }
                            //        }
                            //    }
                            //}
                            //foreach (MenuItem Item in MenuItems)
                            //{
                            //    PersonalNav.Items.Add(Item);
                            //} 
                            #endregion

                            //new code to check against site
                            using (SPSite Site = new SPSite(SiteURL + "/" + Item["Title"].ToString()))
                            {
                                using (SPWeb Web = Site.OpenWeb())
                                {
                                    if (Web.DoesUserHavePermissions(CurrentUser.LoginName, SPBasePermissions.Open))
                                    {
                                        MenuItem MyNiemItem = new MenuItem(Web.Title, Web.Title, "", Web.Url);
                                        int CompareNumber = 0;
                                        for (int i = 0; i < MenuItems.Count && CompareNumber >= 0; i++)
                                        {
                                            string CompareItem = MyNiemItem.Text;
                                            CompareNumber = CompareItem.CompareTo(MenuItems[i].Text);
                                            if (CompareNumber < 0)
                                                MenuItems.AddAt(i, MyNiemItem);
                                        }
                                        if (CompareNumber >= 0)
                                            MenuItems.Add(MyNiemItem);
                                    }
                                }
                            }

                        });
                    }
                    foreach (MenuItem Item in MenuItems)
                    {
                        PersonalNav.Items.Add(Item);
                    } 
                }
            }
            catch (SPException ex)
            {
                UpdateErrorLog(ex);
            }
        }
        #endregion

        //#region GetSubWebs
        ///// <summary>
        ///// Rescurses through all the subwebs.
        ///// </summary>
        ///// <param name="Web"></param>
        ///// <param name="Title"></param>
        ///// <param name="FoundWeb"></param>
        ///// <returns></returns>
        //private bool GetSubWebs(SPWeb Web, string Title, bool FoundWeb)
        //{
        //    foreach (SPWeb SubWeb in Web.Webs)
        //    {
        //        if (SubWeb.Url.ToLower() == Title)
        //            FoundWeb = true;
        //        FoundWeb = GetSubWebs(SubWeb, Title, FoundWeb);
        //    }
        //    return FoundWeb;
        //} 
        //#endregion

        #region UpdateErrorLog
        /// <summary>
        /// Code to update the error log in the event that there is an issue with the control.
        /// </summary>
        /// <param name="ex"></param>
        protected void UpdateErrorLog(SPException ex)
        {
            //Guid RootWebGuid = SPContext.Current.Site.RootWeb.ID;
            //Guid SiteGuid = SPContext.Current.Site.ID;

            //SPSecurity.RunWithElevatedPrivileges(delegate()
            //{
            //    using (SPSite Site = new SPSite(SiteGuid))
            //    {
            //        using (SPWeb Web = Site.OpenWeb(RootWebGuid))
            //        {
            //            SPList List = Web.Lists[ErrorLog];
            //            Web.AllowUnsafeUpdates = true;

            //            //writes to the error list
            //            SPListItem Item = List.Items.Add();
            //            Item["Title"] = "MyNiemControl Error";
            //            Item["Error Code"] = Convert.ToString(ex.ErrorCode);
            //            Item["Error"] = ex.Message;
            //            Item["Stack Trace"] = ex.StackTrace;
            //            Item.Update();


            //            Web.AllowUnsafeUpdates = false;

            //            //writes to the ULS Log
            //            SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory("QuickLaunchControl", TraceSeverity.Unexpected, EventSeverity.Error), TraceSeverity.Unexpected, ex.Message, ex.StackTrace);
            //        }
            //    }
            //});
        }
        #endregion
    }

}
