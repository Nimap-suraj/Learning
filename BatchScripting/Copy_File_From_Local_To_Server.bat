@echo off
setlocal

:: === CONFIGURATION ===
set SERVER_IP=95.111.230.3
set SERVER_USERNAME=administrator
set SERVER_PASSWORD=RahulJain@1234

set LOCAL_FOLDER=C:\DemoFiles
set DEST_FOLDER=\\%SERVER_IP%\BatFolder

:: === CHECKING LOCALHOST CONNECTION ===
ping 127.0.0.1 -n 2 >nul
if errorlevel 1 (
    echo ERROR: Localhost is not responding.
    pause
    exit /b 1
) else (
    echo Maintaining connection with LocalMachine is successful...
)

:: === CHECKING SERVER CONNECTION ===
ping %SERVER_IP% -n 2 >nul
if errorlevel 1 (
    echo ERROR: Cannot reach server %SERVER_IP%.
    pause
    exit /b 1
) else (
    echo Maintaining connection with Server is successful...
)

:: === CHECK LOCAL FOLDER EXISTS ===
if not exist "%LOCAL_FOLDER%" (
    echo Source folder does not exist: %LOCAL_FOLDER%
    pause
    exit /b 1
)

:: === CHECK FOR FILES IN LOCAL FOLDER ===
dir /b "%LOCAL_FOLDER%\*.*" >nul 2>&1
if errorlevel 1 (
    echo No files found in %LOCAL_FOLDER%.
    pause
    exit /b 1
)
echo Source Folder: %LOCAL_FOLDER%
echo Destination Folder: %DEST_FOLDER%

:: === CONNECTING TO SERVER ===
net use x: \\%SERVER_IP%\C$ >nul 2>&1
if errorlevel 1 (
    echo ERROR: Cannot map network drive to the server.
    pause
    exit /b 1
)

:: === COPY FILES ===
xcopy "%LOCAL_FOLDER%" "%DEST_FOLDER%" /e /v /h
if errorlevel 1 (
    echo File copy failed.
    pause
    exit /b 1
) else (
    echo SUCCESS: Files copied to %DEST_FOLDER%.
)

:: === CLEANUP: REMOVE NETWORK DRIVE ===
net use x: /delete /yes >nul 2>&1

pause
exit /b 0
