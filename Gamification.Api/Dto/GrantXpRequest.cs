using System.ComponentModel.DataAnnotations;

namespace Gamification.Api.Dto;

/// <summary>Запрос на начисление опыта за действие игрока.</summary>
public class GrantXpRequest
{
    /// <summary>Сколько опыта начислить, от 1 до 10000.</summary>
    /// <example>150</example>
    [Required(ErrorMessage = "Количество опыта обязательно.")]
    [Range(1, 10000, ErrorMessage = "Опыт должен быть от 1 до 10000.")]
    public int? Amount { get; set; }

    /// <summary>Источник опыта: lesson, purchase, login и т. п.</summary>
    /// <example>lesson</example>
    [Required(ErrorMessage = "Источник обязателен.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Источник должен быть от 2 до 50 символов.")]
    [RegularExpression(@"^[A-Za-zА-Яа-яЁё0-9_]+$",
        ErrorMessage = "Источник: только латиница, кириллица, цифры и подчёркивание.")]
    public string Source { get; set; } = "";
}
