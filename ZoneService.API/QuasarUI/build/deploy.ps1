cls
Set-PSDebug -Trace 0

# this block can be used to initial env for testing
"Debug: ${$env:CI_PROJECT_DIR}"
if( [string]::IsNullOrEmpty($env:CI_PROJECT_DIR)) {
  Write-Host "Initialize Debug Enviroment"
  $env:CI_PROJECT_DIR = "C:\Alpha"
  $env:BUILD_TO = "build"
  $env:DEPLOY_ENV = "Dev"
  $env:DEPLOY_DEST = "c:\\Alpha\\${env:DEPLOY_ENV}\\WebUi"
  $env:DEPLOY_NAME = "AlphaService"
}

$source = Join-path $env:CI_PROJECT_DIR $env:BUILD_TO
$dest = $env:DEPLOY_DEST

$environment = $env:DEPLOY_ENV
$name = $env:DEPLOY_NAME

$commandLine = "${dest}\Alpha.exe --environment `"$environment""" 
$displayName = "${name}_${environment}"
 
write-host "Source : ${source}..."
write-host "Dest   : ${dest}..."
write-host "Env    : ${environment}..."
write-host "Name   : ${name}..."
write-host "Command: ${commandLine}..."
write-host "Display: ${displayName}..."

if( !(Test-Path $dest)) {
    New-Item $dest -itemtype directory -force
}

Copy-Item $source\* $dest -Recurse -Force

