using System.ComponentModel.DataAnnotations;

namespace Gamification.Api.Dto;

/// <summary>Запрос на регистрацию игрока.</summary>
public class CreatePlayerRequest
{
    /// <summary>Ник игрока, от 3 до 30 символов.</summary>
    /// <example>Alex</example>
    [Required(ErrorMessage = "Ник обязателен.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Ник должен быть от 3 до 30 символов.")]
    public string Nickname { get; set; } = "";
}
