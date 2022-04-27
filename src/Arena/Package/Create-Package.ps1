$ErrorActionPreference = "Stop"

$ModName = 'Arena'
$OutputPath = 'output'
$ZipPath = "$(Get-Location)\$ModName.zip"

dotnet build ..\..\$ModName.sln /warnaserror --no-incremental --configuration Release --output $OutputPath
if (!$?) {
    exit 1
}

Compress-Archive "$OutputPath\$ModName.dll", 'icon.png', 'README.md', 'manifest.json' -DestinationPath $ZipPath -Force

Remove-Item $OutputPath -Recurse

Write-Host "`nPackage is successfully created at: $ZipPath" -ForegroundColor Green
