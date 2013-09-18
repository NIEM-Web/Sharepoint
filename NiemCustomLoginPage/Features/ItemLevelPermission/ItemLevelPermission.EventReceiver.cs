using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Collections.Generic;

namespace lmd.NIEM.FarmSolution.Features.ItemLevelPermission
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("166b9058-52c0-4a68-873c-5f158a099aa1")]
    public class ItemLevelPermissionEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        string assemblyName = "lmd.NIEM.FarmSolution, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f557e1aff1f6f296";
        string className = "lmd.NIEM.FarmSolution.ItemLevelEventReceiver";
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;
            UpdateEventReceivers(web, true);
        }

        private void UpdateEventReceivers(SPWeb web, bool add)
        {
            SPListCollection lists = web.Lists;
            for (int count = 0; count < lists.Count; count++)
            {
                SPList list = lists[count];
                if (list.BaseTemplate == SPListTemplateType.DiscussionBoard || list.BaseTemplate == SPListTemplateType.DocumentLibrary)
                {
                    if (add)
                    {
                        list.EventReceivers.Add(SPEventReceiverType.ItemAdded, assemblyName, className);
                        list.Update();
                    }
                    else
                    {
                        SPEventReceiverDefinitionCollection eventReceivers = list.EventReceivers;
                        int eventReceiversLength = eventReceivers.Count;
                        List<Guid> ids = new List<Guid>();

                        for (int counter = eventReceiversLength - 1; counter > 0; counter--)
                        {
                            if (string.Compare(eventReceivers[counter].Assembly, assemblyName) == 0)
                            {
                                ids.Add(eventReceivers[counter].Id);
                            }
                        }

                        foreach (var id in ids)
                        {
                            eventReceivers[id].Delete();
                        }
                    }
                }
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;
            UpdateEventReceivers(web, false);
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
