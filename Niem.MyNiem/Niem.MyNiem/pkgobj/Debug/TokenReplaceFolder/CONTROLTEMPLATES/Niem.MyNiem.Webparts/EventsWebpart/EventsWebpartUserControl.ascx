<%@ Assembly Name="Niem.MyNiem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2148a85e71ad92fb" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.WebControls" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventsWebpartUserControl.ascx.cs" Inherits="Niem.MyNiem.Webparts.EventsWebpart.EventsWebpartUserControl" %>
<script type="text/javascript">
    function DisplayItem(Url, Title) {

        //debugger; 

        SP.UI.ModalDialog.showModalDialog({

            url: Url,

            title: Title,

            allowMaximize: true,

            showClose: true,

            autoSize: true,

            dialogReturnValueCallback: silentCallback

        });

    };

    function silentCallback(dialogResult, returnValue) {

    }



    function refreshCallback(dialogResult, returnValue) {

        SP.UI.ModalDialog.RefreshPage(SP.UI.DialogResult.OK);

    }
</script>
<table style="height:110px">
<tr style="margin-top:6px;">
<td style="vertical-align:top;margin-right:6px;height:25px">&nbsp;</td>
<td colspan='2'>&nbsp;</td>
</tr>
<tr style="margin-top:6px">
<td style="vertical-align:top;margin-right:6px;">&nbsp;</td>
<td colspan='2'>&nbsp;</td>
</tr>
<tr style="margin-top:6px;">
<td>Search: </td>
<td><div><asp:TextBox ID="txtSearch" CssClass="ms-sbplain" Width="125px" runat="server" /></div></td>
<td ><div id="divSearch"><asp:Button ID="btnSearch" runat="server" /></div></td>
</tr>  
</table>

<asp:listview runat="server" id="lvResources" groupitemcount="1" OnPagePropertiesChanging="DataPager_PagePropertiesChanging">
<LayoutTemplate>
    <table runat="server" id="table1">
      <tr runat="server" id="groupPlaceholder">
      </tr>
    </table>
      
  </LayoutTemplate>
  <GroupTemplate>
    <tr runat="server" id="tableRow">
      <td runat="server" id="itemPlaceholder" />
    </tr>
  </GroupTemplate>
   <ItemTemplate>
   <td>
   <table border="0" cellpadding="0" class="docTabWrap">
   <tbody>
    

    <tr>
<td valign="top">
	<div class="docMain"><h2 class="resTitle"><a href='#' onclick="<%# "DisplayItem('" +Eval("LinkField")+"','"+Eval("Title")+ "');" %>" ><asp:Literal ID="lcTitle" Text='<%#Eval("Title") %>' runat="server" /></a></h2><br />
        <asp:Literal ID="lcEventDate" Text='<%# string.Format("{0:MMMM dd, yyyy}", System.Convert.ToDateTime(Eval("EventDate"))) %>' runat="server" /> - <asp:Literal ID="lcCommittees" Text='<%#Eval("Location") %>' runat="server" />
    </div>
 <div class="docDesc">
	<p><asp:Literal ID="lcComments" Text='<%#Eval("Description") %>' runat="server" /></p><strong><a href="#" onclick="<%# "DisplayItem('" +Eval("LinkField")+"','"+Eval("Title")+ "');" %>">Read More...</a></strong><br/>
</div>
</td>
<td>
<asp:PlaceHolder ID="ratingsPlaceHolder" runat="server" />
</td>
</tr>
   <tr>
<td colspan='2'><hr/></td>
</tr>  
    
   </tbody>
   </table>
        
   </td>

    </ItemTemplate>
</asp:listview>
<asp:DataPager runat="server" ID="DataPager" PageSize="10" 
      PagedControlID="lvResources">
  <Fields>
    <asp:NumericPagerField ButtonCount="10"
         CurrentPageLabelCssClass="CurrentPage"
         NumericButtonCssClass="PageNumbers"
         NextPreviousButtonCssClass="PageNumbers" NextPageText=" > "
         PreviousPageText=" < " />
  </Fields>
</asp:DataPager>
<table> 
<asp:Repeater ID="eventsList" runat="server" >  
<ItemTemplate>
<tr>
<td style="width: 624px">
	<h2 class="resTitle"><a href='#' onclick="<%# "DisplayItem('" +Eval("LinkField")+"','"+Eval("Title")+ "');" %>" ><asp:Literal ID="lcTitle" Text='<%#Eval("Title") %>' runat="server" /></a></h2><br />
 <asp:Literal ID="lcEventDate" Text='<%# string.Format("{0:MMMM dd, yyyy}", System.Convert.ToDateTime(Eval("EventDate"))) %>' runat="server" /> - <asp:Literal ID="lcCommittees" Text='<%#Eval("Location") %>' runat="server" />
 
 <div class="docDesc">
	<p><asp:Literal ID="lcComments" Text='<%#Eval("Description") %>' runat="server" /></p><strong><a href="#" onclick="<%# "DisplayItem('" +Eval("LinkField")+"','"+Eval("Title")+ "');" %>">Read More...</a></strong><br/>
</div>
</td>
<td>
<asp:PlaceHolder ID="ratingsPlaceHolder" runat="server" />
</td>
</tr>
<tr>
<td colspan='2'><hr/></td>
</tr>  
</ItemTemplate>   
</asp:Repeater>  
</table>
