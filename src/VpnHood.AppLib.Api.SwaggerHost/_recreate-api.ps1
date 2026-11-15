$curDir = $PSScriptRoot;

# variables
$projectFile="$curDir/VpnHood.AppLib.Api.SwaggerHost.csproj";
$namespace = "VpnHood.Client.Api";
$nswagFile = "$curDir/Api/Api.nswag";
$outBaseFile = "VpnHood.Client.Api";
$noBuild = $false;

# calculated
$outputFile = "$curDir/Api/$outBaseFile.ts";

# run; nswag is a local dotnet tool, so it must run from the manifest root (./.config)
$variables="/variables:namespace=$namespace,apiBaseFile=$outBaseFile,projectFile=$projectFile,nobuid=$noBuild";
Push-Location $curDir;
try {
	dotnet tool restore;
	if ($LASTEXITCODE -ne 0) { throw "dotnet tool restore failed."; }
	# NSwag's own build reports no failure: it reads the host's last build and says Done, so a client
	# made after a pin that broke the host is the old one. Build here, and stop on a failure.
	dotnet build $projectFile;
	if ($LASTEXITCODE -ne 0) { throw "The Swagger host does not build."; }
	dotnet tool run nswag run $nswagFile $variables;
	if ($LASTEXITCODE -ne 0) { throw "nswag generation failed."; }
}
finally {
	Pop-Location;
}

# NSwag copies the C# doc comments as they are, so a */ in one, as in the automatic location's */*, ends
# its comment early and the client does not parse: within a comment's line, such a */ is written *\/.
$client = [System.IO.File]::ReadAllText($outputFile);
$client = [regex]::Replace($client, '(?m)^(?<open>[ \t]*/\*\*)(?<body>.*)(?<close>\*/\r?)$', {
	param($m)
	$m.Groups['open'].Value + $m.Groups['body'].Value.Replace('*/', '*\/') + $m.Groups['close'].Value
});
[System.IO.File]::WriteAllText($outputFile, $client, [System.Text.UTF8Encoding]::new($false));

# the SPA beside this host is the one consumer of the client
$uiProjectTarget = Join-Path $PSScriptRoot "..\VpnHood.AppUi.Presentation.Classic.Spa\src\services\VpnHood.Client.Api.ts";
if (Test-Path $uiProjectTarget) {
	Copy-Item $outputFile $uiProjectTarget -Force;
	Write-Host "Output has been copied to the SPA. $uiProjectTarget";
}
else {
	Write-Host "Could not update the SPA. $uiProjectTarget" -ForegroundColor Yellow;
}
