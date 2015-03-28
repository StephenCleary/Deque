pushd
cd artifacts\bin\Nito.Collections.Deque\Debug\net45
$env:KRE_APPBASE = "../../../../../src/UnitTests"
iex ((Get-ChildItem ($env:USERPROFILE + '\.k\packages\OpenCover'))[0].FullName + '\OpenCover.Console.exe' + ' -register:user -target:"k.cmd" -targetargs:"test" -output:coverage.xml -skipautoprops -returntargetcode -filter:"+[Nito*]*"')
#iex ((Get-ChildItem ($env:USERPROFILE + '\.k\packages\coveralls.io'))[0].FullName + '\tools\coveralls.net.exe' + ' --opencover coverage.xml --full-sources')
iex ((Get-ChildItem ($env:USERPROFILE + '\.k\packages\ReportGenerator'))[0].FullName + '\ReportGenerator.exe -reports:coverage.xml -targetdir:.')
./index.htm
popd