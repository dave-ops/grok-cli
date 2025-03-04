REM sonar-scanner -Dsonar.projectKey=grok-cli -Dsonar.sources=. -Dsonar.host.url=http://localhost:9000 -Dsonar.token=sqp_0515278ce7a65d14e704f39a75ded3592b1e34eb
REM Get the directory where this batch file is located
set SCRIPT_DIR=%~dp0
set "PATH=%SCRIPT_DIR%\publish;%PATH%"
echo %SCRIPT_DIR%\publish added to path.
