param([Parameter(Position=0, Mandatory=$true, HelpMessage="Path to .md file")][string]$Path)

function GetMarkdownFiles ([string] $Path, [string] $Filter)
{	
	if (-not (Test-Path $Path)) 
	{
		throw "$Path not found."
	}

	if ((Get-Item $Path) -is [System.IO.DirectoryInfo])
	{
		#Is Folder
		return (Get-ChildItem $Path -Filter $Filter -Recurse) | Select -ExpandProperty FullName
	}
	else
	{
		#Is file
		return @((Get-Item $Path).FullName)	
	}
}

function ConvertTo-Encoding ([string]$From, [string]$To){
	Begin{
		$encFrom = [System.Text.Encoding]::GetEncoding($from)
		$encTo = [System.Text.Encoding]::GetEncoding($to)
	}
	Process{
		$bytes = $encTo.GetBytes($_)
		$bytes = [System.Text.Encoding]::Convert($encFrom, $encTo, $bytes)
		$encTo.GetString($bytes)
	}
}

$markdownFiles = GetMarkdownFiles $Path

$nMdFiles = $markdownFiles.Length
Write-Host "Found $nMdFiles files:"
$markdownFiles | % { Write-Host " -" (Split-Path $_ -leaf)}
Write-Host "   "

Write-Host "BUILD HTML:"

foreach ($mdFileName in $markdownFiles)
{
	$extension = [System.IO.Path]::GetExtension($mdFileName)
	$htmlFileName = $mdFileName.Substring(0, $mdFileName.Length - $extension.Length) + ".html";

	Write-Host $(Split-Path $mdFileName -Leaf) ">> Generate html file:" $(Split-Path $htmlFileName -Leaf)
	
	markdown $mdFileName | ConvertTo-Encoding "utf-8" "cp866" | Out-File -Encoding UTF8 $htmlFileName

	Write-Host $(Split-Path $mdFileName -Leaf) ">> Add HTML styles"

	(Get-Content -Encoding UTF8 $htmlFileName).
		replace('<table>', '<table style="border-collapse: collapse;">').
		replace('<th>', '<th style="background: #006cb1; color:#ffffff; text-align: center; border: 1px solid black; padding: 4px;">').
		replace('<td>', '<td style="border: 1px solid black; padding: 4px;">').
		replace('<h1 ', '<h1 style="color:#006cb1" ').
		replace('<h2 ', '<h2 style="color:#006cb1" ').
		replace('<h3 ', '<h3 style="color:#006cb1" ').
		replace('<h4 ', '<h4 style="color:#006cb1" ').
		replace('<h5 ', '<h5 style="color:#006cb1" ').
		replace('<h6 ', '<h6 style="color:#006cb1" ') | Set-Content -Encoding UTF8 $htmlFileName
}