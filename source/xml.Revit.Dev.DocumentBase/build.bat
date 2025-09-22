@echo off
setlocal enabledelayedexpansion

echo Building xml.Revit.Debuger
set project=".\xml.Revit.Dev.DocumentBase.csproj"

for %%Y in (24 25) do (
    echo Building for Release R%%Y
    dotnet build -c "Release R%%Y" %project%
)

echo Copying built files to the Addins directory
set dest_root="C:\ProgramData\Autodesk\Revit\Addins\"

:: Copy the files only if the source directory exists
for %%Y in (24 25) do (
    set "year=20%%Y"
    set "src_dir=C:\xml.Revit.Dev.DocumentBase\source\xml.Revit.Dev.DocumentBase\bin\!year! Release R%%Y"

    if exist "!src_dir!" (
        xcopy "!src_dir!\*" "!dest_root!\!year!\" /s /e /y /i
    ) else (
        echo Source directory "!src_dir!" does not exist. Skipping...
    )
)

echo Compilation and copying completed.
