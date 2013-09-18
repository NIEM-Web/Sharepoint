using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;

namespace lmd.NIEM.FarmSolution.Features.LayoutsPageMapping
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("a329e0ed-44ca-4e42-aec6-a0e67e55e061")]
    public class LayoutsPageMappingEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.
        private const string Page_NIEM_AccessDenied = "/_layouts/niem/AccessDenied.aspx";
        private const string Page_NIEM_Confirmation = "/_layouts/niem/confirmation.aspx";
        private const string Page_NIEM_Error = "/_layouts/niem/error.aspx";
        private const string Page_NIEM_Login = "/_layouts/niem/login.aspx";
        private const string Page_NIEM_RequestAccess = "/_layouts/niem/reqacc.aspx";

        private const string Page_AccessDenied = "/_layouts/AccessDenied.aspx";
        private const string Page_Confirmation = "/_layouts/confirmation.aspx";
        private const string Page_Error = "/_layouts/error.aspx";
        private const string Page_Login = "/_layouts/login.aspx";
        private const string Page_RequestAccess = "/_layouts/reqacc.aspx";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;

            bool isAccessDeniedSet =
                webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.AccessDenied, Page_NIEM_AccessDenied);

            bool isConfirmationSet =
                webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.Confirmation, Page_NIEM_Confirmation);

            bool isErrorSet =
                webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.Error, Page_NIEM_Error);

            bool isLoginSet =
                webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.Login, Page_NIEM_Login);

            bool isRequestAccessSet =
                webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.RequestAccess, Page_NIEM_RequestAccess);
            
            webApp.Update(true);

        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;

            bool isAccessDeniedSet =
                webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.AccessDenied, Page_AccessDenied);

            bool isConfirmationSet =
                webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.Confirmation, Page_Confirmation);

            bool isErrorSet =
                webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.Error, Page_Error);

            bool isLoginSet =
                webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.Login, Page_Login);

            bool isRequestAccessSet =
                webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.RequestAccess, Page_RequestAccess);

            webApp.Update();

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
