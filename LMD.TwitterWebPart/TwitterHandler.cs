using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;   // Added as part of System.Web.Extensions.
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Xml;

//Classes to support serialization of twitter JSON objects.
namespace LMD.TwitterWebPart
{

    public class TwitterNameUrl
    {
        public TwitterUrls[] urls { get; set; }
    }
    public class TwitterNameEntities
    {
        public TwitterNameUrl url { get; set; }
        public TwitterNameUrl description { get; set; }
    }
    public class TwitterName
    {
        public string id { get; set; }
        public string name { get; set; }
        public string screen_name { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public TwitterNameEntities entities { get; set; }
        public string followers_count { get; set; }
        public string friends_count { get; set; }
        public string listed_count { get; set; }
        public string created_at { get; set; }
        public string favourites_count { get; set; }
        public string utc_offset { get; set; }
        public string time_zone { get; set; }
        public string geo_enabled { get; set; }
        public string verified { get; set; }
        public string statuses_count { get; set; }
        public string lang { get; set; }
        public string contributors_enabled { get; set; }
        public string is_translator { get; set; }
        public string profile_background_color { get; set; }
        public string profile_background_image_url { get; set; }
        public string profile_background_image_url_https { get; set; }
        public string profile_background_tile { get; set; }
        public string profile_image_url { get; set; }
        public string profile_image_url_https { get; set; }
        public string profile_banner_url { get; set; }
        public string profile_link_color { get; set; }
        public string profile_sidebar_border_color { get; set; }
        public string profile_sidebar_fill_color { get; set; }
        public string profile_text_color { get; set; }
        public string profile_use_background_image { get; set; }
        public string default_profile { get; set; }
        public string default_profile_image { get; set; }
        public string following { get; set; }
        public string follow_request_sent { get; set; }
        public string notifications { get; set; }
    }
    public class TwitterHashtags
    {
        public string text { get; set; }
        public int[] indices { get; set; }
    }
    public class TwitterUserMentions
    {
        public string screen_name { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public int[] indices { get; set; }
    }
    public class TwitterEntities
    {
        public TwitterHashtags[] hashtags { get; set; }
        public TwitterUrls[] urls { get; set; }
        public TwitterUserMentions[] user_mentions { get; set; }
        //symbols
    }
    public class TwitterUrls
    {
        public string url { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public int[] indices { get; set; }
    }
    public class TwitterFormatting
    {
        public int startindex { get; set; }
        public int stopindex { get; set; }
        public int lengthSubString { get; set; }
        public string hyperlinkReplacement { get; set; }
        public string text { get; set; }
        public string replacement { get; set; }
    }
    public class Tweet
    {
        public string created_at { get; set; }
        public string id { get; set; }
        public string text { get; set; }
        public string source { get; set; }
        public string truncated { get; set; }
        public string in_reply_to_status_id { get; set; }
        public string in_reply_to_status_id_str { get; set; }
        public string in_reply_to_user_id { get; set; }
        public string in_reply_to_user_id_str { get; set; }
        public string in_reply_to_screen_name { get; set; }
        public TwitterName user { get; set; }
        public string geo { get; set; }
        public string coordinates { get; set; }
        public string place { get; set; }
        public string contributors { get; set; }
        public string retweet_count { get; set; }
        public string favorite_count { get; set; }
        public TwitterEntities entities { get; set; }
        public string favorited { get; set; }
        public string retweeted { get; set; }
        public string possibly_sensitive { get; set; }
        public string lang { get; set; }

        public string GetFormattedTweet()
        {

            // gather up all of the entities that gets links
            string formattedTweet = this.text;
            List<TwitterFormatting> replacements = new List<TwitterFormatting>();
            TwitterFormatting formatObj;
         
            for (int i = 0; i < this.entities.hashtags.Length; i++)
            {
                formatObj = new TwitterFormatting();
                formatObj.startindex = this.entities.hashtags[i].indices[0];
                formatObj.stopindex = this.entities.hashtags[i].indices[1];
                formatObj.replacement = String.Format("#<a href=\"https://twitter.com/search?q=%23{0}&src=hash\">{0}</a>", this.entities.hashtags[i].text);
                replacements.Add(formatObj);
            }

            for (int i = 0; i < this.entities.urls.Length; i++)
            {
                formatObj = new TwitterFormatting();
                formatObj.startindex = this.entities.urls[i].indices[0];
                formatObj.stopindex = this.entities.urls[i].indices[1];
                formatObj.replacement = string.Format("<a href=\"{0}\">{1}</a>", this.entities.urls[i].url,this.entities.urls[i].display_url);
                replacements.Add(formatObj);
            }

            for (int i = 0; i < this.entities.user_mentions.Length; i++)
            {
                formatObj = new TwitterFormatting();
                formatObj.startindex = this.entities.user_mentions[i].indices[0];
                formatObj.stopindex = this.entities.user_mentions[i].indices[1];
                formatObj.replacement = string.Format("<a href=\"https://twitter.com/{0}\">{0}</a>", this.entities.user_mentions[i].screen_name);
                replacements.Add(formatObj);
            }

            // Sort the list in descending order so we can process them from the end to the beginning so that indices
            // are still relevant.
            List<TwitterFormatting> sortedReplacements = replacements.OrderByDescending(o => o.startindex).ToList();
            foreach (TwitterFormatting replaceObj in sortedReplacements)
            {
                if ((formattedTweet.Length - replaceObj.stopindex) != 0)
                {
                    formattedTweet = formattedTweet.Substring(0, replaceObj.startindex) + replaceObj.replacement + formattedTweet.Substring(replaceObj.stopindex, formattedTweet.Length - replaceObj.stopindex);
                }
                else
                {
                    formattedTweet = formattedTweet.Substring(0, replaceObj.startindex) + replaceObj.replacement;
                }
            }
            return formattedTweet;
        }

        public string GetRssStyleDate(string newTwitterDate)
        {
            // New twitter style date "Mon Jul 08 22:22:55 +0000 2013"
            // the Twitter Rss feed style is "Wed, 17 Jul 2013 06:22:29 +1000"
            string[] s = newTwitterDate.Split(' ');
            return String.Format("{0}, {1} {2} {3} {4} {5}", s[0], s[2], s[1], s[5], s[3], s[4]);

        }

    }


    public class TwitterHandler
    {

        public static List<Tweet> GetTweets(string oauth_token, string oauth_token_secret, string oauth_consumer_key, string oauth_consumer_secret, string screen_name, string count, bool transformUrls)
        {
            string responseData = GetTwitterData(oauth_token, oauth_token_secret, oauth_consumer_key, oauth_consumer_secret, screen_name, count);

            List<Tweet> UserTweets = ConvertToList(responseData);

            if (transformUrls == true)
            {
                return TranformUrls(UserTweets);
            }

            return UserTweets;
        }       

        public static string GetTweetsAsXmlString(string oauth_token, string oauth_token_secret, string oauth_consumer_key, string oauth_consumer_secret, string screenName, string count, bool transformUrls)
        {
            List<Tweet> tweets = GetTweets(oauth_token, oauth_token_secret, oauth_consumer_key, oauth_consumer_secret, screenName, count, transformUrls);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(tweets.GetType());
            string result = string.Empty;
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, tweets);
                result = writer.ToString();
            }
            
            return (result.Replace("ArrayOfTweet", "Tweets"));
        }

        private static List<Tweet> ConvertToList(string responseData)
        {
            return(new JavaScriptSerializer().Deserialize<List<Tweet>>(responseData));
        }




        private static List<Tweet> TranformUrls(List<Tweet> tweets)
        {
           foreach (Tweet tweet in tweets)
            {
                tweet.text = tweet.GetFormattedTweet();
            }

            return tweets;
        }

        private static string GetTwitterData(string oauth_token, string oauth_token_secret, string oauth_consumer_key, string oauth_consumer_secret, string screen_name, string count)
        {
            //
            // The source code was pulled from a combination of sites.  
            //
            //   Here is one that contains the majority of the oAuth and Twitter interfacing
            //     http://stackoverflow.com/questions/15049927/using-twitter-oauth-via-api-1-1-without-3rd-party-library
            //   This site contained the logic to convert the response and a generic list of "Tweet" objects
            //     http://www.markcoatsworth.com/blog/simple-c-oauth-implementation-for-twitter-api-1-1
            //   This forums describes how to get the count parameter to work.  The interface is EXTREMELY unforgiving if you do not have the 
            //     the oauth signature parameters in alpha order the call returns a 401 unauthorized.  Also, the count parameter must
            //     be added in BOTH locations (part of the oauth AND as part of the web request) otherwise you get a 401 unauthorized.
            //     https://dev.twitter.com/discussions/15206
            // OAuth keys generated at http://dev.twitter.com/apps. For security purposes would be better to put these in web.config.
            //

            // oauth implementation details
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";

            // unique request details
            var oauth_nonce = Convert.ToBase64String(
                new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            // message api details
            var resource_url = "https://api.twitter.com/1.1/statuses/user_timeline.json";

            // create oauth signature
            var baseFormat = "count={7}&oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}"
                + "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&screen_name={6}";

            var baseString = string.Format(baseFormat
                                , oauth_consumer_key
                                , oauth_nonce
                                , oauth_signature_method
                                , oauth_timestamp
                                , oauth_token
                                , oauth_version
                                , Uri.EscapeDataString(screen_name)
                                , Uri.EscapeDataString(count)
                                );


            baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                    "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            // create the request header
            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                               "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                               "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                               "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString(oauth_signature_method),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_consumer_key),
                                    Uri.EscapeDataString(oauth_token),
                                    Uri.EscapeDataString(oauth_signature),
                                    Uri.EscapeDataString(oauth_version)
                            );


            // make the request
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.ServerCertificateValidationCallback =
                      (obj, certificate, chain, errors) => true;


            var postBody = "screen_name=" + Uri.EscapeDataString(screen_name);//
            resource_url += "?" + postBody + "&count=" + count;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";


            WebResponse response = request.GetResponse();
            string responseData = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseData;
        }

    }

}
