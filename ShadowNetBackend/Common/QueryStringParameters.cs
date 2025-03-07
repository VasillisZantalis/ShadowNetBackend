namespace ShadowNetBackend.Common;

public abstract class QueryStringParameters
{
    private const int MaxPageSize = 50;
    private int? _pageSize;

    public int? PageNumber { get; set; }

    public int? PageSize
    {
        get => _pageSize;
        set => _pageSize = (value.HasValue && value > MaxPageSize) ? MaxPageSize : value;
    }

    public string? OrderBy { get; set; }
    public string? Fields { get; set; }
}
