@echo off
echo "Pulling file from server to specific path of local machine"

set SERVER_IP=95.111.230.3
set "SHARE_NAME=BatFolder"
set "DRIVE_LETTER=X:"
:: === CHECK LOCALHOST CONNECTION ===
ping 127.0.0.1 -n 2 >nul
if %errorlevel% neq 0 (
    echo ERROR: Localhost is not responding...
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
:loop
cls
echo Q - Quit
echo P - Pull File From Server
echo.

set /p choice=Enter your choice (Q/P): 

if /i "%choice%"=="q" (
    echo Exiting the script.
    pause
    goto :eof
)

if /i "%choice%"=="p" (
    goto PullFile
)

echo Invalid choice. Please enter Q or P.
pause
goto loop
:PullFile
cls
:: === GET FILE NAME TO COPY FROM SERVER ===
set /p FILE_NAME=Enter the file name on the server (e.g., c# Notes.txt): 

:: === GET LOCAL DESTINATION PATH ===
set /p LOCAL_DEST_PATH=Enter full local destination path (e.g., C:\Users\Suraj\Desktop\Copy.txt): 

:: === EXTRACT FOLDER PATH FROM LOCAL_DEST_PATH ===
for %%F in ("%LOCAL_DEST_PATH%") do set "DEST_FOLDER=%%~dpF"

:: Forcefully disconnect X: if already mapped
net use %DRIVE_LETTER% /delete >nul 2>&1

:: === CHECK IF DESTINATION FOLDER EXISTS ===
if not exist "%DEST_FOLDER%" (
    echo ERROR: Destination folder does not exist: %DEST_FOLDER%
    pause
    goto loop
)

:: === CONNECT TO SHARED SERVER FOLDER ===
net use x: \\%SERVER_IP%\%SHARE_NAME% /user:administrator RahulJain@1234
if %errorlevel% neq 0 (
    echo ERROR: Failed to connect to shared folder.
    pause
   goto loop
)

:: === CHECK IF FILE EXISTS ON SERVER ===
if not exist "x:\%FILE_NAME%" (
    echo ERROR: File does not exist on server: %FILE_NAME%
    net use x: /delete
    pause
    goto loop
)

:: === COPY FROM SERVER TO LOCAL DESTINATION PATH ===
echo Copying file from server...
copy "x:\%FILE_NAME%" "%LOCAL_DEST_PATH%" /Y
if %errorlevel% equ 0 (
    echo File successfully copied to: %LOCAL_DEST_PATH%
) else (
    goto loop
)

:: === DISCONNECT SERVER DRIVE ===
net use x: /delete

:: Ask to continue or quit
set /p input=Do you want to continue? Type q to quit, any other key to continue: 

if /i "%input%"=="q" (
    echo Exiting the script.
    pause
    goto :eof
) else (
    goto loop
)