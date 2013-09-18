using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Niem.ForumContentTypeFix.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("1c9b57d3-dec9-41e2-9d1a-0c8c91c8557c")]
    public class ForumContentTypeFixFeature : SPFeatureReceiver
    {

        const string _discussionContentType = "Discussion";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {

            SPSite site = properties.Feature.Parent as SPSite;
            try
            {

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite elevatedSite = new SPSite(site.ID))
                    using (SPWeb elevatedRootWeb = elevatedSite.OpenWeb())
                    {

                        try
                        {
                            EnumerateSiteCollectionContentType(elevatedRootWeb);
                        }
                        catch (Exception ex)
                        {
                            AddErrorEntryToErrorLogList(elevatedRootWeb, "FeatureActivated", ex);
                        }
                    }

                });

            }
            catch
            {
            }

            // Change the site columns from single select to multi select
            // update the root site content type
            // iterate through all of the sites and discussions
            //   for each discussion
            //      copy list items to separate area
            //      remove list items
            //      remove existing content type
            //      add updated content type
            //      re-add data using the new content type
            //      remove temporary list

        }

        #region AddErrorEntryToErrorLogList

        private void AddErrorEntryToErrorLogList(SPWeb web, string method, Exception ex)
        {
            try
            {

                SPList errorList = web.Lists["Error Log List"];

                SPListItem item = errorList.AddItem();
                item["Title"] = ex.Message;
                item["Error Code"] = method + "::" + ex.Source;
                item["Error"] = ex.InnerException;
                item["Stack Trace"] = ex.StackTrace;
                item.Update();

            }
            catch
            {
            }

        }

        #endregion

        #region AddInfoEntryToErrorLogList

        private void AddInfoEntryToErrorLogList(SPWeb web, string title, string errorCode, string message, string stackTrace)
        {
            try
            {

                SPList errorList = web.Lists["Error Log List"];

                SPListItem item = errorList.AddItem();
                item["Title"] = title;
                item["Error Code"] = "Info";
                item["Error"] = message;
                item["Stack Trace"] = "";
                item.Update();

            }
            catch
            {
            }

        }

        #endregion

        #region EnumerateSiteCollectionContentType

        private void EnumerateSiteCollectionContentType(SPWeb elevatedRootWeb)
        {

            try
            {

                StringBuilder msg = new StringBuilder();                

                SPContentType rootContentType = elevatedRootWeb.ContentTypes[_discussionContentType];

                msg.AppendFormat("Enumeration of the SiteCollection level {0} content type" + Environment.NewLine, _discussionContentType);

                for(int i = 0; i < rootContentType.Fields.Count; i++)
                {
                    if (i > 0)
                    {
                        msg.Append(", ");
                    }
                    msg.Append(rootContentType.Fields[i].InternalName);
                }

                AddInfoEntryToErrorLogList(elevatedRootWeb, "Content Type Enumeration", "", msg.ToString(), "")lock
            }
            catch (Exception ex)
            {
                AddErrorEntryToErrorLogList(elevatedRootWeb, "EnumerateSiteCollectionContentType", ex);
            }

        }

        #endregion

        #region New Content Type
        //Title
        //Body
        //EmailSender
        //ActiveItem
        //Category_x0020_Subject_x0020_Area_x002F_Audience
        //Category_x0020_Domains
        //AverageRating
        #endregion


    }
}
