using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace lmd.NIEM.FarmSolution
{
    public class PersmissiveScriptControl : WebControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                RegisterScript();
            }
            catch { }
        }

        private void RegisterScript()
        {
            SPUser currentUser = SPContext.Current.Web.CurrentUser;
            string script = "var isUserMember = ";
            string currentUserGroupNames = "/*currentUserGroupNames*//*";
            string restrictedGroupNames = "/*restrictedGroupNames*//*";
            bool isUserInGroup = false;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                if (!SPContext.Current.Web.IsRootWeb)
                {
                    List<string> groups = new List<string>();
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList groupsPermission =
                            web.GetList("/Lists/GroupsPermission");

                        SPListItemCollection restrictedGroups =
                            groupsPermission.GetItems(new string[] { "Title" });

                        groups = (from restrictedGroup in restrictedGroups.Cast<SPListItem>()
                                  select restrictedGroup.Title.ToLowerInvariant()).ToList();

                        for (int i = 0; i < groups.Count; i++)
                        {
                            restrictedGroupNames += groups[i] + ";";
                        }
                        restrictedGroupNames += "*/";
                        SPGroupCollection currentUserGroups = currentUser.Groups;

                        for (int i = 0; i < currentUserGroups.Count; i++)
                        {
                            string groupName = currentUserGroups[i].Name.ToLowerInvariant();
                            currentUserGroupNames += groupName + ";";
                            if (groups.Contains(groupName))
                            {
                                isUserInGroup = true;
                                break;
                            }
                        }
                        currentUserGroupNames += "*/";
                    }
                }
            });

            if (isUserInGroup)
            {
                script += "true;"+ currentUserGroupNames + restrictedGroupNames;
            }
            else
            {
                script += "false;" + currentUserGroupNames + restrictedGroupNames;
            }

            Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "PermissiveScript", script, true);
        }
    }
}
