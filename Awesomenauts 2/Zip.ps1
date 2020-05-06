
param (
    [Parameter(Mandatory=$true)][string]$in,
    [Parameter(Mandatory=$true)][string]$out
 )

Add-Type -Assembly System.IO.Compression.FileSystem
$compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
[System.IO.Compression.ZipFile]::CreateFromDirectory($in, $out)
