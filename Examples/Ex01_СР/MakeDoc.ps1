
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

Write-Host ">> Check markdown style and syntax"
markdownlint -c .\markdownlint.cfg.json .\研_GPN.Curator.md
Write-Host "  "

Write-Host ">> Spell checking"
mdspell --report --ignore-numbers -d ..\ru_RU .\研_GPN.Curator.md
Write-Host "  "

Write-Host ">> Build 研_GPN.Curator_Actual.md"
.\LivingDoc.MDMerge.exe -s "研_Struct.md.xml" "研_GPN.Curator_Actual.md" "研_GPN.Curator.md" "研_GPN.Curator.WspDetails.md" "研_Default.md"
Write-Host "  "

Write-Host ">> Build HTML"
markdown .\研_GPN.Curator_Actual.md | ConvertTo-Encoding "utf-8" "cp866" | Out-File -Encoding UTF8 .\研_GPN.Curator_Actual.html

Write-Host ">> Add inline styles in HTML"
(Get-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html).replace('<table>', '<table style="border-collapse: collapse;">') | Set-Content -Encoding UTF8  .\研_GPN.Curator_Actual.html
(Get-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html).replace('<th>', '<th style="background: #006cb1; color:#ffffff; text-align: center; border: 1px solid black; padding: 4px;">') | Set-Content -Encoding UTF8  .\研_GPN.Curator_Actual.html
(Get-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html).replace('<td>', '<td style="border: 1px solid black; padding: 4px;">') | Set-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html
(Get-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html).replace('<h1 ', '<h1 style="color:#006cb1" ') | Set-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html
(Get-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html).replace('<h2 ', '<h2 style="color:#006cb1" ') | Set-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html
(Get-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html).replace('<h3 ', '<h3 style="color:#006cb1" ') | Set-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html
(Get-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html).replace('<h4 ', '<h4 style="color:#006cb1" ') | Set-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html
(Get-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html).replace('<h5 ', '<h5 style="color:#006cb1" ') | Set-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html
(Get-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html).replace('<h6 ', '<h6 style="color:#006cb1" ') | Set-Content -Encoding UTF8 .\研_GPN.Curator_Actual.html

