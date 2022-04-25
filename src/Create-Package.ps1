Param(
    [string]
    $DestinationPath
)

$ProjectName = "Arena"
$PackagePath = "$ProjectName\Package"
$OutputPath = "output"

dotnet build /warnaserror --no-incremental --configuration Release --output $OutputPath

if (!$?) {
    exit 1
}

Copy-Item $OutputPath\$ProjectName.dll -Destination $PackagePath

Remove-Item $OutputPath -Recurse

if (!$DestinationPath) {
    $DestinationPath = Get-Location
}

$ZipPath = "$DestinationPath\$ProjectName.zip"

Compress-Archive $PackagePath\* -DestinationPath $ZipPath -Force

Remove-Item $PackagePath\$ProjectName.dll

Write-Host "`nPackage is successfully created at: $ZipPath" -ForegroundColor Green
