using System.ComponentModel.DataAnnotations;
using Gamification.Api.Dto;
using Gamification.Api.Mapping;
using Gamification.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

/// <summary>Рейтинг игроков.</summary>
[ApiController]
[Route("api/leaderboard")]
[Produces("application/json")]
public class LeaderboardController : ControllerBase
{
    private readonly IGamificationEngine _engine;
    private readonly ILogger<LeaderboardController> _logger;

    /// <summary>Фасад библиотеки и логгер приходят из DI-контейнера.</summary>
    public LeaderboardController(IGamificationEngine engine, ILogger<LeaderboardController> logger)
    {
        _engine = engine;
        _logger = logger;
    }

    /// <summary>Топ игроков по суммарному опыту.</summary>
    /// <param name="top">Сколько игроков вернуть (1–100).</param>
    /// <response code="200">Рейтинг.</response>
    /// <response code="400">top вне диапазона.</response>
    [HttpGet]
    [ProducesResponseType<List<LeaderboardEntryResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public ActionResult<List<LeaderboardEntryResponse>> Get(
        [FromQuery, Range(1, 100, ErrorMessage = "top должен быть от 1 до 100.")] int top = 10)
    {
        _logger.LogInformation("Запрошен рейтинг, top = {Top}", top);
        return _engine.GetLeaderboard(top)
            .Select((player, index) => player.ToLeaderboardEntry(index + 1))
            .ToList();
    }
}
