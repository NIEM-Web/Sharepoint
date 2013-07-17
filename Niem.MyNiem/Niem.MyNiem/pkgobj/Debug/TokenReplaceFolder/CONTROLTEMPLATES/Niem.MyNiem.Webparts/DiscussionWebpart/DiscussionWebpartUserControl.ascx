<%@ Assembly Name="Niem.MyNiem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2148a85e71ad92fb" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscussionWebpartUserControl.ascx.cs" Inherits="Niem.MyNiem.Webparts.DiscussionWebpart.DiscussionWebpartUserControl" %>
<script src="/_layouts/JS/pagination.js" type="text/javascript"></script>

<div id="discussionBoardsDiv" runat="server">
</div>
<%--<asp:listview runat="server" id="lvResources" groupitemcount="1" OnPagePropertiesChanging="DataPager_PagePropertiesChanging">
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
            <div class="picblock">
                 <a href='<%#Eval("FileRef")%>' class="docthumb">
                    <%#Eval("PublishingRollupImage")%> 
                  </a>
            </div>
            <div class="docMain">
    <h2 class="resTitle">
                <a href='<%#Eval("FileRef")%>'>
                    <%#Eval("Title")%> 
                </a>
            </h2>
            <span class="resSpace">|</span> 
        <div class="resFolderName"><%# Eval("FileDirRef")%> </div>
            </div>
              <div class="docDesc"><%#Eval("_Comments")%></div>
              <div class="picDwLinks">
            <a href='Eval("Flipbook_x0020_Url0"' class="viewdoc" target="_blank">View Online</a>
            <a href='<%#Eval("Pdf_x0020_Url")%>' class="downloaddoc" target="_blank">
            Download PDF</a>
           &nbsp;&nbsp; <NiemLike:NiemLikeLink ID="NiemLikeLink1" runat="server" URL='<% #Eval("FileRef")%>' CType="Resource" />
        </div>
        </td>
        <td valign="top">
            <div class="div-ms-vb resRat">
               <asp:PlaceHolder ID="ratingsPlaceHolder" runat="server" /> 
           </div>
        </td>
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
</asp:DataPager>--%>
