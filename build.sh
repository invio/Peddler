#!/usr/bin/env bash

#exit if any command fails
set -e

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

dotnet restore

# Ideally we would use the 'dotnet test' command to test netcoreapp and net462 so restrict for now
# but this currently doesn't work due to https://github.com/dotnet/cli/issues/3073 so restrict to netcoreapp

dotnet test ./test/Peddler.Tests -c Release -f netcoreapp1.0

#  Note from Caruso (9/21/2016):
#
#  WE CANNOT TEST .NET Framework 4.6.2 right now for Mono. It is not yet supported.
#  Here is the bug tracking it: https://bugzilla.xamarin.com/show_bug.cgi?id=42327
#  We'll have to rely on Appveyor + Windows doing this instead.
#
#   dotnet build ./test/Peddler.Tests -c Release -f net462

#   mono \
#   ./test/Peddler.Tests/bin/Release/net462/*/dotnet-test-xunit.exe \
#   ./test/Peddler.Tests/bin/Release/net462/*/Peddler.Tests.dll

revision=${TRAVIS_JOB_ID:=1}
revision=$(printf "%04d" $revision)

dotnet pack ./src/Peddler -c Release -o ./artifacts
