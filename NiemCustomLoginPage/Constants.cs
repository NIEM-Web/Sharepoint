using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lmd.NIEM.FarmSolution
{
    public class Constants
    {
        //template for columns Col_ListName_ColumnName
        //template for ListNames List_ListName

        #region Column Names
        public const string Col_RedirectionMapping_Title = "Title";
        public const string Col_RedirectionMapping_RedirectToUrl = "RedirectToUrl";
        public const string Col_RedirectionMapping_RedirectType = "RedirectType";
        public const string Col_RedirectionMapping_HttpAction = "HttpAction";
        public const string Col_RedirectionMapping_Priority = "Priority";
        public const string Col_RedirectionMapping_ID = "ID";
        public const string Col_RedirectionMapping_IsExternal = "IsExternal";
        #endregion

        #region List Urls
        public const string List_Prefix = "Lists/";
        public const string List_RedirectionMapping_Url = List_Prefix + "RedirectionMapping";
        #endregion

        #region Cache Keys
        public const string Cache_RedirectionData = "redirectionData";
        #endregion

        #region Redirection Mapping Types
        public const string Redirection_Single = "single";
        public const string Redirection_Folder = "folder mapping";
        public const string Redirection_Wildcard = "wildcard";
        #endregion

        #region General
        public const string Protocol_Default = "http://";
        public const string Protocol_Http = "http://";
        public const string Protocol_Https = "https://";
        #endregion
    }
}
