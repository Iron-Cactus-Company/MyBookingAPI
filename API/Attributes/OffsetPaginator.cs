namespace API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class OffsetPaginator : Attribute
{
    public int PageSize { get; set; } = 20;
    public int MaxSize { get; set; } = 20;
}