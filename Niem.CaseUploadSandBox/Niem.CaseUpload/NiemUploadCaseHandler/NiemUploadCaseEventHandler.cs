using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Niem.CaseUploadSandBox.NiemUploadCaseHandler
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class NiemUploadCaseEventHandler : SPItemEventReceiver
    {

       public override void ItemUpdating(SPItemEventProperties properties)
       {
           base.ItemUpdating(properties);

           Common.LogMessage("NiemUploadCaseEventHandler.ItemUpdating", "Started ItemUpdating", properties.SiteId);

           if (properties.ListTitle == "UploadCase")
           {

               if (properties.AfterProperties["vti_contenttag"] == null)
               {
                   properties.Cancel = true;
                   properties.ErrorMessage = "You must provide a different file name";
               }
           }

       }

    }
}
