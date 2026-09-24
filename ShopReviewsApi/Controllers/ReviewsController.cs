using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ShopReviewsApi.Models;
using ShopReviewsApi.Repositories;

namespace ShopReviewsApi.Controllers;

/// <summary>
/// Публічне API відгуків до предметів каталогу (версія 1).
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Tags("Reviews")]
[Route("api/v{version:apiVersion}")]
public sealed class ReviewsController : ControllerBase
{
    private readonly IReviewRepository _reviews;

    public ReviewsController(IReviewRepository reviews)
    {
        _reviews = reviews;
    }

    /// <summary>
    /// Повертає всі відгуки конкретного предмета каталогу.
    /// </summary>
    /// <param name="itemId">Цілочисельний ідентифікатор предмета каталогу.</param>
    /// <returns>Список відгуків. Порожній масив, якщо відгуків ще немає.</returns>
    [HttpGet("items/{itemId:int}/reviews")]
    [ProducesResponseType(typeof(IReadOnlyList<Review>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<Review>> GetByItem(int itemId)
    {
        return Ok(_reviews.GetByItemId(itemId));
    }

    /// <summary>
    /// Додає новий відгук до предмета каталогу.
    /// </summary>
    /// <param name="itemId">Цілочисельний ідентифікатор предмета каталогу.</param>
    /// <param name="request">Автор (обов'язково), оцінка 1–5 (обов'язково), коментар (необов'язково).</param>
    [HttpPost("items/{itemId:int}/reviews")]
    [ProducesResponseType(typeof(Review), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Review> Create(int itemId, [FromBody] CreateReviewRequest request)
    {
        var created = _reviews.Add(itemId, request);
        return CreatedAtAction(nameof(GetByItem), new { itemId, version = "1.0" }, created);
    }

    /// <summary>
    /// Частково оновлює існуючий відгук: лише оцінку і/або коментар. Ім'я автора не змінюється.
    /// </summary>
    /// <param name="id">Номер відгуку.</param>
    /// <param name="request">Поля для зміни. Пропущене поле залишається без змін.</param>
    [HttpPatch("reviews/{id:int}")]
    [ProducesResponseType(typeof(Review), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Review> Update(int id, [FromBody] UpdateReviewRequest request)
    {
        var updated = _reviews.Update(id, request);
        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    /// <summary>
    /// Добірка «хороших» відгуків для віджета на сайті: лише з оцінкою не нижче порога.
    /// </summary>
    /// <param name="itemId">Цілочисельний ідентифікатор предмета каталогу.</param>
    /// <param name="minRating">
    /// Мінімальна оцінка (1–5). Відгуки з нижчою оцінкою не потрапляють у відповідь.
    /// Якщо параметр не передати, використовується 4 (сьогоднішній запит маркетингу: «від 4 і вище»).
    /// </param>
    [HttpGet("items/{itemId:int}/reviews/featured")]
    [ProducesResponseType(typeof(IReadOnlyList<Review>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IReadOnlyList<Review>> GetFeatured(
        int itemId,
        [FromQuery]
        [Range(1, 5)]
        [DefaultValue(4)]
        int minRating = 4)
    {
        return Ok(_reviews.GetFeatured(itemId, minRating));
    }
}
