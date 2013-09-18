using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Server.Data;
using Microsoft.SharePoint.Publishing.Navigation;
using System.Web.UI;
using Microsoft.SharePoint;

namespace lmd.NIEM.FarmSolution
{
    public class NiemPortalSiteMapDataSource : PortalSiteMapDataSource
    {
        protected override HierarchicalDataSourceView GetHierarchicalView(string viewPath)
        {

            int siteLevel = GetCurrentWebSiteLevel(SPContext.Current.Web);

            this.StartingNodeOffset = siteLevel == 0 ?

                0 : 0 - siteLevel + 1;

            return base.GetHierarchicalView(viewPath);

        }

        private int GetCurrentWebSiteLevel(SPWeb currentWeb)
        {

            //We can't really work out the level from the URL because the URL

            //might contain managed path that is not part of the site collection

            //(e.g. http://server/ or http://server/sites/).

            int level = 0;

            var tempWeb = currentWeb;

            while (!tempWeb.IsRootWeb)
            {

                level++;

                tempWeb = tempWeb.ParentWeb;

                //Official guidance from MS is that we do not need to call Dispose on SPWeb.ParentWeb.

            }

            return level;

        }

    }
}
