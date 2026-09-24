namespace ShopReviewsApi.Models;

/// <summary>
/// Відгук про предмет каталогу (товар, книжка, курс тощо).
/// </summary>
public sealed class Review
{
    /// <summary>Унікальний номер відгуку.</summary>
    public int Id { get; init; }

    /// <summary>Цілочисельний ідентифікатор предмета каталогу.</summary>
    public int ItemId { get; init; }

    /// <summary>Ім'я автора. Після створення не змінюється.</summary>
    public required string Author { get; init; }

    /// <summary>Оцінка від 1 (погано) до 5 (відмінно).</summary>
    public int Rating { get; set; }

    /// <summary>Необов'язковий текстовий коментар.</summary>
    public string? Comment { get; set; }
}
