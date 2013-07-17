using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Xml.Linq;

namespace Niem.MyNiem.Features.HotFix
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("3ff9fb82-5dd0-4fdb-80bd-0c70de11f874")]
    public class HotFixEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        void UpdateCategoryDomains(Guid siteID)
        {
            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPField lookupField = web.Fields.TryGetFieldByStaticName("Category_x0020_Domains");
                    SPList list = web.Lists["Communities"];
                    if (lookupField != null)
                    {
                        // Getting Schema of field
                        XDocument fieldSchema = XDocument.Parse(lookupField.SchemaXml);

                        // Get the root element of the field definition
                        XElement root = fieldSchema.Root;

                        // Check if list definition exits exists
                        // if (root.Attribute("List") != null)
                        {
                            // Getting value of list url
                            // string listurl = root.Attribute("List").Value;

                            // Get the correct folder for the list
                            SPFolder listFolder = list.RootFolder; // web.GetFolder(listurl);
                            if (listFolder != null && listFolder.Exists == true)
                            {
                                // Setting the list id of the schema
                                XAttribute attrList = root.Attribute("List");
                                if (attrList != null)
                                {
                                    // Replace the url wit the id
                                    attrList.Value = listFolder.ParentListId.ToString();
                                }

                                // Setting the souce id of the schema
                                XAttribute attrWeb = root.Attribute("SourceID");
                                if (attrWeb != null)
                                {
                                    // Replace the sourceid with the correct webid
                                    attrWeb.Value = web.ID.ToString();
                                }

                                // Update field with new schema
                                lookupField.SchemaXml = fieldSchema.ToString();
                            }
                        }
                    }

                }
            }
        }
        void UpdateSubjectArea(Guid siteID)
        {
            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPField lookupField = web.Fields.TryGetFieldByStaticName("Category_x0020_Subject_x0020_Area_x002F_Audience");
                    SPList list = web.Lists["Audience"];
                    if (lookupField != null)
                    {
                        // Getting Schema of field
                        XDocument fieldSchema = XDocument.Parse(lookupField.SchemaXml);

                        // Get the root element of the field definition
                        XElement root = fieldSchema.Root;

                        // Check if list definition exits exists
                        // if (root.Attribute("List") != null)
                        {
                            // Getting value of list url
                            // string listurl = root.Attribute("List").Value;

                            // Get the correct folder for the list
                            SPFolder listFolder = list.RootFolder; // web.GetFolder(listurl);
                            if (listFolder != null && listFolder.Exists == true)
                            {
                                // Setting the list id of the schema
                                XAttribute attrList = root.Attribute("List");
                                if (attrList != null)
                                {
                                    // Replace the url wit the id
                                    attrList.Value = listFolder.ParentListId.ToString();
                                }

                                // Setting the souce id of the schema
                                XAttribute attrWeb = root.Attribute("SourceID");
                                if (attrWeb != null)
                                {
                                    // Replace the sourceid with the correct webid
                                    attrWeb.Value = web.ID.ToString();
                                }

                                // Update field with new schema
                                lookupField.SchemaXml = fieldSchema.ToString();
                            }
                        }
                    }

                }
            }
        }
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite targetSite = properties.Feature.Parent as SPSite;
            try
            {
                UpdateCategoryDomains(targetSite.ID);
            }
            catch (Exception) { }
            try
            {
                UpdateSubjectArea(targetSite.ID);
            }
            catch (Exception) { }


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
