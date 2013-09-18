using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Navigation;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Navigation;
using System.Web;

namespace Niem.NavigationControls.ControlTemplates.Niem.NavigationControls
{
    public partial class QuickLaunchCustomization : UserControl
    {
        #region Properties
        //properties
        private string _SettingsListName;
        private string _AllowOldNav;
        private string _AddStaticBottom;
        private string _ErrorLog;

        public string SettingsListName
        {
            get
            {
                return _SettingsListName;
            }
            set
            {
                _SettingsListName = "Settings List";
            }
        }

        public string AllowOldNav
        {
            get
            {
                return _AllowOldNav;
            }
            set
            {
                _AllowOldNav = "AllowOldNav";
            }
        }

        public string AddStaticBottom
        {
            get
            {
                return _AddStaticBottom;
            }
            set
            {
                _AddStaticBottom = "AddStaticAtBottom";
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
        /// Page load event checks if the old nav or new nav should be loaded and grabs the appropriate function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {


                Guid WebGuid = SPContext.Current.Web.ID;
                Guid SiteGuid = SPContext.Current.Site.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite Site = new SPSite(SiteGuid))
                    {
                        using (SPWeb Web = Site.OpenWeb(WebGuid))
                        {

                            SPWeb WebRoot = Site.RootWeb;
                            SPList List = WebRoot.Lists[SettingsListName];
                            SPQuery Query = new SPQuery();
                            Query.Query = "<Where><Eq><FieldRef Name='Title'/><Value Type='Text'>" + AllowOldNav + "</Value></Eq></Where>";
                            SPListItemCollection Items = List.GetItems(Query);
                            SPListItem Item = Items[0];


                            //gets the static bottom items.
                            SPQuery QueryNewNav = new SPQuery();
                            QueryNewNav.Query = "<Where><Eq><FieldRef Name='Title'/><Value Type='Text'>" + AddStaticBottom + "</Value></Eq></Where>";
                            SPListItemCollection BottomItems = List.GetItems(QueryNewNav);
                            SPListItem BottomItem = BottomItems[0];
                            LoadNav(Web, BottomItem["Value"].ToString().Split(';'));
                        }
                    }
                });

            }
            catch (SPException ex)
            {
                UpdateErrorLog(ex);
            }


        }
        #endregion

        #region LoadNav
        /// <summary>
        /// Loads in the new navigation for the quicklaunch with parent, children, and siblings.
        /// </summary>
        protected void LoadNav(SPWeb Web, string[] StaticBottom)
        {
            if (PublishingWeb.IsPublishingWeb(Web))
            {
                PortalSiteMapProvider currentNavSiteMapProvider = PortalSiteMapProvider.CurrentNavSiteMapProvider;
                SiteMapNode currentSiteNodes = currentNavSiteMapProvider.FindSiteMapNode(Web.ServerRelativeUrl);
                foreach (PortalSiteMapNode Node in currentSiteNodes.ChildNodes)
                {
                    MenuItem Item = new MenuItem(Node.Title, Node.Title, "", Node.Url);
                    if (Web.ServerRelativeUrl.ToLower() == Node.Url.ToLower())
                    {
                        Item.Selected = true;
                    }
                    foreach (PortalSiteMapNode ChildNode in Node.ChildNodes)
                    {
                        MenuItem ChildrenItem = new MenuItem(ChildNode.Title, ChildNode.Title, "", ChildNode.Url);
                        // RSnyder (LCG) - 02/13/2013 - commented this line to prevent duplicates
                        //Item.ChildItems.Add(ChildrenItem);
                        if (!MenuItemContains(Item.ChildItems, ChildrenItem))
                        {
                            Item.ChildItems.Add(ChildrenItem);
                        }
                    }

                    // RSnyder (LCG) - 02/13/2013 - commented this line to prevent duplicates
                    //CustomNav.Items.Add(Item);                    
                    if (!MenuItemContains(CustomNav.Items, Item))
                    {
                        CustomNav.Items.Add(Item);
                    }
                }
            }

            //adds the static bottom
            foreach (string s in StaticBottom)
            {
                string[] p = s.Split(',');
                MenuItem BottomItem = new MenuItem(p[0], p[0], "", p[1]);

                // RSnyder (LCG) - 02/13/2013 - commented this line to prevent duplicates
                //CustomNav.Items.Add(BottomItem);
                if (!MenuItemContains(CustomNav.Items, BottomItem))
                {
                    CustomNav.Items.Add(BottomItem);
                }

            }
        }
        #endregion

        #region UpdateErrorLog
        /// <summary>
        /// Code to update the error log in the event that there is an issue with the control.
        /// </summary>
        /// <param name="ex"></param>
        protected void UpdateErrorLog(SPException ex)
        {
            Guid RootWebGuid = SPContext.Current.Site.RootWeb.ID;
            Guid SiteGuid = SPContext.Current.Site.ID;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite Site = new SPSite(SiteGuid))
                {
                    using (SPWeb Web = Site.OpenWeb(RootWebGuid))
                    {
                        SPList List = Web.Lists[ErrorLog];
                        Web.AllowUnsafeUpdates = true;

                        //writes to the error list
                        SPListItem Item = List.Items.Add();
                        Item["Title"] = "QuickLaunchControl Error";
                        Item["Error Code"] = Convert.ToString(ex.ErrorCode);
                        Item["Error"] = ex.Message;
                        Item["Stack Trace"] = ex.StackTrace;
                        Item.Update();  // RSnyder (LCG) - I added the update so errors get saved to the Error Log list


                        Web.AllowUnsafeUpdates = false;

                        //writes to the ULS Log
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory("QuickLaunchControl", TraceSeverity.Unexpected, EventSeverity.Error), TraceSeverity.Unexpected, ex.Message, ex.StackTrace);
                    }
                }
            });
        }
        #endregion

        #region MenuItemContains
        /// <summary>
        /// RSnyder (LCG) - 02/14/2013 - This function was created to test to see if a node was about to be added
        /// to the menuitem collection that already exists
        /// </summary>
        /// <param name="curMenuItems"></param>
        /// <param name="menuItemToAdd"></param>
        /// <returns></returns>
        private bool MenuItemContains(MenuItemCollection curMenuItems, MenuItem menuItemToAdd)
        {

            bool found = false;

            foreach (MenuItem menuItem in curMenuItems)
            {

                if (menuItemToAdd.NavigateUrl.ToLower() != String.Empty)
                {
                    // first check the url
                    if (menuItem.NavigateUrl.ToLower() == menuItemToAdd.NavigateUrl.ToLower())
                    {
                        // now check the caption if the caption and title are the same then consider it a duplicate
                        if (menuItem.Text.ToLower() == menuItemToAdd.Text.ToLower())
                        {
                            found = true;
                            break;
                        }
                    }
                }
                else
                {
                    //Assume a heading so just check the caption
                    if (menuItem.Text.ToLower() == menuItemToAdd.Text.ToLower())
                    {
                        found = true;
                        break;
                    }
                }
            }

            return found;
        }

        #endregion
    }
}
