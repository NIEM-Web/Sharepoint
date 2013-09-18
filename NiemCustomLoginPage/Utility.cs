using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.Office.Server.UserProfiles;

namespace lmd.NIEM.FarmSolution
{
    public class Utility
    {
        public static List<UserDetail> GetUserDetails()
        {
            List<UserDetail> userDetails = new List<UserDetail>();
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                {
                    SPUserCollection users = web.SiteUsers;

                    SPServiceContext context = SPServiceContext.Current;
                    UserProfileManager upm = new UserProfileManager(context);

                    foreach (SPUser user in users)
                    {
                        UserProfile profile = null;
                        if (upm.UserExists(user.LoginName))
                        {
                            profile = upm.GetUserProfile(user.LoginName);
                        }
                        UserDetail userDetail = GetUserDetail(user, profile);
                        userDetails.Add(userDetail);
                    }
                }
            });
            return userDetails;
        }

        private static UserDetail GetUserDetail(SPUser user, UserProfile profile)
        {
            UserDetail userDetail = new UserDetail();
            userDetail.Email = GetPropertyValue(profile, "WorkEmail");
            userDetail.LoginName = user.LoginName;
            userDetail.FirstName = GetPropertyValue(profile, "FirstName");
            userDetail.LastName = GetPropertyValue(profile, "LastName");
            userDetail.Name = user.Name;
            return userDetail;
        }

        private static string GetPropertyValue(UserProfile profile, string propertyName)
        {
            try
            {
                return Convert.ToString(profile[propertyName]);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
