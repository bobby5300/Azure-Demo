pushd %~dp0
pushd ..\..
del /q /f *.nupkg

.\.nuget\nuget.exe pack .\tools\nupkg\Microsoft.Azure.Storage.DataMovement.nuspec
popd

popd