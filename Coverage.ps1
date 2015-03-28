pushd
cd artifacts\bin\Nito.Collections.Deque\Debug\net45

# Execute OpenCover with a target of "k test"
$original_KRE_APPBASE = $env:KRE_APPBASE
$env:KRE_APPBASE = "../../../../../test/UnitTests"
iex ((Get-ChildItem ($env:USERPROFILE + '\.k\packages\OpenCover'))[0].FullName + '\OpenCover.Console.exe' + ' -register:user -target:"k.cmd" -targetargs:"test" -output:coverage.xml -skipautoprops -returntargetcode -filter:"+[Nito*]*"')
$env:KRE_APPBASE = $original_KRE_APPBASE

# Either display or publish the results
If ($env:CI -eq 'True')
{
  iex ((Get-ChildItem ($env:USERPROFILE + '\.k\packages\coveralls.io'))[0].FullName + '\tools\coveralls.net.exe' + ' --opencover coverage.xml --full-sources')
}
Else
{
  iex ((Get-ChildItem ($env:USERPROFILE + '\.k\packages\ReportGenerator'))[0].FullName + '\ReportGenerator.exe -reports:coverage.xml -targetdir:.')
  ./index.htm
}

popd