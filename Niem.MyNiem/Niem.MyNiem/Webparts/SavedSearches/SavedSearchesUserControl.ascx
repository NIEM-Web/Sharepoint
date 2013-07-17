<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SavedSearchesUserControl.ascx.cs" Inherits="Niem.MyNiem.Webparts.SavedSearches.SavedSearchesUserControl" %>
<script type="text/javascript">
    function ChangeDDLSearch() {
        if ($("#ddlSearches option:selected")!=null && $("#ddlSearches option:selected").attr("data-url") != "") {
            window.location = $("#ddlSearches option:selected").attr("data-url") + "&saved=" + $("#ddlSearches option:selected").attr("value");
        }
    }
</script><br /><br />

					<table  class="srch-advancedtable" border="0" style="margin:0px; padding:0px;">
						<tr>
							<td class="ms-advsrchText-v1" width="123px" style="width:123px">&nbsp;</td><td class="ms-advsrchText-v2" align="left">
                            <asp:Repeater runat="server" ID="rptDropDown">
    <HeaderTemplate>
        <select id="ddlSearches" onchange="javascript:ChangeDDLSearch();">
            <option value="">Select Search</option>
    </HeaderTemplate>
    <ItemTemplate>
        <option data-url="<%# DataBinder.Eval(Container.DataItem, "SearchURL") %>" 
        value="<%# DataBinder.Eval(Container.DataItem, "ID") %>">
        <%# DataBinder.Eval(Container.DataItem, "Title") %></option>
    </ItemTemplate>
    <FooterTemplate>
        </select>
    </FooterTemplate>        
</asp:Repeater>
</td>
</tr>
</table>