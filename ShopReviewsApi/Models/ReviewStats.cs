namespace ShopReviewsApi.Models;

/// <summary>
/// Агрегована статистика відгуків по предмету каталогу.
/// </summary>
public sealed class ReviewStats
{
    /// <summary>Ідентифікатор предмета каталогу.</summary>
    public int ItemId { get; init; }

    /// <summary>Кількість відгуків. 0, якщо відгуків ще немає.</summary>
    public int Count { get; init; }

    /// <summary>Середня оцінка. 0, якщо відгуків немає.</summary>
    public double AverageRating { get; init; }
}
