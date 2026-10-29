# Проверка части 1: сценарий п. 3.2 ТЗ.
# Сначала запустите API (F5 в Visual Studio или: dotnet run --project Gamification.Api),
# затем в другом окне PowerShell:  powershell -ExecutionPolicy Bypass -File demo.ps1

[Console]::OutputEncoding = [Text.Encoding]::UTF8
$Host_ = "http://localhost:5257"

function Call([string]$Method, [string]$Path, $Body = $null) {
    $params = @{ Method = $Method; Uri = "$Host_$Path"; UseBasicParsing = $true; ContentType = "application/json; charset=utf-8" }
    if ($null -ne $Body) { $params.Body = [Text.Encoding]::UTF8.GetBytes(($Body | ConvertTo-Json -Compress)) }
    try {
        $r = Invoke-WebRequest @params
        $code = [int]$r.StatusCode
        $text = [Text.Encoding]::UTF8.GetString($r.RawContentStream.ToArray())
    } catch [System.Net.WebException] {
        $resp = $_.Exception.Response
        if ($null -eq $resp) { Write-Host "Нет ответа от $Host_ — API запущен?" -ForegroundColor Red; exit 1 }
        $code = [int]$resp.StatusCode
        $text = (New-Object IO.StreamReader($resp.GetResponseStream(), [Text.Encoding]::UTF8)).ReadToEnd()
    }
    $color = if ($code -lt 300) { "Green" } elseif ($code -lt 500) { "Yellow" } else { "Red" }
    Write-Host "$Method $Path -> $code" -ForegroundColor $color
    Write-Host $text
    Write-Host ""
    return $text
}

Write-Host "=== 1. Регистрация игрока ===" -ForegroundColor Cyan
$player = Call POST "/api/players" @{ nickname = "Alex" } | ConvertFrom-Json
$id = $player.id

Write-Host "=== 2-4. Действия игрока: начисление опыта ===" -ForegroundColor Cyan
Call POST "/api/players/$id/xp" @{ amount = 150; source = "lesson" } | Out-Null
Call POST "/api/players/$id/xp" @{ amount = 1000; source = "purchase" } | Out-Null

Write-Host "=== 5. Профиль, прогресс, достижения ===" -ForegroundColor Cyan
Call GET "/api/players/$id" | Out-Null
Call GET "/api/players/$id/progress" | Out-Null
Call GET "/api/players/$id/achievements" | Out-Null

Write-Host "=== 6. Рейтинг и каталог ===" -ForegroundColor Cyan
Call POST "/api/players" @{ nickname = "Bob" } | Out-Null
Call GET "/api/leaderboard?top=10" | Out-Null
Call GET "/api/achievements" | Out-Null

Write-Host "=== Ошибки: 400 и 404 ===" -ForegroundColor Cyan
Call POST "/api/players" @{ nickname = "" } | Out-Null
Call POST "/api/players/$id/xp" @{ amount = -5; source = "!!" } | Out-Null
Call GET "/api/players/00000000-0000-0000-0000-000000000001" | Out-Null
Call GET "/api/leaderboard?top=0" | Out-Null
