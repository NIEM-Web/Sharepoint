using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;

namespace Niem.MyNiem
{
    public partial class UserInterestFieldControl : BaseFieldControl
    {

        private double _value;
        private Label lblLikesCount;
        private HyperLink hlkLike;
        private HiddenField hdnCount;
        protected override string DefaultTemplateName
        {
            get
            {
                return "UserInterestFieldTemplate";
            }
        }

        protected override void CreateChildControls()
        {

            if (Field == null)
            {
                return;
            }
            base.CreateChildControls();
            FindControls();
        }

        private void FindControls()
        {
            bool doesCurrentUserLike = DoesCurrentUserLikes();
            hlkLike = TemplateContainer.FindControl("hlkLike") as HyperLink;
            if (doesCurrentUserLike)
            {
                hlkLike.Text = "Unlike";
            }
            else
            {
                hlkLike.Text = "Like";
            }

            //shows the likes count
            lblLikesCount = TemplateContainer.FindControl("lblLikesCount") as Label;
            lblLikesCount.Text = (this.ItemFieldValue == null) ? "0" : this.ItemFieldValue.ToString();
            //check whether current user likes the item?
            //if he likes then choose script for Like Item
            //else choose script for unlike item
            var script = string.Format("javascript:LikeUnlikeItem('{0}','{1}','{2}', {3}, '{4}','{5}');", this.ItemIdAsString, this.ListId, this.Web.ID, (!doesCurrentUserLike).ToString().ToLower(), hlkLike.ClientID, lblLikesCount.ClientID);
            
            //hyperlink script
            hlkLike.Attributes.Add("onclick", script);
        }

        private bool DoesCurrentUserLikes()
        {
            int itemId = this.ItemId;
            string listId = this.ListId.ToString();
            string webId = this.Web.ID.ToString();
            return DoesItemExists(itemId, listId, webId);
        }

        private bool DoesItemExists(int itemId, string listId, string webId)
        {
            bool doesItemExist = false;
            SPSite site = this.Web.Site;
            using (SPWeb web = site.OpenWeb("/"))
            {
                SPList userInterestList = web.Lists.TryGetList("User Interest List");
                if (userInterestList != null)
                {
                    SPQuery query = new SPQuery();
                    query.Query = string.Format(@"<Where>
                                        <And>
                                            <And>
                                                <And>   
                                                    <Eq>    
                                                        <FieldRef Name='WebID'/><Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                    <Eq>
                                                        <FieldRef Name='ListID'/><Value Type='Text'>{1}</Value>
                                                    </Eq>
                                                </And>
                                                <Eq>
                                                    <FieldRef Name='ItemID'/><Value Type='Text'>{2}</Value>
                                                </Eq>
                                            </And>
                                            <Eq>
                                                <FieldRef Name='User'/><Value Type='Integer'><UserID/></Value>
                                            </Eq>
                                        </And>
                                    </Where>", webId, listId, itemId);
                    query.RowLimit = 1;

                    SPListItemCollection items = userInterestList.GetItems(query);
                    if (items.Count > 0)
                    {
                        doesItemExist = true;
                    }
                }
            }
            return doesItemExist;
        }

        public override string DisplayTemplateName
        {
            get
            {
                base.DisplayTemplateName = "UserInterestFieldTemplate";
                return base.DisplayTemplateName;
            }
            set
            {
                base.DisplayTemplateName = value;
            }
        }
        //public override object Value
        //{
        //    get
        //    {
        //        EnsureChildControls();
        //        _value = Convert.ToDouble(base.Value);
        //        SetValue(_value);
        //        return _value;
        //    }
        //    set
        //    {
        //        EnsureChildControls();
        //        _value = Convert.ToDouble(value);
        //        SetValue(_value);
        //        base.Value = _value;
        //    }
        //}

        //private void SetValue(double value)
        //{
        //    _value = value;
        //    //lblLikesCount.Text = "Likes Count:" + value;
        //}

    }
}
