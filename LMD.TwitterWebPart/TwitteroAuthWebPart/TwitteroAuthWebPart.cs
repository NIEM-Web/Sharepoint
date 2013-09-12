using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

// Added as part of System.Web.Extensions.
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;


namespace LMD.TwitterWebPart.TwitteroAuthWebPart
{
    [ToolboxItemAttribute(false)]
    public class TwitteroAuthWebPart : System.Web.UI.WebControls.WebParts.WebPart
    {
 
        #region Class Level Variables and Controls

        string _oauth_token = String.Empty;
        string _oauth_token_secret = String.Empty;
        string _oauth_consumer_key = String.Empty;
        string _oauth_consumer_secret = String.Empty;
        string _count = String.Empty;
        string _dateFormat = string.Empty;
        string _screenName = String.Empty;
        string _xslUrl = String.Empty;
        string _cssClass = String.Empty;
        bool _tranformUrls = true;

        #endregion

        #region Properties

        #region Count

        [WebBrowsable(true)
            , Category("Parameters")
            , DefaultValue("5")
            , WebDisplayName("Count")
            , Personalizable(PersonalizationScope.Shared)
            , WebDescription("This is the number of most recent tweets that are returned from Twitter for the provided screen name.  The default value is 5.")
            , FriendlyNameAttribute("Count")
            , WebPartStorage(Storage.Shared)
            , Description("Count")
            , XmlElement(ElementName = "Count")]
        public string Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }

        #endregion

        #region CSSClassName
        [WebBrowsable(true)
           , Category("Parameters")
           , DefaultValue("NIEM-Twitter")
           , WebDisplayName("CSS Class Name")
           , Personalizable(PersonalizationScope.Shared)
           , WebDescription("This specifies which CSS class to use for the outer container.")
           , FriendlyNameAttribute("CSSClassName")
           , WebPartStorage(Storage.Shared)
           , Description("CSSClassName")
           , XmlElement(ElementName = "CSSClassName")]
        public string CSSClassName
        {
            get
            {
                return _cssClass;
            }
            set
            {
                _cssClass = value;
            }
        } 

        #endregion 

        #region TransformURLs
        [WebBrowsable(true)
           , Category("Parameters")
           , DefaultValue("true")
           , WebDisplayName("Transform URLs")
           , Personalizable(PersonalizationScope.Shared)
           , WebDescription("If selected, all urls, mentions and hashtags will be converted to clickable links.")
           , FriendlyNameAttribute("TransformURLs")
           , WebPartStorage(Storage.Shared)
           , Description("TransformURLs")
           , XmlElement(ElementName = "TransformURLs")]
        public bool TransformURLs
        {
            get
            {
                return _tranformUrls;
            }
            set
            {
                _tranformUrls = value;

            }
        }

        #endregion

        #region ScreenName

        [WebBrowsable(true)
            , Category("Parameters")
            , DefaultValue("")
            , WebDisplayName("Screen Name")
            , Personalizable(PersonalizationScope.Shared)
            , WebDescription("This is the Twitter handle to follow.")
            , FriendlyNameAttribute("Screen Name")
            , WebPartStorage(Storage.Shared)
            , Description("Screen Name")
            , XmlElement(ElementName = "ScreenName")]
        public string ScreenName
        {
            get
            {
                return _screenName;
            }
            set
            {
                _screenName = value;
            }
        }

        #endregion

        #region AuthenticationToken

        [WebBrowsable(true)
            , Category("Parameters")
            , DefaultValue("")
            , WebDisplayName("Authentication Token")
            , Personalizable(PersonalizationScope.Shared)
            , WebDescription("This is the Twitter Authentication Token.")
            , FriendlyNameAttribute("Authentication Token")
            , WebPartStorage(Storage.Shared)
            , Description("Twitter Authentication Token")
            , XmlElement(ElementName = "AuthenticationToken")]
        public string oAuth_Token
        {
            get
            {
                return _oauth_token;
            }
            set
            {
                _oauth_token = value;
            }
        }

        #endregion

        #region AuthenticationTokenSecret

        [WebBrowsable(true)
            , Category("Parameters")
            , DefaultValue("")
            , WebDisplayName("Authentication Token Secret")
            , Personalizable(PersonalizationScope.Shared)
            , WebDescription("This is the Twitter Authentication Token Secret.")
            , FriendlyNameAttribute("Authentication Token Secret")
            , WebPartStorage(Storage.Shared)
            , Description("Twitter Authentication Token Secret")
            , XmlElement(ElementName = "AuthenticationTokenSecret")]
        public string oAuth_Token_Secret
        {
            get
            {
                return _oauth_token_secret;
            }
            set
            {
                _oauth_token_secret = value;
            }
        }

        #endregion

        #region ConsumerKey

        [WebBrowsable(true)
            , Category("Parameters")
            , DefaultValue("")
            , WebDisplayName("Consumer Key")
            , Personalizable(PersonalizationScope.Shared)
            , WebDescription("This is the Twitter Consumer Key")
            , FriendlyNameAttribute("Consumer Key")
            , WebPartStorage(Storage.Shared)
            , Description("Twitter Consumer Key")
            , XmlElement(ElementName = "ConsumerKey")]
        public string Consumer_Key
        {
            get
            {
                return _oauth_consumer_key;
            }
            set
            {
                _oauth_consumer_key = value;
            }
        }

        #endregion

        #region ConsumerKeySecret

        [WebBrowsable(true)
            , Category("Parameters")
            , DefaultValue("")
            , WebDisplayName("Consumer Key Secret")
            , Personalizable(PersonalizationScope.Shared)
            , WebDescription("This is the Twitter Consumer Key Secret")
            , FriendlyNameAttribute("Consumer Key Secret")
            , WebPartStorage(Storage.Shared)
            , Description("Twitter Consumer Key Secret")
            , XmlElement(ElementName = "ConsumerKeySecret")]
        public string Consumer_Key_Secret
        {
            get
            {
                return _oauth_consumer_secret;
            }
            set
            {
                _oauth_consumer_secret = value;
            }
        }

        #endregion

        #region XslUrl

        [WebBrowsable(true)
            , Category("Parameters")
            , DefaultValue("")
            , WebDisplayName("Xsl Url")
            , Personalizable(PersonalizationScope.Shared)
            , WebDescription("This is the url to the Xsl file for formatting the Rss twitter output. If no value is specified, then default rendering will occur.")
            , FriendlyNameAttribute("Xsl Url")
            , WebPartStorage(Storage.Shared)
            , Description("Xsl Url")
            , XmlElement(ElementName = "XslUrl")]
        public string XslUrl
        {
            get
            {
                return _xslUrl;
            }
            set
            {
                _xslUrl = value;
            }
        }

        #endregion

        #endregion

        #region "CreateChildControls"

        protected override void CreateChildControls()
        {

            var oauth_token = _oauth_token;
            var oauth_token_secret = _oauth_token_secret;
            var oauth_consumer_key = _oauth_consumer_key;
            var oauth_consumer_secret = _oauth_consumer_secret;
            var count = _count;
            var screenName = _screenName;
            var xslUrl = _xslUrl;

            try
            {
                if (count != String.Empty && screenName != String.Empty
                        && oauth_token != String.Empty && oauth_token_secret != String.Empty
                        && oauth_consumer_key != String.Empty && oauth_consumer_secret != String.Empty)
                {

                    if (string.IsNullOrEmpty(xslUrl))
                    {
                        RenderAsDefault(oauth_token, oauth_token_secret, oauth_consumer_key, oauth_consumer_secret, screenName, count);
                    }
                    else
                    {
                        RenderAsXml(oauth_token, oauth_token_secret, oauth_consumer_key, oauth_consumer_secret, screenName, count);
                    }

                }
                else
                {
                    StringBuilder missingProperties = new StringBuilder();
                    if (oauth_token == String.Empty)
                    {
                        missingProperties.AppendLine("A site collection administrator need to provide a value for the \"Authentication Token\" web part property<BR/>");
                    }
                    if (oauth_token_secret == String.Empty)
                    {
                        missingProperties.AppendLine("A site collection administrator need to provide a value for the \"Authentication Token Secret\" web part property<BR/>");
                    }
                    if (oauth_consumer_key == String.Empty)
                    {
                        missingProperties.AppendLine("A site collection administrator need to provide a value for the \"Consumer Key\" web part property<BR/>");
                    }
                    if (oauth_consumer_secret == String.Empty)
                    {
                        missingProperties.AppendLine("A site collection administrator need to provide a value for the \"Consumer Key Secret\" web part property<BR/>");
                    }
                   
                    if (screenName == String.Empty)
                    {
                        missingProperties.AppendLine("A site collection administrator need to provide a value for the \"Screen Name\" web part property<BR/>");
                    }
                    if (count == String.Empty)
                    {
                        missingProperties.AppendLine("A site collection administrator need to provide a value for the \"Count\" web part property<BR/>");
                    }

                    Label lbl = new Label();
                    lbl.Text = missingProperties.ToString();
                    this.Controls.Add(lbl);

                }
            }
            catch (Exception ex)
            {
                Label lbl = new Label();
                lbl.Text = ex.ToString() + "Error: " + ex.ToString();
                this.Controls.Add(lbl);
            }
        }

       

        #endregion

        private void RenderNoTweets(string screenName)
        {
            Label lbl = new Label();
            lbl.Text = "No tweets were found for " + screenName;
            this.Controls.Add(lbl);
        }

        private void RenderAsXml(string oauth_token, string oauth_token_secret, string oauth_consumer_key, string oauth_consumer_secret, string screenName, string count)
        {
            string xmlData = TwitterHandler.GetTweetsAsXmlString(oauth_token, oauth_token_secret, oauth_consumer_key, oauth_consumer_secret, screenName, count, TransformURLs);

            if (xmlData.Length > 0)
            {
                XslCompiledTransform xslt = new XslCompiledTransform(true);
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                xslt.Load(_xslUrl);

                XmlDocument XmlDoc = new XmlDocument();

                StringReader strReader = new StringReader(xmlData.ToString());
                XmlTextReader xmlReader = new XmlTextReader(strReader);

                //Create an XmlTextWriter which outputs to memory stream
                Stream stream = new MemoryStream();
                XmlWriter writer = XmlWriter.Create(stream, xslt.OutputSettings);

                // Execute the transform and output the results to a file.
                xslt.Transform(xmlReader, writer);

                stream.Position = 0;
                XmlDoc.Load(stream);


                Label lbl = new Label();
                lbl.Text = XmlDoc.InnerXml;
                this.Controls.Add(lbl);
            }
            else
            {
                RenderNoTweets(screenName);
            }
        }

        private void RenderAsDefault(string oauth_token, string oauth_token_secret, string oauth_consumer_key, string oauth_consumer_secret, string screenName, string count)
        {
            List<Tweet> tweets = new List<Tweet>();
            tweets = TwitterHandler.GetTweets(oauth_token, oauth_token_secret, oauth_consumer_key, oauth_consumer_secret, screenName, count, TransformURLs);

            if (tweets.Count > 0)
            {
                LiteralControl ctrl = new LiteralControl();
                StringBuilder html = new StringBuilder();
                html.AppendLine(@"<ul class=" + CSSClassName + ">");
                string rssStyleDate = String.Empty;

                foreach (Tweet tweet in tweets)
                {
                    rssStyleDate = tweet.GetRssStyleDate(tweet.created_at);

                    html.AppendLine("<li>");
                    html.AppendLine(tweet.text);
                    html.AppendLine("<a href=\"https://twitter.com/" + tweet.user.name + "/status/" + tweet.id + "\"><span id=\"twdate\">" + rssStyleDate.Substring(0, rssStyleDate.Length - 14) + " " + DateTime.Parse(rssStyleDate).ToString("hh:mm:ss tt") + "</span></a>");
                    html.AppendLine("</li>");
                }

                html.AppendLine("</ul>");
                ctrl.Text = html.ToString();
                this.Controls.Add(ctrl);
            }
            else
            {
                RenderNoTweets(screenName);
            }
        }

    }
}
