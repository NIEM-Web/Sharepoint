<%@ Assembly Name="Niem.MyNiem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2148a85e71ad92fb" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SaveSearchUserControl.ascx.cs"
    Inherits="Niem.MyNiem.Webparts.SaveSearch.SaveSearchUserControl" %>
<style type="text/css">
    .hide
    {
        display: none;
    }
</style>
<script type="text/javascript">
    function ShowForm(show) {
        if (show) {
            $('*[id$="searchSaveForm"]').removeClass("hide");
            $('*[id$="searchHelpMessage"]').addClass("hide");
        }
        else {
            $('*[id$="searchSaveForm"]').addClass("hide");
            $('*[id$="searchHelpMessage"]').removeClass("hide");
        }
    }
    function ValidateTextBox(txtNameId){
        var textBox = $("#"+ txtNameId );
        if(textBox.val().trim()==""){
            alert("Invalid Name");
            textBox.focus();
            return false;
        }
        else{
            return true;
        }
    }
</script>
<div class="search-message">
    Want to Save Search Results?
    <p id="searchHelpMessage"> Click <a href="#" onclick="javascript:ShowForm(true);">
        here </a>to save.
        </p>
</div>
<div id="searchSaveForm" class="hide" runat="server">
    <p>
        Save Search Form:</p>
    <p>
        <asp:textbox id="txtName" runat="server" />
    </p>
    <p>
    <asp:Button id="btnSaveSearch" runat="server" Text="Save" OnClick="btnSaveSearch_Click" />
    </p>
</div>
