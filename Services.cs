using ProductivityOptimizerApi.Models;

namespace ProductivityOptimizerApi.Services;

public interface IProductivityService
{
    RecommendationResponse GetRecommendations(string energyLevel, string taskType, int limit);
}

public interface IFocusScoreService
{
    FocusScoreResponse CalculateScore(FocusScoreRequest request);
}

public class ProductivityService : IProductivityService
{
    private static readonly List<ProductivityTip> ProductivityTips = new()
    {
        new() { Id = 1, Category = "focus", Tip = "Use the Pomodoro Technique: 25 minutes of focused work, 5 minutes break", Duration = "25 min", EnergyRequired = "high" },
        new() { Id = 2, Category = "energy", Tip = "Take a 10-minute walk to boost mental clarity and oxygen flow", Duration = "10 min", EnergyRequired = "low" },
        new() { Id = 3, Category = "creativity", Tip = "Switch to a different environment for 30 minutes to spark new ideas", Duration = "30 min", EnergyRequired = "medium" },
        new() { Id = 4, Category = "focus", Tip = "Eliminate notifications and use Do Not Disturb mode", Duration = "0 min", EnergyRequired = "none" },
        new() { Id = 5, Category = "energy", Tip = "Drink a glass of water - dehydration impacts focus by 30%", Duration = "2 min", EnergyRequired = "none" },
        new() { Id = 6, Category = "creativity", Tip = "Brainstorm with a colleague for fresh perspectives", Duration = "15 min", EnergyRequired = "medium" }
    };

    public RecommendationResponse GetRecommendations(string energyLevel, string taskType, int limit)
    {
        var validEnergyLevels = new[] { "low", "medium", "high" };
        var validTaskTypes = new[] { "focus", "creativity", "energy" };

        if (!validEnergyLevels.Contains(energyLevel) || !validTaskTypes.Contains(taskType))
        {
            return new RecommendationResponse
            {
                Success = false,
                Error = $"Invalid parameters. Valid energyLevels: {string.Join(", ", validEnergyLevels)}. Valid taskTypes: {string.Join(", ", validTaskTypes)}"
            };
        }

        var filtered = ProductivityTips
            .Where(tip => tip.Category == taskType)
            .Where(tip =>
            {
                return energyLevel switch
                {
                    "low" => tip.EnergyRequired != "high",
                    "medium" => true,
                    "high" => true,
                    _ => false
                };
            })
            .OrderBy(_ => Guid.NewGuid())
            .Take(Math.Max(1, limit))
            .ToList();

        return new RecommendationResponse
        {
            Success = true,
            Timestamp = DateTime.UtcNow,
            EnergyLevel = energyLevel,
            TaskType = taskType,
            RecommendationCount = filtered.Count,
            Tips = filtered,
            NextCheckIn = DateTime.UtcNow.AddMinutes(25)
        };
    }
}

public class FocusScoreService : IFocusScoreService
{
    public FocusScoreResponse CalculateScore(FocusScoreRequest request)
    {
        double score = 50;

        // Temperature factor: optimal between 20-22°C
        if (request.Temperature >= 18 && request.Temperature <= 24)
            score += 15;
        else if (request.Temperature >= 15 && request.Temperature <= 27)
            score += 8;

        // Humidity factor: optimal between 40-60%
        if (request.Humidity >= 40 && request.Humidity <= 60)
            score += 15;
        else if (request.Humidity >= 30 && request.Humidity <= 70)
            score += 8;

        // Cloud cover factor: slightly cloudy is optimal
        if (request.CloudCover >= 20 && request.CloudCover <= 60)
            score += 12;

        // Rain factor: light rain can help focus
        if (request.IsRaining && request.CloudCover > 50)
            score += 10;
        else if (request.IsRaining)
            score -= 5;

        // Day of week factor
        var dayBonus = new Dictionary<string, double>
        {
            { "Monday", 0 },
            { "Tuesday", 5 },
            { "Wednesday", 10 },
            { "Thursday", 5 },
            { "Friday", -5 },
            { "Saturday", -10 },
            { "Sunday", -10 }
        };

        if (dayBonus.TryGetValue(request.DayOfWeek, out var bonus))
            score += bonus;

        score = Math.Max(0, Math.Min(100, score));

        var focusLevel = score switch
        {
            >= 75 => "Excellent",
            >= 50 => "Good",
            >= 25 => "Fair",
            _ => "Poor"
        };

        var recommendations = new List<string>();
        if (request.Temperature < 18 || request.Temperature > 25)
            recommendations.Add("Consider adjusting room temperature");
        if (request.Humidity < 40 || request.Humidity > 65)
            recommendations.Add("Try a humidifier or dehumidifier");
        if (request.IsRaining && request.CloudCover <= 50)
            recommendations.Add("Use blue light to counter low natural light");
        if (score < 40)
            recommendations.Add("Consider a location change for better focus");

        return new FocusScoreResponse
        {
            Timestamp = DateTime.UtcNow,
            FocusScore = score,
            FocusLevel = focusLevel,
            WeatherInput = request,
            Recommendations = recommendations,
            SuggestedFocusWindow = "25-minute focus session recommended"
        };
    }
}
