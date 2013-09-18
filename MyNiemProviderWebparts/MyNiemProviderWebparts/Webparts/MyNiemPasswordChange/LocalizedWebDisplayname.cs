using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Resources;
using Microsoft.SharePoint.Utilities;
using System.Globalization;
using System.ComponentModel;

namespace MyNiemProviderWebparts.Webparts.MyNiemPasswordChange
{
    internal class LocalizedWebDisplayNameAttribute : WebDisplayNameAttribute
    {
        private bool _localized;
        private string _resourceSource;

        public LocalizedWebDisplayNameAttribute(string resourceSource, string displayName)
            : base(displayName)
        {
            _resourceSource = resourceSource;
        }

        public override string DisplayName
        {
            get
            {
                if (!_localized)
                {
                    DisplayNameValue = LocalizedString.GetString(_resourceSource, base.DisplayName);
                    _localized = true;
                }
                return base.DisplayName;
            }
        }
    }
}
