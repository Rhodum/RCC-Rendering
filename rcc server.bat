@echo off
:loop
Start "" /b RCCService.exe -Console -Start -PlaceId:1818
timeout /T 120 /nobreak >nul
taskkill /IM RCCService.exe /F
cls
goto loop

