param([string]$configuration = "Release", [string]$nupkgOutputPath = "./nupkg")

$msbuild = &"${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe

$project = "./src/Manuela/Manuela.csproj";

& $msbuild $project -t:pack /p:configuration=$configuration /restore
        
$folder = $project.SubString(0, $project.LastIndexOf("/"))

if (Test-Path $nupkgOutputPath) {
        Get-ChildItem $nupkgOutputPath -Include *.* -File -Recurse | ForEach-Object { $_.Delete() }
}
else {
        New-Item $nupkgOutputPath -ItemType "directory"
}    

$found = Get-ChildItem $folder -Filter "*.nupkg" -Recurse -Force
Copy-Item $found.FullName $nupkgOutputPath

$found = Get-ChildItem $folder -Filter "*.snupkg" -Recurse -Force
Copy-Item $found.FullName $nupkgOutputPath
