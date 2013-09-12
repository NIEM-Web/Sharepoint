[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SharePoint") 
$web = Get-SPWeb http://niem-3.cld.sr:8001/documentsdb 
$web.AnonymousState = [Microsoft.SharePoint.SPWeb+WebAnonymousState]::Enabled 
$web.AnonymousPermMask64 = "Open, ViewPages" 
$web.Update() 
