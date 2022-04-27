$ErrorActionPreference = "Stop"

$ModName = 'Arena'
$OutputPath = 'output'
$PackagePath = 'package'
$ZipPath = "$(Get-Location)\$ModName.zip"

dotnet build ..\..\$ModName.sln /warnaserror --no-incremental --configuration Release --output $OutputPath
if (!$?) {
    exit 1
}

New-Item $PackagePath -ItemType Directory | Out-Null

Copy-Item "$OutputPath\$ModName.dll", 'icon.png', 'README.md', 'manifest.json' -Destination $PackagePath

Compress-Archive $PackagePath\* -DestinationPath $ZipPath -Force

Remove-Item $OutputPath, $PackagePath -Recurse

Write-Host "`nPackage is successfully created at: $ZipPath" -ForegroundColor Green
