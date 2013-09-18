<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyNiemUserProfileUserControl.ascx.cs" Inherits="MyNiemProviderWebparts.Webparts.MyNiemUserProfile.MyNiemUserProfileUserControl" %>

<span class="regLab">Please update your profile information. Thank You.</span>

<table class="regTable">
<tr>
<td class="regLab"><asp:Label ID="Label1" runat="server" Text="First Name"></asp:Label></td>
</tr>
<tr>
<td class="regText"><asp:TextBox ID="txtFirstname" runat="server" ></asp:TextBox></td>
</tr>
<tr>
<td class="regLab">Last Name</td>
</tr>
<tr>
<td class="regText"><asp:TextBox ID="txtLastName" runat="server"></asp:TextBox></td>
</tr>
<tr>
<td class="regLab">Email</td>
</tr>
<tr>
<td class="regText"><asp:TextBox ID="txtemail" runat="server"></asp:TextBox></td>
</tr>
<tr>
<td class="regLab">Organization</td>
</tr>
<tr>
<td class="regText"><asp:TextBox ID="txtOrg" runat="server"></asp:TextBox></td>
</tr>
<tr>
<td class="regLab">Organization Type</td>
</tr>
<tr>
<td class="regText"><asp:DropDownList ID="ddlOrgType" runat="server">
                       <%--<asp:ListItem>Federal Government</asp:ListItem>
                       <asp:ListItem>Contractor</asp:ListItem>--%>
                    </asp:DropDownList>
</td>
</tr>
<tr>
<td class="regLab">Position</td>
</tr>
<tr>
<td class="regText"><asp:TextBox ID="txtPosition" runat="server"></asp:TextBox></td>
</tr>
<tr>
<td colspan="2" class="regBut"><asp:Button ID="btnEdit" runat="server" onclick="btnEdit_Click" Text="Submit Updates" /></td>
</tr>
<tr>
    <td colspan="2" class="regMess"><asp:Label runat="server" ID="lblMessage" /></td>
</tr>
</table>