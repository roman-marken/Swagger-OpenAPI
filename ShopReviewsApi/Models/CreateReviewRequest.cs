using System.ComponentModel.DataAnnotations;

namespace ShopReviewsApi.Models;

/// <summary>
/// Тіло запиту на створення відгуку.
/// </summary>
public sealed class CreateReviewRequest
{
    /// <summary>Ім'я автора. Обов'язкове, короткий текст.</summary>
    [Required]
    [MinLength(1)]
    [MaxLength(80)]
    public required string Author { get; set; }

    /// <summary>Оцінка. Ціле число від 1 до 5.</summary>
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    /// <summary>Необов'язковий коментар (кілька речень, до 500 символів).</summary>
    [MaxLength(500)]
    public string? Comment { get; set; }
}
