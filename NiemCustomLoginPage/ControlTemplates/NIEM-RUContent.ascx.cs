using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace lmd.NIEM.FarmSolution.ControlTemplates
{
    public partial class NIEM_RUContent : UserControl
    {
        private string listName;
        public string ListName
        {
            get { return listName; }
            set { listName = value; }
        }
        private int itemID;
        public int ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }
        private string fieldName;
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ListName))
                {
                    SPList list = SPContext.Current.Site.RootWeb.Lists[ListName];
                    SPListItem item = list.GetItemById(itemID);
                    pnlContent.Controls.Add(new LiteralControl(item[FieldName].ToString()));
                }
            }
            catch (Exception)
            { }
        }
    }
}
