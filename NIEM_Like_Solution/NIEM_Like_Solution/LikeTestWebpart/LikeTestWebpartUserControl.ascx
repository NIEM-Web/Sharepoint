<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LikeTestWebpartUserControl.ascx.cs" Inherits="NIEM_Like_Solution.LikeTestWebpart.LikeTestWebpartUserControl" %>
<%@ Register TagPrefix="NiemLike" TagName="NiemLikeLink" Src="~/_controltemplates/NIEM_Like_Solution/NIEM_Like_Control.ascx" %>

<NiemLike:NiemLikeLink ID="lnk1" runat="server" WebId="5cb05def-9c67-4e5e-b4d4-87df7c8dfde6" ListID="8457EA62-7003-4596-9F4F-BC8AD3867609" ItemID="1" />
<NiemLike:NiemLikeLink ID="NiemLikeLink1" runat="server" URL="SitePages/Home.aspx" />
