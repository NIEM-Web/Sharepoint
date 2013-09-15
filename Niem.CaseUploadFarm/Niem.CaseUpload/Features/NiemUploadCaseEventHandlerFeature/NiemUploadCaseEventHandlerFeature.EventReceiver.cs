using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Niem.CaseUpload.Features.Niem_UploadCase_EventHandler_Feature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("11e55631-eb18-4e4c-b528-65997f75bb6a")]
    public class Niem_UploadCase_EventHandler_FeatureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {

            try
            {

                SPWeb myWeb = (SPWeb)properties.Feature.Parent;

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite sysSite = new SPSite(myWeb.Url))
                    {
                        using (SPWeb sysWeb = sysSite.OpenWeb(myWeb.ServerRelativeUrl))
                        {
                            this.AddNiemUploadEventReceiver(sysWeb, "UploadCase");
                        }
                    }
                });

            }
            catch (Exception ex)
            {
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            System.Diagnostics.Debugger.Launch();
            try
            {

                SPWeb myWeb = (SPWeb)properties.Feature.Parent;

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite sysSite = new SPSite(myWeb.Url))
                    {
                        using (SPWeb sysWeb = sysSite.OpenWeb(myWeb.ServerRelativeUrl))
                        {
                            this.RemoveNiemUploadEventReceiver(sysWeb, "UploadCase");
                        }
                    }
                });

            }
            catch (Exception ex)
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


        #region AddNiemUploadEventReceiver

        public void AddNiemUploadEventReceiver(SPWeb sysWeb, string spListTitle)
        {


            SPList spListCustom = null;

            spListCustom = sysWeb.Lists.TryGetList(spListTitle);

            if (spListCustom != null)
            {

                // add the TitleFieldUpdateListEventReceiver to specific lists
                string assemblyName = "Niem.CaseUpload, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6f7c7006f477a0a5";
                string className = "Niem.CaseUpload.NiemUploadCaseHandler.NiemUploadCaseEventHandler";

                // When the user is tyring 
                //SPEventReceiverDefinition erAdding = spListCustom.EventReceivers.Add();
                //erAdding.Assembly = assemblyName;
                //erAdding.Class = className;
                //erAdding.Type = SPEventReceiverType.ItemAdding;
                //erAdding.Name = "NiemUploadCaseHandlerEventReceiverItemAdding";
                //erAdding.Update();

                SPEventReceiverDefinition erUpdating = spListCustom.EventReceivers.Add();
                erUpdating.Assembly = assemblyName;
                erUpdating.Class = className;
                erUpdating.Type = SPEventReceiverType.ItemUpdating;
                erUpdating.Name = "NiemUploadCaseHandlerEventReceiverItemUpdating";
                erUpdating.Update();

                spListCustom.Update();

            }
        }

        #endregion

        #region RemoveNiemUploadEventReceiver

        public void RemoveNiemUploadEventReceiver(SPWeb sysWeb, string spListTitle)
        {

            
            List<Guid> eventHandlers = new List<Guid>();
            SPList spListCustom = null;

            spListCustom = sysWeb.Lists.TryGetList(spListTitle);

            if (spListCustom != null)
            {
                
                foreach (SPEventReceiverDefinition eventReceiverDefinition in spListCustom.EventReceivers)
                {
                    if (eventReceiverDefinition.Name == "NiemUploadCaseHandlerEventReceiverItemAdding")
                    {
                        eventHandlers.Add(eventReceiverDefinition.Id);
                    }
                    else if (eventReceiverDefinition.Name == "NiemUploadCaseHandlerEventReceiverItemUpdating")
                    {
                        eventHandlers.Add(eventReceiverDefinition.Id);
                    }
                }

                foreach (Guid eventHdlrId in eventHandlers)
                {
                    spListCustom.EventReceivers[eventHdlrId].Delete();
                }
                spListCustom.Update();
            }
        }

        #endregion
    }
}
