$login = ""
	$passwd = ""
	$addr = ""

	$secPasswd = ConvertTo-SecureString $passwd -AsPlainText -Force
	$cred = New-Object System.Management.Automation.PSCredential($login, $secPasswd)

	$session = New-SSHSession -ComputerName $addr -Credential $cred -Verbose

	$from = ".\StorageViewService\bin\Release\PublishOutput"
	$to = "/home/gendalf/services/storage-view"

    Invoke-SSHCommand -SSHSession $session -Command "rm -r $($to)" -Verbose
	Set-SCPFolder -LocalFolder $from -RemoteFolder $to -ComputerName $addr -Credential $cred -Verbose
	Invoke-SSHCommand -SSHSession $session -Command "echo $($passwd) | sudo -S systemctl restart storage-view-service" -Verbose
	Remove-SSHSession -SSHSession $session -Verbose