using Microsoft.AspNetCore.Mvc;
using ProductivityOptimizerApi.Models;
using ProductivityOptimizerApi.Services;

namespace ProductivityOptimizerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductivityController : ControllerBase
{
    private readonly IProductivityService _productivityService;

    public ProductivityController(IProductivityService productivityService)
    {
        _productivityService = productivityService;
    }

    /// <summary>
    /// Get personalized productivity recommendations based on energy level and task type
    /// </summary>
    [HttpGet("recommend")]
    public IActionResult GetRecommendations(
        [FromQuery] string energyLevel = "medium",
        [FromQuery] string taskType = "focus",
        [FromQuery] int limit = 3)
    {
        var result = _productivityService.GetRecommendations(energyLevel, taskType, limit);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class FocusScoreController : ControllerBase
{
    private readonly IFocusScoreService _focusScoreService;

    public FocusScoreController(IFocusScoreService focusScoreService)
    {
        _focusScoreService = focusScoreService;
    }

    /// <summary>
    /// Calculate focus score based on weather conditions
    /// </summary>
    [HttpPost("calculate")]
    public IActionResult CalculateFocusScore([FromBody] FocusScoreRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = _focusScoreService.CalculateScore(request);
        return Ok(result);
    }
}

[ApiController]
[Route("")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Health check endpoint for Azure monitoring
    /// </summary>
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            uptime = TimeSpan.FromMilliseconds(Environment.TickCount),
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production"
        });
    }

    /// <summary>
    /// API documentation endpoint - redirects to Swagger UI
    /// </summary>
    [HttpGet("/")]
    public IActionResult Root()
    {
        return Redirect("/swagger");
    }
}
