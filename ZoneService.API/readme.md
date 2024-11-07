https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-8.0&tabs=visual-studio

New-LocalUser -Name AlphaControllerServiceUser

$acl = Get-Acl "ZoneService.API.exe"
$aclRuleArgs = ".\AlphaControllerServiceUser", "Read,Write,ReadAndExecute", "ContainerInherit,ObjectInherit", "None", "Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($aclRuleArgs)
$acl.SetAccessRule($accessRule)
$acl | Set-Acl "ZoneService.API.exe"

New-Service -Name ZoneService -BinaryPathName "ZoneService.API.exe --contentRoot ." -Credential ".\AlphaControllerServiceUser" -Description "Alpha Service" -DisplayName "Alpha Service" -StartupType Automatic