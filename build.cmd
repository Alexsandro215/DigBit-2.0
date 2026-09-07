@echo off
rem Lanzador que evita el bloqueo de execution policy de PowerShell.
rem El Bypass aplica solo a este proceso; no cambia la configuracion del sistema.
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0build.ps1" %*
