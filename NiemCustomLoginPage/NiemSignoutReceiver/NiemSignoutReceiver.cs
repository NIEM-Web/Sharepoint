using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.Administration;

namespace NiemCustomLoginPage.NiemSignoutReceiver
{
    /// <summary>
    /// Web Events
    /// </summary>
    public class NiemSignoutReceiver : SPFeatureReceiver
    {

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            const string page = "~/_layouts/NiemCustomLoginPage/signout.aspx";
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
            if (webApp != null)
            {
                if (!webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.Signout, page))
                {
                    throw new Exception("Cannot Replace Signout");
                }
                webApp.Update();
            }
            //base.FeatureActivated(properties);
        }
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {

            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
            if (webApp != null)
            {
                if (!webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.Signout, null))
                {
                    throw new Exception("Cannot reset Signout");
                }
                webApp.Update();
            }

            //base.FeatureDeactivating(properties);
        }


    }
}
