using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace NIEM.Reviews.Features.NIEM.Reviews
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("0e966b67-2acb-4d6f-a0c9-1158aaf1fef8")]
    public class NIEMEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.
        private const string JS_FOLDER = "/Style Library/NIEM3/js";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            using (SPSite siteCollection = (SPSite)properties.Feature.Parent)
            {
                using (SPWeb rootWeb = siteCollection.RootWeb)
                {
                    
                    CheckinFeatureFiles(rootWeb, JS_FOLDER);
                }
            }
        }

        private void CheckinFeatureFiles(SPWeb rootWeb, string folderPath)
        {
            // Check in all files in the specified folder so that they will be applied properly
            SPFolder altStyleFolder = rootWeb.GetFolder(folderPath);
            foreach (SPFile styleFile in altStyleFolder.Files)
            {
                if (styleFile.CheckOutType != SPFile.SPCheckOutType.None)
                {
                    styleFile.CheckIn("Checked in via feature activation.", SPCheckinType.MajorCheckIn);
                    //styleFile.Approve("Approved via feature activation.");
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
