<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true"  Inherits="lmd.NIEM.FarmSolution.NeimLoginForm, lmd.NIEM.FarmSolution, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f557e1aff1f6f296" DynamicMasterPageFile="~masterurl/custom.master" %>
<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
<!--CodeBehind="NeimLoginForm.aspx.cs" -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">

 <SharePoint:EncodedLiteral ID="ClaimsFormsPageTitle" runat="server" text="<%$Resources:wss,login_pagetitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
 
 <SharePoint:EncodedLiteral ID="ClaimsFormsPageTitleInTitleArea" runat="server" text="<%$Resources:wss,login_pagetitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderSiteName" runat="server"/>
<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderMain" runat="server">
 

<div id="NiemContentLogin">
 
 <div class="niemTopInt">
<p class="intro_text"><strong>Please note:</strong> Currently, the NIEM website 
(NIEM.gov) and the NIEM Tools website (tools.NIEM.gov) maintain separate registration databases.</p>
</div>
<table width="100%" border="0" id="niemTab2col">
  <tr>
    <td width="48%" valign="top" class="niemTabColLft">
    <h2>Log in to your NIEM.gov account:</h2>
    <p>Log into NIEM.gov to access and post to NIEM forums and to upload and share documents with the NIEM community. 
    Become a member of a NIEM domain and you'll also have access to the domain's collaboration zone, where you can access additional resources and work with other domain members.</p>


 <asp:Login ID="signInControl" FailureText="<%$Resources:wss,login_pageFailureText%>" MembershipProvider="FBAMembershipProvider" runat="server" DisplayRememberMe="true">
    <layouttemplate>
        <asp:label id="FailureText" class="ms-error" runat="server"/>    
<table cellpadding="0" border="0" id="niemSignInCtrl">
			<tr>
				<td class="ms-standardheader ms-inputformheader"><label for="UserName">User name</label></td>
            </tr>
           <tr> 
             <td><asp:textbox id="UserName" autocomplete="off" runat="server" class="ms-input ms-login-textbox"/><span id="UserNameRequired" title="User Name is required." style="color:Red;">*</span></td>
	   	   </tr>
            <tr>
				<td class="ms-standardheader ms-inputformheader"><label for="Password">Password</label></td>
            </tr>
			<tr>
            <td><asp:textbox id="password" TextMode="Password" autocomplete="off" runat="server" class="ms-input ms-login-textbox"/><span id="PasswordRequired" title="Password is required." style="color:Red;">*</span></td>
			</tr>
            <tr>
				<td>
                <asp:CheckBox id="RememberMe" text="Remember me next time" runat="server" />
               </td>
			</tr>
            <tr>
				<td>
               <asp:button id="Button1" commandname="Login" text="Log In" runat="server" />
                </td>
			</tr>
</table>
  </layouttemplate>
 </asp:login>
 
     <asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="Red" EnableViewState="false"></asp:Label>
      
      <asp:Panel ID="LoginFooterInfo" runat="server">
      <ul class="logAddition"><li>
       <a href="/Pages/Register.aspx">New user? Click here to register.</a>
      </li><li>
        <a href="/SignUp/Pages/PasswordQuestion.aspx"> Forgot your password? Recover it here.</a>
      </li></ul>
      </asp:Panel>
    <div class="LoginAduser">
 <asp:LinkButton ID="lbInternalUsers" Text="Administrator Login" runat="server" CssClass="ms-standardheader ms-inputformheader" OnClick="lbInternalUsers_OnClick" />
</div>

 <SharePoint:EncodedLiteral runat="server" EncodeMethod="HtmlEncode" ID="ClaimsFormsPageMessage" Visible="false" />
 
     </td>
    <td width="4%" align="center" class="niemTabColCtr">
    <div class="niemORSep" valign="top"><img name="or" src="/Style Library/NIEM3/images/niem-or-center.png" width="32" alt="or" ></div></td>
    <td width="48%" valign="top" class="niemTabColRgt">
    
<h2>Access your NIEM Tools account:</h2>
<p>Create and validate IEPDs using the NIEM Tools website. With each NIEM release, you'll find a set of free tools that implement all of the structural and content features of the release. These tools will help you build IEPDs using NIEM tools, terminology, governance, methodology, and community/user support.</p>
    
    <a href="http://tools.niem.gov/niemtools/user/user-login.iepd"><img name="AccessNIEMTools" src="/Style Library/NIEM3/images/niem-tools-button.jpg" width="242" height="47" alt="Access your NIEM Tools" >
    </a>
    </td>
  </tr>
</table>
 


 </div>

 
</asp:Content>