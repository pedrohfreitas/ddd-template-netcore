 param (
    [string]$NameProject 
 )

$configFiles = Get-ChildItem . *.* -rec

foreach ($file in $configFiles)
{
    if ((Test-Path $_.PSPath)) {
        (Get-Content $file.PSPath) | Foreach-Object { 
            $_ -replace "Template", $NameProject 
        } |   Set-Content $file.PSPath
    }
}

Get-ChildItem -File -Recurse | % { 
    if ((Test-Path $_.PSPath)) {
        Rename-Item -Path $_.PSPath -NewName $_.Name.replace("Template",$NameProject) -Force
    }
}

foreach($Directory in Get-ChildItem -Path . -Recurse -Directory){
    
    if ((Test-Path $Directory)) {
      Write-Warning "$Directory absent from both locations"
      Rename-Item $Directory -NewName $($Directory.Name.replace("Template",$NameProject))
    }
}