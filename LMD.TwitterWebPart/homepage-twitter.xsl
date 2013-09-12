<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:ms="urn:schemas-microsoft-com:xslt" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:WebControls="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
  <xsl:output omit-xml-declaration="yes" method="html" version="4.0" indent="yes"/>

  <xsl:template match="*">
    
    <ul class="NIEM-Twitter">
      <xsl:apply-templates select="Tweet"/>
    </ul>
  </xsl:template>
  <xsl:template match="Tweet">
    <xsl:variable name="count" select="7"/>

    <xsl:if test="position() &lt; $count">
      <xsl:variable name="hour">
        <xsl:value-of select="substring(created_at, 17,3)"/>
      </xsl:variable>
      <xsl:variable name="minute">
        <xsl:value-of select="substring(created_at, 20,3)"/>
      </xsl:variable>
      <xsl:variable name="second">
        <xsl:value-of select="substring(created_at, 23,3)"/>
      </xsl:variable>
      <xsl:variable name="timeString" select="concat($hour , $minute, $second, ' -0500')"/>

      <li>
        <xsl:copy-of select="text"/>
        <a>
          <xsl:attribute name="href">http://twitter.com/<xsl:value-of select="link"/>
          </xsl:attribute>
          <span id="twdate">
            <xsl:text></xsl:text>
            <xsl:value-of select="substring(created_at, 0, string-length(created_at) - 14)"/>
            <xsl:text> </xsl:text>
            <xsl:value-of select="$timeString"/>
          </span>
        </a>
      </li>
      Hi!
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>