<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExportUsersToExcelUserControl.ascx.cs" Inherits="lmd.NIEM.FarmSolution.ExportUsersToExcel.ExportUsersToExcelUserControl" %>

<div>
<span class="lkexl"><a href="<%=SPContext.Current.Web.Url.ToString() %>/_layouts/niem/ExportUsers.aspx?f=excel" target="_blank">Export To Excel</a></span> |
<span class="lkcsv"><a href="<%=SPContext.Current.Web.Url.ToString() %>/_layouts/niem/ExportUsers.aspx?f=csv" target="_blank">Export To CSV</a></span>
</div>
<div>
    <asp:Label id="lblMessage" runat="server" />
</div>
<asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField HeaderText="Name" DataField="Name" />
        <asp:BoundField HeaderText="First Name" DataField="FirstName"/>
        <asp:BoundField HeaderText="Last Name" DataField="LastName"/>
        <asp:BoundField HeaderText="Login Name" DataField="LoginName"/>
        <asp:BoundField HeaderText="Email" DataField="Email"/>
    </Columns>
</asp:GridView>