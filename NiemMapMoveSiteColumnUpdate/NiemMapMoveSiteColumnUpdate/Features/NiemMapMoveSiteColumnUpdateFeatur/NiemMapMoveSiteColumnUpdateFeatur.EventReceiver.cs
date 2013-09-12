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
                string webUrl = (targetWeb.ServerRelativeUrl.TrimEnd('/') + "/").ToLower();

                if (webUrl.EndsWith("/documentsdb/"))
                {
                    UpdateProjectID(targetWeb);
                }

            }
            catch
            {
            }

        }


        private static void UpdateProjectID(SPWeb resourceDbWeb)
        {

            string internalName = "ProjectID";
            string invalidListGuid = "";
            string newListGuid = "";

            using (SPWeb rootWeb = resourceDbWeb.Site.OpenWeb(resourceDbWeb.Site.RootWeb.ID))
            {
                invalidListGuid = rootWeb.Lists["NIEM Project Info"].ID.ToString();
            }
            newListGuid = resourceDbWeb.Lists["NIEM Project Info"].ID.ToString();

            SPField lookupField = null;

            lookupField = resourceDbWeb.Lists["NIEM Project Administration"].Fields.TryGetFieldByStaticName(internalName);
            if (lookupField != null)
            {
                lookupField.SchemaXml = lookupField.SchemaXml.Replace(invalidListGuid, newListGuid);
                lookupField.Update();
            }

        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


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
