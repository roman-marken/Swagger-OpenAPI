using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ShopReviewsApi.Models;
using ShopReviewsApi.Repositories;

namespace ShopReviewsApi.Controllers;

/// <summary>
/// Статистика відгуків (версія 2). Форма відповіді інша, ніж список у v1, тому винесено окремо.
/// </summary>
[ApiController]
[ApiVersion("2.0")]
[Tags("Reviews")]
[Route("api/v{version:apiVersion}")]
public sealed class ReviewsV2Controller : ControllerBase
{
    private readonly IReviewRepository _reviews;

    public ReviewsV2Controller(IReviewRepository reviews)
    {
        _reviews = reviews;
    }

    /// <summary>
    /// Повертає середню оцінку та кількість відгуків по предмету каталогу.
    /// </summary>
    /// <param name="itemId">Цілочисельний ідентифікатор предмета каталогу.</param>
    [HttpGet("items/{itemId:int}/reviews/stats")]
    [ProducesResponseType(typeof(ReviewStats), StatusCodes.Status200OK)]
    public ActionResult<ReviewStats> GetStats(int itemId)
    {
        return Ok(_reviews.GetStats(itemId));
    }
}
