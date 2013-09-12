<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.WebControls" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="NiemLike" TagName="NiemLikeLink" Src="~/_controltemplates/NIEM_Like_Solution/NIEM_Like_Control.ascx" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
 <%@ Register Tagprefix="SharePointPortalControls" Namespace="Microsoft.SharePoint.Portal.WebControls" Assembly="Microsoft.SharePoint.Portal, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourcesWebpartUserControl.ascx.cs"
    Inherits="Niem.MyNiem.Webparts.ResourcesWebpart.ResourcesWebpartUserControl" %>
<script type="text/javascript">
    $(document).ready(function () {

        $('.viewdoc').each(function () {
            if ($(this).attr('href') == '') {
                $(this).css('display', 'none');
            }

        });

        $('.downloaddoc').each(function () {
            if ($(this).attr('href') == '') {
                $(this).css('display', 'none');
            }

        });

    });

    function OpenDialog(url, title)
    {
        top.location = '/Pages/ReviewList.aspx?url=' + url + '&amp;title=' + title;
    }
</script>
<table>
    <tr>
        <td style="vertical-align: top; margin-right: 6px;">
            Document Category:
        </td>
        <td colspan='2'>
            <div>
                <asp:dropdownlist id="ddlCategory" runat="server" />
            </div>
        </td>
    </tr>
    <tr style="margin-top: 6px;">
        <td style="vertical-align: top; margin-right: 6px;">
            Established Communities:
        </td>
        <td colspan='2'>
            <div>
                <asp:dropdownlist id="ddlCommunities" runat="server" />
            </div>
        </td>
    </tr>
    <tr style="margin-top: 6px">
        <td style="vertical-align: top; margin-right: 6px;">
            Your Audience:
        </td>
        <td colspan='2'>
            <div>
                <asp:dropdownlist id="ddlAudience" runat="server" />
            </div>
        </td>
    </tr>
    <tr style="margin-top: 6px;">
        <td>
            Search:
        </td>
        <td>
            <div>
                <asp:textbox id="txtSearch" CssClass="ms-sbplain" width="150px" runat="server" />
            </div>
        </td>
        <td>
            <div id="divSearch">
                <asp:button id="btnSearch" runat="server" />
            </div>
        </td>
    </tr>
</table>
<asp:listview runat="server" id="lvResources" groupitemcount="1" OnItemDataBound="lvResources_ItemDataBound" OnPagePropertiesChanging="DataPager_PagePropertiesChanging">
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
        <td valign="top" width="90%">
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
                <%--# ((int)Eval("DateDiff")) <= 7 ? "<img src=\"/_layouts/1033/images/new.gif\"/>" : string.Empty
                --%>
               
                
                
            </h2>
            <span class="resSpace">|</span> 
        <div class="resFolderName"><%# Eval("FileDirRef")%> </div>
            </div>
              <div class="docDesc"><%#Eval("_Comments")%></div>
              <div class="picDwLinks">
            <a href='<%#Eval("Flipbook_x0020_Url0")%>' class="viewdoc" target="_blank">View Online</a>
            <a href='<%# pdfLink(Eval("Pdf_x0020_Url").ToString())%>' class="downloaddoc" target="_blank">
            Download PDF</a>
            </div>
        </td>
        <td valign="top">
            <div class="div-ms-vb resRat">
            <asp:PlaceHolder ID="ratingsPlaceHolder" runat="server" />
            <br />
            <a href="javascript:OpenDialog('<%#Eval("FileRef") %>','<%#Eval("Title") %>');">See Reviews</a>
            <br/><br />
            <NiemLike:NiemLikeLink ID="NiemLikeLink1" runat="server" URL='<% #Eval("FileRef")%>' CType="Resource" />
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
</asp:DataPager>
<table>
    <asp:repeater id="resourcesList" runat="server">  
<ItemTemplate>
<tr>
<td style="width: 624px">
	<h2 class="resTitle"><a href='<%#Eval("FileRef") %>'><asp:Literal ID="lcTitle" Text='<%#Eval("Title") %>' runat="server" /></a></h2>
     <span class="resSpace">|</span>

<%--<span class="resFolderName"> 
	 <asp:Literal ID="lcCategory" Text='<%#Eval("Resource_x0020_Category") %>' runat="server" />
</span>--%>

 <div class="resCom">
	<asp:Literal ID="lcComments" Text='<%#Eval("_Comments") %>' runat="server" /><br/>
</div>

</td>
 <td valign="top">
            <div class="div-ms-vb resRat">
            <%# getRating(System.Convert.ToDouble(string.IsNullOrEmpty(Eval("AverageRating").ToString()) ? "0" : Eval("AverageRating")))%>
             </div>
        </td>
</tr>
</ItemTemplate>   
</asp:repeater>
</table>
