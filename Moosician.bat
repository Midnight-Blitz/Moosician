@ECHO off
@TITLE Moosician
ECHO MOOSICIAN START MENU
ECHO --------------------
ECHO 1. Start Moosician with restart enabled.
ECHO 2. Start Moo without restart commands.
ECHO 3. Kill batch file.
ECHO Find some way to fix this.
ECHO.

CHOICE /C 123 /M "Enter your choice:"

If ERRORLEVEL 3 GOTO terminate
IF ERRORLEVEL 2 GOTO start
IF ERRORLEVEL 1 GOTO restart

:restart
CD /D "%~dp0src\NadekoBot"
dotnet run -c Release
ECHO Moosician has successfully restarted.
goto restart

:start
CD /D "%~dp0src\NadekoBot"
dotnet run -c Release
GOTO terminate

:terminate
TITLE Moosician is now shutting down. Press the any key to continue...
CD /D "%~dp0"
PAUSE >nul 2>&1