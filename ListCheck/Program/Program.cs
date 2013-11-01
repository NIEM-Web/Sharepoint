using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Xml;
using System.Web.UI.WebControls.WebParts;

using LMD.ListCheck.WS_List;
using LMD.ListCheck.WS_WebSvcWebs;

namespace LMD.ListCheck
{
    class Program
    {


        static string _userName = "rsnyder";
        static string _password = "<password>";

        static void Main(string[] args)
        {


            string url = "https://www.niem.gov/";
            //string url = "https://www.niem.gov/communities";
            string internalName = "Category_x0020_Domains";
            //string path = @"C:\deploy\niemoutput.txt";
            string path = @"C:\source\enumeration.txt";

            // This text is always added, making the file longer over time 
            // if it is not deleted. 
            using (StreamWriter sw = File.AppendText(path))
            {

                sw.WriteLine("=============================================================");
                sw.WriteLine("Start run at " + DateTime.Now.ToString());
                sw.WriteLine("Execute iterateThroughWebsEnumeratingLists");
                sw.WriteLine("at " + url);
                sw.WriteLine("");


                WSSAuthentication.Authentication _wssAuthentication = new WSSAuthentication.Authentication();
                _wssAuthentication.Url = "https://www.niem.gov/_vti_bin/authentication.asmx";
                _wssAuthentication.CookieContainer = new System.Net.CookieContainer();
                _wssAuthentication.AllowAutoRedirect = true;

                WSSAuthentication.LoginResult login_result = _wssAuthentication.Login(_userName, _password);

                iterateThroughWebsEnumeratingLists(sw, _wssAuthentication, url);
                //iterateThroughWebsLookingAtPermissions(sw, _wssAuthentication, url);

                //getUserGroupsForSite(sw, _wssAuthentication, url);

                //getPermissionLevels(sw, _wssAuthentication, url);

                //getContentTypes(sw, _wssAuthentication, url);

                //getWebFields(sw, _wssAuthentication, url, internalName);
                //iterateThroughWebsLookingForSiteColumn(sw, _wssAuthentication, url, internalName);

            }	
        }
        public static void iterateThroughWebsLookingAtPermissions(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url)
        {

            string webUrl = String.Empty;
            string webId = String.Empty;
            string listTitle = String.Empty;

            LMD.ListCheck.WS_WebSvcWebs.Webs myservice = new LMD.ListCheck.WS_WebSvcWebs.Webs();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/webs.asmx";

            XmlNode ndWebs = myservice.GetWebCollection();

            getUserGroupsForSite(sw, wssAuthentication, webUrl);


            foreach (System.Xml.XmlNode node in ndWebs)
            {
                if (node.Name == "Web")
                {
                    webUrl = node.Attributes["Url"].Value;
                    getUserGroupsForSite(sw, wssAuthentication, webUrl);
                    iterateThroughWebsLookingAtPermissions(sw, wssAuthentication, webUrl);

                }
            }

        }

        public static void iterateThroughWebsLookingForSiteColumn(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url, string internalName)
        {

            string webUrl = String.Empty;
            string webId = String.Empty;
            string listTitle = String.Empty;

            LMD.ListCheck.WS_WebSvcWebs.Webs myservice = new LMD.ListCheck.WS_WebSvcWebs.Webs();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/webs.asmx";

            XmlNode ndWebs = myservice.GetWebCollection();


            foreach (System.Xml.XmlNode node in ndWebs)
            {
                if (node.Name == "Web")
                {
                    webUrl = node.Attributes["Url"].Value;
                    iterateThroughWebsLookingForSiteColumn(sw, wssAuthentication, webUrl, internalName);
                    iterateThroughLists(sw, wssAuthentication, webUrl, internalName);
                }
            }

        }

        public static void iterateThroughWebsEnumeratingLists(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url)
        {

            string webUrl = String.Empty;
            string webId = String.Empty;
            string listTitle = String.Empty;

            LMD.ListCheck.WS_WebSvcWebs.Webs myservice = new LMD.ListCheck.WS_WebSvcWebs.Webs();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/webs.asmx";

            XmlNode ndWebs = myservice.GetWebCollection();


            foreach (System.Xml.XmlNode node in ndWebs)
            {
                if (node.Name == "Web")
                {
                    webUrl = node.Attributes["Url"].Value;
                    enumerateListsInWeb(sw, wssAuthentication, webUrl);

                    iterateThroughWebsEnumeratingLists(sw, wssAuthentication, webUrl);
                }
            }

        }

        public static void enumerateListsInWeb(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url)
        {

            string listId = String.Empty;
            string listTitle = String.Empty;

            LMD.ListCheck.WS_List.Lists myservice = new LMD.ListCheck.WS_List.Lists();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/lists.asmx";

            XmlNode ndLists = myservice.GetListCollection();


            foreach (System.Xml.XmlNode node in ndLists)
            {
                //if (node.Name == "List")
                //{

                    listId = node.Attributes["ID"].Value;
                    listTitle = node.Attributes["Title"].Value;

                    //sw.WriteLine("Url: " + url + ", List Title: " + listTitle);

                    enumerateListFields(sw, wssAuthentication, url, listId, listTitle);

                //}
            }

        }

        public static void enumerateListFields(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url, string listId, string listName)
        {
            string fieldTitle = String.Empty;
            LMD.ListCheck.WS_List.Lists myservice = new LMD.ListCheck.WS_List.Lists();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/lists.asmx";
            try
            {

                System.Xml.XmlNode nodes = myservice.GetList(listName);

                foreach (System.Xml.XmlNode node in nodes)
                {
                    if (node.Name == "Fields")
                    {
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            if (node.ChildNodes[i].Name == "Field")
                            {
                                fieldTitle = node.ChildNodes[i].Attributes["DisplayName"].Value;
                                sw.WriteLine("Url: " + url + ", List Title: " + listName + ", Field DisplayName: " + fieldTitle); //+ ", Field SchemaXml: " + node.ChildNodes[i].OuterXml);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public static void iterateThroughLists(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url, string internalName)
        {

            string listId = String.Empty;
            string listTitle = String.Empty;

            LMD.ListCheck.WS_List.Lists myservice = new LMD.ListCheck.WS_List.Lists();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/lists.asmx";

            XmlNode ndLists = myservice.GetListCollection();


            foreach (System.Xml.XmlNode node in ndLists)
            {
                if (node.Name == "List")
                {

                    listId = node.Attributes["ID"].Value;
                    listTitle = node.Attributes["Title"].Value;

                    getListFields(sw, wssAuthentication, url, listId, listTitle, internalName);

                }
            }

        }

        public static void getWebFields(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url, string internalName)
        {

            LMD.ListCheck.WS_WebSvcWebs.Webs myservice = new LMD.ListCheck.WS_WebSvcWebs.Webs();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/webs.asmx";
            try
            {

                System.Xml.XmlNode nodes = myservice.GetColumns();

                foreach (System.Xml.XmlNode node in nodes)
                {
                            if (node.Name == "Field")
                            {

                                if (node.Attributes["Name"].Value == internalName)
                                {
                                    sw.WriteLine("Url: " + url + ", Field SchemaXml: " + node.OuterXml);
                                }
                            }

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public static void getContentTypes(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url)
        {

            LMD.ListCheck.WS_WebSvcWebs.Webs myservice = new LMD.ListCheck.WS_WebSvcWebs.Webs();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/webs.asmx";
            try
            {

                System.Xml.XmlNode nodes = myservice.GetContentTypes();

                foreach (System.Xml.XmlNode node in nodes)
                {
                    if (node.Name == "ContentType")
                    {

                            sw.WriteLine("Url: " + url + ", Field SchemaXml: " + node.OuterXml);

                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public static void getListData()
        {

            WSSAuthentication.Authentication _wssAuthentication = new WSSAuthentication.Authentication();
            _wssAuthentication.Url = "https://www.niem.gov/_vti_bin/authentication.asmx";
            _wssAuthentication.CookieContainer = new System.Net.CookieContainer();
            _wssAuthentication.AllowAutoRedirect = true;

            WSSAuthentication.LoginResult login_result = _wssAuthentication.Login(_userName, _password);



            LMD.ListCheck.WS_List.Lists myservice = new LMD.ListCheck.WS_List.Lists();
            myservice.CookieContainer = _wssAuthentication.CookieContainer;
            myservice.Url = "https://www.niem.gov/aboutniem/_vti_bin/lists.asmx";
            try
            {
                /* Assign values to pass the GetListItems method*/
                string listName = "{64F28F78-8392-40CA-8EEE-3C09B4319EB5}";
                string viewName = "{788D6C3B-218B-49B9-9FEE-540F110DE1FE}";
                string rowLimit = "100";

                // Instantiate an XmlDocument object
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                System.Xml.XmlElement query = xmlDoc.CreateElement("Query");
                System.Xml.XmlElement viewFields = xmlDoc.CreateElement("ViewFields");
                System.Xml.XmlElement queryOptions = xmlDoc.CreateElement("QueryOptions");

                /*Use CAML query*/
                query.InnerXml = "<ViewFields Properties=\"True\" /><Where><Gt><FieldRef Name=\"ID\" />" +
                "<Value Type=\"Counter\">0</Value></Gt></Where>";
                viewFields.InnerXml = "<FieldRef Name=\"Title\" /><FieldRef Name=\"Category Domains\" />";
                queryOptions.InnerXml = "";

                System.Xml.XmlNode nodes = myservice.GetListItems(listName, viewName, query, viewFields, rowLimit, null, null);

                foreach (System.Xml.XmlNode node in nodes)
                {
                    if (node.Name == "rs:data")
                    {
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            if (node.ChildNodes[i].Name == "z:row")
                            {

                                if (node.ChildNodes[i].OuterXml.Contains("Category Domains"))
                                {
                                    //getSiteColumn();

                                    Console.Write(node.ChildNodes[i].Attributes["ows_Title"].Value + "</br>");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public static void getListFields(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url, string listId, string listName, string internalName)
        {

            LMD.ListCheck.WS_List.Lists myservice = new LMD.ListCheck.WS_List.Lists();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/lists.asmx";
            try
            {

                System.Xml.XmlNode nodes = myservice.GetList(listName);

                foreach (System.Xml.XmlNode node in nodes)
                {
                    if (node.Name == "Fields")
                    {
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            if (node.ChildNodes[i].Name == "Field")
                            {

                                if (node.ChildNodes[i].Attributes["Name"].Value == internalName)
                                {
                                    sw.WriteLine("Url: " + url + ", List Title: " + listName + ", Field SchemaXml: " + node.ChildNodes[i].OuterXml);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public static void getListPermisions(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url, string listTitle)
        {

            if (url == string.Empty) return;

            string maskBin = String.Empty;
            int tailleMaks = 0;
            string userRights = null;

            LMD.ListCheck.WS_Permissions.Permissions myservice = new LMD.ListCheck.WS_Permissions.Permissions();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/Permissions.asmx";

            System.Collections.Generic.Dictionary<int, string> dicoRights = new System.Collections.Generic.Dictionary<int, string>();
            dicoRights.Add(0, " ViewListItems");             //                 1
            dicoRights.Add(1, " AddListItems");              //                 2
            dicoRights.Add(2, " EditListItems");             //                 4
            dicoRights.Add(3, " DeleteListItems");           //                 8
            //dicoRights.Add(4, " ApproveItems");              //                16
            //dicoRights.Add(5, " OpenItems");                 //                32
            //dicoRights.Add(6, " ViewVersions");              //                64
            //dicoRights.Add(7, " DeleteVersions");            //               128
            //dicoRights.Add(8, " CancelCheckout");            //               256
            //dicoRights.Add(9, " ManagePersonalViews");       //               512
            //dicoRights.Add(11, " ManageLists");              //             2 048
            //dicoRights.Add(12, " ViewFormPages");            //             4 096
            //dicoRights.Add(16, " Open");                     //            65 536
            //dicoRights.Add(17, " ViewPages");                //           131 072
            //dicoRights.Add(18, " AddAndCustomizePages");     //           262 144
            //dicoRights.Add(19, " ApplyThemeAndBorder");      //           524 288
            //dicoRights.Add(20, " ApplyStyleSheets");         //         1 048 576
            //dicoRights.Add(21, " ViewUsageData");            //         2 097 152
            //dicoRights.Add(22, " CreateSSCSite");            //         4 194 304
            //dicoRights.Add(23, " ManageSubwebs");            //         8 388 608
            //dicoRights.Add(24, " CreateGroups");             //        16 777 216
            //dicoRights.Add(25, " ManagePermissions");        //        33 554 432
            //dicoRights.Add(26, " BrowseDirectories");        //        67 108 864
            //dicoRights.Add(27, " BrowseUserInfo");           //       134 217 728
            //dicoRights.Add(28, " AddDelPrivateWebParts");    //       268 435 456
            //dicoRights.Add(29, " UpdatePersonalWebParts");   //       536 870 912
            //dicoRights.Add(30, " ManageWeb");                //     1 073 741 824
            //dicoRights.Add(36, " UseClientIntegration ");    //    68 719 476 736                                           
            //dicoRights.Add(37, " UseRemoteAPIs");            //   137 438 953 472
            //dicoRights.Add(38, " ManageAlerts");             //   274 877 906 944
            //dicoRights.Add(39, " CreateAlerts");             //   549 755 813 888
            //dicoRights.Add(40, " EditMyUserInfo");           // 1 099 511 627 776


            try
            {

                System.Xml.XmlNode nodes = myservice.GetPermissionCollection(listTitle, "List");


                foreach (System.Xml.XmlNode node in nodes)
                {
                    if (node.Name == "Permissions")
                    {
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            if (node.ChildNodes[i].Name == "Permission")
                            {

                                maskBin = Convert.ToString(Convert.ToInt32(node.ChildNodes[i].Attributes["Mask"].Value), 2);
                                tailleMaks = maskBin.Length;
                                userRights = null;
                                foreach (System.Collections.Generic.KeyValuePair<int, string> kvp in dicoRights)
                                {
                                    try
                                    {
                                        if (maskBin[(maskBin.Length - (kvp.Key + 1))] == '1') userRights += kvp.Value;
                                    }
                                    catch { }
                                }

                                if (node.ChildNodes[i].Attributes["Mask"].Value == "-1")
                                {
                                    userRights = "Full Control";
                                }
                                

                                if (node.ChildNodes[i].Attributes["MemberIsUser"].Value == "False")
                                {
                                    sw.WriteLine("    List: " + listTitle.PadRight(25) + ", Group Name: " + node.ChildNodes[i].Attributes["GroupName"].Value + ",    Permissions: " + userRights);
                                }
                                else
                                {
                                    sw.WriteLine("    List: " + listTitle.PadRight(25) + ", User Name: " + node.ChildNodes[i].Attributes["UserLogin"].Value + ",    Permissions: " + userRights);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public static void getPermissionsForSite(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url)
        {
            string maskBin = String.Empty;
            int tailleMaks = 0;
            string userRights = null;

            LMD.ListCheck.WS_Permissions.Permissions myservice = new LMD.ListCheck.WS_Permissions.Permissions();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/Permissions.asmx";

            System.Collections.Generic.Dictionary<int, string> dicoRights = new System.Collections.Generic.Dictionary<int, string>();
            dicoRights.Add(0, " ViewListItems");             //                 1
            dicoRights.Add(1, " AddListItems");              //                 2
            dicoRights.Add(2, " EditListItems");             //                 4
            dicoRights.Add(3, " DeleteListItems");           //                 8
            dicoRights.Add(4, " ApproveItems");              //                16
            dicoRights.Add(5, " OpenItems");                 //                32
            dicoRights.Add(6, " ViewVersions");              //                64
            dicoRights.Add(7, " DeleteVersions");            //               128
            dicoRights.Add(8, " CancelCheckout");            //               256
            dicoRights.Add(9, " ManagePersonalViews");       //               512
            dicoRights.Add(11, " ManageLists");              //             2 048
            dicoRights.Add(12, " ViewFormPages");            //             4 096
            dicoRights.Add(16, " Open");                     //            65 536
            dicoRights.Add(17, " ViewPages");                //           131 072
            dicoRights.Add(18, " AddAndCustomizePages");     //           262 144
            dicoRights.Add(19, " ApplyThemeAndBorder");      //           524 288
            dicoRights.Add(20, " ApplyStyleSheets");         //         1 048 576
            dicoRights.Add(21, " ViewUsageData");            //         2 097 152
            dicoRights.Add(22, " CreateSSCSite");            //         4 194 304
            dicoRights.Add(23, " ManageSubwebs");            //         8 388 608
            dicoRights.Add(24, " CreateGroups");             //        16 777 216
            dicoRights.Add(25, " ManagePermissions");        //        33 554 432
            dicoRights.Add(26, " BrowseDirectories");        //        67 108 864
            dicoRights.Add(27, " BrowseUserInfo");           //       134 217 728
            dicoRights.Add(28, " AddDelPrivateWebParts");    //       268 435 456
            dicoRights.Add(29, " UpdatePersonalWebParts");   //       536 870 912
            dicoRights.Add(30, " ManageWeb");                //     1 073 741 824
            dicoRights.Add(36, " UseClientIntegration ");    //    68 719 476 736                                           
            dicoRights.Add(37, " UseRemoteAPIs");            //   137 438 953 472
            dicoRights.Add(38, " ManageAlerts");             //   274 877 906 944
            dicoRights.Add(39, " CreateAlerts");             //   549 755 813 888
            dicoRights.Add(40, " EditMyUserInfo");           // 1 099 511 627 776

            try
            {

                System.Xml.XmlNode nodes = myservice.GetPermissionCollection(url, "Web");
                //XmlNode ndPermissions = permService.GetPermissionCollection("List_Name","List");

                foreach (System.Xml.XmlNode node in nodes)
                {
                    if (node.Name == "Permissions")
                    {
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            if (node.ChildNodes[i].Name == "Permission")
                            {

                                maskBin = Convert.ToString(Convert.ToInt32(node.ChildNodes[i].Attributes["Mask"].Value), 2);
                                tailleMaks = maskBin.Length;
                                userRights = null;
                                foreach (System.Collections.Generic.KeyValuePair<int, string> kvp in dicoRights)
                                {
                                    try
                                    {
                                        if (maskBin[(maskBin.Length - (kvp.Key + 1))] == '1') userRights += kvp.Value;
                                    }
                                    catch { }
                                }
 


                                if (node.ChildNodes[i].Attributes["MemberIsUser"].Value == "False")
                                {
                                    sw.WriteLine("Url: " + url + ", Group Name: " + node.ChildNodes[i].Attributes["GroupName"].Value + ",    Permissions: " + userRights);
                                }
                                else
                                {
                                    sw.WriteLine("Url: " + url + ", User Name: " + node.ChildNodes[i].Attributes["UserLogin"].Value + ",    Permissions: " + userRights);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public static void getUserGroupsForSite(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url)
        {

            if (url == string.Empty) return;

            string groups = null;
            string roles = String.Empty;
            string groupName = String.Empty;

            LMD.ListCheck.WS_usergroup.UserGroup myservice = new LMD.ListCheck.WS_usergroup.UserGroup();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/usergroup.asmx";

            try
            {

                System.Xml.XmlNode nodes = myservice.GetGroupCollectionFromWeb();

                sw.WriteLine("");
                sw.WriteLine("");
                sw.WriteLine("Url: " + url);

                sw.WriteLine("  Site Permissions");

                foreach (System.Xml.XmlNode node in nodes)
                {
                    if (node.Name == "Groups")
                    {

                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            groupName = node.ChildNodes[i].Attributes["Name"].Value;
                            roles = String.Empty;

                            System.Xml.XmlNode nodes2 = myservice.GetRoleCollectionFromGroup(groupName);
                            foreach (System.Xml.XmlNode node2 in nodes2)
                            {
                                if (node2.Name == "Roles")
                                {
                                    for (int k = 0; k < node2.ChildNodes.Count; k++)
                                    {
                                        if (roles != String.Empty)
                                            roles += ", ";
                                        roles += node2.ChildNodes[k].Attributes["Name"].Value;                                        
                                    }
                                }
                            }

                            sw.WriteLine("    Group: " + groupName + ", Roles: " + roles);

                        }
                    }

                }

                enumerateListPermissions(sw, wssAuthentication, url);

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public static void enumerateListPermissions(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url)
        {

            string listId = String.Empty;
            string listTitle = String.Empty;

            LMD.ListCheck.WS_List.Lists myservice = new LMD.ListCheck.WS_List.Lists();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/lists.asmx";

            sw.WriteLine("  List Permissions");

            try
            {

                System.Xml.XmlNode nodes = myservice.GetListCollection();

                foreach (System.Xml.XmlNode node in nodes)
                {
                    if (node.Name == "List")
                    {
                        listId = node.Attributes["ID"].Value;
                        listTitle = node.Attributes["Title"].Value;

                        if (listTitle != "Workflow Tasks" && listTitle != "Workflow History" && listTitle != "Documents" && listTitle != "Workflow Tasks" && listTitle != "Pages" && listTitle != "Master Page Gallery" && listTitle != "Images" && listTitle != "fpdatasources")
                        {
                            getListPermisions(sw, wssAuthentication, url, listTitle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }


        public static void getSiteColumn()
        {

            WSSAuthentication.Authentication _wssAuthentication = new WSSAuthentication.Authentication();
            _wssAuthentication.Url = "https://www.niem.gov/_vti_bin/authentication.asmx";
            _wssAuthentication.CookieContainer = new System.Net.CookieContainer();
            _wssAuthentication.AllowAutoRedirect = true;

            WSSAuthentication.LoginResult login_result = _wssAuthentication.Login(_userName, _password);



            LMD.ListCheck.WS_WebSvcWebs.Webs webs = new LMD.ListCheck.WS_WebSvcWebs.Webs();
            webs.CookieContainer = _wssAuthentication.CookieContainer;
            webs.Url = "https://www.niem.gov/_vti_bin/webs.asmx";
            try
            {
                XmlNode myNode = webs.GetColumns();

                //Create XML document.
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode d;          
                d = xmlDoc.CreateXmlDeclaration("1.0", "", "yes");
                xmlDoc.AppendChild(d);

                //Move Web service data into XML document and save.
                XmlNode root = xmlDoc.CreateElement("Fields");
                root.InnerXml = myNode.InnerXml;
                xmlDoc.AppendChild(root);
                xmlDoc.Save("c:\\deploy\\SiteColumns.xml");
           }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public static void getPermissionLevels(StreamWriter sw, WSSAuthentication.Authentication wssAuthentication, string url)
        {

            Dictionary<string, string> sc = new Dictionary<string, string>();

            if (url == string.Empty) return;

            LMD.ListCheck.WS_usergroup.UserGroup myservice = new LMD.ListCheck.WS_usergroup.UserGroup();
            myservice.CookieContainer = wssAuthentication.CookieContainer;
            myservice.Url = url + "/_vti_bin/usergroup.asmx";

            try
            {

                System.Xml.XmlNode nodes = myservice.GetRolesAndPermissionsForSite();

                sw.WriteLine("Url: " + url);

                sw.WriteLine("  Site Permissions");

                foreach (System.Xml.XmlNode node in nodes)
                {
                    if (node.Name == "Role")
                    {
                        sc.Add(node.Attributes["Name"].Value, node.Attributes["BasePermissions"].Value);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

    }
}

