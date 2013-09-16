<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.WebControls" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReviewViewer.ascx.cs" Inherits="NIEM.Reviews.ReviewViewer.ReviewViewer" %>
<style type="text/css">
.reviewDate
{
    font-size:smaller;
    font-style: italic;
    float: left;
}
.reviewAuthor
{
    font-size:smaller;
    font-style:italic;
    float:right;
}
.reviewBody
{
    clear: both;
    padding-bottom: 5px;
}
.reviewHeader
{
    padding-bottom: 2px;
    font-size: larger;
    font-weight: bold;
    float: left;
}
.reviewItem
{
    clear: both;
    width: 100%;
}
.editLink
{
    float: right;
}
</style>
<script type="text/javascript">
    function CreateEditReview(formUrl, articleUrl)
    {
        var options =
         {
          url: formUrl
        , args: {
            articleUrl: articleUrl
        },
        dialogReturnValueCallback: function (dialogResult)
        {
            SP.UI.ModalDialog.RefreshPage(dialogResult)
        } 
      };
   
        modalDialog = SP.UI.ModalDialog.showModalDialog(options);

         
     }
</script>
<asp:Repeater Visible="true" ID="reviewListContainer" runat="server">
<HeaderTemplate>
    <div class="reviewHeader"><a href="<%=articleUrl %>">Reviews for: <%=articleTitle %></a></div>
    <div class="editLink"><a href="<%=formHyperlink %>"><%=formHyperlinkText %></a></div> 
    <br />  
</HeaderTemplate>
<ItemTemplate>
    <div class="reviewItem">
        <h3 class="reviewHeader"><%#Eval("Title") %></h3>
        <div>
        
        </div>
    </div>
     <br />
    <div class="reviewBody"><%#Eval("Body") %></div>
    <div class="reviewDate">Published: <%#Eval("PublishDate") %></div>
    <div class="reviewAuthor">Author: <%#Eval("Author") %></div>
</ItemTemplate>

<SeparatorTemplate>
<br />
<hr />
</SeparatorTemplate>
<FooterTemplate>
    <div style="clear: both"><a href="<%=returnUrl %>">Go Back</a></div>
</FooterTemplate>
</asp:Repeater>


<asp:Label ID="notificationSection" Visible="false" runat="server"><h3>No query was passed.</h3></asp:Label>
