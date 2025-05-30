@echo off
echo "Copying specified file from Local to Server"

set SERVER_IP=95.111.230.3
set "SHARE_NAME=BatFolder"
set "DRIVE_LETTER=X:"
:: === CHECK LOCALHOST CONNECTION ===
ping 127.0.0.1 -n 2 >nul
if %errorlevel% neq 0 (
    echo ERROR: Localhost is not responding.
    pause
    exit /b 1
) else (
    echo Maintaining connection with LocalMachine is successful...
)

:: === CHECK SERVER CONNECTION ===
ping %SERVER_IP% -n 2 >nul
if %errorlevel% neq 0 (
    echo ERROR: Cannot reach server %SERVER_IP%.
    pause
    exit /b 1
) else (
    echo Maintaining connection with Server is successful...
)

:: === GET FILE PATH FROM USER ===
set /p FILE_PATH=Enter full path of file to copy (e.g., C:\DemoFiles\c# Notes.txt): 

:: === CHECK IF FILE EXISTS ===
if not exist "%FILE_PATH%" (
    echo ERROR: File does not exist: %FILE_PATH%
    pause
    exit /b 1
)

:: === EXTRACT FILE NAME ONLY ===
for %%F in ("%FILE_PATH%") do set "FILE_NAME=%%~nxF"

    :: Forcefully disconnect X: if already mapped
net use %DRIVE_LETTER% /delete >nul 2>&1

:: === MOUNT SHARED FOLDER ===
echo Mounting shared folder...
net use x: \\%SERVER_IP%\%SHARE_NAME% /user:administrator RahulJain@1234
if %errorlevel% neq 0 (
    echo ERROR: Could not connect to shared folder.
    pause
    exit /b 1
)

:: === COPY THE SPECIFIC FILE ===
echo Copying "%FILE_NAME%" to server...
copy "%FILE_PATH%" "x:\%FILE_NAME%" /Y
if %errorlevel% equ 0 (
    echo File "%FILE_NAME%" copied successfully to \\%SERVER_IP%\%SHARE_NAME%
) else (
    echo ERROR: File copy failed.
)

:: === UNMOUNT SHARED FOLDER ===
net use x: /delete

pause
