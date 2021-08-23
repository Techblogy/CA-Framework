@echo on
SET A=%~dp0
cd A
SET ASPNETCORE_URLS=https://localhost:5001
SET ASPNETCORE_ENVIRONMENT=Development
dotnet "bin\Debug\netcoreapp3.1\CAF.UI.WebApi.dll"
TIMEOUT 3