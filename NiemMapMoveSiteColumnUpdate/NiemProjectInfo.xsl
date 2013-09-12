<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
	<xsl:output method="html" indent="no"/>
	<xsl:decimal-format NaN=""/>
	<xsl:param name="dvt_apos">&apos;</xsl:param>
	<xsl:param name="ManualRefresh"/>
  <xsl:param name="WebID">{35a70913-5617-49a7-b079-1ee87de7a4d3}</xsl:param>
  <xsl:param name="ListID">{7AF8549F-607D-4B0B-BD8D-75FA7C3F4758}</xsl:param>
	<xsl:param name="search" />
	<xsl:param name="dvt_firstrow">1</xsl:param>
	<xsl:param name="dvt_nextpagedata" />
	<xsl:variable name="dvt_1_automode">0</xsl:variable>

	

	<xsl:template match="/" xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:SharePoint="Microsoft.SharePoint.WebControls">
		<xsl:choose>
			<xsl:when test="($ManualRefresh = 'True')">
				<table width="100%" border="0" cellpadding="0" cellspacing="0">
					<tr>
						<td valign="top">
							<xsl:call-template name="dvt_1"/>
						</td>
						<td width="1%" class="ms-vb" valign="top">
							<img src="/_layouts/images/staticrefresh.gif" id="ManualRefresh" border="0" onclick="javascript: {ddwrt:GenFireServerEvent('__cancel')}" alt="Click here to refresh the dataview."/>
						</td>
					</tr>
				</table>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="dvt_1"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="dvt_1">
		<xsl:variable name="dvt_StyleName">RepForm1</xsl:variable>
		<xsl:variable name="Rows" select="/dsQueryResponse/Rows/Row[($search !='' and contains(translate(@ProjectTitle,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ'),translate($search,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ'))) or ($search='')]" />		

		<xsl:variable name="dvt_RowCount" select="count($Rows)" />
		<xsl:variable name="RowLimit" select="5" />
		<xsl:variable name="FirstRow" select="$dvt_firstrow" />
		<xsl:variable name="LastRow">
			<xsl:choose>
				<xsl:when test="($FirstRow + $RowLimit - 1) &gt; $dvt_RowCount"><xsl:value-of select="$dvt_RowCount" /></xsl:when>
				<xsl:otherwise><xsl:value-of select="$FirstRow + $RowLimit - 1" /></xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		
		<xsl:variable name="IsEmpty" select="$dvt_RowCount = 0 or $RowLimit = 0" />
		
		
		
		<xsl:variable name="dvt_IsEmpty" select="$dvt_RowCount = 0" />
				<style type="text/css">
					#pl-search-criteria {
						width: 250px;
						height:35px;
						line-height:35px;
						padding-left:30px;
						font-size: 11px;
						font-style:normal;
						border-color: #e3e3e3;
						border-width: 0px;
						border-style: solid;
						border-right:none;
						background-image:url(&quot;/Style%20Library/NIEM3/images/NIEM-sprite.png&quot;);
						background-attachment:scroll;
						background-repeat:no-repeat;
						background-position: -520px -100px;
						background-color: transparent;
						vertical-align:top;
					}
					#pl-search-button {
						vertical-align:top;
					}	
					#pl-search-button &gt; img {
						border:none;
					}
					#pl-new-project {
						padding-left:80px;
						vertical-align:top;
					}
					#pl-new-project &gt; img {
						border:none;
					}
				</style>   

		<xsl:choose>
			<xsl:when test="$dvt_IsEmpty">
				<xsl:call-template name="dvt_1.empty" />
			</xsl:when>
			<xsl:otherwise>
				<div>
					<input id="pl-search-criteria" />	
					<a id="pl-search-button" href="javascript:void(0);"><img alt="Search Projects" src="/Style Library/niem/js/planet/images/searchProjects.png" /></a>
					<a id="pl-new-project" href="javascript:void(0);"><img alt="Submit New Project" src="/Style Library/niem/js/planet/images/submitProject.png" /></a>		
				</div>
				
				<table border="0" width="100%">
					<xsl:call-template name="dvt_1.body">
						<xsl:with-param name="Rows" select="$Rows[position() &gt;= $FirstRow and position() &lt;= $LastRow]" />
						
						
						
						<xsl:with-param name="FirstRow" select="1" />
						<xsl:with-param name="LastRow" select="$dvt_RowCount" />

						

						

						

					</xsl:call-template>
				</table>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:call-template name="dvt_1.commandfooter">
			<xsl:with-param name="FirstRow" select="$FirstRow" />
			<xsl:with-param name="LastRow" select="$LastRow" />
			<xsl:with-param name="RowLimit" select="$RowLimit" />
			<xsl:with-param name="dvt_RowCount" select="$dvt_RowCount" />
			<xsl:with-param name="RealLastRow" select="number(ddwrt:NameChanged('',-100))" />
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="dvt_1.body">
		<xsl:param name="Rows" />
		<xsl:param name="FirstRow" />
		<xsl:param name="LastRow" />
		<xsl:for-each select="$Rows">
			<xsl:variable name="dvt_KeepItemsTogether" select="false()" />
			<xsl:variable name="dvt_HideGroupDetail" select="false()" />
			<xsl:if test="(position() &gt;= $FirstRow and position() &lt;= $LastRow) or $dvt_KeepItemsTogether">
				<xsl:if test="not($dvt_HideGroupDetail)" ddwrt:cf_ignore="1">
					<xsl:call-template name="dvt_1.rowview" />
				</xsl:if>
			</xsl:if>
		</xsl:for-each>

		

	</xsl:template>
	<xsl:template name="dvt_1.rowview">
		<!--
		<xsl:if test="ddwrt:IfHasRights(16)">
			<tr>
				<td width="100%" colspan="2" class="ms-vb">
				
				</td>
			</tr>
		</xsl:if>
		-->
		<tr>
			<td width="100%" class="ms-vb">
				<b>
					<a href="javascript:void(0);" onclick="javascript:openDisplayForm({@ID});">
										

						<xsl:if test="@BestOfNIEM='1' or msxsl:string-compare(string(@BestOfNIEM),'Yes','','i')=0 or msxsl:string-compare(string(@BestOfNIEM),'True','','i')=0">
							<img alt="Best of NIEM" src="/Style Library/niem/js/planet/images/BestOfNIEM.png" style="padding-right:10px; padding-left:2px; vertical-align: middle; border:none" />

						</xsl:if>
						<xsl:if test="@CaseStudy='1' or msxsl:string-compare(string(@CaseStudy),'Yes','','i')=0 or msxsl:string-compare(string(@CaseStudy),'True','','i')=0">
							<img alt="Case Study" src="/Style Library/niem/js/planet/images/CaseStudy.png" style="padding-right:10px; padding-left:2px; vertical-align: middle; border:none" />
						</xsl:if>

						<xsl:value-of select="@ProjectTitle"/>
					</a>
				</b>
				<span style="padding-left:20px">
					<xsl:value-of select="@WorkCity" />, <xsl:value-of select="@WorkState" />
				</span>
			</td>
		</tr>
		<tr>
			<td width="100%" class="ms-vb">
				<xsl:value-of select="@ProjectDescription" />
				<hr />
			</td>
		</tr>
		<xsl:if test="$dvt_1_automode = '1'" ddwrt:cf_ignore="1">
			<tr>
				<td width="100%" colspan="2" class="ms-vb">
					<span ddwrt:amkeyfield="ID" ddwrt:amkeyvalue="ddwrt:EscapeDelims(string(@ID))" ddwrt:ammode="view" />
				</td>
			</tr>
		</xsl:if>
	</xsl:template>
	<xsl:template name="dvt_1.empty">
		<xsl:variable name="dvt_ViewEmptyText">There are no items to show in this view.</xsl:variable>
		<table border="0" width="100%">
			<tr>
				<td style="height: 64px; vertical-align:top">

					<div>
						<input id="pl-search-criteria" />	
						<a id="pl-search-button" href="javascript:void(0);"><img alt="Search Projects" src="/Style Library/niem/js/planet/images/searchProjects.png" /></a>
						<a id="pl-new-project" href="javascript:void(0);"><img alt="Submit New Project" src="/Style Library/niem/js/planet/images/submitProject.png" /></a>		
					</div>
				</td>
			</tr>
			<tr>
				<td class="ms-vb">
					<xsl:value-of select="$dvt_ViewEmptyText"/>
				</td>
			</tr>
		</table>
	</xsl:template>
	<xsl:template name="dvt_1.commandfooter">
		<xsl:param name="FirstRow" />
		<xsl:param name="LastRow" />
		<xsl:param name="RowLimit" />
		<xsl:param name="dvt_RowCount" />
		<xsl:param name="RealLastRow" />
		<table cellspacing="0" cellpadding="4" border="0" width="100%">
			<tr>
				<xsl:if test="$FirstRow &gt; 1 or $LastRow &lt; $dvt_RowCount">
					<xsl:call-template name="dvt_1.navigation">
						<xsl:with-param name="FirstRow" select="$FirstRow" />
						<xsl:with-param name="LastRow" select="$LastRow" />
						<xsl:with-param name="RowLimit" select="$RowLimit" />
						<xsl:with-param name="dvt_RowCount" select="$dvt_RowCount" />
						<xsl:with-param name="RealLastRow" select="$RealLastRow" />
					</xsl:call-template>
				</xsl:if>
			</tr>
		</table>
	</xsl:template>
	<xsl:template name="dvt_1.navigation">
		<xsl:param name="FirstRow" />
		<xsl:param name="LastRow" />
		<xsl:param name="RowLimit" />
		<xsl:param name="dvt_RowCount" />
		<xsl:param name="RealLastRow" />
		<xsl:variable name="PrevRow">
			<xsl:choose>
				<xsl:when test="$FirstRow - $RowLimit &lt; 1">1</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$FirstRow - $RowLimit" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="LastRowValue">
			<xsl:choose>
				<xsl:when test="$LastRow &gt; $RealLastRow">
					<xsl:value-of select="$LastRow"></xsl:value-of>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$RealLastRow"></xsl:value-of>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="NextRow">
			<xsl:value-of select="$LastRowValue + 1"></xsl:value-of>
		</xsl:variable>
		<td nowrap="nowrap" class="ms-paging" align="right">
			<xsl:if test="$dvt_firstrow &gt; 1" ddwrt:cf_ignore="1">
				<a>
				<xsl:attribute name="href">javascript: <xsl:value-of select="ddwrt:GenFireServerEvent('dvt_firstrow={1};dvt_startposition={}')" />;</xsl:attribute>
				Start</a>
				<xsl:text disable-output-escaping="yes" ddwrt:nbsp-preserve="yes">&amp;nbsp;</xsl:text>
				<a>
				<xsl:attribute name="href">javascript: history.back();</xsl:attribute>
				<img src="/_layouts/images/prev.gif" border="0" alt="Previous" />
				</a>
				<xsl:text disable-output-escaping="yes" ddwrt:nbsp-preserve="yes">&amp;nbsp;</xsl:text>
			</xsl:if>
			<xsl:value-of select="$FirstRow" />
			 - <xsl:value-of select="$LastRowValue" />
			<xsl:text xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" ddwrt:nbsp-preserve="yes" disable-output-escaping="yes">&amp;nbsp;</xsl:text>
			<xsl:if test="$LastRowValue &lt; $dvt_RowCount or string-length($dvt_nextpagedata)!=0" ddwrt:cf_ignore="1">
				<a>
				<xsl:attribute name="href">javascript: <xsl:value-of select="ddwrt:GenFireServerEvent(concat('dvt_firstrow={',$NextRow,'};dvt_startposition={',$dvt_nextpagedata,'}'))" />;</xsl:attribute>
				<img src="/_layouts/images/next.gif" border="0" alt="Next" />
				</a>
			</xsl:if>
		</td>
	</xsl:template>
</xsl:stylesheet>