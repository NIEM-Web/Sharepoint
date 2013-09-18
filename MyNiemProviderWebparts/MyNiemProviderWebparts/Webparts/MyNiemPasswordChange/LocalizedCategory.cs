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
    internal class LocalizedCategoryAttribute : CategoryAttribute
    {
        private string _resourceSource;

        public LocalizedCategoryAttribute(string resourceSource, string category)
            : base(category)
        {
            _resourceSource = resourceSource;
        }

        protected override string GetLocalizedString(string value)
        {
            return LocalizedString.GetString(_resourceSource, base.Category);
        }

    }
}
