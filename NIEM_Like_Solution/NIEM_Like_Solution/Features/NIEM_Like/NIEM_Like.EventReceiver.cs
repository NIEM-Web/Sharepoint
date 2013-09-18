using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Linq;

namespace NIEM_Like_Solution.Features.NIEM_Like
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("740eb01e-3778-49c7-af41-77c194686e7b")]
    public class NIEM_LikeEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        void LogText(string message)
        {
          // System.IO.File.AppendAllText("C:\\temp\\log.txt", message + "\r\n");
        }
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                //get the current web object.
                SPWeb web = properties.Feature.Parent as SPWeb;
                LogText("1 - " + web.Url);
                SPWeb rootWeb = web.Site.RootWeb;
                LogText("2 - " + rootWeb.Url);
                SPList list = rootWeb.EnsureList("LikeStatus", "This list contains User likes details.", SPListTemplateType.GenericList);
                

                if (list != null)
                {
                    LogText("3 - " + list.DefaultViewUrl);
                    SPField field = list.EnsureField("Title", "", SPFieldType.Text, true);
                    field.Title = "URL";
                    field.Update();
                    LogText("4 - field updated.");
                    SPField fld = null;

                    fld = list.EnsureField("WebID", "Web ID", SPFieldType.Text, true);

                    if (fld != null)
                    {
                        SPView view = list.Views[0];
                        view.ViewFields.Add(fld);
                        view.Update();
                        LogText("5 - field updated.");
                        fld = null;
                    }



                    fld = list.EnsureField("ListID", "List ID", SPFieldType.Text, true);

                    if (fld != null)
                    {
                        SPView view = list.Views[0];
                        view.ViewFields.Add(fld);
                        view.Update();
                        LogText("6 - field updated.");
                        fld = null;
                    }

                    fld = list.EnsureField("ItemID", "Item ID", SPFieldType.Integer, true);

                    if (fld != null)
                    {
                        SPView view = list.Views[0];
                        view.ViewFields.Add(fld);
                        view.Update();
                        LogText("7 - field updated.");
                        fld = null;
                    }


                    fld = list.EnsureField("SPUser", "User", SPFieldType.Text, true);
                    
                    if (fld != null)
                    {
                        SPView view = list.Views[0];
                        view.ViewFields.Add(fld);
                        view.Update();
                        LogText("8 - field updated.");
                        fld = null;
                    }

                    fld = list.EnsureField("CType", "Content Type", SPFieldType.Text, false);

                    if (fld != null)
                    {
                        SPView view = list.Views[0];
                        view.ViewFields.Add(fld);
                        view.Update();
                        LogText("9 - field updated.");
                        fld = null;
                    }

                   // //SPUser oUser = web.Site.Owner;
                   //// SPMember oMember = web.Site.Owner;
                   //// web.SiteGroups.Add("NiemLikes", oMember, oUser, "User group for manage likes");
                   // SPRoleAssignment roleAssignment = new SPRoleAssignment("c:0(.s|true", string.Empty, string.Empty, string.Empty);
                   //// SPRoleAssignment roleAssignment = new SPRoleAssignment(web.SiteGroups["NiemLikes"]);
                   // roleAssignment.RoleDefinitionBindings.Add(web.RoleDefinitions["Full Control"]);
                   // web.RoleAssignments.Add(roleAssignment);
                   // if (!list.HasUniqueRoleAssignments)
                   // {
                   //     list.BreakRoleInheritance(true); // Ensure we don't inherit permissions from parent
                   // }
                   // list.RoleAssignments.Add(roleAssignment);
                   // list.Update();

                   // web.Update();
                   // // "c:0(.s|true" = All authenticated users
                   // //web.SiteGroups["NiemLikes"].AddUser("c:0(.s|true", string.Empty, string.Empty, string.Empty);
                   // web.Update();


                    
                    
                    
                    
                    
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                LogText(ex.Message);
                throw ex;
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
            //    SPWeb web = properties.Feature.Parent as SPWeb;
              //  SPWeb rootWeb = web.Site.RootWeb;
             //   rootWeb.DeleteList("LikeStatus");
             //   rootWeb.SiteGroups.Remove("NiemLikes");
             //   rootWeb.Update();
            }
            catch (Exception)
            { }
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

    public static class Utilities
    {

        public static SPList EnsureList(this SPWeb web, string listTitle, string desc, SPListTemplateType lstTemplateType)
        {

            SPListCollection lstCollection = web.Lists;


            SPList lstObj = (from SPList lst in lstCollection
                             where string.Equals(lst.Title, listTitle, StringComparison.InvariantCultureIgnoreCase) == true
                             select lst).FirstOrDefault();


            if (lstObj != null)
            {
                return lstObj;
            }

            Guid lstGuid = web.Lists.Add(listTitle, desc, lstTemplateType);
            try
            {
                SPList newList = web.Lists.GetList(lstGuid, true);
                newList.OnQuickLaunch = true;
                newList.Update();
                return newList;
            }
            catch
            {
                return null;
            }
        }

        public static SPField EnsureField(this SPList list, string fldDisplayName, string fldDesc, SPFieldType fldType, bool isMadatory)
        {

            SPFieldCollection fieldCollection = list.Fields;


            SPField spField = (from SPField field in fieldCollection
                               where string.Equals(field.Title, fldDisplayName, StringComparison.InvariantCultureIgnoreCase) == true
                               select field).FirstOrDefault();


            if (spField != null)
            {
                return spField;
            }

            try
            {
                list.Fields.Add(fldDisplayName, fldType, isMadatory);
                SPField spfield = list.Fields.GetField(fldDisplayName);
                spfield.Description = fldDesc;
                spfield.Update();
                return spfield;
            }
            catch
            {
                return null;
            }
        }

        public static bool DeleteList(this SPWeb web, string listTitle)
        {

            SPListCollection lstCollection = web.Lists;


            SPList lstObj = (from SPList lst in lstCollection
                             where
                                 string.Equals(lst.Title, listTitle, StringComparison.InvariantCultureIgnoreCase) == true
                             select lst).FirstOrDefault();


            if (lstObj != null)
            {
                try
                {
                    lstObj.Delete();
                    return true;
                }
                catch (Exception ex)
                {
                }
            }
            return false;
        }
    }
}
