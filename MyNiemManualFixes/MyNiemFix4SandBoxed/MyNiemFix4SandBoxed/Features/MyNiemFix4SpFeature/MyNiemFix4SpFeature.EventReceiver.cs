using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace MyNiemFix4SandBoxed.Features.MyNiemFix4SpFeature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("9ae73200-304f-4c52-94fa-b94c55086855")]
    public class MyNiemFix4SpFeatureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb targetWeb = properties.Feature.Parent as SPWeb;
            try
            {
                UpdateCategorySubjectAreaOrAudience(targetWeb.Site.ID);
            }
            catch
            {
            }
        }

        private static void UpdateCategorySubjectAreaOrAudience(Guid siteID)
        {

            string internalName = "Category_x0020_Subject_x0020_Area_x002f_Audience";
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

                    //https://www.niem.gov/communities/biometrics/biometrics-community, List Title: Pages
                    UpdateReference(site, "/communities/biometrics/biometrics-community", "Pages", internalName, invalidListGuid, list.ID.ToString());
                    
                    //https://www.niem.gov/communities/cbrn/cbrn-community, List Title: Pages
                    UpdateReference(site, "/communities/cbrn/cbrn-community", "Pages", internalName, invalidListGuid, list.ID.ToString());
                    
                    //https://www.niem.gov/communities/comm-toolkit, List Title: CT Resources
                    UpdateReference(site, "/communities/comm-toolkit", "CT Resources", internalName, invalidListGuid, list.ID.ToString());
                    
                    //https://www.niem.gov/glossary, List Title: Pages
                    UpdateReference(site, "/glossary", "Pages", internalName, invalidListGuid, list.ID.ToString());
                    
                    //https://www.niem.gov/news, List Title: Pages
                    UpdateReference(site, "/news", "Pages", internalName, invalidListGuid, list.ID.ToString());
                    
                    //https://www.niem.gov/documentsdb, List Title: Documents
                    UpdateReference(site, "/documentsdb", "Documents", internalName, invalidListGuid, list.ID.ToString());
                    
                    //https://www.niem.gov/documentsdb, List Title: Drop Off Library
                    UpdateReference(site, "/documentsdb", "Drop Off Library", internalName, invalidListGuid, list.ID.ToString());
                    
                    //https://www.niem.gov/documentsdb, List Title: Upload A Case
                    UpdateReference(site, "/documentsdb", "Upload A Case", internalName, invalidListGuid, list.ID.ToString());
                    
                    //https://www.niem.gov/spotlight, List Title: Pages
                    UpdateReference(site, "/spotlight", "Pages", internalName, invalidListGuid, list.ID.ToString());
                    
                    //https://www.niem.gov/previous-leftover/faq, List Title: Pages
                    UpdateReference(site, "/previous-leftover/faq", "Pages", internalName, invalidListGuid, list.ID.ToString());
                    
                    //https://www.niem.gov/previous-leftover/government/tribal, List Title: Pages
                    UpdateReference(site, "/previous-leftover/government/tribal", "Pages", internalName, invalidListGuid, list.ID.ToString());
                    
                    //https://www.niem.gov/previous-leftover/international, List Title: Pages
                    UpdateReference(site, "/previous-leftover/international", "Pages", internalName, invalidListGuid, list.ID.ToString());

                    //https://www.niem.gov/previous-leftover/testing, List Title: Pages
                    UpdateReference(site, "/previous-leftover/testing", "Pages", internalName, invalidListGuid, list.ID.ToString());


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
