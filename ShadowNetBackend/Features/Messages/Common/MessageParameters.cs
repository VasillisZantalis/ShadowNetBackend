namespace ShadowNetBackend.Features.Messages.Common;

public class MessageParameters : QueryStringParameters
{
    public Guid? SenderId { get; set; }
    public Guid? RecipientId { get; set; }
    public string? Text { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
}
