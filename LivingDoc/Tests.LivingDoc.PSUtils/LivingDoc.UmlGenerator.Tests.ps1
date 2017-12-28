# include test cript
. ..\LivingDoc.PSUtils\LivingDoc.UmlGenerator.ps1 -UmlTxtPath "TEST"

Describe "LivingDoc.UmlGenerator tests" {

	Context "function GetUmlTxtFiles(): unit tests" {

		It "If path not exists, return Exception"	{
				{ GetUmlTxtFiles "TestDrive:\NonExistentFile.txt" } | Should -Throw "TestDrive:\NonExistentFile.txt not found."
		}

		It "If path is null or empty, return Exception"	{
				{ GetUmlTxtFiles $null } | Should -Throw "'UmlTxtPath' is null or empty."
				{ GetUmlTxtFiles "" } | Should -Throw "'UmlTxtPath' is null or empty."
		}

		It "If path specify file, return array with 1 element" {
				New-Item "TestDrive:\SomeFile.txt" -type file
				GetUmlTxtFiles "TestDrive:\SomeFile.txt" | Split-Path -Leaf | Should -Be @("SomeFile.txt")
		}
		
		It "If path specify directory, return array with all files by filter" {
				New-Item "TestDrive:\MdDir" -type Directory
				New-Item "TestDrive:\MdDir\Spec_01.md" -type file
				New-Item "TestDrive:\MdDir\Spec_02.md" -type file
				New-Item "TestDrive:\MdDir\General.md" -type file

				GetUmlTxtFiles "TestDrive:\MdDir" "Spec_*.md" | Split-Path -Leaf | Should -Be @("Spec_01.md", "Spec_02.md")
		}
	}

	Context "Acceptance tests" {

		It "Ex01: Trivial test for plantuml.jar" {
			Copy-Item ".\UmlGeneratorTestFiles\Ex01_TrivialTest" "TestDrive:\Ex01_TrivialTest\" -Recurse
			
			..\LivingDoc.PSUtils\LivingDoc.UmlGenerator.ps1 -UmlTxtPath "TestDrive:\Ex01_TrivialTest\" 

			"TestDrive:\Ex01_TrivialTest\Trivial.uml.png" | Should -Exist
		}

		It "Ex02: Generate class diagram" {
			New-Item "TestDrive:\Ex02_ClassDiagram" -type Directory
			Copy-Item ".\UmlGeneratorTestFiles\Ex02_ClassDiagram\ClassDiagram.uml.txt" "TestDrive:\Ex02_ClassDiagram\ClassDiagram.uml.txt" -Recurse
			
			..\LivingDoc.PSUtils\LivingDoc.UmlGenerator.ps1 -UmlTxtPath "TestDrive:\Ex02_ClassDiagram\ClassDiagram.uml.txt" 

			"TestDrive:\Ex02_ClassDiagram\ClassDiagram.uml.png" | Should -Exist
		}

		It "Ex03: Generate component diagram" {
			New-Item "TestDrive:\Ex03_ComponentDiagram" -type Directory
			Copy-Item ".\UmlGeneratorTestFiles\Ex03_ComponentDiagram\ComponentDiagram.uml.txt" "TestDrive:\Ex03_ComponentDiagram\ComponentDiagram.uml.txt" -Recurse
			
			..\LivingDoc.PSUtils\LivingDoc.UmlGenerator.ps1 -UmlTxtPath "TestDrive:\Ex03_ComponentDiagram\ComponentDiagram.uml.txt" 

			"TestDrive:\Ex03_ComponentDiagram\ComponentDiagram.uml.png" | Should -Exist
		}
	}

}