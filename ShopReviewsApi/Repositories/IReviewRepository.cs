using ShopReviewsApi.Models;

namespace ShopReviewsApi.Repositories;

public interface IReviewRepository
{
    IReadOnlyList<Review> GetByItemId(int itemId);
    IReadOnlyList<Review> GetFeatured(int itemId, int minRating);
    Review Add(int itemId, CreateReviewRequest request);
    Review? GetById(int id);
    Review? Update(int id, UpdateReviewRequest request);
    bool Delete(int id);
    ReviewStats GetStats(int itemId);
}
