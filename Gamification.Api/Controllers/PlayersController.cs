using Gamification.Api.Dto;
using Gamification.Api.Mapping;
using Gamification.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

/// <summary>Игроки: регистрация, профиль, начисление опыта, достижения, прогресс.</summary>
[ApiController]
[Route("api/players")]
[Produces("application/json")]
public class PlayersController : ControllerBase
{
    private readonly IGamificationEngine _engine;
    private readonly ILogger<PlayersController> _logger;

    /// <summary>Фасад библиотеки и логгер приходят из DI-контейнера.</summary>
    public PlayersController(IGamificationEngine engine, ILogger<PlayersController> logger)
    {
        _engine = engine;
        _logger = logger;
    }

    /// <summary>Регистрирует нового игрока.</summary>
    /// <response code="201">Игрок создан.</response>
    /// <response code="400">Некорректный ник.</response>
    [HttpPost]
    [ProducesResponseType<PlayerResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public ActionResult<PlayerResponse> Create(CreatePlayerRequest request)
    {
        var player = _engine.RegisterPlayer(request.Nickname);
        _logger.LogInformation("Зарегистрирован игрок {Nickname} ({Id})", player.Nickname, player.Id);

        var response = player.ToResponse(_engine.GetProgress(player.Id));
        return CreatedAtAction(nameof(GetById), new { id = player.Id }, response);
    }

    /// <summary>Профиль игрока: уровень, опыт, прогресс.</summary>
    /// <response code="200">Профиль.</response>
    /// <response code="404">Игрок не найден.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<PlayerResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public ActionResult<PlayerResponse> GetById(Guid id)
    {
        _logger.LogInformation("Запрошен профиль игрока {Id}", id);
        var player = _engine.GetProfile(id);
        return player.ToResponse(_engine.GetProgress(id));
    }

    /// <summary>Начисляет опыт за действие игрока.</summary>
    /// <response code="200">Опыт начислен; в ответе новые уровень и достижения.</response>
    /// <response code="400">Некорректные данные.</response>
    /// <response code="404">Игрок не найден.</response>
    [HttpPost("{id:guid}/xp")]
    [ProducesResponseType<GrantXpResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public ActionResult<GrantXpResponse> GrantXp(Guid id, GrantXpRequest request)
    {
        var outcome = _engine.GrantXp(id, request.Amount!.Value, request.Source);
        _logger.LogInformation("Игроку {Id} начислено {Amount} XP ({Source}), уровень {Level}",
            id, request.Amount, request.Source, outcome.Level);
        return outcome.ToResponse();
    }

    /// <summary>Достижения игрока с датой выдачи.</summary>
    /// <response code="200">Список достижений.</response>
    /// <response code="404">Игрок не найден.</response>
    [HttpGet("{id:guid}/achievements")]
    [ProducesResponseType<List<PlayerAchievementResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public ActionResult<List<PlayerAchievementResponse>> GetAchievements(Guid id)
    {
        _logger.LogInformation("Запрошены достижения игрока {Id}", id);
        return _engine.GetPlayerAchievements(id).Select(a => a.ToResponse()).ToList();
    }

    /// <summary>Прогресс до следующего уровня.</summary>
    /// <response code="200">Прогресс.</response>
    /// <response code="404">Игрок не найден.</response>
    [HttpGet("{id:guid}/progress")]
    [ProducesResponseType<LevelProgressResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public ActionResult<LevelProgressResponse> GetProgress(Guid id)
    {
        _logger.LogInformation("Запрошен прогресс игрока {Id}", id);
        return _engine.GetProgress(id).ToResponse();
    }
}
