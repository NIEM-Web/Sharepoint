<%@ Assembly Name="Niem.MyNiem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2148a85e71ad92fb" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register TagPrefix="SharePointPortalControls" Namespace="Microsoft.SharePoint.Portal.WebControls" Assembly="Microsoft.SharePoint.Portal, Version=14.0.0.0, Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ToolsWebpartUserControl.ascx.cs" Inherits="Niem.MyNiem.Webparts.ToolsWebpart.ToolsWebpartUserControl" %>
<script type="text/javascript">
function isconfirm()
{
var ck=confirm("Do you want to delete this item?");
if(ck==true)
return true;
else
return false;
}
    function DisplayItem(Url, Title) {

        //debugger; 

        SP.UI.ModalDialog.showModalDialog({

            url: Url,

            title: Title,

            allowMaximize: true,

            showClose: true,

            autoSize:true,

            dialogReturnValueCallback: silentCallback

        });

    };

    function silentCallback(dialogResult, returnValue) {

    }



    function refreshCallback(dialogResult, returnValue) {

        SP.UI.ModalDialog.RefreshPage(SP.UI.DialogResult.OK);

    }
</script>
<table>
<asp:Repeater ID="toolsList" runat="server">  
<ItemTemplate>
<tr>
<td style="width: 624px">
<div class="tolOuterWrp">
   <div class="tolWrpleft">
        <div class="toolname">
        <h3>
        <a href='#' onclick="<%# "DisplayItem('" +Eval("LinkField")+"','"+Eval("Title")+ "');" %>" >
        <asp:Literal ID="lcTitle" Text='<%#Eval("Title") %>' runat="server" />
            </a></h3>
        </div>
        <p class="tooldesc">
        <asp:Literal ID="lcComments" Text='<%#Eval("_Comments") %>' runat="server" />
        </p>
        <div class="toolmpd">
        <strong>Relevant MPD Classes: </strong>
       <asp:Literal ID="lcMPD" Text='<%#Eval("MPD_x0020_Classes") %>' runat="server" />
        </div>
        <div class="tooliepd">
        <strong>IEPD Lifecycle Phases: </strong>
        <%#Eval("IEPD_x0020_Lifecycle_x0020_Phase") %>
        </div>
        <div class="toolartifact">
        <strong>Artifacts Produced: </strong>
        <%#Eval("Artifacts_x0020_Produced") %>
        </div>
        <div class="tooladmin">
        <strong>Administrative Contact :</strong>
        <a href='mailto:<%#Eval("EMail") %>'><%#Eval("EMail") %></a>
        </div>
        
   </div>
   <div class="tolWrpRight">
   <asp:PlaceHolder ID="ratingsPlaceHolder" runat="server" />
   <asp:LinkButton ID="btnRemove" Text="Remove" runat="server" CommandArgument='<%#Eval("ListId")+":"+Eval("ID") %>' CommandName="Delete" OnClientClick="return isconfirm()"/>
   </div>
</div>    
   
</td>

</tr>
</ItemTemplate>   
</asp:Repeater>  
</table>
