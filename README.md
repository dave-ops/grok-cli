# GrokCS

## create
```
dotnet new console -n GrokCS
dotnet add package Grpc.Net.Clienth
dotnet add package Google.Protobuf
dotnet add package Grpc.Tools
dotnet add package System.Text.Json
dotnet add package Newtonsoft.Json
dotnet build
dotnet run
```

## release
```
dotnet run --configuration Release
```

## publish
1|-c Release|Builds in Release configuration for better performance.
2|-r win-x64|Targets Windows 64-bit (adjust to your OS, e.g., win-x86, linux-x64, or osx-x64).
3|--self-contained true|Includes the .NET runtime in the output, making it a standalone executable.
4|-o publish|Outputs the files to a publish folder in your project directory.
```
dotnet publish -c Release -r win-x64 --self-contained true -o publish
ren C:\dev\repos\GrokCS\publish\GrokCS.exe grok.exe
```


## run
```
dotnet run -- arg1 arg2
```


