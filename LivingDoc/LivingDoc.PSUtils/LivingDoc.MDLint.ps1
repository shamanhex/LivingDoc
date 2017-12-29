param([Parameter(Position=0, Mandatory=$true, HelpMessage="File or folder path *.md files")][string]$Path,
      [Parameter(Position=1, Mandatory=$false, HelpMessage="Files filter")][string]$Filter = "*.md",
      [Parameter(Position=2, Mandatory=$false, HelpMessage="Dic file name (without extension)")][string]$DicPath,
      [Parameter(Position=3, Mandatory=$false, HelpMessage="JSON configuration file for markdownlint-cli")][string]$MDLintConfigFile	  
)

function CheckAndAutocorrectDicPath([string] $DicPath)
{
    if (-not [string]::IsNullOrEmpty($DicPath))
    {
        if ($DicPath.EndsWith(".dic") -or $DicPath.EndsWith(".aff"))
        {
            $DicPath = $DicPath.Substring(0, $DicPath.Length - 4)
        }

        if (-not (Test-Path ($DicPath+".dic")))
        {
            throw "$DicPath.dic not found."
        }
    }

    return $DicPath
}

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

#Check for test mode (only definition)
if ($Path -eq "TEST")
{
    Write-Verbose "TEST MODE"
    return
}

$DicPath = CheckAndAutocorrectDicPath $DicPath

if (-not [string]::IsNullOrEmpty($MDLintConfigFile) -and -not (Test-Path $MDLintConfigFile))
{
    throw [System.IO.FileNotFoundException] "$MDLintConfigFile not found."
}

#Check Path
$markdownFiles = GetMarkdownFiles $Path

$nMdFiles = @($markdownFiles).Length
Write-Host "Found $nMdFiles files:"
$markdownFiles | % { Write-Host " -" (Split-Path $_ -leaf)}
Write-Host "   "

foreach ($mdFileName in $markdownFiles)
{
    #Spell check
    Write-Host "SPELL CHECK:" (Split-Path $mdFileName -leaf)

    $spellCheckCmd = "mdspell --report --ignore-numbers"

    if (-not [string]::IsNullOrEmpty($DicPath))
    {
        $spellCheckCmd += " -d '$DicPath'"
    }

    $spellCheckCmd += " '$mdFileName'"

    Write-Verbose ">> $spellCheckCmd"

    Invoke-Expression $spellCheckCmd

    Write-Host "   "

    #Linting check
    Write-Host "MARKDOWN LINTING:" (Split-Path $mdFileName -leaf)	

    $lintingCmd = "markdownlint"
    
    if (-not [string]::IsNullOrEmpty($MDLintConfigFile))
    {
        $lintingCmd += " -c '$MDLintConfigFile'"
    }

    $lintingCmd += " '$mdFileName'"

    Write-Verbose ">> $lintingCmd"

    Invoke-Expression $lintingCmd

    Write-Host "   "
}
