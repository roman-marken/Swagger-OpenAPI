using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ShopReviewsApi.Repositories;

namespace ShopReviewsApi.Controllers;

/// <summary>
/// Внутрішній інструмент служби підтримки. Не входить до публічної документації.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/v{version:apiVersion}/reviews")]
public sealed class SupportReviewsController : ControllerBase
{
    private readonly IReviewRepository _reviews;

    public SupportReviewsController(IReviewRepository reviews)
    {
        _reviews = reviews;
    }

    /// <summary>
    /// Видаляє відгук за номером (спам / образливий контент). Лише для операторів підтримки.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        return _reviews.Delete(id) ? NoContent() : NotFound();
    }
}
