$webAppUrl = "http://sp2010app:2222"
$outFile = ".\WebFeatures.txt"

$allWebFeatures = Get-SPFeature | Where-Object { $_.Scope -eq "Web" }
$results = @()

$site = Get-SPSite $webAppUrl

foreach($web in $site.AllWebs)
{

    $activeFeatures = Get-SPFeature -Web $web.Url

    $allWebFeatures | ForEach-Object {

        $active = $false
        foreach($feature in $activeFeatures)
        {
          if($feature.Id.ToString() -eq $_.Id.ToString())
          {
             $active = $true
          }
        }


        $obj = New-Object PSObject;
        $obj | Add-Member NoteProperty  WebTitle  $web.Title;
        $obj | Add-Member NoteProperty  WebUrl    $web.Url;

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
    $web.Dispose();
}
$site.Dispose();
 
write-Output "Site Collection Url: "  $webAppUrl | Out-String -Width 4096 | Out-File $outFile 
$results | Sort -property @{Expression="WebTitle";Descending=$false}, @{Expression="Title";Descending=$false} | FT -Property * -AutoSize | Out-String -Width 4096 | Out-File $outFile -append

