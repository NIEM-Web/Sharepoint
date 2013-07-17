<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.WebControls" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="NiemLike" TagName="NiemLikeLink" Src="~/_controltemplates/NIEM_Like_Solution/NIEM_Like_Control.ascx" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsEventsWebpartUserControl.ascx.cs" Inherits="Niem.MyNiem.Webparts.NewsEventsWebpart.NewsEventsWebpartUserControl" %>
<script type="text/javascript">
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
<table style="height:110px">
<tr style="margin-top:6px;">
<td style="vertical-align:top;margin-right:6px;">Established Communities: </td>
<td colspan='2'><div><asp:DropDownList ID="ddlCommunities" runat="server" /></div></td>
</tr>
<tr style="margin-top:6px">
<td style="vertical-align:top;margin-right:6px;">Your Audience:</td>
<td colspan='2'><div><asp:DropDownList ID="ddlAudience" runat="server" /></div></td>
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
	<div class="docMain"><h2><a href='<%# Eval("FileRef") %>' onclick="" ><asp:Literal ID="lcTitle" Text='<%#Eval("Title") %>' runat="server" /></a></h2><%# ((int)Eval("DateDiff")) <= 7 ? "<img src=\"/_layouts/1033/images/new.gif\"/>" : string.Empty%>
       <asp:Literal ID="lcArticleDate" Text='<%#Eval("ArticleStartDate", "{0:MMMM dd, yyyy}") %>' runat="server" />- <%#Eval("ArticleByLine")%>  <%---<asp:Literal ID="lcCommittees" Text='<%#Eval("Category_x0020_Committee") %>' runat="server" />--%>
 <div class="docDesc">
	<p><asp:Literal ID="lcComments" Text='<%#Eval("PublishingPageContent") %>' runat="server" /></p><strong><a href='<%#Eval("FileRef") %>' >Read More...</a></strong>&nbsp;&nbsp;<NiemLike:NiemLikeLink ID="NiemLikeLink1" runat="server" URL='<%#Eval("FileRef")%>' CType="News" /><br/>
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
<asp:Repeater ID="NewsList" runat="server">  
<ItemTemplate>
<tr>
<td style="width: 624px">
	<%--<h2 class="resTitle"><a href='#' onclick="<%# "DisplayItem('" +Eval("LinkField")+"','"+Eval("Title")+ "');" %>" ><asp:Literal ID="lcTitle" Text='<%#Eval("Title") %>' runat="server" /></a></h2><br />--%>
    <h3><a href='<%# Eval("FileRef") %>' onclick="" ><asp:Literal ID="lcTitle" Text='<%#Eval("Title") %>' runat="server" /></a></h3>
 <asp:Literal ID="lcArticleDate" Text='<%#Eval("ArticleStartDate", "{0:MMMM dd, yyyy}") %>' runat="server" /> - <%#Eval("ArticleByLine")%>  <%--<asp:Literal ID="lcCommittees" Text='<%#Eval("Category_x0020_Committee")runat="server" />--%>
 <div class="docDesc">
	<p><asp:Literal ID="lcComments" Text='<%#Eval("PublishingPageContent") %>' runat="server" /></p><strong><a href='<%#Eval("FileRef") %>' >Read More...</a></strong>&nbsp;&nbsp;<NiemLike:NiemLikeLink ID="NiemLikeLink1" runat="server" URL='<%#Eval("FileRef")%>' CType="News" /><br/>
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