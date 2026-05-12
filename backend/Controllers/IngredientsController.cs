using backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
    private readonly IIngredientRepository _ingredientRepository;

    public IngredientsController(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        return Ok(await _ingredientRepository.GetAllAsync(ct));
    }
}
