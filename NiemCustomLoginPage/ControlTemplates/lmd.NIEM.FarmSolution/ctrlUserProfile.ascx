<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlUserProfile.ascx.cs" Inherits="lmd.NIEM.FarmSolution.ctrlUserProfile" %>
<table class="regTable">
<tr>
<td class="regLab">User</td>
<td class="regTextStatic"><asp:Label runat="server" Text="" ID="lblUser" /></td>
</tr>
<tr>
<td class="regLab"><asp:Label ID="Label1" runat="server" Text="First Name"></asp:Label></td>
<td class="regText"><asp:TextBox ID="txtFirstname" runat="server" ></asp:TextBox></td>
</tr>
<tr>
<td class="regLab">Last Name</td>
<td class="regText"><asp:TextBox ID="txtLastName" runat="server" ReadOnly="True"></asp:TextBox></td>
</tr>
<tr>
<td class="regLab">Email</td>
<td class="regText"><asp:TextBox ID="txtemail" runat="server" ReadOnly="True"></asp:TextBox></td>
</tr>
<tr>
<td class="regLab">Organization</td>
<td class="regText"><asp:TextBox ID="txtOrg" runat="server" ReadOnly="True"></asp:TextBox></td>
</tr>
<tr>
<td colspan="2" class="regBut"><asp:Button ID="btnEdit" runat="server" onclick="btnEdit_Click" Text="Save" /></td>
</tr>
<tr>
    <td colspan="2" class="regMess"><asp:Label runat="server" ID="lblMessage" /></td>
</tr>
</table>
<asp:HiddenField ID="profileID" runat="server" />




