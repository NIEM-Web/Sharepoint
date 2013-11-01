using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace NIEM.Map.ProjectInfoEventHandler
{
    public class Common
    {

        public static void LogError(string title, Exception ex, Guid siteId)
        {
            try
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb web = site.RootWeb)
                {

                    SPList logList = null;
                    logList = web.Lists.TryGetList("Error Log List");

                    if (logList != null)
                    {
                        SPListItem item = logList.AddItem();
                        item["Title"] = title;
                        item["Error Code"] = "Error";
                        item["Error"] = ex.Message;
                        item["Stack Trace"] = ex.StackTrace;
                        item.Update();
                    }

                }
            }
            catch
            {
            }
        }

        public static void LogMessage(string title, string message, Guid siteId)
        {
            try
            {
                using (SPSite site = new SPSite(siteId))
                using (SPWeb web = site.RootWeb)
                {

                    SPList logList = null;
                    logList = web.Lists.TryGetList("Error Log List");

                    if (logList != null)
                    {
                        SPListItem item = logList.AddItem();
                        item["Title"] = title;
                        item["Error Code"] = "Info";
                        item["Error"] = message;
                        item["Stack Trace"] = "";
                        item.Update();
                    }

                }
            }
            catch
            {
            }
        }

    }
}
