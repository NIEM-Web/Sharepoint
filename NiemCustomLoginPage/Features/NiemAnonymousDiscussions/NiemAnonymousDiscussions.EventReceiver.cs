using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace lmd.NIEM.FarmSolution.Features.Feature2
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("fcb20523-7da5-4f56-b6bb-b987ec9e1fa0")]
    public class Feature2EventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

       public override void FeatureActivated(SPFeatureReceiverProperties properties)
       {
           SPWeb featureWeb = properties.Feature.Parent as SPWeb;
           SPSecurity.RunWithElevatedPrivileges(delegate()
           {
               using (SPSite site = new SPSite(featureWeb.Site.Url))
               using (SPWeb web = site.OpenWeb(featureWeb.ID))
               {
                   foreach (SPList list in web.Lists)
                   {
                       if (list.BaseTemplate == SPListTemplateType.DiscussionBoard)
                       {
                           //break the inheritance
                           list.BreakRoleInheritance(true);
                           //add permission for anonymous users to view form pages
                           list.AnonymousPermMask64 |= SPBasePermissions.ViewFormPages;
                           list.Update();
                       }
                   }
               }
           });
       }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

       public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
       {
           SPWeb featureWeb = properties.Feature.Parent as SPWeb;
           SPSecurity.RunWithElevatedPrivileges(delegate()
           {
               using (SPSite site = new SPSite(featureWeb.Site.Url))
               using (SPWeb web = site.OpenWeb(featureWeb.ID))
               {
                   foreach (SPList list in web.Lists)
                   {
                       if (list.BaseTemplate == SPListTemplateType.DiscussionBoard)
                       {
                           //make the list inherit the permissions from parent
                           list.ResetRoleInheritance();
                           list.Update();
                       }
                   }
               }
           });
       }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
