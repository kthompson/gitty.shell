@echo off
%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\regasm -u bin\Debug\Gitty.Shell.dll
%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\regasm -u bin\Release\Gitty.Shell.dll
echo Please restart all explorer processes
pause