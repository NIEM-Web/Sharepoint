using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace MyNiemFix5SandBoxed.Features.MyNiemFix5SpFeature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("55a81152-312f-445f-bd4b-cdab700b0dc6")]
    public class MyNiemFix5SpFeatureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb targetWeb = properties.Feature.Parent as SPWeb;
            try
            {
                UpdateFilterCategory(targetWeb.Site.ID);
            }
            catch
            {
            }
        }

        private static void UpdateFilterCategory(Guid siteID)
        {

            string internalName = "FilterCategory";
            string invalidListGuid = "83044d28-45d1-419c-adbe-51667dc23265";
            string lookupListTitle = "Audience";

            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb web = site.OpenWeb())
                {

                    SPField lookupField = null;
                    SPList list = null;

                    try
                    {
                        lookupField = web.Fields.TryGetFieldByStaticName(internalName);
                        list = web.Lists[lookupListTitle];
                    }
                    catch
                    {
                    }

                    try
                    {
                        if (lookupField != null)
                        {
                            lookupField.SchemaXml = lookupField.SchemaXml.Replace("Type=\"Text\"", "Type=\"Lookup\"").Replace(invalidListGuid, list.ID.ToString());
                            lookupField.Update();
                        }
                    }
                    catch
                    {
                    }

                    //Url: https://www.niem.gov/documentsdb, List Title: Upload A Case
                    UpdateReference(site, "/documentsdb", "Upload A Case", internalName, invalidListGuid, list.ID.ToString());


                }
            }
        }

        private static void UpdateReference(SPSite site, string webUrl, string listTitle, string internalName, string invalidListGuid, string validListGuid)
        {
            try
            {
                using (SPWeb aboutNeimWeb = site.AllWebs[webUrl])
                {
                    SPField lookupField = aboutNeimWeb.Lists[listTitle].Fields.TryGetFieldByStaticName(internalName);
                    if (lookupField != null)
                    {
                        lookupField.SchemaXml = lookupField.SchemaXml.Replace("Type=\"Text\"", "Type=\"Lookup\"").Replace(invalidListGuid, validListGuid);
                        lookupField.Update();
                    }
                }
            }
            catch
            {
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
