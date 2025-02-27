@echo off
REM Get the directory where this batch file is located
set SCRIPT_DIR=%~dp0
REM Add it to PATH for this session
set "PATH=%SCRIPT_DIR%;%PATH%"
REM Run the Python script with all arguments
python "%SCRIPT_DIR%dotnet run" %*