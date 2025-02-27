@echo off
REM Get the directory where this batch file is located
set SCRIPT_DIR=%~dp0
REM Add it to PATH for this session
set "PATH=%SCRIPT_DIR%;%PATH%"
echo %SCRIPT_DIR%