<%@ Assembly Name="Niem.MyNiem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2148a85e71ad92fb" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProfileWebpartUserControl.ascx.cs" Inherits="Niem.MyNiem.Webparts.ProfileWebpart.ProfileWebpartUserControl" %>
<style type="text/css">
/*ul.NIEM-profileButtons{
	list-style-type:none;
	margin:0;
	padding-left:0;
}
ul.NIEM-profileButtons li{
	margin-bottom:5px;
}
a.NIEM-editInfo {
    background: url("/Style%20Library/niem3/images/NIEM-sprite.png") no-repeat scroll right -702px transparent;
    color: transparent;
    display: block;
    height: 35px;
    overflow: hidden;
    text-indent: -9999px;
    width: 251px;
}

a.NIEM-editInfo:hover {
    background: url("/Style%20Library/niem3/images/NIEM-sprite.png") no-repeat scroll right -747px transparent;
}
a.NIEM-changePass {
    background: url("/Style%20Library/niem3/images/NIEM-sprite.png") no-repeat scroll -778px -791px transparent;
    color: transparent;
    display: block;
    height: 35px;
    overflow: hidden;
    text-indent: -9999px;
    width: 223px;
}
a.NIEM-changePass:hover {
background: url("/Style%20Library/niem3/images/NIEM-sprite.png") no-repeat scroll -778px -835px transparent;
}*/
</style>
<script type="text/javascript">
    function EditProfile() {

        //debugger;
        var RelativeURL = $().SPServices.SPGetCurrentSite().toString() + "/sitepages/editprofile.aspx";
        SP.UI.ModalDialog.showModalDialog({

            url: RelativeURL,

            title: "Edit Profile",

            allowMaximize: true,

            showClose: true,

            width: 350,

            height: 515,

            dialogReturnValueCallback: silentCallback

        });
        $(".ms-dlgBorder").attr('id', 'EditProfileIcon');

    };

    function ChangePassword() {

        //debugger; 
        var RelativeURL = $().SPServices.SPGetCurrentSite().toString() + "/sitepages/changepassword.aspx";
        SP.UI.ModalDialog.showModalDialog({

            url: RelativeURL,

            title: "Change Password",

            allowMaximize: true,

            showClose: true,

            width: 380,

            height: 350,

            dialogReturnValueCallback: silentCallback

        });
        $(".ms-dlgBorder").attr('id', 'ChangePassIcon');

    };

    function silentCallback(dialogResult, returnValue) {

    }



    function refreshCallback(dialogResult, returnValue) {

        SP.UI.ModalDialog.RefreshPage(SP.UI.DialogResult.OK);

    }

    ExecuteOrDelayUntilScriptLoaded(SetUserNameInTitle, "sp.js");

    function SetUserNameInTitle() {

        $('#NiemUser').html(helloUserNameText);
    }

</script>


<div>
    <table cellpadding="3" cellspacing="3" width="100%">
    <tr>
    
    <td valign="top" align="left">
        <h2>My Information</h2>
        <strong><asp:Label ID="lblName" runat="server" /></strong><br />
        <em><asp:Label ID="lblPosition" runat="server" /></em><br />
        <asp:Label ID="lblOrganization" runat="server"/><br />
        <small><asp:Label ID="lblOrgType" runat="server"/></small><br />
        <asp:Label ID="lblEmail" runat="server"/><br />
    </td>
    <td>
        <ul class="NIEM-profileButtons">
	        <li><a class="NIEM-editInfo" href="#" onclick="EditProfile();">Edit My Information</a></li>
	        <li><a class="NIEM-changePass" href="#" onclick="ChangePassword();">Change Password</a></li>
        </ul>
    </td>
    </tr>
    <tr>
    <td colspan="2"><hr /></td>
    </tr>
    <tr>
    <td valign="top">
        <h2>My Communities of Interest</h2>
        <asp:CheckBoxList ID="cbEstablishedCommunities" AutoPostBack="true" runat="server">
        </asp:CheckBoxList>
    </td>
    <td valign="top">
        <h2>&nbsp;</h2>
        <asp:CheckBoxList ID="cbAudience" AutoPostBack="true" runat="server">
        </asp:CheckBoxList>
    </td>
    </tr>
    </table>
</div>

