# Скрипт для перемещения файлов из папки src в соответствующие проекты

Write-Host "Начинаем перемещение файлов из папки src..."

# 1. PilotLookUp.Plugin - App.cs и Commands
Write-Host "Перемещаем файлы в PilotLookUp.Plugin..."
if (Test-Path "src\App.cs") {
    Copy-Item "src\App.cs" "PilotLookUp.Plugin\App.cs" -Force
    Write-Host "  - App.cs скопирован"
}

if (Test-Path "src\Commands") {
    if (!(Test-Path "PilotLookUp.Plugin\Commands")) {
        New-Item -ItemType Directory -Path "PilotLookUp.Plugin\Commands" -Force
    }
    Copy-Item "src\Commands\*" "PilotLookUp.Plugin\Commands\" -Recurse -Force
    Write-Host "  - Commands скопированы"
}

# 2. PilotLookUp.Core - Contracts, Core, Extensions, Interfaces, Utils
Write-Host "Перемещаем файлы в PilotLookUp.Core..."
$coreFolders = @("Contracts", "Core", "Extensions", "Interfaces", "Utils")
foreach ($folder in $coreFolders) {
    if (Test-Path "src\$folder") {
        Copy-Item "src\$folder" "PilotLookUp.Core\" -Recurse -Force
        Write-Host "  - $folder скопирован"
    }
}

# 3. PilotLookUp.Infrastructure - Model/Services и Directors
Write-Host "Перемещаем файлы в PilotLookUp.Infrastructure..."
if (Test-Path "src\Model\Services") {
    if (!(Test-Path "PilotLookUp.Infrastructure\Model")) {
        New-Item -ItemType Directory -Path "PilotLookUp.Infrastructure\Model" -Force
    }
    Copy-Item "src\Model\Services" "PilotLookUp.Infrastructure\Model\" -Recurse -Force
    Write-Host "  - Model/Services скопированы"
}

if (Test-Path "src\Directors") {
    Copy-Item "src\Directors" "PilotLookUp.Infrastructure\" -Recurse -Force
    Write-Host "  - Directors скопированы"
}

# 4. PilotLookUp.UI - View, ViewModel, Resources
Write-Host "Перемещаем файлы в PilotLookUp.UI..."
if (Test-Path "src\View") {
    Copy-Item "src\View" "PilotLookUp.UI\" -Recurse -Force
    Write-Host "  - View скопированы"
}

if (Test-Path "src\ViewModel") {
    Copy-Item "src\ViewModel" "PilotLookUp.UI\" -Recurse -Force
    Write-Host "  - ViewModel скопированы"
}

if (Test-Path "src\Resources") {
    Copy-Item "src\Resources" "PilotLookUp.UI\" -Recurse -Force
    Write-Host "  - Resources скопированы"
}

Write-Host "Перемещение файлов завершено!" 