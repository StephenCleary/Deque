cd src
Get-ChildItem | foreach {
  cd $_
  dotnet pack -c Release
  cd ..
}
cd ..