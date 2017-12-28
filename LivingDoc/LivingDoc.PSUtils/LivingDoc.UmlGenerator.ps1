param([Parameter(Position=0, Mandatory=$true,  HelpMessage="File or folder path *.md files")][string]$UmlTxtPath,
	  [Parameter(Position=1, Mandatory=$false, HelpMessage="Files filter")][string]$Filter = "*.uml.txt"
)

function GetUmlTxtFiles ([string] $UmlTxtPath, [string] $Filter)
{
	$umlTxtFiles = $null

	#Test path to uml txt
	if ([string]::IsNullOrEmpty($UmlTxtPath)) 
	{
		throw "'UmlTxtPath' is null or empty."
	}

	if (-not (Test-Path $UmlTxtPath)) 
	{
		throw "$UmlTxtPath not found."
	}

	if ((Get-Item $UmlTxtPath) -is [System.IO.DirectoryInfo])
	{
		#Is Folder
		$umlTxtFiles = (Get-ChildItem $UmlTxtPath -Filter $Filter -Recurse) | Select -ExpandProperty FullName
	}
	else
	{
		#Is file
		$umlTxtFiles = @(Get-Item $UmlTxtPath)
	}

	return $umlTxtFiles
}

if ($UmlTxtPath -eq "TEST")
{
	Write-Verbose "Unit test mode."
	return
}

$umlTxtFiles = GetUmlTxtFiles $UmlTxtPath $Filter

$nUmlTxtFiles = $umlTxtFiles.Length
Write-Host "Found $nUmlTxtFiles files:"
$umlTxtFiles | % { Write-Host " -" (Split-Path $_ -leaf)}
Write-Host "   "

$location = Split-Path -parent $MyInvocation.MyCommand.Definition

foreach ($umlTxtFileName in $umlTxtFiles)
{
	#Spell check
	Write-Host "GENERATE UML:" (Split-Path $umlTxtFileName -leaf)

	$umlGenerateCmd = "java -jar '$location\plantuml.jar' -Verbose $umlTxtFileName"

	Write-Verbose ">> $umlGenerateCmd"

	Invoke-Expression $umlGenerateCmd

	Write-Host "   "

}
