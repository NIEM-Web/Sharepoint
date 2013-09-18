<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubscribeForEmailWPUserControl.ascx.cs" Inherits="lmd.NIEM.FarmSolution.SubscribeForEmailWP.SubscribeForEmailWPUserControl" %>
<div id="bc-email-subs-container">
    <h3 class="subscribHeader">Subscribe for email alerts.</h3>
    <table cellspacing="2" cellpadding="2" class="subscribTable">
        <tr>
            <td class="subscribLab">
                <span class="bc-required">*</span> First Name:
            </td>
            <td class="subscribText">
                <asp:TextBox ID="TxtFirstName" runat="server" validation="required" ErrorMessage="Please fill in the first name." />
            </td>
        </tr>
        <tr>
            <td class="subscribLab">
                <span class="bc-required">*</span> Last Name:
            </td>
            <td class="subscribText">
                <asp:TextBox ID="TxtLastName" runat="server" validation="required;" ErrorMessage="Please fill in the last name.;" />
            </td>
        </tr>
        <tr>
            <td class="subscribLab">
                <span class="bc-required">*</span> Email - Id:
            </td>
            <td class="subscribText">
                <asp:TextBox ID="TxtEmail" runat="server" validation="required;email;" ErrorMessage="Please fill in the email address.;Please enter a valid email format." />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="subscribBut">
                <asp:Button ID="BtnSubscribe" Text="Subscribe for E-mail alerts" runat="server" OnClick="BtnSubscribe_Click"
                    OnClientClick="return ValidateInput();" />
            </td>
        </tr>
    </table>
    <asp:Label ID="LblMessage" runat="server" />
</div>
