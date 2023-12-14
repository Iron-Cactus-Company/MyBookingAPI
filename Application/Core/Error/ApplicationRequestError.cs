using Application.Core.Error.Enums;

namespace Application.Core.Error;

public class ApplicationRequestError
{
    public string? Field { get; set; }
    public ErrorType Type { get; set; }
}