@echo off
:start
Title user input yes or no
set/p user_input=DO you Like to Continue(y/n)?: 
if not defined user_input goto start
if /i%user_input% == y goto yes
if /i%user_input% == n (goto no) else goto invalid
pause


:yes
echo User entered yes
pause
goto start

:no
echo User entered no
pause
exit

:invalid
echo User entered Invalid entry try  again
set user_input=
pause
go to start