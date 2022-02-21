if [ -z "$1" ]; then
    echo "Error: no version number supplied"
    exit 1
fi

version=$1
package_path="./SimpleGOAP.Core/bin/Release/SimpleGOAP.Core.$version.nupkg"
if [ ! -f $package_path ]; then
    echo "Error: no .nupkg file for that version exists"
    exit 1
fi

if [ ! -f .api_key ]; then
    echo "Error: no .api_key file exists"
    exit 1
fi

api_key=`cat .api_key`
api_url='https://api.nuget.org/v3/index.json'

nuget push -ApiKey $api_key -Source $api_url $package_path
