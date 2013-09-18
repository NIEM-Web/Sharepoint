using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.IO;
using System.Web.Caching;
using System.Data;
using System.Globalization;

namespace lmd.NIEM.FarmSolution.HttpModules
{
    public class RedirectModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            //application parameters
            HttpApplication application = sender as HttpApplication;
            HttpRequest applicationRequest = application.Request;
            string path = applicationRequest.Url.GetComponents(UriComponents.Path, UriFormat.UriEscaped);
            string extension = Path.GetExtension(path);
            string queryString = applicationRequest.Url.GetComponents(UriComponents.Query, UriFormat.UriEscaped);

            //if the page extension is aspx or html or none i.e don't do for JS or any other files
            if (extension.Equals(".aspx") || extension.Equals(".html") || extension.Equals(".htm") || string.IsNullOrEmpty(extension))
            {
                //get redirection url from mapping list
                string status = string.Empty;
                string redirectionUrl = GetRedirectionUrl(application, path, queryString, out status);

                if (redirectionUrl != null)
                {
                    HttpResponse applicationResponse = application.Response;
                    applicationResponse.Status = status;
                    applicationResponse.RedirectLocation = redirectionUrl;
                    applicationResponse.Redirect(redirectionUrl, true);
                }
            }
        }
        
        #region Methods
        
        private string GetRedirectionUrl(HttpApplication application, string path, string queryString, out string status)
        {
            string redirectionUrl = null;
            status = string.Empty;
            try
            {
                HttpRequest applicationRequest = application.Request;
                HttpResponse applicationResponse = application.Response;

                DataTable redirectionUrls = GetRedirectionDataTable(application);
                
                redirectionUrl = GetRedirectionalUrlFromDataTable(redirectionUrls, path, queryString, out status);

            }
            catch (Exception)
            {
                //do nothing
            }
            return redirectionUrl;
        }

        private DataTable GetRedirectionDataTable(HttpApplication application)
        {
            DataTable redirectionUrls = null;

            HttpRequest applicationRequest = application.Request;

            Cache cache = application.Context.Cache;
            //if cache is not there then get data from sp list
            if (cache[Constants.Cache_RedirectionData] == null)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(applicationRequest.Url.ToString()))
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList redirectionMappingList = web.GetList(Constants.List_RedirectionMapping_Url);
                        SPQuery query = new SPQuery();
                        query.Query = @" <OrderBy>
                                                <FieldRef Name='" + Constants.Col_RedirectionMapping_Priority + @"' />
                                                <FieldRef Name='Created' Ascending='False' />
                                             </OrderBy>";
                        query.ViewFields = @"<FieldRef Name='" + Constants.Col_RedirectionMapping_Title + @"' />
                                                 <FieldRef Name='" + Constants.Col_RedirectionMapping_RedirectType + @"' />
                                                 <FieldRef Name='" + Constants.Col_RedirectionMapping_RedirectToUrl + @"' />
                                                 <FieldRef Name='" + Constants.Col_RedirectionMapping_Priority + @"' />
                                                 <FieldRef Name='" + Constants.Col_RedirectionMapping_ID + @"' />
                                                 <FieldRef Name='" + Constants.Col_RedirectionMapping_IsExternal + @"' />
                                                 <FieldRef Name='" + Constants.Col_RedirectionMapping_HttpAction + @"' />";
                        query.ViewFieldsOnly = true;
                        SPListItemCollection redirectionItems = redirectionMappingList.GetItems(query);
                        redirectionUrls = redirectionItems.GetDataTable();
                        cache.Insert(Constants.Cache_RedirectionData, redirectionUrls, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                });
            }
            else
            {
                redirectionUrls = cache[Constants.Cache_RedirectionData] as DataTable;
            }
            return redirectionUrls;
        }

        private string GetRedirectionalUrlFromDataTable(DataTable redirectionUrls, string path, string queryString, out string status)
        {
            string redirectionUrl = null;
            string fileName = Path.GetFileName(path);
            string folderName = path.Substring(0, path.LastIndexOf(fileName));
            char[] trimChars = new char[] { '/' };
            folderName = folderName.Trim(trimChars);

            Uri uriFromFolder = new Uri(folderName, UriKind.RelativeOrAbsolute);
            status = string.Empty;
            foreach (DataRow row in redirectionUrls.Rows)
            {

                string mappedUrl = row[Constants.Col_RedirectionMapping_Title].ToString();
                mappedUrl = mappedUrl.Trim(trimChars);

                //if mapped url is equal to folder name of the requested url or
                //its equal to path
                if (string.Compare(mappedUrl, folderName, true) == 0
                    || string.Compare(mappedUrl, path, true) == 0 || folderName.StartsWith(mappedUrl + "/", StringComparison.OrdinalIgnoreCase))
                {
                    string redirectionType = Convert.ToString(row[Constants.Col_RedirectionMapping_RedirectType]).ToLower();
                    int isExternal = Convert.ToInt32(row[Constants.Col_RedirectionMapping_IsExternal]);
                    if (string.Compare(redirectionType, Constants.Redirection_Single, true) == 0)
                    {
                        redirectionUrl = Convert.ToString(row[Constants.Col_RedirectionMapping_RedirectToUrl]);
                        if (isExternal == 1)
                        {
                            if (!(redirectionUrl.StartsWith(Constants.Protocol_Http, true, CultureInfo.InstalledUICulture) ||
                                redirectionUrl.StartsWith(Constants.Protocol_Https, true, CultureInfo.InstalledUICulture)))
                            {
                                redirectionUrl = Constants.Protocol_Default + redirectionUrl;
                                
                            }
                        }
                        status = Convert.ToString(row[Constants.Col_RedirectionMapping_HttpAction]);
                        break;
                    }
                    else if (string.Compare(redirectionType, Constants.Redirection_Folder, true) == 0 || folderName.StartsWith(mappedUrl + "/", StringComparison.OrdinalIgnoreCase))
                    {
                        string subFolder = folderName.Substring(folderName.IndexOf(mappedUrl));
                        redirectionUrl = Convert.ToString(row[Constants.Col_RedirectionMapping_RedirectToUrl])
                            + "/" + (string.IsNullOrEmpty(subFolder) ? string.Empty : subFolder + "/")
                            + fileName + queryString;
                        if (isExternal == 1)
                        {
                            if (!(redirectionUrl.StartsWith(Constants.Protocol_Http, true, CultureInfo.InstalledUICulture) ||
                                redirectionUrl.StartsWith(Constants.Protocol_Https, true, CultureInfo.InstalledUICulture)))
                            {
                                redirectionUrl = Constants.Protocol_Default + redirectionUrl;
                                
                            }
                        }
                        status = Convert.ToString(row[Constants.Col_RedirectionMapping_HttpAction]);
                        break;
                    }
                    else if (string.Compare(redirectionType, Constants.Redirection_Wildcard, true) == 0 && folderName.StartsWith(mappedUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        string redirectionPath = Convert.ToString(row[Constants.Col_RedirectionMapping_RedirectToUrl]);
                        string protocolPrefix = string.Empty;
                        if (isExternal == 1)
                        {
                            if (!(redirectionPath.StartsWith(Constants.Protocol_Http, true, CultureInfo.InstalledUICulture) ||
                                redirectionPath.StartsWith(Constants.Protocol_Https, true, CultureInfo.InstalledUICulture)))
                            {
                                protocolPrefix = Constants.Protocol_Default;
                            }
                        }

                        redirectionUrl = protocolPrefix + redirectionPath;
                        status = Convert.ToString(row[Constants.Col_RedirectionMapping_HttpAction]);
                        break;
                    }
                }
            }
            return redirectionUrl;
        }
        #endregion
    }
}