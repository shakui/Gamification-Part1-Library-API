using System.Reflection;
using Gamification.Api.Errors;
using Gamification.Api.Extensions;
using Gamification.Core.Events;
using Gamification.Core.Services;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Контроллеры с атрибутом [ApiController]: автоматическая проверка DataAnnotations → 400
builder.Services.AddControllers();

// Единообразные ошибки в формате ProblemDetails
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DomainExceptionHandler>();

// Swagger UI по адресу /swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gamification API",
        Version = "v1",
        Description = "Опыт, уровни и достижения. Библиотека Gamification.Core подключена через DI."
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

// Подключение библиотеки классов: регистрация её сервисов в DI-контейнере
builder.Services.AddGamification();

var app = builder.Build();

app.UseExceptionHandler();   // исключения → ProblemDetails (400 / 404 / 500)
app.UseStatusCodePages();    // пустые ответы с ошибкой (например, 404 на неизвестный адрес) → ProblemDetails

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// Подписка на события библиотеки стандартным механизмом .NET (event)
var engine = app.Services.GetRequiredService<IGamificationEngine>();
var eventLogger = app.Services.GetRequiredService<ILogger<GamificationEngine>>();
engine.GameEventOccurred += gameEvent => LogGameEvent(eventLogger, gameEvent);

app.Run();

static void LogGameEvent(ILogger logger, IGameEvent gameEvent)
{
    switch (gameEvent)
    {
        case LevelUpEvent levelUp:
            logger.LogInformation("Событие LevelUp: игрок {Id}, уровень {Old} → {New}",
                levelUp.PlayerId, levelUp.OldLevel, levelUp.NewLevel);
            break;
        case AchievementUnlockedEvent unlocked:
            logger.LogInformation("Событие AchievementUnlocked: игрок {Id}, достижение {Code}",
                unlocked.PlayerId, unlocked.Achievement.Code);
            break;
    }
}

/// <summary>Точка входа. Класс объявлен public, чтобы его могли использовать интеграционные тесты.</summary>
public partial class Program;
