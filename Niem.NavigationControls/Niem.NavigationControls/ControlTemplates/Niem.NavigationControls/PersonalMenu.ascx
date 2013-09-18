<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonalMenu.ascx.cs" Inherits="Niem.NavigationControls.ControlTemplates.Niem.NavigationControls.PersonalMenu" %>
<asp:LoginView ID="LoginView1" runat="server">
<AnonymousTemplate>
</AnonymousTemplate>
<LoggedInTemplate>
    <div id="NIEMPersonalMenu">
	<h2 class="NIEM-qlTop"><em>My</em>Communities</h2>
        <SharePoint:AspMenu ID="PersonalNav" mEncodeTitle="false" CustomSelectionEnabled="true" runat="server" EnableViewState="true" UseSeparateCSS="false" UseSimpleRendering="true" Orientation="Vertical" StaticDisplayLevels="1" MaximumDynamicDisplayLevels="0" CssClass="s4-ql"  SkipLinkText="<%$Resources:cms,masterpages_skiplinktext%>"/>
    <div class="NIEM-qlBottom"></div>
	</div>
</LoggedInTemplate>
</asp:LoginView>