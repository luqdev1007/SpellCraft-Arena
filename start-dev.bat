@echo off
setlocal enabledelayedexpansion

REM =====================================================================
REM  SpellCraft Arena — старт рабочего окружения одним файлом.
REM  Положи этот .bat в КОРЕНЬ проекта (рядом с папками Assets и ProjectSettings).
REM =====================================================================

REM Флаги для Claude Code. Для боевого проекта по твоим же правилам можно
REM поменять на:  --permission-mode acceptEdits
set "CLAUDE_ARGS=--dangerously-skip-permissions"

REM Корень проекта = папка, где лежит сам .bat
set "PROJECT_DIR=%~dp0"
set "PROJECT_DIR=%PROJECT_DIR:~0,-1%"
echo Project dir: %PROJECT_DIR%
echo.

REM --- 1) Unity (если открываешь Unity сам — удали этот блок целиком) ---
set "VERSION_FILE=%PROJECT_DIR%\ProjectSettings\ProjectVersion.txt"
if exist "%VERSION_FILE%" (
    for /f "tokens=2" %%v in ('findstr /b "m_EditorVersion:" "%VERSION_FILE%"') do set "UNITY_VERSION=%%v"
    set "UNITY_EXE=C:\Program Files\Unity\Hub\Editor\!UNITY_VERSION!\Editor\Unity.exe"
    if exist "!UNITY_EXE!" (
        echo Launching Unity !UNITY_VERSION! ...
        start "" "!UNITY_EXE!" -projectPath "%PROJECT_DIR%"
    ) else (
        echo [SKIP] Unity.exe not found: !UNITY_EXE!
    )
) else (
    echo [SKIP] ProjectVersion.txt not found.
)

REM --- 2) Visual Studio (.sln из корня проекта) ------------------------
set "SLN="
for %%f in ("%PROJECT_DIR%\*.sln") do set "SLN=%%f"
if defined SLN (
    echo Opening solution: !SLN!
    start "" "!SLN!"
) else (
    echo [SKIP] No .sln in project root yet.
)

REM --- 3) GitHub Desktop -----------------------------------------------
set "GHD=%LOCALAPPDATA%\GitHubDesktop\GitHubDesktop.exe"
if exist "%GHD%" (
    echo Launching GitHub Desktop ...
    start "" "%GHD%"
) else (
    echo [SKIP] GitHub Desktop not found: %GHD%
)

REM --- 4) Claude Code в терминале, в папке проекта ---------------------
echo Launching Claude Code ...
start "Claude Code" /D "%PROJECT_DIR%" cmd /k claude %CLAUDE_ARGS%

endlocal