using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace NIEM.TermsEventHandler.EventHandlers
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class TermsEventHandlerEventReceiver : SPItemEventReceiver
    {
        public override void ItemAdded(SPItemEventProperties properties)
        {

            try
            {
                if (properties.ListTitle == "Terms")
                {

                    Guid siteID, webID, listID;
                    int itemID;
                    listID = properties.ListId;
                    itemID = properties.ListItem.ID;
                    using (SPWeb web = properties.OpenWeb())
                    {
                        siteID = web.Site.ID;
                        webID = web.ID;
                    }

                    //run this block as System Account
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite site = new SPSite(siteID))
                        {
                            using (SPWeb web = site.OpenWeb(webID))
                            {
                                web.AllowUnsafeUpdates = true;
                                SPListItem item = web.Lists[listID].GetItemById(itemID);
                                if (item != null)
                                {

                                    if (item["Author"].ToString() == "-1;#")
                                    {

                                        //Impersonate the Author to be System Account
                                        item["Author"] = web.AllUsers[@"SHAREPOINT\system"];
                                        item.SystemUpdate();

                                    }

                                    //start the by add the item and workflow name
                                    StartWorkflow(item, "TERMS_Approval", properties.SiteId);

                                }
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    });
                }
                else
                {
                    return;
                }
            }
            catch(Exception ex)
            {
                Common.LogError("TermsEventHandlerEventReceiver.ItemAdded", ex, properties.SiteId);
            }
        }

        //method to start the workflow
        private static void StartWorkflow(SPListItem listItem, string workflowName, Guid siteId)  
        {  
            try
            {
                SPWorkflowAssociation wfAssoc = listItem.ParentList.WorkflowAssociations.GetAssociationByName(workflowName, System.Globalization.CultureInfo.CurrentCulture);  
                listItem.Web.Site.WorkflowManager.StartWorkflow(listItem, wfAssoc, wfAssoc.AssociationData, true);  
                listItem.Update();
            }
            catch (Exception ex)
            {
                Common.LogError("TermsEventHandlerEventReceiver.StartWorkflow", ex, siteId);
            }
        }

    }

}
