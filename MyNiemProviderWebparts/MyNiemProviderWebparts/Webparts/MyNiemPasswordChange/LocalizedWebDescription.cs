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
    internal class LocalizedWebDescriptionAttribute : WebDescriptionAttribute
    {
        private bool _localized;
        private string _resourceSource;

        public LocalizedWebDescriptionAttribute(string resourceSource, string description)
            : base(description)
        {
            _resourceSource = resourceSource;
        }

        public override string Description
        {
            get
            {
                if (!_localized)
                {
                    DescriptionValue = LocalizedString.GetString(_resourceSource, base.Description);
                    _localized = true;
                }
                return base.Description;
            }
        }
    }
}
