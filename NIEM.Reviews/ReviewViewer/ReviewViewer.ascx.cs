using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Xml.Serialization;
using Microsoft.SharePoint.WebPartPages;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;

namespace NIEM.Reviews.ReviewViewer
{
    [ToolboxItem(false)]
    public partial class ReviewViewer : System.Web.UI.WebControls.WebParts.WebPart
    {

        #region data model
        //Internal data model abastraction for SP List Data
        private struct ReviewModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public DateTime PublishDate { get; set; }
            public string Body { get; set; }
            public string Author { get; set; }

        }
        #endregion data model

        #region properties

        [Browsable(true)]
        [WebBrowsable(true)]
        [Category("Custom Properties")]
        [Description("Specify the list to receive reviews from.")]
        [XmlElement(ElementName = "ListName")]
        [DefaultValue(defaultListName)]
        [FriendlyName("ListName")]
        [Personalizable(true)]
        public string ListName { get; set; }

        #endregion properties

        #region private data

        private string articleTitle = string.Empty;
        private string articleUrl = string.Empty;
        private string formHyperlinkText = string.Empty;
        private string formHyperlink = string.Empty;
        private bool linkCreated = false;


        private const string defaultListName = "Reviews";

        #endregion private data

        /// <summary>
        /// Creates and initializes a new instance of the ReviewViewer web part.
        /// </summary>
        public ReviewViewer() :base()
        {
            if (String.IsNullOrEmpty(ListName))
            {
                ListName = defaultListName;
            }
        }

        /// <summary>
        /// Initializes control elements for the web part.
        /// </summary>
        /// <param name="e">Default <see cref="EventArgs"/>.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        /// <summary>
        /// Handles the loading of data withing the control. If data is found it is bound to the default repeater.
        /// </summary>
        /// <param name="sender">The caller of the event.</param>
        /// <param name="e">Default <see cref="EventArgs"/>.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            articleUrl = HttpContext.Current.Request.QueryString["url"];
            articleTitle = HttpContext.Current.Request.QueryString["title"];
            notificationSection.Visible = false;

            //Only try and load data if an article is specified.
            if (!string.IsNullOrEmpty(articleUrl))
            {
                try
                {
                    var list = SPContext.Current.Site.RootWeb.Lists[ListName];
                    var query = GetQuery(articleUrl);
                    var listItems = list.GetItems(query);
                    BindData(PopulateDataValues(listItems));
                }
                catch (Exception ex)
                {
                    ShowNotificationMessage("An error occurred<br>" + ex.Message);
                }
            }
            else
            {
                ShowNotificationMessage("No query was passed, so no items found.");
            }
        }

        /// <summary>
        /// Binds the main repeater control to data found.
        /// </summary>
        /// <param name="data">A <see cref="List"/> of <see cref="ReviewModel"/ data elements.></param>
        private void BindData(List<ReviewModel> data)
        {
            if (data.Count > 0)
            {
                reviewListContainer.Visible = true;
                reviewListContainer.DataSource = data;
                reviewListContainer.DataBind();
            }
            else
            {
                CreateFormHyperlink(true, string.Empty);
                ShowNotificationMessage(String.Format("There are no reviews for <strong>{0}</strong>. <a href=\"{1}\"'>Why don't you add one?</a><br/><br/> <a href='javascript:history.go(-1)'>Go back</a>", articleTitle, formHyperlink));
                
            }
        }

        /// <summary>
        /// Creates a URL based on whether or not the calling user has created a review for the article specified. If the user hasn't
        /// created a review then it directs the user to the NewForm page otherwise it directs them to the EditForm page.
        /// </summary>
        /// <param name="createMode">Specifies whether the link should go to the Newform page (if true) or the EditForm page (if false).</param>
        /// <param name="itemId">The review item id.</param>
        private void CreateFormHyperlink(bool createMode, string itemId)
        {
            if (!linkCreated)
            {
                if (createMode == true)
                {
                    formHyperlinkText = "Create Review";
                    formHyperlink = string.Format("javascript:CreateEditReview('{0}','{1}')", "/Lists/Reviews/NewForm.aspx", articleUrl);
                }
                else
                {
                    formHyperlinkText = "Edit Review";
                    formHyperlink = string.Format("javascript:CreateEditReview('/Lists/Reviews/EditForm.aspx?ID={0}','{1}')", itemId, articleUrl);
                    //if a user has a review, then make sure they have an edit link.
                    linkCreated = true;
                }
            }
        }

        /// <summary>
        /// Converts the list items returned from the data query to a <see cref="List"/> of <see cref="ReviewModel"/> items. This
        /// method will also create the create / edit form hyperlink.
        /// </summary>
        /// <param name="listItems">A <see cref="SPListItemCollection"/> passed from the query.</param>
        /// <returns>A <see cref="List"/> of <see cref="ReviewModel"/> items.</returns>
        private List<ReviewModel> PopulateDataValues(SPListItemCollection listItems)
        {
            var data = new List<ReviewModel>();
            foreach (SPListItem item in listItems)
            {
                string author = item["Author"].ToString();
           
                if (IsUserAuthor(author))
                {
                    CreateFormHyperlink(false, item["ID"].ToString());
                }
                else
                {
                    CreateFormHyperlink(true, string.Empty);
                }

                data.Add(new ReviewModel
                {
                    Id = (int)item["ID"],
                    Title = item["Title"] as string,
                    PublishDate = (DateTime)item["Modified"],
                    Body = item["Review"].ToString(),
                    Author = FormatAuthor(author)
                    
                });
            }

            return (data);
        }

        /// <summary>
        /// Formats the the author text displayed on the page. Since the site uses mixed mode (claims and Windows) authentication, 
        /// the name format is not consistent.
        /// </summary>
        /// <param name="author">The author of the review.</param>
        /// <returns>This will return either the email address if found or the user's name.</returns>
        private string FormatAuthor(string author)
        {
            SPUser creator = new SPFieldUserValue(SPContext.Current.Web, author).User;
            return (string.IsNullOrEmpty(creator.Email) ? creator.Name : creator.Email);
        }

        /// <summary>
        /// Determines whether the current logged in user is the author of a specific review.
        /// </summary>
        /// <param name="author">The author of the specified review.</param>
        /// <returns>True if the author and the current user are the same, false otherwise.</returns>
        private bool IsUserAuthor(string author)
        {
            SPUser creator = new SPFieldUserValue(SPContext.Current.Web, author).User;
            SPUser currentUser = SPContext.Current.Web.CurrentUser;
           
           return creator.ID.Equals(currentUser.ID);
        }

        /// <summary>
        /// Creates an <see cref="SPQuery"/> object that uses an article url as a filter.
        /// </summary>
        /// <param name="url">The url of an article to filter on.</param>
        /// <returns>An <see cref="SPQuery"/> object.</returns>
        private SPQuery GetQuery(string url)
        {
            SPQuery query = new SPQuery();
            query.ViewFields = String.Concat("<FieldRef Name='ID' />"
                                              , "<FieldRef Name='Title' />"
                                              , "<FieldRef Name='Modified' />"
                                              , "<FieldRef Name='Review' />"
                                              , "<FieldRef Name='WebId' />"
                                              , "<FieldRef Name='ListId' />"
                                              , "<FieldRef Name='ItemId' />"
                                              , "<FieldRef Name='Url' />"
                                              , "<FieldRef Name='Author' />");

            query.Query = String.Concat("<Where>"
                                          , "<Eq>"
                                             , "<FieldRef Name='Url' />"
                                             , "<Value Type='Text'>"
                                             ,url
                                             ,"</Value>"
                                          , "</Eq>"
                                       , "</Where>"
                                       , "<OrderBy>"
                                          , "<FieldRef Name='Modified' Ascending='FALSE' />"
                                       , "</OrderBy>");
            return (query);
        }

        /// <summary>
        /// Used to display notification messages to the end user.
        /// </summary>
        /// <param name="message">The message to display.</param>
        private void ShowNotificationMessage(string message)
        {
            reviewListContainer.Visible = false;
            notificationSection.Visible = true;
            notificationSection.Text = message;
        }


    }
}
