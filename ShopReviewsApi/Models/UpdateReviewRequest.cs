using System.ComponentModel.DataAnnotations;

namespace ShopReviewsApi.Models;

/// <summary>
/// Часткове оновлення відгуку: можна змінити оцінку і/або коментар. Автор не змінюється.
/// Поле, яке не передано в JSON, залишається як було.
/// </summary>
public sealed class UpdateReviewRequest
{
    /// <summary>Нова оцінка (1–5). Якщо не передано — оцінка не змінюється.</summary>
    [Range(1, 5)]
    public int? Rating { get; set; }

    /// <summary>Новий коментар (до 500 символів). Якщо не передано — коментар не змінюється.</summary>
    [MaxLength(500)]
    public string? Comment { get; set; }
}
