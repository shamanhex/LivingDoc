# include test cript
. ..\LivingDoc.PSUtils\LivingDoc.MDLint.ps1 -Path "TEST"

Describe "LivingDoc.MDLint tests" {

	Context "CheckAndAutocorrectDicPath(): unit tests" {
		
		It "If DirPath is null or empty, ret string without changes" {
			CheckAndAutocorrectDicPath $null | Should -BeNullOrEmpty
			CheckAndAutocorrectDicPath "" | Should -Be ""
		}		

		It "If DirPath ends with .dic or .aff, ret string without extension" {
			New-Item "TestDrive:\ru_RU.dic" -ItemType File
			New-Item "TestDrive:\ru_RU.aff" -ItemType File

			CheckAndAutocorrectDicPath "TestDrive:\ru_RU.dic" | Should -Be "TestDrive:\ru_RU"
			CheckAndAutocorrectDicPath "TestDrive:\ru_RU.aff" | Should -Be "TestDrive:\ru_RU"
		}

		It "If .dic file not exist, throw Exception" {
			{ CheckAndAutocorrectDicPath "TestDrive:\de_DE.aff" } | Should -Throw "TestDrive:\de_DE.dic not found."
		}
	}
}