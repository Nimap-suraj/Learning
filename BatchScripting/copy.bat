@echo off
setlocal enabledelayedexpansion

:: ===== CONFIGURATION =====
set SERVER_IP=95.111.230.3
set SERVER_SHARE=\\%SERVER_IP%\BatFolder
set LOCAL_FOLDER=C:\DemoFiles
set LOG_FILE=C:\CopyLog_%date:~-4,4%%date:~-7,2%%date:~-10,2%.txt
set SERVER_USERNAME=administrator
set SERVER_PASSWORD=RahulJain@1234

:: ===== RUN AS ADMIN CHECK =====
NET FILE >nul 2>&1 || (
    echo ERROR: This script must be run as Administrator.
    echo Right-click and select "Run as Administrator".
    pause
    exit /b 1
)

:: ===== LOGGING INIT =====
echo [%date% %time%] NetCopy Operation Started > "%LOG_FILE%"
echo. >> "%LOG_FILE%"

:: ===== CHECK SOURCE FOLDER =====
if not exist "%LOCAL_FOLDER%\" (
    echo ERROR: Source folder missing: %LOCAL_FOLDER% >> "%LOG_FILE%"
    echo ERROR: Source folder missing: %LOCAL_FOLDER%
    pause
    exit /b 1
)

dir /b "%LOCAL_FOLDER%\*" >nul 2>&1 || (
    echo ERROR: No files found in %LOCAL_FOLDER% >> "%LOG_FILE%"
    echo ERROR: No files found in %LOCAL_FOLDER%
    pause
    exit /b 1
)

:: ===== NETWORK COPY WITH CREDENTIALS =====
:TRY_NETWORK_COPY
echo [%time%] Attempting network copy to %SERVER_IP%... >> "%LOG_FILE%"
ping %SERVER_IP% -n 2 >nul && (
    echo [%time%] Server is reachable. Mapping drive with credentials... >> "%LOG_FILE%"
    
    :: Map network drive (temporary)
    net use Z: "%SERVER_SHARE%" /user:%SERVER_USERNAME% %SERVER_PASSWORD% /persistent:no >nul 2>&1
    if errorlevel 1 (
        echo ERROR: Failed to authenticate with server. Check username/password. >> "%LOG_FILE%"
        goto LOCAL_COPY
    )
    
    if not exist "Z:\" (
        echo ERROR: Could not access server share after authentication. >> "%LOG_FILE%"
        goto LOCAL_COPY
    )
    
    robocopy "%LOCAL_FOLDER%" "Z:\" /e /zb /r:1 /w:1 /np /log+:"%LOG_FILE%"
    set "COPY_RESULT=%errorlevel%"
    
    :: Unmap drive
    net use Z: /delete >nul 2>&1
    
    if %COPY_RESULT% leq 7 (
        echo SUCCESS: Files copied to %SERVER_SHARE% >> "%LOG_FILE%"
        echo SUCCESS: Files copied to server!
        pause
        exit /b 0
    ) else (
        echo WARNING: Network copy failed (Robocopy error: %COPY_RESULT%). Trying local... >> "%LOG_FILE%"
        goto LOCAL_COPY
    )
) || (
    echo [%time%] Server unavailable. Trying local copy... >> "%LOG_FILE%"
    goto LOCAL_COPY
)

:: ===== LOCAL FALLBACK COPY =====
:LOCAL_COPY
set LOCAL_BACKUP=C:\Users\Public\Documents\LocalFileSave\BatFolder
echo [%time%] Attempting local copy to %LOCAL_BACKUP%... >> "%LOG_FILE%"

if not exist "%LOCAL_BACKUP%\" (
    mkdir "%LOCAL_BACKUP%" >nul 2>&1 || (
        echo ERROR: Could not create local backup folder. Check permissions. >> "%LOG_FILE%"
        echo ERROR: Could not create local backup folder.
        pause
        exit /b 1
    )
)

robocopy "%LOCAL_FOLDER%" "%LOCAL_BACKUP%" /e /zb /r:1 /w:1 /np /log+:"%LOG_FILE%"
if %errorlevel% leq 7 (
    echo SUCCESS: Files copied to local backup (%LOCAL_BACKUP%) >> "%LOG_FILE%"
    echo SUCCESS: Files copied to local backup!
) else (
    echo ERROR: All copy attempts failed. See %LOG_FILE% for details. >> "%LOG_FILE%"
    echo ERROR: Copy failed completely. Check log: %LOG_FILE%
)

pause
exit /b %errorlevel%