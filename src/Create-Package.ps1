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

Compress-Archive $PackagePath\* -DestinationPath $DestinationPath\$ProjectName.zip -Force

Remove-Item $PackagePath\$ProjectName.dll
