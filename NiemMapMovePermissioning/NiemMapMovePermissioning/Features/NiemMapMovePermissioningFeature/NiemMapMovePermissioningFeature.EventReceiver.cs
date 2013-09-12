using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace NiemMapMoveSiteColumnUpdate.Features.NiemMapMoveSiteColumnUpdateFeatur
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("fe77f66b-ace0-4210-b300-9f0feb72317a")]
    public class NiemMapMoveSiteColumnUpdateFeaturEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {

            SPWeb targetWeb = properties.Feature.Parent as SPWeb;
            try
            {

                var list = targetWeb.Lists.TryGetList("NIEM Project Info");
                if (list != null)
                {
                    list.BreakRoleInheritance(true, false);
                    list.AllowEveryoneViewItems = true;
                    list.AnonymousPermMask64 = SPBasePermissions.ViewPages | SPBasePermissions.OpenItems
                        | SPBasePermissions.ViewVersions | SPBasePermissions.Open
                        | SPBasePermissions.UseClientIntegration
                        | SPBasePermissions.ViewFormPages
                        | SPBasePermissions.ViewListItems;
                    list.Update();
                }

            }
            catch
            {
            }

        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb targetWeb = properties.Feature.Parent as SPWeb;
            try
            {

                targetWeb.AllowUnsafeUpdates = true;
                var list = targetWeb.Lists.TryGetList("NIEM Project Info");
                if (list != null)
                {
                    list.AnonymousPermMask64 = SPBasePermissions.EmptyMask;
                    list.Update();
                }

            }
            catch
            {
            }
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
