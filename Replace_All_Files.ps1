$currentDirectory = (Get-Location).path

$configFiles = Get-ChildItem . *.* -rec

foreach ($file in $configFiles)
{
    (Get-Content $file.PSPath) |
    Foreach-Object { $_ -replace "GEMP", "Template" } |
    Set-Content $file.PSPath
}

foreach ($file in $configFiles)
{
    (Get-Content $file.PSPath) |
    Foreach-Object { $_ -replace "Gemp", "Template" } |
    Set-Content $file.PSPath
}

Get-ChildItem -File -Recurse | % { 
    Rename-Item -Path $_.PSPath -NewName $_.Name.replace("Gemp","Template") -Force
}

foreach($Directory in Get-ChildItem -Path . -Recurse -Directory){
    
    if ((Test-Path $Directory)) {
      Write-Warning "$Directory absent from both locations"
    }

    #Rename-Item $Directory -NewName $($Directory.Name.replace("Gemp","Template"))
}