using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace lmd.NIEM.FarmSolution.SubscribeForEmailWP
{
    public partial class SubscribeForEmailWPUserControl : UserControl
    {
        #region Constants
        private const string SuccessfulMessage = "Thank you for showing your interest. You have been registered for email subscriptions successfully!";
        private const string UnsuccessfulMessage = "Thank you for showing your interest. Sorry, we couldn't register you for email alerts!";
        public  string FldFirstName { get; set;} //= "Title";
        public string FldLastName { get; set; } // "LastName";
        public string FldEmail { get; set; } // "EmailId";
        public string LstEmailSubscriptionUrl
        {
            get;
            set;
        }
        //private  string LstEmailSubscriptionUrl = "Lists/EmailSubscriptions";
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        // Summary:
        //      Click event of subscribe button. Adds entry in the 
        //      Email Subscription List.
        protected void BtnSubscribe_Click(object sender, EventArgs e)
        {
            try
            {
                //validates the input
                ValidateInput(TxtFirstName.Text, TxtLastName.Text, TxtEmail.Text);
                //add entry
                SubscribeUserForEmailAlerts(TxtFirstName.Text, TxtLastName.Text, TxtEmail.Text);
                //clean UI
                RefreshUI();
                //success
                LblMessage.Text = SuccessfulMessage;
            }
            catch (Exception ex)
            {
                //failure
                LblMessage.Text = UnsuccessfulMessage +ex.Message;
                //Todo: Log error.
            }
        }
        #endregion

        #region Private Methods
        // Summary:
        //      Validates the input
        // Parameters:
        //      firstName:
        //          A string object that represents the first name.
        //      lastName:
        //          A string object that represents the last name.
        //      email:
        //          A string object that represents the email id. This should be unique in the list.
        private void ValidateInput(string firstName, string lastName, string email)
        {
            if (string.IsNullOrEmpty(firstName.Trim()) || string.IsNullOrEmpty(lastName.Trim()) || string.IsNullOrEmpty(email.Trim()))
            {
                throw new Exception("Validation exception");
            }
        }
        // Summary:
        //      clears the UI screen
        private void RefreshUI()
        {
            TxtEmail.Text = string.Empty;
            TxtFirstName.Text = string.Empty;
            TxtLastName.Text = string.Empty;
        }
        // Summary:
        //      Adds entries in the email subscription list
        // Parameters:
        //      firstName:
        //          A string object that represents the first name.
        //      lastName:
        //          A string object that represents the last name.
        //      email:
        //          A string object that represents the email id. This should be unique in the list.
        private void SubscribeUserForEmailAlerts(string firstName, string lastName, string email)
        {
            //run with elevated priveleges as items need to be inserted 
            //by anonymous users
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    site.AllowUnsafeUpdates = true;
                    //get reference to root web, no need to dispose
                    SPWeb rootWeb = site.RootWeb;
                    rootWeb.AllowUnsafeUpdates = true;
                    SPList emailSubscriptionList = rootWeb.GetList(LstEmailSubscriptionUrl);
                    //add item
                    SPListItem newItem = emailSubscriptionList.AddItem();
                    //fill the fields
                    //FirstName
                    newItem["Title"] = firstName.Trim() + " " + lastName.Trim();
 
                    newItem[FldFirstName] = firstName.Trim();
                    newItem[FldLastName] = lastName.Trim();
                    newItem[FldEmail] = email.Trim();
                    newItem.Update();
                }
            });
        }
        #endregion
    }
}
