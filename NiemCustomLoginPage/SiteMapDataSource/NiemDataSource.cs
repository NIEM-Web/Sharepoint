using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Publishing.Navigation;
using System.Web.UI;

namespace NiemCustomLoginPage.SiteMapDataSource
{
    public class NiemDataSource : PortalSiteMapDataSource
    {
        protected override HierarchicalDataSourceView GetHierarchicalView(string viewPath)
        {
            base.ShowStartingNode = false;
            base.StartFromCurrentNode = true;
            PortalSiteMapNode node = PortalSiteMapProvider.CurrentNavSiteMapProvider.CurrentNode as PortalSiteMapNode;
            base.StartingNodeOffset = GetOffset(node);
            return base.GetHierarchicalView(viewPath);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        private int GetOffset(PortalSiteMapNode node)
        {
            int offSet = 0;
            int level = 0;
            //get level
            CurrentNodeLevel(node, ref level);
            if (level > 0)
            {
                offSet = -1 * (level + 1);
            }
            return offSet;
        }

        private int CurrentNodeLevel(PortalSiteMapNode node, ref int level)
        {
            PortalSiteMapNode parentNode = node.ParentNode as PortalSiteMapNode;
            if (parentNode != null)
            {
                level++;
                CurrentNodeLevel(parentNode, ref level);
            }
            return level;
        }

    }
}
