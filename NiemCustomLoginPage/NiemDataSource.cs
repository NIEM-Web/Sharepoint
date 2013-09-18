using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Publishing.Navigation;
using System.Web;

namespace lmd.NIEM.FarmSolution
{
    public class NiemDataSource : PortalSiteMapDataSource
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected override void TrimNode(PortalHierarchyData data)
        {
            base.TrimNode(data);
            PortalSiteMapNode node = data.PortalSiteMapNode;
            int nodeLevel = 0;
            nodeLevel = GetLevel(node, ref nodeLevel);

            PortalSiteMapProvider provider = node.PortalProvider;
            SiteMapNode currentNode = provider.CurrentNode;
            int currentNodeLevel = 0;
            currentNodeLevel = GetLevel(currentNode, ref currentNodeLevel);

            //if data corresponds to current node 
            //then show node's children
            if (currentNode.Key == node.Key)
            {
                data.ShowChildren = true;
                data.ShowNode = true;
            }
            //else if the levels are same for both nodes
            else if (currentNodeLevel == nodeLevel)
            {
                //if the parent nodes are same for both then show children
                if (currentNode.ParentNode == node.ParentNode)
                {
                    data.ShowChildren = false;
                    data.ShowNode = true;
                }
                else
                {
                    data.ShowChildren = false;
                    data.ShowNode = false;
                }
            }
            else if (currentNodeLevel > nodeLevel)
            {
                //check if the node is in the parent heirarchy
                //or if the node's parent is in the heirarchy
                if (CheckHeirarychy(currentNode, node))
                {
                    data.ShowNode = true;
                }
                else
                {
                    data.ShowChildren = false;
                    data.ShowNode = false;
                }

            }
            else if (currentNodeLevel < nodeLevel)
            {
                if (currentNode == node.ParentNode)
                {
                    data.ShowChildren = false;
                    data.ShowNode = true;
                }
                else
                {
                    data.ShowNode = false;
                    data.ShowNode = false;
                }
            }
            else
            {
                //do nothing
            }

            //all levels upto 1st level should be shown i.e root and immediate children
            if (nodeLevel <= 2)
            {
                data.ShowNode = true;
            }
        }

        private bool CheckHeirarychy(SiteMapNode currentNode, PortalSiteMapNode node)
        {
            bool flag = false;
            SiteMapNode tempNode = currentNode;
            while (tempNode != null)
            {
                if (tempNode == node ||
                    tempNode == node.ParentNode)
                {
                    flag = true;
                    break;
                }
                tempNode = tempNode.ParentNode;
            }
            return flag;
        }

        int GetLevel(SiteMapNode node, ref int level)
        {
            if (node != null)
            {
                level++;
                GetLevel(node.ParentNode, ref level);
            }
            return level;
        }

    }
}
