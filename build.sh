#!/usr/bin/env bash

#exit if any command fails
set -e

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

dotnet restore

dotnet test ./test/Peddler.Tests/Peddler.Tests.csproj -c Release

dotnet pack -c Release -o ../../artifacts
