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
rmdir publish /s
rmdir Tools /s
dotnet publish -c Release -r win-x64 --self-contained true -o publish
ren C:\_dev\repos\GrokCS\publish\GrokCS.exe grok.exe
copy C:\_dev\repos\GrokCS\publish\grok.exe C:\Tools
```
## run
```
dotnet run -- arg1 arg2
```

## folder structure
```
GrokCS/
│
├── src/                                      # Source code root
│   ├── Commands/                             # Command handlers
│   │   ├── UploadCommand.cs                  # Logic for 'upload' command
│   │   ├── GrokCommand.cs                    # Logic for 'grok' command
│   │   └── RateLimitCommand.cs              # Logic for 'ratelimit' command
│   │
│   ├── Models/                               # Data models or DTOs
│   │   ├── GrokRequest.cs                    # Request model for Grok API calls
│   │   ├── UploadRequest.cs                  # Request model for upload API calls
│   │   └── RateLimitResponse.cs             # Response model for rate limit
│   │
│   ├── Services/                             # Service classes for API interactions
│   │   ├── GrokService.cs                    # Handles Grok API calls (e.g., Grok3 logic)
│   │   ├── UploadService.cs                  # Handles file upload logic (e.g., Upload class)
│   │   └── RateLimitService.cs              # Handles rate limit API calls (e.g., GetRateLimit)
│   │
│   ├── Utils/                                # Utility classes
│   │   ├── FileHelper.cs                     # File handling utilities
│   │   └── ParseGrokResponse.cs             # Parsing utilities for Grok responses
│   │
│   ├── Protos/                               # Protocol Buffer definitions
│   │   └── grok.proto                        # gRPC or Protobuf definitions (if used with gRPC)
│   │
│   ├── Program.cs                            # Entry point (top-level statements or Main)
│   │
│   └── PreviewImage.cs                       # Image preview logic (if separate from other services)
│
├── tests/                                    # Unit tests (optional, if you add testing)
│   ├── Commands/                             # Test for commands
│   ├── Services/                             # Test for services
│   └── Utils/                                # Test for utilities
│
├── .gitignore                                # Git ignore file
├── GrokCS.csproj                             # Project file
├── README.md                                 # Project documentation
├── package.json                              # Optional, for non-.NET dependencies (if any)
├── start.bat                                 # Batch file for launching the application
└── dist/                                     # Distribution/output directory (optional for artifacts)
```



