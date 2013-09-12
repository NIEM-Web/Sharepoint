#These powershell commands were pulled directly from the Microsoft site 
# http://technet.microsoft.com/en-us/library/ff645391%28v=office.14%29.aspx

$webAppUrl = "http://sp2010app:2222"

#Retrieve Web Application information. The default depth of 2 does not return much detail--we recommend that you use a depth of 4 for this cmdlet.
$webApp = Get-SPWebApplication $webAppUrl
$webApp | Export-Clixml .\WebAppFilename.xml -depth 4



#Retrieve custom layout information. 
$webApp  | Get-SPCustomLayoutsPage | Export-Clixml .\Get-SPCustomLayoutsPage.xml

#Determine how SharePoint designer access is configured.
$webApp  | Get-SPDesignerSettings  | Export-Clixml .\Get-SPDesignerSettings.xml

#Retrieve information about alternate access mapping
Get-SPAlternateURL –WebApplication $webAppUrl | Export-Clixml .\Get-SPAlternateURL.xml

#Retrieve information about all SharePoint Products installed in the farm, and the versions of all updates installed for each product.
Get-SPProduct  | Export-Clixml .\Get-SPProduct.xml


#Retrieve information about deployed solutions
Get-SPSolution  | Export-Clixml .\Get-SPSolution.xml

#Retrieve information about sandboxed solutions deployed in a site collection
Get-SPSite $webAppUrl | Get-SPUserSolution  | Export-Clixml .\Get-SPUserSolution.xml

#Retrieve information about the sites in the farm
Get-SPSite $webAppUrl | Export-Clixml .\Get-SPSite.xml



$webApp.MimeMappings | Export-Clixml .\WebAppFilename.xml -depth 4
$webApp.Policies | Export-CSV .\webapp_Policies.csv
$webApp.PolicyRoles | Export-CSV .\webapp_PolicyRoles.csv
$webApp.DocumentConverters | Export-CSV .\webapp_DocumentConverters.csv
$webApp.prefixes | Export-CSV .\webapp_prefixes.csv
$webApp.HttpThrottleSettings | Export-CSV .\webapp_HttpThrottleSettings.csv





#Search 
#Retrieve search information
#Note:  A Search service application must be provisioned for the following cmdlets to succeed. 
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchAdministrationComponent  | Export-Clixml .\Get-SPEnterpriseSearchAdministrationComponent.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchCrawlContentSource  | Export-Clixml .\Get-SPEnterpriseSearchCrawlContentSource.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchCrawlCustomConnector  | Export-Clixml .\Get-SPEnterpriseSearchCrawlCustomConnector.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchCrawlDatabase  | Export-Clixml .\Get-SPEnterpriseSearchCrawlDatabase.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchCrawlExtension  | Export-Clixml .\Get-SPEnterpriseSearchCrawlExtension.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchCrawlMapping  | Export-Clixml .\Get-SPEnterpriseSearchCrawlMapping.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchCrawlRule  | Export-Clixml .\Get-SPEnterpriseSearchCrawlRule.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchCrawlTopology  | Export-Clixml .\Get-SPEnterpriseSearchCrawlTopology.xml
$searchApp = Get-SPEnterpriseSearchServiceApplication; Get-SPEnterpriseSearchExtendedClickThroughExtractorJobDefinition -SearchApplication $searchApp | Export-Clixml .\Get-SPEnterpriseSearchExtendedClickThroughExtractorJobDefinition.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchExtendedConnectorProperty  | Export-Clixml .\Get-SPEnterpriseSearchExtendedConnectorProperty.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchExtendedQueryProperty  | Export-Clixml .\Get-SPEnterpriseSearchExtendedQueryProperty.xml
###WARNING: The following cmdlet generates a 120MB file that records the out of the box settings###
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchLanguageResourcePhrase  | Export-Clixml .\Get-SPEnterpriseSearchLanguageResourcePhrase.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchMetadataCategory  | Export-Clixml .\Get-SPEnterpriseSearchMetadataCategory.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchMetadataCrawledProperty  | Export-Clixml .\Get-SPEnterpriseSearchMetadataCrawledProperty.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchMetadataManagedProperty  | Export-Clixml .\Get-SPEnterpriseSearchMetadataManagedProperty.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchMetadataMapping  | Export-Clixml .\Get-SPEnterpriseSearchMetadataMapping.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchPropertyDatabase  | Export-Clixml .\Get-SPEnterpriseSearchPropertyDatabase.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchQueryAuthority  | Export-Clixml .\Get-SPEnterpriseSearchQueryAuthority.xml
Get-SPEnterpriseSearchServiceApplication | Get-SPEnterpriseSearchQueryDemoted  | Export-Clixml .\Get-SPEnterpriseSearchQueryDemoted.xml
 Get-SPEnterpriseSearchQueryAndSiteSettingsService  | Export-Clixml .\Get-SPEnterpriseSearchQueryAndSiteSettingsService.xml
Get-SPEnterpriseSearchQueryAndSiteSettingsServiceInstance  | Export-Clixml .\Get-SPEnterpriseSearchQueryAndSiteSettingsServiceInstance.xml
Get-SPEnterpriseSearchQueryAndSiteSettingsServiceProxy  | Export-Clixml .\Get-SPEnterpriseSearchQueryAndSiteSettingsServiceProxy.xml
Get-SPEnterpriseSearchService  | Export-Clixml .\Get-SPEnterpriseSearchService.xml
Get-SPEnterpriseSearchServiceInstance  | Export-Clixml .\Get-SPEnterpriseSearchServiceInstance.xml
Get-SPSearchService  | Export-Clixml .\Get-SPSearchService.xml
Get-SPSearchServiceInstance  | Export-Clixml .\Get-SPSearchServiceInstance.xml 