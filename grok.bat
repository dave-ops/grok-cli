@echo off
setlocal EnableDelayedExpansion

echo Paste your multi-line text (press Ctrl+Z then Enter when done):
echo (To paste, right-click the console window or use Ctrl+V, then signal end with Ctrl+Z + Enter)

:: Create a temporary file to store the input
set "tempFile=%TEMP%\multiLineInput.txt"
del /Q /F "%tempFile%" 2>nul

:: Redirect input to the temporary file
copy con "%tempFile%" >nul

:: Check if the file was created and contains data
if not exist "%tempFile%" (
    echo No input provided. Exiting...
    goto :end
)

echo Processing multi-line input:
for /f "tokens=*" %%l in (%tempFile%) do (
    echo Line: %%l
    :: Add your logic here to process each line (e.g., save to variables, execute commands, etc.)
)

:end
:: Clean up the temporary file
del /Q /F "%tempFile%" 2>nul
endlocal
pause