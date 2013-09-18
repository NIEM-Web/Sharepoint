using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.IdentityModel;
using Microsoft.SharePoint.IdentityModel.Pages;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Administration;
using System.Web;
using Microsoft.SharePoint.Utilities;

namespace lmd.NIEM.FarmSolution
{
    public partial class NeimLoginForm : FormsSignInPage
    {

        protected override bool AllowAnonymousAccess
        {
            get
            {
                return true;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
            }
            catch { }
        }


        protected void lbInternalUsers_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (null != SPContext.Current && null != SPContext.Current.Site)
                {
                    SPIisSettings iisSettings = SPContext.Current.Site.WebApplication.IisSettings[SPUrlZone.Default];
                    if (null != iisSettings && iisSettings.UseWindowsClaimsAuthenticationProvider)
                    {
                        SPAuthenticationProvider provider = iisSettings.WindowsClaimsAuthenticationProvider;
                        Redirect(provider);
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void Redirect(SPAuthenticationProvider provider)
        {
            string comp = HttpContext.Current.Request.Url.GetComponents(UriComponents.Query, UriFormat.SafeUnescaped);
            string url = provider.AuthenticationRedirectionUrl.ToString();
            if (provider is SPWindowsAuthenticationProvider)
            {
                comp = EnsureUrl(comp, true);
            }

            SPUtility.Redirect(url, SPRedirectFlags.Default, this.Context, comp);
        }

        private string EnsureUrl(string url, bool urlIsQueryStringOnly)
        {
            if (!url.Contains("ReturnUrl="))
            {
                if (urlIsQueryStringOnly)
                {
                    url = url + (string.IsNullOrEmpty(url) ? "" : "&");
                }
                else
                {
                    url = url + ((url.IndexOf('?') == -1) ? "?" : "&");
                }
                url = url + "ReturnUrl=";
            }
            return url;
        }
    }

}


