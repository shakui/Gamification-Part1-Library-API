using Gamification.Api.Dto;
using Gamification.Api.Mapping;
using Gamification.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

/// <summary>Каталог достижений.</summary>
[ApiController]
[Route("api/achievements")]
[Produces("application/json")]
public class AchievementsController : ControllerBase
{
    private readonly IGamificationEngine _engine;
    private readonly ILogger<AchievementsController> _logger;

    /// <summary>Фасад библиотеки и логгер приходят из DI-контейнера.</summary>
    public AchievementsController(IGamificationEngine engine, ILogger<AchievementsController> logger)
    {
        _engine = engine;
        _logger = logger;
    }

    /// <summary>Все достижения. У секретных условие скрыто.</summary>
    /// <response code="200">Каталог.</response>
    [HttpGet]
    [ProducesResponseType<List<AchievementResponse>>(StatusCodes.Status200OK)]
    public ActionResult<List<AchievementResponse>> GetAll()
    {
        _logger.LogInformation("Запрошен каталог достижений");
        return _engine.GetAchievementCatalog().Select(a => a.ToResponse()).ToList();
    }
}
