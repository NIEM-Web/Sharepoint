<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NIEM_Like_Control.ascx.cs" Inherits="NIEM_Like_Solution.ControlTemplates.NIEM_Like_Solution.NIEM_Like_Control" %>
<asp:LinkButton ID="lnkLike" runat="server" Text="Like" OnClick="lnkLike_Click" 
    onprerender="lnkLike_PreRender" />
<asp:HiddenField ID="hdnUrl" runat="server" />
<asp:HiddenField ID="hdnWebId" runat="server" />
<asp:HiddenField ID="hdnListId" runat="server" />
<asp:HiddenField ID="hdnItemId" runat="server" />
<asp:HiddenField ID="hdnCType" runat="server" />
