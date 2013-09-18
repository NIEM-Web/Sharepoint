using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace MyNiemFix3.Features.MyNiemFix3Feature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("fa38b042-ed27-4344-87bc-eedc3fa53627")]
    public class MyNiemFix3FeatureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb targetWeb = properties.Feature.Parent as SPWeb;
            try
            {
                UpdateCategoryDomains(targetWeb.Site.ID);
            }
            catch
            {
            }
        }

        private static void UpdateCategoryDomains(Guid siteID)
        {

            string internalName = "Category_x0020_Domains";
            string invalidListGuid = "85de703b-6753-4989-95f3-701c145ee5fc";

            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb web = site.OpenWeb())
                {

                    SPField lookupField = null;
                    SPList list = null;

                    try
                    {
                        lookupField = web.Fields.TryGetFieldByStaticName(internalName);
                        list = web.Lists["Communities"];
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

                    try
                    {
                        using (SPWeb aboutNeimWeb = site.AllWebs["/aboutniem"])
                        {
                            lookupField = aboutNeimWeb.Lists["Pages"].Fields.TryGetFieldByStaticName(internalName);
                            if (lookupField != null)
                            {
                                lookupField.SchemaXml = lookupField.SchemaXml.Replace("Type=\"Text\"", "Type=\"Lookup\"").Replace(invalidListGuid, list.ID.ToString());
                                lookupField.Update();
                            }
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        using (SPWeb nbacWeb = site.AllWebs["/communities/nbac"])
                        {
                            lookupField = nbacWeb.Lists["Pages"].Fields.TryGetFieldByStaticName(internalName);
                            if (lookupField != null)
                            {
                                lookupField.SchemaXml = lookupField.SchemaXml.Replace("Type=\"Text\"", "Type=\"Lookup\"").Replace(invalidListGuid, list.ID.ToString());
                                lookupField.Update();
                            }
                        }
                    }
                    catch
                    {
                    }

                }
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
