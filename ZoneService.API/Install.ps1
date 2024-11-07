$usr = "alpha2"
$password = "edgewise"
$exe = "ZoneService.API.exe"

$secpasswd = ConvertTo-SecureString $password -AsPlainText -Force
$cred = New-Object System.Management.Automation.PSCredential (".\${usr}", $secpasswd)

New-LocalUser -Name $usr

$acl = Get-Acl $exe
$aclRuleArgs = ".\${usr}", "Read,Write,ReadAndExecute", "ContainerInherit,ObjectInherit", "None", "Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($aclRuleArgs)
$acl | Set-Acl $exe

New-Service -Name KVlControl -BinaryPathName "${exe} --contentRoot ." -Credential $cred -Description "Alpha Service" -DisplayName "Alpha Service" -StartupType Automatic
$acl.SetAccessRule($accessRule)
 

sc.exe create NewService binpath= c:\windows\system32\NewServ.exe type= share start= auto depend= +TDI NetBIOS
sc.exe remove KVLControl
