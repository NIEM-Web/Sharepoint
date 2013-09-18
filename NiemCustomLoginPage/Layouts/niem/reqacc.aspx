<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> <%@ Page Language="C#" Inherits="Microsoft.SharePoint.ApplicationPages.RequestAccess" MasterPageFile="~/_layouts/simple.master"       %> <%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,reqacc_pagetitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,reqacc_pagetitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content contentplaceholderid="PlaceHolderPageDescription" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,reqacc_desc_text%>" EncodeMethod='HtmlEncode'/>
	<br/>
</asp:Content>
<asp:Content contentplaceholderid="PlaceHolderAdditionalPageHead" runat="server">
	<meta name="Robots" content="NOINDEX " />
	<script type="text/javascript" src="/_layouts/<%=System.Threading.Thread.CurrentThread.CurrentUICulture.LCID%>/commonvalidation.js"></script>
<script type="text/javascript">
// <![CDATA[
function ULSOnM(){var o=new Object;o.ULSTeamName="Microsoft SharePoint Foundation";o.ULSFileName="reqacc.aspx";return o;}
var L_SizeLimitExceeded_ERR= "<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,reqacc_L_SizeLimitExceeded_ERR%>' EncodeMethod='EcmaScriptStringLiteralEncode'/>";
var L_DefaultEditText_TXT = "<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,reqacc_L_DefaultEditText_TXT%>' EncodeMethod='EcmaScriptStringLiteralEncode'/>";
function _spBodyOnLoad()
{ULSOnM:;
	var form = document.forms.<%SPHttpUtility.NoEncode(Form.ClientID,Response.Output);%>;
	var textArea = form.<%SPHttpUtility.NoEncode(txtareaMessage.ClientID,Response.Output);%>;
	if (textArea.value == "")
		textArea.value = L_DefaultEditText_TXT;
	try
	{
		textArea.focus();
		textArea.select();
	}
	catch(e)
	{
	}
}
function _spFormOnSubmit()
{ULSOnM:;
	var form = document.forms.<%SPHttpUtility.NoEncode(Form.ClientID,Response.Output);%>;
	var textArea = form.<%SPHttpUtility.NoEncode(txtareaMessage.ClientID,Response.Output);%>;
	if (textArea.value == L_DefaultEditText_TXT)
		textArea.value = "";
	if (textArea.value.length > 2000)
	{
		alert(L_SizeLimitExceeded_ERR);
		textArea.focus();
		return false;
	}
	return true;
}
// ]]>
</script>
<link rel="stylesheet" type="text/css" href="/Style%20Library/niem/css/niem-sys.css" charset="utf-8" />
<script type="text/javascript" src="/Style%20Library/niem/js/libraries/jquery-1.5.1.min.js"></script>
<script type="text/javascript" src="/Style%20Library/niem/js/libraries/niem-sys.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
<table border="0" cellpadding="0" cellspacing="0" width="99%">
		  <tr>
			<td id="RoleName" class="ms-descriptiontext"><SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,reqacc_logonname%>" EncodeMethod='HtmlEncode'/>
			&#160;<b><asp:Label id="LabelUserName" runat="server"/></b></td>
		  </tr>
		  <tr>
		   <td id="RoleDescription" class="ms-descriptiontext"><br/><SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,reqacc_entermsg%>" EncodeMethod='HtmlEncodeAllowSimpleTextFormatting'/></td>
		  </tr>
		  <tr><td rowspan="1" height="10"><img src="/_layouts/images/blank.gif" width='1' height='1' alt="" /></td></tr>
		  <tr>
		   <td class="ms-descriptiontext">
				<textarea name="txtareaMessage" id="txtareaMessage" rows="5" cols="64" class="ms-descriptiontext" runat=Server></textarea>
		   </td>
		  </tr>
		  <tr><td rowspan="1" height="10"><img src="/_layouts/images/blank.gif" width='1' height='1' alt="" /></td>
		  </tr>
		  <tr>
			<td>
			  <input type="submit" value="<%$Resources:wss,reqacc_sendreq%>" id="btnSendRequest" accesskey="s" runat="Server"/>
			</td>
		  </tr>
		  <tr><td rowspan="1" height="10"><img src="/_layouts/images/blank.gif" width='1' height='1' alt="" /></td></tr>
		  <tr><td class="ms-sectionline" height="1"><img src="/_layouts/images/blank.gif" width='1' height='1' alt="" /></td></tr>
		  <tr><td rowspan="1" height="10"><img src="/_layouts/images/blank.gif" width='1' height='1' alt="" /></td></tr>
</table>
</asp:Content>
