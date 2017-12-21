
Get-ChildItem -Filter img\*.uml.txt | %{java -jar .\plantuml.jar -verbose $_.FullName}