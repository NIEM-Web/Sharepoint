$webAppUrl = "http://sp2010app:2222"
$outFile = ".\AuthenticationProviders.txt"


write-Output "Web Application Url: "  $webAppUrl | Out-File $outFile 

write-Output "" | Out-File $outFile   -append
write-Output "Zone: Default" | Out-File $outFile   -append
Get-SPAuthenticationProvider -WebApplication $webAppUrl -Zone Default | Out-String -Width 4096 | Out-File $outFile  -append

write-Output "" | Out-File $outFile   -append
write-Output "Zone: Intranet" | Out-File $outFile   -append
Get-SPAuthenticationProvider -WebApplication $webAppUrl -Zone Intranet | Out-String -Width 4096 | Out-File $outFile  -append

write-Output "" | Out-File $outFile   -append
write-Output "Zone: Internet" | Out-File $outFile   -append
Get-SPAuthenticationProvider -WebApplication $webAppUrl -Zone Internet | Out-String -Width 4096 | Out-File $outFile  -append

write-Output "" | Out-File $outFile   -append
write-Output "Zone: Extranet" | Out-File $outFile   -append
Get-SPAuthenticationProvider -WebApplication $webAppUrl -Zone Extranet | Out-String -Width 4096 | Out-File $outFile  -append

write-Output "" | Out-File $outFile   -append
write-Output "Zone: Custom" | Out-File $outFile   -append
Get-SPAuthenticationProvider -WebApplication $webAppUrl -Zone Custom | Out-String -Width 4096 | Out-File $outFile  -append
