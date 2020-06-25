

Open solution:

    src\Solutions\MyCompany.MyExamples.ProjectParser.sln

Set startup project to:

    MyCompany.MyExamples.ProjectParser.ConsoleOne.csproj
	
Manually edit file Program.csproj

    string dotnetFrameworkCsProjFullFileName = @"C:\MySubFolder\MyCsProject.csproj";
	

For better results, you should go build your project (aka go build : "C:\MySubFolder\MyCsProject.csproj";)

....

Run 

MyCompany.MyExamples.ProjectParser.ConsoleOne.csproj

................

Optional.  You can tweak the "ignores"

Program.cs

		parseSettings.IgnorePrefixes = new List<string> { "System", "Microsoft", "mscorlib" };