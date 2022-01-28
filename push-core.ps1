$version=$args[0]
$api_key = Get-Content -Path ./.api_key
$api_url = 'https://api.nuget.org/v3/index.json'

nuget push -ApiKey $api_key -Source $api_url ./SimpleGOAP.Core/bin/Release/SimpleGOAP.Core.$version.nupkg
