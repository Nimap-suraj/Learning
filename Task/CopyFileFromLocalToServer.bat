@echo off
:: === SET SERVER IP ===
set SERVER_IP=95.111.230.3
set "DRIVE_LETTER=X:"
:: === CHECKING LOCALHOST CONNECTION ===
ping 127.0.0.1 -n 2 >nul
if %errorlevel% neq 0 (
    echo ERROR: Localhost is not responding.
    pause
    exit /b 1
) else (
    echo Maintaining connection with LocalMachine is successful...
)

:: === CHECKING SERVER CONNECTION ===
ping %SERVER_IP% -n 2 >nul
if %errorlevel% neq 0 (
    echo ERROR: Cannot reach server %SERVER_IP%.
    pause
    exit /b 1
) else (
    echo Maintaining connection with Server is successful...
)

:: === SET FILE PATHS ===
set "SOURCE_FOLDER=C:\DemoFiles"
set "SHARE_NAME=BatFolder"

:: === CHECK LOCAL FOLDER EXISTS ===
if not exist "%SOURCE_FOLDER%" (
    echo Source folder does not exist: %SOURCE_FOLDER%
    pause
    exit /b 1
)

:: === CHECK FOR FILES IN LOCAL FOLDER ===
dir /b "%SOURCE_FOLDER%\*.*" >nul 2>&1
if errorlevel 1 (
    echo No files found in %SOURCE_FOLDER%.
    pause
    exit /b 1
)

 :: Forcefully disconnect X: if already mapped
net use %DRIVE_LETTER% /delete >nul 2>&1
:: === MOUNT SHARED FOLDER WITH USERNAME AND PASSWORD ===
echo Mounting shared folder...
net use x: \\%SERVER_IP%\%SHARE_NAME% /user:administrator RahulJain@1234
if %errorlevel% neq 0 (
    echo ERROR: Could not connect to shared folder.
    pause
    exit /b 1
)
:: === COPYING ALL FILES INCLUDING SUBFOLDERS ===
echo Copying all files including subfolders...
xcopy "%SOURCE_FOLDER%\*.mdb" x:\ /S /E /Y /I
if %errorlevel% equ 0 (
    echo Files copied successfully to \\%SERVER_IP%\%SHARE_NAME%
) else (
    echo ERROR: File copy failed.
)

:: === UNMOUNT SHARED FOLDER ===
net use x: /delete

pause
