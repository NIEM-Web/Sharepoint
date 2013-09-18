using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace lmd.NIEM.FarmSolution.SubscribeForEmailWP
{
    [ToolboxItemAttribute(false)]
    public class SubscribeForEmailWP : WebPart
    {
            [Personalizable(PersonalizationScope.Shared),
         WebBrowsable(true),
         WebDisplayName("List Path"),
         WebDescription("Path to the Email Subscription List"),
         Category("Custom Configuration")]
        [DefaultValue("Lists/EmailSubscriptions")]
        public string ListPath
        {
            get;
            set;
        }
          
        [Personalizable(PersonalizationScope.Shared),
            WebBrowsable(true),
            WebDisplayName("Email Field"),
            WebDescription("Name of the  Email Field"),
            Category("Custom Configuration")]
        [DefaultValue("EmailId")]
            public string EmailId
            {
                get;
                set;
            }

            [Personalizable(PersonalizationScope.Shared),
            WebBrowsable(true),
            WebDisplayName("First Name Field"),
            WebDescription("Name of the First Name Field"),
            Category("Custom Configuration")]
            [DefaultValue("Title")]
            public string FirstNameField
            {
                get;
                set;
            }

            [Personalizable(PersonalizationScope.Shared),
              WebBrowsable(true),
              WebDisplayName("Last Name Field"),
              WebDescription("Name of the Last Name Field"),
              Category("Custom Configuration")]
            [DefaultValue("LastName")]
            public string LastNameField
            {
                get;
                set;
            }



        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
            private const string _ascxPath = @"~/_CONTROLTEMPLATES/lmd.NIEM.FarmSolution/SubscribeForEmailWPUserControl.ascx";

        protected override void CreateChildControls()
        {
            SubscribeForEmailWPUserControl control = (SubscribeForEmailWPUserControl)Page.LoadControl(_ascxPath);
            control.LstEmailSubscriptionUrl = ListPath;
            control.FldEmail = EmailId;
            control.FldFirstName = FirstNameField;
            control.FldLastName = LastNameField;
            Controls.Add(control);
        }
    }
}
