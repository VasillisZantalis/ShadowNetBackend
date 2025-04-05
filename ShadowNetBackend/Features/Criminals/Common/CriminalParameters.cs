namespace ShadowNetBackend.Features.Criminals.Common;

public class CriminalParameters : QueryStringParameters
{
    public string? Name { get; set; }
    public string? Alias { get; set; }
}
