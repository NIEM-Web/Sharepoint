using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace lmd.NIEM.FarmSolution
{
    public class ItemLevelEventReceiver : SPItemEventReceiver
    {
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            AssignPermissionsToCreator(properties);
        }

        private void AssignPermissionsToCreator(SPItemEventProperties properties)
        {
            SPListItem item = properties.ListItem;
            object createdBy = item["Author"];
            if (!item.HasUniqueRoleAssignments)
            {
                item.BreakRoleInheritance(true);
                SPRoleAssignmentCollection roleAssignments = item.RoleAssignments;
                SPWeb web = properties.Web.Site.RootWeb;
                SPRoleDefinition read = web.RoleDefinitions["NiemRestricted"];//copy read permission with add permission as user cannot add replies

                for (int count = 0; count < roleAssignments.Count; count++)
                {
                    SPRoleDefinitionBindingCollection definitionBindings = roleAssignments[count].RoleDefinitionBindings;
                    SPPrincipal principal = roleAssignments[count].Member;
                    definitionBindings.RemoveAll();
                    definitionBindings.Add(read);
                    roleAssignments[count].Update();
                }
            }
        }

    }
}
