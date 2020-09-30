try {
	$targetFile = $args[0]

	$doc = [xml](Get-content $targetFile)

	$exists = $doc.PreferenceSettings.CustomPackageFolders.InnerText -like "*C:\ProgramData\BHoM\Assemblies*"

	if (-Not $exists)
	{
		$e = $doc.CreateNode("element", "string", "")
		$e.InnerText = "C:\ProgramData\BHoM\Assemblies"
		
		$doc.PreferenceSettings.CustomPackageFolders.AppendChild($e)
		$doc.save($targetFile)
	}
}
catch {
	exit 1
}