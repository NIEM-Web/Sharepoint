<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserStatusLink.ascx.cs" Inherits="lmd.NIEM.FarmSolution.ControlTemplates.UserStatusLink" %>
<script type="text/javascript">
    var userinfoUpdated = true;
    function EditProfileInitial() {
        setTimeout('WindowEditProfileInitial()', 3000);
    }

    function WindowEditProfileInitial() {
        if(!userinfoUpdated)
        openDialog(400, 525, "Personal Information", "bgSearhTerm", "url", "/myniem/SitePages/EditProfile.aspx");
    }
    
    _spBodyOnLoadFunctionNames.push("EditProfileInitial");   
    

 </script>
<asp:Panel CssClass="login" ID="loginUtilAnony" runat="server">
<asp:HyperLink ID="hlRegister" runat="server" NavigateUrl="/Pages/Register.aspx" Text="Register here" /> / <asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="/_layouts/niem/login.aspx" Text="Login" />
</asp:Panel>
<asp:Panel CssClass="login" ID="loginUtil" runat="server">
Hello, <asp:Label ID="lblUserName" runat="server" /><br /><asp:HyperLink ID="hlCommittees" runat="server" NavigateUrl="" Text="" /> <asp:Label ID="lblDiv" runat="server"></asp:Label> <asp:HyperLink ID="hlSignout" runat="server" NavigateUrl="/_layouts/signout.aspx" Text="Logout" />
</asp:Panel>
<asp:Panel ID="showProfileScript" runat="server" Visible="false">
<script type="text/javascript">
    userinfoUpdated = true;
</script>
</asp:Panel>