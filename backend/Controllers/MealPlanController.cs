using backend.Application.Interfaces;
using backend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MealPlanController : ControllerBase
{
    private readonly IMealPlannerService _mealPlannerService;

    public MealPlanController(IMealPlannerService mealPlannerService)
    {
        _mealPlannerService = mealPlannerService;
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> Calculate([FromBody] CalculateMealPlanRequest request, CancellationToken ct)
    {
        try
        {
            return Ok(await _mealPlannerService.CalculateAsync(request, ct));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("preview")]
    public async Task<IActionResult> Preview([FromBody] CalculateMealPlanRequest request, CancellationToken ct)
    {
        try
        {
            return Ok(await _mealPlannerService.PreviewAsync(request, ct));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("availability")]
    public async Task<IActionResult> Availability([FromBody] CalculateMealPlanRequest request, CancellationToken ct)
    {
        try
        {
            return Ok(await _mealPlannerService.AvailabilityAsync(request, ct));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("advise")]
    public async Task<IActionResult> Advise(CancellationToken ct)
    {
        return Ok(await _mealPlannerService.AdviseAsync(ct));
    }
}
