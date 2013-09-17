<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal" xmlns:NIEMLIKE="NIEM_Like_Solution"
xmlns:Portal="Microsoft.SharePoint.Portal.WebControls">
<xsl:output method="html" indent="no"/>
<xsl:decimal-format NaN=""/>
<xsl:param name="dvt_apos">&apos;</xsl:param>
<xsl:param name="ManualRefresh"></xsl:param>
<xsl:param name="Folder" />
<xsl:param name="FilterAudience" />
<xsl:param name="FilterContent" />
<xsl:param name="FilterDomain" />
<xsl:param name="User" />
<xsl:param name="dvt_firstrow">1</xsl:param>
<xsl:param name="dvt_nextpagedata" />
<xsl:param name="ListID">{C440242E-26F4-49B5-9BEF-F3D84C4DDA9C}</xsl:param>
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
							<xsl:variable name="dvt_StyleName">Table</xsl:variable>
							<xsl:variable name="Rows" select="/dsQueryResponse/Rows/Row[
(boolean($FilterAudience='') and boolean($FilterDomain='') and boolean($FilterContent='') and boolean(@ID!=-1) and boolean($Folder=''))
or
							
(contains(translate(@FileDirRef,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), 
          translate(concat('/', $Folder),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')) 
and boolean($Folder!='') and boolean($FilterAudience='') and boolean($FilterDomain='') and boolean($FilterContent='')) 

or

(boolean($FilterAudience='') and boolean($FilterDomain='') and boolean($Folder='') and boolean($FilterContent!='') and
boolean(contains(translate(@_Comments,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterContent,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')
)))

or
(boolean($FilterAudience='') and boolean($Folder='') and boolean($FilterDomain!='') and boolean($FilterContent='') and
boolean(contains(translate(@Category_x0020_Domains.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterDomain,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')
)))

or
(boolean($FilterAudience!='') and boolean($Folder='') and boolean($FilterDomain='') and boolean($FilterContent='') and
boolean(contains(translate(@Category_x0020_Subject_x0020_Area_x002F_Audience.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterAudience,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'))))

or
(boolean($FilterAudience!='') and boolean($FilterDomain!='') and boolean($Folder='') and boolean($FilterContent='') and
boolean(contains(translate(@Category_x0020_Subject_x0020_Area_x002F_Audience.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterAudience,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'))) and
boolean(contains(translate(@Category_x0020_Domains.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterDomain,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'))))

or
(boolean($FilterAudience!='') and boolean($FilterDomain='') and boolean($Folder='') and boolean($FilterContent!='') and
boolean(contains(translate(@Category_x0020_Subject_x0020_Area_x002F_Audience.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterAudience,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'))) and
boolean(contains(translate(@_Comments,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterContent,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'))))

or
(boolean($FilterAudience='') and boolean($FilterDomain!='') and boolean($FilterContent!='') and boolean($Folder='')and
boolean(contains(translate(@Category_x0020_Domains.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterDomain,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
and
boolean(contains(translate(@_Comments,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterContent,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'))))

or
(
boolean($Folder='')
and
boolean($FilterAudience!='') 
and 
boolean($FilterDomain!='') 
and 
boolean($FilterContent!='') 
and
boolean(contains(translate(@Category_x0020_Domains.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterDomain,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
and
boolean(contains(translate(@_Comments,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterContent,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
and
boolean(contains(translate(@Category_x0020_Subject_x0020_Area_x002F_Audience.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterAudience,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
)

or

(boolean($FilterAudience='') and boolean($FilterDomain='') and boolean($Folder!='') and boolean($FilterContent!='') and
boolean(contains(translate(@_Comments,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterContent,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')
)) and contains(translate(@FileDirRef,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), 
          translate(concat('/', $Folder),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))

or
(boolean($FilterAudience='') and boolean($Folder!='') and boolean($FilterDomain!='') and boolean($FilterContent='') and
boolean(contains(translate(@Category_x0020_Domains.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterDomain,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')
)) and contains(translate(@FileDirRef,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), 
          translate(concat('/', $Folder),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))

or
(boolean($FilterAudience!='') and boolean($FilterDomain!='') and boolean($Folder!='') and boolean($FilterContent='') and
boolean(contains(translate(@Category_x0020_Subject_x0020_Area_x002F_Audience.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterAudience,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'))) and
boolean(contains(translate(@Category_x0020_Domains.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterDomain,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
and contains(translate(@FileDirRef,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), 
          translate(concat('/', $Folder),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))

or
(boolean($FilterAudience!='') and boolean($FilterDomain='') and boolean($Folder!='') and boolean($FilterContent!='') and
boolean(contains(translate(@Category_x0020_Subject_x0020_Area_x002F_Audience.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterAudience,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'))) and
boolean(contains(translate(@_Comments,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterContent,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
and contains(translate(@FileDirRef,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), 
          translate(concat('/', $Folder),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))

or
(boolean($FilterAudience!='') and boolean($Folder!='') and boolean($FilterDomain='') and boolean($FilterContent='') and
boolean(contains(translate(@Category_x0020_Subject_x0020_Area_x002F_Audience.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterAudience,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
and contains(translate(@FileDirRef,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), 
          translate(concat('/', $Folder),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
		  
or
(boolean($FilterAudience='') and boolean($FilterDomain!='') and boolean($FilterContent!='') and boolean($Folder!='')and
boolean(contains(translate(@Category_x0020_Domains.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterDomain,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
and
boolean(contains(translate(@_Comments,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterContent,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
and contains(translate(@FileDirRef,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), 
          translate(concat('/', $Folder),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
		  
or
(
boolean($Folder!='')
and
boolean($FilterAudience!='') 
and 
boolean($FilterDomain!='') 
and 
boolean($FilterContent!='') 
and
boolean(contains(translate(@Category_x0020_Domains.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterDomain,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
and
boolean(contains(translate(@_Comments,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterContent,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
and
boolean(contains(translate(@Category_x0020_Subject_x0020_Area_x002F_Audience.,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),
translate($FilterAudience,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')))
and contains(translate(@FileDirRef,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), 
          translate(concat('/', $Folder),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'))
)
]"/>
							
<xsl:variable name="dvt_RowCount" select="count($Rows)"/>
							<xsl:variable name="RowLimit" select="10" />
							<xsl:variable name="FirstRow" select="$dvt_firstrow" />
							<xsl:variable name="LastRow">
								<xsl:choose>
									<xsl:when test="($FirstRow + $RowLimit - 1) &gt; $dvt_RowCount"><xsl:value-of select="$dvt_RowCount" /></xsl:when>
									<xsl:otherwise><xsl:value-of select="$FirstRow + $RowLimit - 1" /></xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
<xsl:variable name="IsEmpty" select="$dvt_RowCount = 0 or $RowLimit = 0" />
		
							
							
							<xsl:variable name="dvt_IsEmpty" select="$dvt_RowCount = 0"/>
							
							<xsl:choose>
								<xsl:when test="$dvt_IsEmpty">
									<xsl:call-template name="dvt_1.empty"/>
			</xsl:when>
								<xsl:otherwise>
				<table border="0" width="100%" cellpadding="2" cellspacing="0" class="full_length">
					<xsl:call-template name="dvt_1.body">
						<xsl:with-param name="Rows" select="$Rows[position() &gt;= $FirstRow and position() &lt;= $LastRow]"/>
						
						
						
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
							<xsl:param name="Rows"/>
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
		<tr>
			<xsl:if test="position() mod 2 = 1">
				<xsl:attribute name="class">ms-alternating</xsl:attribute>
			</xsl:if>
			<xsl:if test="$dvt_1_automode = '1'" ddwrt:cf_ignore="1">
				<td class="ms-vb" width="1%" nowrap="nowrap">
					<span ddwrt:amkeyfield="ID" ddwrt:amkeyvalue="ddwrt:EscapeDelims(string(@ID))" ddwrt:ammode="view"></span>
				</td>
			</xsl:if>
			<td class="ms-vb">
		
<xsl:if test="not(position() = 1)"><hr /></xsl:if>

<table border="0" cellpadding="0" cellspacing="0" class="docTabWrap">
<tr><td valign="top">

<div class="picblock">
<xsl:if test="not(normalize-space(@PublishingRollupImage) = '')"><a href="{@FileRef}" class="docthumb">
<xsl:value-of select="@PublishingRollupImage" disable-output-escaping="yes" /></a></xsl:if>
</div>

<div class="docMain">
<h2 class="resTitle">
<a href="{@FileRef}"><xsl:value-of select="@Title" /></a></h2> <span class="resSpace">|</span> <div class="resFolderName"> <xsl:value-of select="substring(@FileDirRef,24,24)" /></div>
</div>
<div class="docDesc"><xsl:value-of select="@_Comments" disable-output-escaping="yes" /></div>

<div class="picDwLinks">
<xsl:if test="not(normalize-space(@Flipbook_x0020_Url0) = '')"><a href="{@Flipbook_x0020_Url0}" class="viewdoc" target="_blank">View Online</a></xsl:if>
<xsl:if test="not(normalize-space(@Pdf_x0020_Url) = '')"><a href="{@Pdf_x0020_Url}" class="downloaddoc" target="_blank">
Download PDF</a></xsl:if></div>


</td>
<td valign="top">
  <div class="div-ms-vb resRat">
    <xsl:choose>
      <xsl:when test="$User!=''">
        <Portal:AverageRatingFieldControl  runat="Server" itemid="{@ID}"  id="MyRating{generate-id()}" FieldName="AverageRating" ControlMode="Edit" />
        
      </xsl:when>
      <xsl:otherwise>
        <xsl:if test="@AverageRating!=0 and @AverageRating!=''">
          <div style="width:80px; background-image:url('/_layouts/Images/Ratings.png'); height:16px;background-position:{@AverageRating*16 -240}px 0px;">
          </div>
        </xsl:if>
        <xsl:if test="@AverageRating='' or @AverageRating =0">
          <div style="width:80px; background-image:url('/_layouts/Images/Ratings.png'); height:16px;background-position:-80px 0px;" />
        </xsl:if>
        <xsl:if test="floor(@AverageRating)!=ceiling(@AverageRating) and @AverageRating!=0 and @AverageRating!=''">
          <div style="width:80px; background-image:url('/_layouts/Images/Ratings.png'); height:16px;background-position:{-floor(@AverageRating)*16 -304}px 0px;">
          </div>
        </xsl:if>
        
      </xsl:otherwise>
    </xsl:choose>
    <a onclick="GoToLink(this);return false;" href="/Pages/ReviewList.aspx?url={@FileRef}&amp;title='{@Title}">See Reviews</a>
  </div>
<br/><br/><NIEMLIKE:NIEMLike runat="server" URL="{@FileRef}" CType="Resource" />
</td></tr></table>			
					
			</td>
		</tr>
	</xsl:template>
						<xsl:template name="dvt_1.empty">
							<xsl:variable name="dvt_ViewEmptyText">There are no items to show in this view.</xsl:variable>
		<table border="0" width="100%">
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
		<xsl:text disable-output-escaping="yes" ddwrt:nbsp-preserve="yes" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime">&amp;nbsp;</xsl:text>
		
		<xsl:if test="$LastRowValue &lt; $dvt_RowCount or string-length($dvt_nextpagedata)!=0" ddwrt:cf_ignore="1">
				<a>
			<xsl:attribute name="href">javascript: <xsl:value-of select="ddwrt:GenFireServerEvent(concat('dvt_firstrow={',$NextRow,'};dvt_startposition={',$dvt_nextpagedata,'}'))" />;</xsl:attribute>
			<img src="/_layouts/images/next.gif" border="0" alt="Next" /></a>
			</xsl:if>
	</td>
</xsl:template>
									</xsl:stylesheet>