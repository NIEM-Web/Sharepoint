using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace Niem.MyNiem.Receivers
{
    enum EventType
    {
        Added,
        Deleting
    }
    public class UserInterestListEventReceiver : SPItemEventReceiver
    {
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            UpdateLikeColumn(properties, EventType.Added);
        }


        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
        }
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            try
            {
                UpdateLikeColumn(properties, EventType.Deleting);
            }
            catch { }
            base.ItemDeleting(properties);
        }

        private void UpdateLikeColumn(SPItemEventProperties properties, EventType eventType)
        {
            Guid siteId = properties.SiteId;
            SPListItem userInterestListItem = properties.ListItem;
            SPList userInterestList = properties.List;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                Guid webId = new Guid(Convert.ToString(userInterestListItem["WebID"]));
                Guid listId = new Guid(Convert.ToString(userInterestListItem["ListID"]));
                int itemId = Convert.ToInt32(userInterestListItem["ItemID"]);
                if (webId != Guid.Empty && listId != Guid.Empty && itemId > 0)
                {
                    using (SPSite site = new SPSite(siteId))
                    {

                        SPWeb web = site.OpenWeb(webId);
                        try
                        {
                            if (web != null)
                            {
                                try
                                {
                                    SPQuery query = new SPQuery();
                                    query.Query = string.Format(@"<Where>
			                                                        <And>
				                                                        <And>
					                                                        <Eq>
						                                                        <FieldRef Name='WebID'/>
						                                                        <Value Type='Text'>{0}</Value>
					                                                        </Eq>
					                                                        <Eq>
						                                                        <FieldRef Name='ListID'/>
						                                                        <Value Type='Text'>{1}</Value>
					                                                        </Eq>
				                                                        </And>
				                                                        <Eq>
					                                                        <FieldRef Name='ItemID'/>
					                                                        <Value Type='Text'>{2}</Value>
				                                                        </Eq>
			                                                        </And>
		                                                        </Where>", webId, listId, itemId);
                                    query.ViewFields = "<FieldRef Name='ID'/>";
                                    query.ViewFieldsOnly = true;
                                    SPListItemCollection items = userInterestList.GetItems(query);
                                    var likesCount = items.Count;

                                    SPList list = web.Lists[listId];
                                    SPListItem item = list.GetItemById(itemId);
                                    if (item.Fields.ContainsField("Like"))
                                    {
                                        if (eventType == EventType.Added)
                                        {
                                            item["Like"] = Convert.ToDouble(likesCount);
                                        }
                                        else if (eventType == EventType.Deleting && likesCount > 0)
                                        {
                                            item["Like"] = Convert.ToDouble(likesCount) - 1;
                                        }
                                        item.SystemUpdate(false);
                                    }
                                }
                                catch (ArgumentNullException)
                                {
                                }
                                catch (ArgumentException)
                                {
                                }

                            }
                            else
                            {
                                //web doesn't exist with that web id
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            if (web != null)
                            {
                                web.Dispose();
                            }
                        }
                    }
                }
                else
                {
                    //invalid WebId
                }
            });
        }

    }
}
