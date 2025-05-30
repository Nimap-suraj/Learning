@echo off
setlocal enabledelayedexpansion

set SERVER_IP=95.111.230.3
set "SHARE_NAME=BatFolder"
set "USERNAME=administrator"
set "PASSWORD=RahulJain@1234"
set "DRIVE_LETTER=X:"

:: === CHECK LOCALHOST CONNECTION ===
ping 127.0.0.1 -n 2 >nul
if !errorlevel! neq 0 (
    echo ERROR: Localhost is not responding...
    pause
    exit /b 1
) else (
    echo Maintaining connection with LocalMachine is successful...
)

:: === CHECK SERVER CONNECTION ===
ping %SERVER_IP% -n 2 >nul
if !errorlevel! neq 0 (
    echo ERROR: Cannot reach server %SERVER_IP%.
    pause
    exit /b 1
) else (
    echo Maintaining connection with Server is successful...
)

:PROMPT_ACTION
echo Do you want to PUSH (local to server) or PULL (server to local)?
set /p ACTION=Type PUSH or PULL: 

:: Trim spaces and convert to uppercase
set "ACTION=%ACTION: =%"
if "%ACTION%"=="" (
    echo Please enter PUSH or PULL.
    goto :PROMPT_ACTION
)

:: Case-insensitive comparison
if /I "%ACTION%"=="PUSH" goto :PUSH
if /I "%ACTION%"=="PULL" goto :PULL

echo Invalid input. Please type PUSH or PULL.
goto :PROMPT_ACTION

:PUSH
    :: === PUSH LOGIC ===
    set /p FILE_PATH=Enter full path of file to copy (e.g., C:\DemoFiles\c# Notes.txt): 

    if not exist "%FILE_PATH%" (
        echo ERROR: File does not exist: %FILE_PATH%
        pause
        exit /b 1
    )

    for %%F in ("%FILE_PATH%") do set "FILE_NAME=%%~nxF"

    :: Forcefully disconnect X: if already mapped
    net use %DRIVE_LETTER% /delete >nul 2>&1

    echo Mounting shared folder...
    net use %DRIVE_LETTER% \\%SERVER_IP%\%SHARE_NAME% /user:%USERNAME% %PASSWORD% /persistent:no
    if !errorlevel! neq 0 (
        echo ERROR: Could not connect to shared folder.
        pause
        exit /b 1
    )

    echo Copying "%FILE_NAME%" to server...
    copy "%FILE_PATH%" "%DRIVE_LETTER%\%FILE_NAME%" /Y
    if !errorlevel! equ 0 (
        echo File "%FILE_NAME%" copied successfully to \\%SERVER_IP%\%SHARE_NAME%
    ) else (
        echo ERROR: File copy failed.
    )

    net use %DRIVE_LETTER% /delete >nul 2>&1
    pause
    exit /b 0

:PULL
    :: === PULL LOGIC ===
    set /p FILE_NAME=Enter the file name on the server (e.g., c# Notes.txt): 
    set /p LOCAL_DEST_PATH=Enter full local destination path (e.g., C:\Users\Suraj\Desktop\Copy.txt): 

    for %%F in ("%LOCAL_DEST_PATH%") do set "DEST_FOLDER=%%~dpF"

    if not exist "%DEST_FOLDER%" (
        echo ERROR: Destination folder does not exist: %DEST_FOLDER%
        pause
        exit /b 1
    )

    :: Forcefully disconnect X: if already mapped
    net use %DRIVE_LETTER% /delete >nul 2>&1

    echo Mounting shared folder...
	
  net use %DRIVE_LETTER% \\%SERVER_IP%\%SHARE_NAME% /user:%USERNAME% %PASSWORD% /persistent:no
if !errorlevel! neq 0 (
    echo ERROR: Failed to connect to shared folder.
    pause
    exit /b 1
)

if not exist "%DRIVE_LETTER%\%FILE_NAME%" (
    echo ERROR: File does not exist on server: %FILE_NAME%
    net use %DRIVE_LETTER% /delete >nul 2>&1
    pause
    exit /b 1
)

echo Copying file from server...
copy "%DRIVE_LETTER%\%FILE_NAME%" "%LOCAL_DEST_PATH%" /Y
if !errorlevel! equ 0 (
    echo File successfully copied to: %LOCAL_DEST_PATH%
) else (
    echo ERROR: File copy failed.
)

    net use %DRIVE_LETTER% /delete >nul 2>&1
    pause
    exit /b 0