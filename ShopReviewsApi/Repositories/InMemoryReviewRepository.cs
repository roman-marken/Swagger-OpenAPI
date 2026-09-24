using ShopReviewsApi.Models;

namespace ShopReviewsApi.Repositories;

/// <summary>
/// Сховище відгуків у пам'яті процесу (як ProductRepository у Модулі 2).
/// </summary>
public sealed class InMemoryReviewRepository : IReviewRepository
{
    private readonly object _lock = new();
    private readonly List<Review> _reviews =
    [
        new() { Id = 1, ItemId = 10, Author = "Олена", Rating = 5, Comment = "Дуже якісний товар, рекомендую." },
        new() { Id = 2, ItemId = 10, Author = "Ігор", Rating = 4, Comment = "Гарно, але доставка трохи затрималась." },
        new() { Id = 3, ItemId = 10, Author = "Марія", Rating = 2, Comment = "Очікувала більше за цю ціну." },
        new() { Id = 4, ItemId = 22, Author = "Тарас", Rating = 5, Comment = null }
    ];
    private int _nextId = 5;

    public IReadOnlyList<Review> GetByItemId(int itemId)
    {
        lock (_lock)
        {
            return _reviews.Where(r => r.ItemId == itemId).ToList();
        }
    }

    public IReadOnlyList<Review> GetFeatured(int itemId, int minRating)
    {
        lock (_lock)
        {
            return _reviews
                .Where(r => r.ItemId == itemId && r.Rating >= minRating)
                .ToList();
        }
    }

    public Review Add(int itemId, CreateReviewRequest request)
    {
        lock (_lock)
        {
            var review = new Review
            {
                Id = _nextId++,
                ItemId = itemId,
                Author = request.Author.Trim(),
                Rating = request.Rating,
                Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim()
            };
            _reviews.Add(review);
            return review;
        }
    }

    public Review? GetById(int id)
    {
        lock (_lock)
        {
            return _reviews.FirstOrDefault(r => r.Id == id);
        }
    }

    public Review? Update(int id, UpdateReviewRequest request)
    {
        lock (_lock)
        {
            var review = _reviews.FirstOrDefault(r => r.Id == id);
            if (review is null)
            {
                return null;
            }

            if (request.Rating.HasValue)
            {
                review.Rating = request.Rating.Value;
            }

            if (request.Comment is not null)
            {
                review.Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim();
            }

            return review;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var review = _reviews.FirstOrDefault(r => r.Id == id);
            if (review is null)
            {
                return false;
            }

            _reviews.Remove(review);
            return true;
        }
    }

    public ReviewStats GetStats(int itemId)
    {
        lock (_lock)
        {
            var itemReviews = _reviews.Where(r => r.ItemId == itemId).ToList();
            return new ReviewStats
            {
                ItemId = itemId,
                Count = itemReviews.Count,
                AverageRating = itemReviews.Count == 0
                    ? 0
                    : Math.Round(itemReviews.Average(r => r.Rating), 2)
            };
        }
    }
}
