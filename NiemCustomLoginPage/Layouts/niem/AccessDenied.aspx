<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> <%@ Page Language="C#" Inherits="Microsoft.SharePoint.ApplicationPages.AccessDeniedPage" MasterPageFile="~/_layouts/simple.master"       %> <%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="<%$Resources:wss,accessDenied_pagetitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" text="<%$Resources:wss,accessDenied_pagetitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Content3" contentplaceholderid="PlaceHolderPageImage" runat="server">
	<img id="onetidtpweb1" src="/_layouts/images/error.gif" alt="" />
</asp:Content>
<asp:Content ID="Content4" contentplaceholderid="PlaceHolderAdditionalPageHead" runat="server">
	<meta name="Robots" content="NOINDEX " />
	<meta name="SharePointError" content="1" />

    <link rel="stylesheet" type="text/css" href="/Style%20Library/niem/css/niem-sys.css" charset="utf-8" />
<script type="text/javascript" src="/Style%20Library/niem/js/libraries/jquery-1.5.1.min.js"></script>
<script type="text/javascript" src="/Style%20Library/niem/js/libraries/niem-sys.js"></script>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderMain" runat="server">
 <table border="0" cellpadding="0">
 <asp:Panel id="PanelUserName" runat="server">
 <tr>
	<td class="ms-sectionheader">
		<img src="/_layouts/images/ListSet.gif" alt="" />
		<SharePoint:EncodedLiteral ID="EncodedLiteral3" runat="server" text="<%$Resources:wss,accessDenied_currentuser%>" EncodeMethod='HtmlEncode'/>
	</td>
 </tr>
 <tr>
	<td valign="top" class="ms-descriptiontext"><SharePoint:EncodedLiteral ID="EncodedLiteral4" runat="server" text="<%$Resources:wss,accessDenied_loggedInAs%>" EncodeMethod='HtmlEncode'/>
		&#160;<b><asp:Label id="LabelUserName" runat="server"/></b>
	</td>
 </tr>
 </asp:Panel>
 <tr>
	<td>&#160;</td>
 </tr>
 <tr>
	<td>
		<asp:HyperLink id="HLinkLoginAsAnother" Text="<%$SPHtmlEncodedResources:wss,accessDenied_logInAsAnotherOne%>"
			CssClass="ms-descriptiontext" runat="server"/>
		<br/>
		<asp:HyperLink id="HLinkRequestAccess" Text="<%$SPHtmlEncodedResources:wss,accessDenied_requestAccess%>"
			CssClass="ms-descriptiontext" runat="server"/>
	</td>
 </tr>
 </table>
</asp:Content>
