copy 'src\Peddler\project.json' 'src\Peddler\project.json.bak'
$project = Get-Content 'src\Peddler\project.json.bak' -raw | ConvertFrom-Json
$project.buildOptions.debugType = "full"
$project | ConvertTo-Json  | set-content 'src\Peddler\project.json'