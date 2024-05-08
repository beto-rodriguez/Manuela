param([string]$configuration = "Release", [string]$nupkgOutputPath = "./nupkg")

$msbuild = &"${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe

if (Test-Path $nupkgOutputPath) {
        Get-ChildItem $nupkgOutputPath -Include *.* -File -Recurse | ForEach-Object { $_.Delete() }
}
else {
        New-Item $nupkgOutputPath -ItemType "directory"
}

$projects = @("./src/Manuela/Manuela.csproj", "./src/Manuela.Generation/Manuela.Generation.csproj")

foreach ($project in $projects) {
    
        & $msbuild $project -t:pack /p:configuration=$configuration /restore
        
        $folder = $project.SubString(0, $project.LastIndexOf("/"))

        $found = Get-ChildItem $folder -Filter "*.nupkg" -Recurse -Force
        Copy-Item $found.FullName $nupkgOutputPath

        $found = Get-ChildItem $folder -Filter "*.snupkg" -Recurse -Force
        # because the source generator do not generrrates .snupkg
        if (![string]::IsNullOrEmpty($found))
        {                
                Copy-Item $found.FullName $nupkgOutputPath
        }
}