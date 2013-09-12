$webAppUrl = "http://sp2010app:2222"
$outFile = ".\SiteCollectionFeatures.txt"

$allSiteFeatures = Get-SPFeature | Where-Object { $_.Scope -eq "Site" }
$activeFeatures = Get-SPFeature -Site $webAppUrl
$results = @()

$allSiteFeatures | ForEach-Object {

    $active = $false
    foreach($feature in $activeFeatures)
    {
      if($feature.Id.ToString() -eq $_.Id.ToString())
      {
         $active = $true
      }
    }

    $obj = New-Object PSObject;


    if(!$active)
    {

      $obj | Add-Member NoteProperty  Title  $_.DisplayName;
      $obj | Add-Member NoteProperty  Hidden $_.Hidden;
      $obj | Add-Member NoteProperty  Status "Not Activated";
      $obj | Add-Member NoteProperty  Id     $_.Id;

    }
    else
    {

      $obj | Add-Member NoteProperty  Title  $_.DisplayName;
      $obj | Add-Member NoteProperty  Hidden $_.Hidden;
      $obj | Add-Member NoteProperty  Status "Activated";
      $obj | Add-Member NoteProperty  Id     $_.Id;

    }

    $results += $obj;


}
 
write-Output "Site Collection Url: "  $webAppUrl | Out-String -Width 4096 | Out-File $outFile 
$results | Sort Title | FT -Property * -AutoSize | Out-String -Width 4096 | Out-File $outFile -append
