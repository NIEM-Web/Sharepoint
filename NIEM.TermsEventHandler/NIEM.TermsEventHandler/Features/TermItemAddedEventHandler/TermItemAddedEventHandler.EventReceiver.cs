using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Navigation;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebPartPages;

namespace NIEM.TermsEventHandler.Features.TermItemAddedEventHandler
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("2c51584c-ce8d-4247-947e-c0119e8cfc26")]
    public class TermItemAddedEventHandlerEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            
            SPWeb web = (SPWeb)properties.Feature.Parent;

            string assemblyName = "NIEM.TermsEventHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=90d8401002fad21d";
            string className = "NIEM.TermsEventHandler.EventHandlers.TermsEventHandlerEventReceiver";

            try
            {

                // add the event handler to specific lists
                SPList spListCustom = web.Lists["Terms"];

                if (spListCustom != null)
                {
                    SPEventReceiverDefinition erAdding = spListCustom.EventReceivers.Add();
                    erAdding.Assembly = assemblyName;
                    erAdding.Class = className;
                    erAdding.Type = SPEventReceiverType.ItemAdded;
                    erAdding.Name = "TermsEventHandlerEventReceiverItemAdded";
                    erAdding.Update();

                    spListCustom.Update();
                }

            }
            catch (Exception ex)
            {
                Common.LogError("TermItemAddedEventHandlerEventReceiver.FeatureActivated", ex, web.Site.ID);
            }
            
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {

            SPWeb web = (SPWeb)properties.Feature.Parent;

            try
            {

                // add the event handler to specific lists
                SPList spListCustom = web.Lists["Terms"];

                if (spListCustom != null)
                {

                    int eventReceiverId = -1;

                    for (int i = 0; i < spListCustom.EventReceivers.Count; i++)
                    {
                        if (spListCustom.EventReceivers[i].Name == "TermsEventHandlerEventReceiverItemAdded")
                        {
                            eventReceiverId = i;
                            break;
                        }
                    }

                    if (eventReceiverId != -1)
                    {
                        spListCustom.EventReceivers[eventReceiverId].Delete();
                    }

                }

            }
            catch (Exception ex)
            {
                Common.LogError("TermItemAddedEventHandlerEventReceiver.FeatureActivated", ex, web.Site.ID);
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
