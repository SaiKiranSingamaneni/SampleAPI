namespace ProductivityOptimizerApi.Models;

public class ProductivityTip
{
    public int Id { get; set; }
    public required string Category { get; set; }
    public required string Tip { get; set; }
    public required string Duration { get; set; }
    public required string EnergyRequired { get; set; }
}

public class RecommendationResponse
{
    public bool Success { get; set; }
    public DateTime Timestamp { get; set; }
    public string? EnergyLevel { get; set; }
    public string? TaskType { get; set; }
    public int RecommendationCount { get; set; }
    public List<ProductivityTip> Tips { get; set; } = new();
    public DateTime NextCheckIn { get; set; }
    public string? Error { get; set; }
}

public class FocusScoreRequest
{
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double CloudCover { get; set; }
    public bool IsRaining { get; set; }
    public required string DayOfWeek { get; set; }
}

public class FocusScoreResponse
{
    public DateTime Timestamp { get; set; }
    public double FocusScore { get; set; }
    public required string FocusLevel { get; set; }
    public object WeatherInput { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
    public required string SuggestedFocusWindow { get; set; }
}
