<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal" xmlns:NIEMLIKE="NIEM_Like_Solution" xmlns:Portal="Microsoft.SharePoint.Portal.WebControls">
	<xsl:output method="html" indent="no"/>
	<xsl:decimal-format NaN=""/>
	<xsl:param name="dvt_apos">&apos;</xsl:param>
	<xsl:param name="ManualRefresh"></xsl:param>
	<xsl:param name="dvt_firstrow">1</xsl:param>
	<xsl:param name="dvt_nextpagedata" />
	<xsl:param name="ListID">{ED24F9EF-C02E-4FCF-A3A2-9657F66B4C21}</xsl:param>
	<xsl:param name="StartRowIndex">0</xsl:param>
	<xsl:param name="nextpagedata">0</xsl:param>
	<xsl:param name="MaximumRows">10</xsl:param>
	<xsl:param name="FilterMpdClass" />
	<xsl:param name="FilterIepdLife" />
	<xsl:param name="FilterArtifact" />
	<xsl:param name="User" />
	<xsl:param name="query" />
	<xsl:param name="option">0</xsl:param>
	<xsl:variable name="dvt_1_automode">0</xsl:variable>
	
	
	
	<xsl:template match="/" xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:NIEMLIKE="NIEM_Like_Solution">

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
		<xsl:variable name="Rows" select="/dsQueryResponse/Rows/Row[$query='' and ($option=0 or $option='')]"/>
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		<xsl:variable name="dvt_RowCount" select="count($Rows)"/>
		<xsl:variable name="RowLimit" select="5" />
		
		<xsl:variable name="FirstRow" select="$dvt_firstrow" />
		<xsl:variable name="LastRow" select="$FirstRow + $dvt_RowCount - 1" />
		<xsl:variable name="IsEmpty" select="$dvt_RowCount = 0 or $RowLimit = 0" />
		
		<xsl:variable name="dvt_IsEmpty" select="$dvt_RowCount = 0"/>
		<xsl:choose>
			<xsl:when test="$dvt_IsEmpty">
				<xsl:call-template name="dvt_1.empty"/>
			</xsl:when>
			<xsl:otherwise>
				<table border="0" width="100%" cellpadding="2" cellspacing="0">
					<xsl:call-template name="dvt_1.body">
						<xsl:with-param name="Rows" select="$Rows"/>
						<xsl:with-param name="FirstRow" select="1" />
						<xsl:with-param name="LastRow" select="$LastRow - $FirstRow + 1" />
					</xsl:call-template>
				</table>
			</xsl:otherwise>
		</xsl:choose>
			<xsl:if test="$query='' and $option=0">
		<xsl:call-template name="dvt_1.commandfooter">
			<xsl:with-param name="FirstRow" select="$FirstRow" />
			<xsl:with-param name="LastRow" select="$LastRow" />
			<xsl:with-param name="RowLimit" select="$RowLimit" />
			<xsl:with-param name="dvt_RowCount" select="$dvt_RowCount" />
			<xsl:with-param name="RealLastRow" select="number(ddwrt:NameChanged('',-100))" />
		</xsl:call-template>
		</xsl:if>
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
			<td class="ms-vb toolinfo">
			
			<xsl:if test="not(position() = 1)"><hr /></xsl:if>
	<div class="tolOuterWrp">
			<div class="tolWrpleft">
				<div class="toolname">
				<h3><a href="{@URL}"><xsl:value-of select="@Title"/></a></h3> 	<xsl:if test="not(normalize-space(@Latest_x0020_Verison) = '')"><div class="toolver">version <strong><xsl:value-of select="@Latest_x0020_Verison" /></strong></div></xsl:if>
				</div>
				
				<p class="tooldesc"><xsl:value-of select="@_Comments"/></p>
				<div class="toolmpd"><strong>Relevant MPD Classes: </strong><xsl:value-of select="@MPD_x0020_Classes"/></div>
				<div class="tooliepd"><strong>IEPD Lifecycle Phases: </strong><xsl:value-of select="@IEPD_x0020_Lifecycle_x0020_Phase"/></div>
				<div class="toolartifact"><strong>Artifacts Produced : </strong><xsl:value-of select="@Artifacts_x0020_Produced"/></div>
				<div class="tooladmin"><strong>Administrative Contact : </strong><a href="mailto:{@EMail}"><xsl:value-of select="@EMail"/></a> </div>
			</div>
			<div class="tolWrpRight">
        <div class="toolrate">
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
         
          <br/>
          <br/>
          <NIEMLIKE:NIEMLike runat="server" URL="{@FileRef}" CType="Tools" />
        </div>
			</div>
    </div>			
			
			</td>	

			</tr>
	</xsl:template>
	<xsl:template name="dvt_1.empty">
		<xsl:variable name="dvt_ViewEmptyText">There are no items to show in this view.</xsl:variable>
		<xsl:if test="$query='' and  $option=0">
		<table border="0" width="100%">
			<tr>
				<td class="ms-vb">
					<xsl:value-of select="$dvt_ViewEmptyText"/>
				</td>
			</tr>
		</table>
		</xsl:if>
	</xsl:template>
	<xsl:template name="dvt_1.commandfooter">
		<xsl:param name="FirstRow" />
		<xsl:param name="LastRow" />
		<xsl:param name="RowLimit" />
		<xsl:param name="dvt_RowCount" />
		<xsl:param name="RealLastRow" />
		<table cellspacing="0" cellpadding="4" border="0" width="100%">
			<tr>
				<xsl:if test="$FirstRow &gt; 1 or $dvt_nextpagedata">
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
				<img src="/_layouts/images/next.gif" border="0" alt="Next" />
				</a>
			</xsl:if>
		</td>
	</xsl:template></xsl:stylesheet>