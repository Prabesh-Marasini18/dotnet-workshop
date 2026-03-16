//inside Models/OpenAPIOptions.cs

namespace ProductAPI;

public class OpenApiOptions
{
    public const string SectionName = "OpenApi";

    public string Title { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public OpenApiAdvancedOptions Advanced { get; set; } = new();
}

public class OpenApiAdvancedOptions
{
    public bool IncludeRequestingHost { get; set; }
    public bool IncludeRequestingUser { get; set; }
    public bool IncludeRequestingUserAgent { get; set; }
}