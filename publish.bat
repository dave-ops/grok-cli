rmdir publish /s
dotnet publish -c Release -r win-x64 --self-contained true -o publish
cd publish
copy appsettings.json publish\appsettings.json
rename GrokCLI.exe grok.exe
